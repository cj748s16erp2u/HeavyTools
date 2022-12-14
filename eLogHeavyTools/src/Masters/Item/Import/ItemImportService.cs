using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eLog.Base.Masters.Item;
using eLog.Base.Setup.CustTariff;
using eLog.Base.Setup.Type;
using eLog.Base.Setup.Unit;
using eLog.HeavyTools.ImportBase;
using eLog.HeavyTools.ImportBase.Xlsx;
using eLog.HeavyTools.Masters.Item.Assortment;
using eLog.HeavyTools.Masters.Item.MainGroup;
using eLog.HeavyTools.Masters.Item.Model;
using eLog.HeavyTools.Masters.Item.Season;
using eLog.HeavyTools.Masters.Partner;
using eLog.HeavyTools.Masters.PriceTable;
using eLog.HeavyTools.Setup.Type;
using eLog.HeavyTools.Webshop;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.Lang;
using BItem = eLog.Base.Masters.Item.Item;
using Utils = eProjectWeb.Framework.Utils;

namespace eLog.HeavyTools.Masters.Item.Import
{
    class ItemImportService : ImportServiceBase<ItemImportResultSet, ItemImportResultSets, ItemRowContext>
    {

        public static string ItemImportRuning = "ItemImportRuning";
        private ItemBL ItemBL;
        private ItemExtBL ItemExtBL;
        private ColorException ColorException;

        public ItemImportService() : base()
        {
        }

        public override ProcessResult Import(string importDescrFileName, string importXlsxFileName)
        {
            return this.Import("Item", importDescrFileName, importXlsxFileName);
        }

        protected override int SaveImport(ItemImportResultSets results)
        {
            this.ItemBL = ItemBL.New();
            this.ItemExtBL = Base.Masters.Item.ItemExtBL.New();

            ColorException = new ColorException();

            var success = 0;

            using (new NS(typeof(BItem).Namespace))
            {
                var count = results.Count();
                var i = 0;
                foreach (var r in results)
                {
                    var ret = this.SaveImport(r, ++i, count);
                    if (ret)
                    {
                        success++;
                    }

                    if (i % 100 == 0)
                    {
                        this.logger.FlushFiles();
                    }
                }

                this.logger.FlushFiles();
            }

            return success;
        }

        private bool SaveImport(ItemImportResultSet result, int? pos = null, int? count = null)
        {
            var itemcode = result.Item?.Entity["itemcode"];

            if (pos != null && count != null)
            {
                var percnt = Math.Round(pos.Value / (count.Value * 1M) * 100, 2, MidpointRounding.AwayFromZero);
                this.logger.Log($"Saving Item [{pos} / {count} ({percnt:#.00}%)]: {itemcode} ");
            }
            else
            {
                this.logger.Log($"Saving Item: {itemcode} ");
            }

            var logText = new StringBuilder(result.LogText);

            string entityType = null;
            try
            {
                using (var db = DB.GetConn(DB.Main, Transaction.Use))
                { 
                    this.SaveItem(result, logText);
                    db.Commit();
                    this.logger.LogLine("...done");
                    return true;
                }
            }
            catch (eProjectWeb.Framework.Rules.ValidateException ex)
            {
                this.logger.LogLine();
                this.logger.LogErrorLine($"Item: {itemcode}, {"$entity".eLogTransl()}: {this.GetEntityName(entityType)}");
                var message = ToTextValidate(ex);
                this.logger.LogErrorLine(message);

                logText.AppendLine(message);
            }
            catch (Exception ex)
            {
                this.logger.LogLine();
                this.logger.LogErrorLine($"Item: {itemcode}, {"$entity".eLogTransl()}: {this.GetEntityName(entityType)}");
                var message = ex.Message;
                this.logger.LogErrorLine(message);

                logText.AppendLine(message);
            }

            result.LogText = logText.ToString();

            return false;
        }

        protected override IEnumerable<ImportSheet> ParseImportDescr(string descrText, params Newtonsoft.Json.JsonConverter[] converters)
        {
            var converterList = new List<Newtonsoft.Json.JsonConverter>();
            if (converters != null)
            {
                converterList.AddRange(converters);
            }

            converterList.Add(new ImportConditionalConverter());

            return base.ParseImportDescr(descrText, converterList.ToArray());
        }

        private string GetEntityName(string entityType)
        {
            if (string.IsNullOrWhiteSpace(entityType))
            {
                return null;
            }

            return $"'{$"${entityType.ToLowerInvariant()}".eLogTransl()}'";
        }

        /// <summary>
        /// Human readable console format
        /// </summary>
        /// <returns></returns>
        private string ToTextValidate(eProjectWeb.Framework.Rules.ValidateException ex)
        {
            string s = "";

            if (ex.Errors.Count > 0)
            {
                s += $"{"$errors".eLogTransl()}:\r\n-------\r\n";
                foreach (var er in ex.Errors)
                {
                    if (!string.IsNullOrEmpty(er.FieldName))
                        s += $"[{$"${er.FieldName}".eLogTransl()}]: ";
                    s += Translator.Translate(er.Text).Replace("\r\n", "  \r\n") + "\r\n";
                }
            }
            if (ex.Exceptions.Count > 0)
            {
                if (s != "")
                    s += "\r\n";
                s += $"{"$exceptions".eLogTransl()}:\r\n-------\r\n";
                foreach (var e in ex.Exceptions)
                {
                    s += Translator.Translate(e.ToString()).Replace("\r\n", "  \r\n") + "\r\n";
                }
            }

            return s;
        }

        private bool? SaveItem(ItemImportResultSet result, StringBuilder logText)
        {
            using (NS ns = new NS("eLog.HeavyTools.Masters.Item.Import"))
            {

                if (result.MultiplePrcTable != null)
                {
                    var ic = ConvertUtils.ToString(result.MultiplePrcTable.Entity["itemcode"]);
                    OlcItem itc = null;
                    OlcItemModelSeason its = null;
                    BItem i;


                    i = BItem.Load(ic);
                    if (i == null)
                    {
                        throw new ItemImportMessageException("$missing_item".eLogTransl(ic));
                    }
                    itc = OlcItem.Load(i.Itemid);
                    its = OlcItemModelSeason.Load(itc.Imsid);


                    var prcs = OlcPrctables.New();

                    prcs.Add(CreatePrc(ConvertUtils.ToDecimal(result.MultiplePrcTable.Entity["Ar5"]), 2, "HUF", PrcType.Original));
                    prcs.Add(CreatePrc(ConvertUtils.ToDecimal(result.MultiplePrcTable.Entity["Ar0"]), 2, "HUF", PrcType.Actual));
                    prcs.Add(CreatePrc(ConvertUtils.ToDecimal(result.MultiplePrcTable.Entity["Ar13"]), 2, "EUR", PrcType.Original, null, null, 2));
                    prcs.Add(CreatePrc(ConvertUtils.ToDecimal(result.MultiplePrcTable.Entity["Ar14"]), 2, "EUR", PrcType.Actual, null, null, 2));
                    prcs.Add(CreatePrc(ConvertUtils.ToDecimal(result.MultiplePrcTable.Entity["Ar16"]), 2, "RON", PrcType.Actual, null, null, 2));
                    prcs.Add(CreatePrc(ConvertUtils.ToDecimal(result.MultiplePrcTable.Entity["Ar15"]), 2, "RON", PrcType.Original, null, null, 2));
                    prcs.Add(CreatePrc(ConvertUtils.ToDecimal(result.MultiplePrcTable.Entity["Ar6"]), 3, "HUF", PrcType.Original, null, 27, 0));
                    prcs.Add(CreatePrc(ConvertUtils.ToDecimal(result.MultiplePrcTable.Entity["Ar1"]), 3, "HUF", PrcType.Actual, null, 27, 0));
                    prcs.Add(CreatePrc(ConvertUtils.ToDecimal(result.MultiplePrcTable.Entity["Ar9"]), 3, "EUR", PrcType.Original, null, 20, 2));
                    prcs.Add(CreatePrc(ConvertUtils.ToDecimal(result.MultiplePrcTable.Entity["Ar4"]), 6, "EUR", PrcType.Actual, WebshopType.sk, 20, 2));
                    prcs.Add(CreatePrc(ConvertUtils.ToDecimal(result.MultiplePrcTable.Entity["Ar11"]), 3, "CZK", PrcType.Original, null, 21, 2));
                    prcs.Add(CreatePrc(ConvertUtils.ToDecimal(result.MultiplePrcTable.Entity["Ar12"]), 3, "CZK", PrcType.Actual, null, 21, 2));
                    prcs.Add(CreatePrc(ConvertUtils.ToDecimal(result.MultiplePrcTable.Entity["Ar17"]), 3, "RON", PrcType.Original, null, 19, 2));
                    prcs.Add(CreatePrc(ConvertUtils.ToDecimal(result.MultiplePrcTable.Entity["Ar18"]), 3, "RON", PrcType.Actual, null, 19, 2));
                    prcs.Add(CreatePrc(ConvertUtils.ToDecimal(result.MultiplePrcTable.Entity["Ar9"]), 6, "EUR", PrcType.Original, WebshopType.sk, 20, 2));
                    prcs.Add(CreatePrc(ConvertUtils.ToDecimal(result.MultiplePrcTable.Entity["Ar4"]), 3, "EUR", PrcType.Actual, WebshopType.sk, 20, 2));
                    prcs.Add(CreatePrc(ConvertUtils.ToDecimal(result.MultiplePrcTable.Entity["Ar10"]), 6, "EUR", PrcType.Original, WebshopType.com, 27, 0));
                    prcs.Add(CreatePrc(ConvertUtils.ToDecimal(result.MultiplePrcTable.Entity["Ar7"]), 6, "EUR", PrcType.Actual, WebshopType.com, 27, 0));
                    prcs.Add(CreatePrc(ConvertUtils.ToDecimal(result.MultiplePrcTable.Entity["Ar11"]), 6, "CZK", PrcType.Original, WebshopType.cz, 21, 2));
                    prcs.Add(CreatePrc(ConvertUtils.ToDecimal(result.MultiplePrcTable.Entity["Ar12"]), 6, "CZK", PrcType.Actual, WebshopType.cz, 21, 2));
                    prcs.Add(CreatePrc(ConvertUtils.ToDecimal(result.MultiplePrcTable.Entity["Ar17"]), 6, "RON", PrcType.Original, WebshopType.ro, 19, 2));
                    prcs.Add(CreatePrc(ConvertUtils.ToDecimal(result.MultiplePrcTable.Entity["Ar18"]), 6, "RON", PrcType.Actual, WebshopType.ro, 19, 2));
                    prcs.Add(CreatePrc(ConvertUtils.ToDecimal(result.MultiplePrcTable.Entity["Ar8"]), 5, "EUR", PrcType.Original, null, null, 2));
                    prcs.Add(CreatePrc(ConvertUtils.ToDecimal(result.MultiplePrcTable.Entity["Ar3"]), 5, "EUR", PrcType.Actual, null, null, 2));
                    prcs.Add(CreatePrc(ConvertUtils.ToDecimal(result.MultiplePrcTable.Entity["Ar2"]), 4, "HUF", PrcType.Actual, null, 27, 0));



                    foreach (OlcPrctable impPrc in prcs.AllRows)
                    {
                        var pk = new Key()
                            {
                                //{ OlcPrctable.FieldPrc.Name, impPrc.Prc },
                                { OlcPrctable.FieldPtid.Name, impPrc.Ptid },
                                { OlcPrctable.FieldPrctype.Name, impPrc.Prctype },
                                { OlcPrctable.FieldCurid.Name, impPrc.Curid },
                                { OlcPrctable.FieldStartdate.Name, impPrc.Startdate },
                                { OlcPrctable.FieldEnddate.Name, impPrc.Enddate },
                                { OlcPrctable.FieldImid.Name, its.Imid }
                            };

                        if (ConvertUtils.ToInt32(itc.Iscollectionarticlenumber) == 0)
                        {
                            var pbl = OlcPrctableBL.New();
                            var pmap = pbl.CreateBLObjects();
                            var pt = OlcPrctable.Load(pk);
                            if (pt == null)
                            {
                             
                                impPrc.Imid = its.Imid;
                                pmap.Default = impPrc;

                                try
                                {
                                    pbl.Save(pmap);
                                }
                                catch (Exception e)
                                {
                                    throw new ItemImportMessageException("$missing_prcerror".eLogTransl(e.Message));
                                }

                            } else
                            {
                                pt.Prc = impPrc.Prc;
                                pmap.Default = pt;
                                try
                                {
                                    pbl.Save(pmap);
                                }
                                catch (Exception e)
                                {
                                    throw new ItemImportMessageException("$missing_prcerror".eLogTransl(e.Message));
                                }

                            }
                        }
                    }

                    return true;
                }

                if (result.Item == null &&  result.ItemSeason!=null)
                {
                    var sbl = OlcItemSeasonBL.New();
                    var map = sbl.CreateBLObjects();

                    var origItem = OlcItemSeason.Load(ConvertUtils.ToString(result.ItemSeason.Entity[OlcItemSeason.FieldIsid]));
                    map.Default = result.ItemSeason.Entity;
                    if (origItem != null)
                    {
                        this.logger.Log(" [modify] "); 
                        origItem.MergeTo(result.ItemSeason.Entity); 


                        result.ItemSeason.Entity.State = DataRowState.Modified;
                        map.SysParams.ActionID = ActionID.Modify;

                    }
                    try
                    {
                        sbl.Save(map);
                    }
                    catch (Exception e)
                    {
                        throw new ItemImportMessageException("$sessionsaveerror".eLogTransl(e.Message));
                    }

                    return true;
                }


                if (result.ItemMainGroup != null)
                {

                    var sbl = OlcItemMainGroupBL.New();
                    var map = sbl.CreateBLObjects();

                    var ck = new Key
                    {
                        { OlcItemMainGroup.FieldCode.Name, ConvertUtils.ToString(result.ItemMainGroup.Entity[OlcItemMainGroup.FieldCode]) }
                    };
                     
                    var origItem = OlcItemMainGroup.Load(ck);
                    map.Default = result.ItemMainGroup.Entity;
                    if (origItem != null)
                    {
                        this.logger.Log(" [modify] ");
                        origItem.MergeTo(result.ItemMainGroup.Entity);

                        result.ItemMainGroup.Entity.State = DataRowState.Modified;
                        map.SysParams.ActionID = ActionID.Modify;
                    }
                    try
                    {
                        Session.Current[ItemImportService.ItemImportRuning] = true;
                        sbl.Save(map);
                        Session.Current[ItemImportService.ItemImportRuning] = false;
                    }
                    catch (Exception e)
                    {
                        throw new ItemImportMessageException("$itemmaingroupsaveerror".eLogTransl(e.Message));
                    }

                    return true;
                }

                if (result.Item != null)
                {
                    var itemSupBL = ItemSupBL.New();

                    var map = new BLObjectMap();
                    map.SysParams.ActionID = ActionID.New;

                    BItem origItem = null;
                    OlcItem origOlcItem = null;

                    OlcItemModel imid = null;
                    int? isrlid = null;


                    var itemcode = ConvertUtils.ToString(result.Item.Entity[BItem.FieldItemcode.Name]);

                    if (!string.IsNullOrWhiteSpace(itemcode))
                    {
                        var ItemKey = new Key
                        {
                            [BItem.FieldItemcode.Name] = result.Item.Entity[BItem.FieldItemcode.Name]
                        };
                        origItem = BItem.Load(ItemKey);
                    }

                    if (origItem != null)
                    {
                        this.logger.Log(" [modify] ");
                        origItem = BItem.Load(origItem.Itemid);
                        origItem.MergeTo(result.Item.Entity);

                        result.Item.Entity.State = DataRowState.Modified;
                        map.SysParams.ActionID = ActionID.Modify;

                        origOlcItem = OlcItem.Load(origItem.Itemid);
                    }
                    else
                    {
                        this.logger.Log(" [create] ");
                        result.Item.Entity[BItem.FieldDelstat] = 0;
                    }

                    var Item = result.Item.Entity as BItem;

                    map.Default = result.Item.Entity;

                    if (result.OlcItem != null)
                    {
                        if (origItem != null)
                        {
                            if (origOlcItem != null)
                            {
                                origOlcItem.MergeTo(result.OlcItem.Entity);
                                result.OlcItem.Entity.State = DataRowState.Modified;
                            }
                        }
                        map.Add(result.OlcItem.Entity);
                    }
                     
                    var origItemCmps = origItem != null ? ItemCmps3.LoadAll(origItem.PK) : null;
                    var itemCmps = ItemCmps.New();
                    foreach (var pc in result.ItemCmps)
                    {
                        var origPc = origItemCmps?.FirstOrDefault(x => Utils.Equals(x["cmpid"], pc.Entity["cmpid"]));
                        if (origPc != null)
                        {
                            origPc.MergeTo(pc.Entity);
                            pc.Entity["itemid"] = origPc["itemid"];
                            pc.Entity.State = DataRowState.Modified;
                        }

                        itemCmps.AddRow(pc.Entity);
                    }
                    map.Add(itemCmps);

                    try
                    {
                        //modell:  A1 W22 300 CP .XL
                        var imgOldCode = Item.Itemcode.Value.Substring(0, 2);
                        var isidCode = Item.Itemcode.Value.Substring(2, 3);
                        var imidCode = Item.Itemcode.Value.Substring(5, 3);
                        string isrlidCode = "";
                        try
                        {
                            isrlidCode = Item.Itemcode.Value.Substring(11);
                        }
                        catch (Exception)
                        { 
                        }




                        ImpColorException ice; 
                        var IsColorException = ColorException.IsColorException(isidCode, imidCode, out ice);
                        if (IsColorException)
                        {
                            result.OlcItem.Entity[OlcItem.FieldColortype1.Name] = ice.ColorValue1;
                            result.OlcItem.Entity[OlcItem.FieldColortype2.Name] = ice.ColorValue2;
                            result.OlcItem.Entity[OlcItem.FieldColortype3.Name] = ice.ColorValue3;
                            result.OlcItem.Entity[OlcItem.FieldPatterntype.Name] = ice.SampleValue1;
                            result.OlcItem.Entity[OlcItem.FieldPatterntype2.Name] = ice.SampleValue2;
                        } else 
                        {
                            try
                            {
                                var colorname = ConvertUtils.ToString(result.Item.Entity[BItem.FieldName01]).Split(' ')[1];
                                var colortype = (int)SqlDataAdapter.ExecuteSingleValue(DB.Main,
                                    string.Format(@"select value from ols_typeline where typegrpid=507 and name={0}", Utils.SqlToString(colorname)));

                                result.OlcItem.Entity[OlcItem.FieldColorname.Name] = colorname;
                                result.OlcItem.Entity[OlcItem.FieldColortype1.Name] = colortype;
                            }
                            catch (Exception)
                            {
                                throw new ItemImportMessageException("$colornamecannotfound".eLogTransl(ConvertUtils.ToString(result.Item.Entity[BItem.FieldName01])));
                            }
                        }

                        var img = OlcItemMainGroup.Load(new Key() { { OlcItemMainGroup.FieldOldcode.Name, imgOldCode } });
                        if (img == null)
                        {
                            throw new ItemImportMessageException("$missing_imgOldCode".eLogTransl(imgOldCode));
                        }
                        var isid = OlcItemSeason.Load(isidCode);
                        if (isid == null)
                        {
                            isid = OlcItemSeason.Load(new Key() { { OlcItemSeason.FieldOldcode.Name, isidCode } });

                            if (isid == null)
                            {
                                throw new ItemImportMessageException("$missing_season".eLogTransl(isidCode));
                            }
                        }

                        var modelcode = imgOldCode + isid.Isid + imidCode;

                        imid = OlcItemModel.Load(new Key { { OlcItemModel.FieldImgid.Name, img.Imgid }, { OlcItemModel.FieldCode.Name, modelcode } });
                        if (imid == null)
                        {
                            try
                            {
                                imid = OlcItemModel.CreateNew();
                                imid.Code = modelcode;
                                imid.Imgid = img.Imgid;
                                imid.Name = ConvertUtils.ToString(result.ItemModel.Entity["name"]);
                                imid.Grossweight = ConvertUtils.ToDecimal(result.ItemModel.Entity["grossweight"]);
                                imid.Netweight = ConvertUtils.ToDecimal(result.ItemModel.Entity["netweight"]);
                                imid.Volume = ConvertUtils.ToDecimal(result.ItemModel.Entity["volume"]);
                                imid.Unitid = "db";
                                imid.Isimported = 1;
                                imid.Save();
                            }
                            catch (Exception e)
                            {
                                throw new ItemImportMessageException("$itemmodelcreateerror".eLogTransl(e.Message));
                            }

                        }
                        else
                        {
                            var c = false;

                            if (imid.Name != ConvertUtils.ToString(result.ItemModel.Entity["name"])) { c = true; }
                            if (imid.Grossweight != ConvertUtils.ToDecimal(result.ItemModel.Entity["grossweight"])) { c = true; }
                            if (imid.Netweight != ConvertUtils.ToDecimal(result.ItemModel.Entity["netweight"])) { c = true; }
                            if (imid.Volume != ConvertUtils.ToDecimal(result.ItemModel.Entity["volume"])) { c = true; }

                            if (c)
                            {
                                imid.Name = ConvertUtils.ToString(result.ItemModel.Entity["name"]);
                                imid.Grossweight = ConvertUtils.ToDecimal(result.ItemModel.Entity["grossweight"]);
                                imid.Netweight = ConvertUtils.ToDecimal(result.ItemModel.Entity["netweight"]);
                                imid.Volume = ConvertUtils.ToDecimal(result.ItemModel.Entity["volume"]);
                                imid.Save();
                            }
                        }


                        var colorOldCode = ConvertUtils.ToInt32(result.OlcItem.Entity[OlcItem.FieldColortype1.Name]);

                        if (!colorOldCode.HasValue)
                        {
                            throw new ItemImportMessageException("$invalidcolor");
                        }

                         var ims = OlcItemModelSeason.Load(new Key {
                    { OlcItemModelSeason.FieldImid.Name, imid.Imid },
                    { OlcItemModelSeason.FieldIsid.Name, isid.Isid},
                    { OlcItemModelSeason.FieldOldcode.Name, colorOldCode}
                });


                        if (ims == null)
                        {
                            var icid = ConvertUtils.ToString(SqlDataAdapter.ExecuteSingleValue(DB.Main, string.Format(@"select top 1 c.icid 
from olc_itemcolor c
outer apply (
select * 
	from olc_itemmodelseason s
	where s.icid=c.icid
	and s.imid={0}
	and s.isid={1}
) x
 where x.imsid is null
order by c.icid", Utils.SqlToString(imid.Imid), Utils.SqlToString(isid.Isid))));
                            if (string.IsNullOrEmpty(icid))
                            {
                                throw new ItemImportMessageException("$nonoreicid");
                            }
                            ims = OlcItemModelSeason.CreateNew();
                            ims.Imid = imid.Imid;
                            ims.Isid = isid.Isid;
                            ims.Oldcode = colorOldCode;
                            ims.Icid = icid;
                            ims.Save();
                        }

                        result.OlcItem.Entity[OlcItem.FieldImsid.Name] = ims.Imsid;

                        var isrhid = img.Isrhid;
                        if (!isrhid.HasValue || string.IsNullOrEmpty(isrlidCode))
                        {
                            throw new ItemImportMessageException("$missingisrhid");
                        }
                        var size = isrlidCode;
                        isrlid = ConvertUtils.ToInt32(SqlDataAdapter.ExecuteSingleValue(DB.Main, $@"select * from olc_itemsizerangeline where isrhid={isrhid} and size={Utils.SqlToString(size)}"));
                        result.OlcItem.Entity[OlcItem.FieldIsrlid.Name] = isrlid;
                    }

                    catch (Exception e)
                    {
                        throw new ItemImportMessageException("$itemcoderror".eLogTransl(e.Message));
                    }

                    if (!ConvertUtils.ToInt32(result.OlcItem.Entity[OlcItem.FieldIsrlid]).HasValue)
                    {
                        result.OlcItem.Entity[OlcItem.FieldIscollectionarticlenumber] = 1;
                    }
                    else
                    {
                        result.OlcItem.Entity[OlcItem.FieldIscollectionarticlenumber] = 0;
                    }

                    var cttext = ConvertUtils.ToString(result.Item.Entity[BItem.FieldCusttarid.Name]);
                    if (!string.IsNullOrEmpty(cttext))
                    {
                        try
                        {
                            var ct = CustTariff.Load(cttext);
                            if (ct == null)
                            {
                                ct = CustTariff.CreateNew();
                                ct.Custtarid = cttext;
                                ct.Name = cttext;
                                var ctbl = CustTariffBL.New();
                                var ctmap = ctbl.CreateBLObjects();
                                ctmap.Default = ct;
                                ctbl.Save(ctmap);
                            }
                        }
                        catch (Exception)
                        {
                            throw new ItemImportMessageException("$cannotcreatecusttariff".eLogTransl(cttext));
                        }
                    }

                    var u = ConvertUtils.ToString(result.Item.Entity[BItem.FieldUnitid]);
                    var unitid = Unit.Load(u);
                    if (unitid == null)
                    {
                        throw new ItemImportMessageException("$missingunitid".eLogTransl(u));
                    }

                    this.ItemBL.Save(map);

                    var itemid = ConvertUtils.ToInt32(result.Item.Entity[BItem.FieldItemid.Name]);

                    var sp = ConvertUtils.ToString(result.ItemSup.Entity[ItemSup.FieldSuppartnid.Name]);

                    if (!string.IsNullOrEmpty(sp))
                    {
                        var p = OlcPartner.Load(new Key { { OlcPartner.FieldOldcode.Name, sp } });
                        if (p != null)
                        {

                            var found = false;
                            var iss = ItemSups.LoadByItemid(itemid);
                            foreach (ItemSup itemsup in iss.AllRows)
                            {
                                if (itemsup.Suppartnid == p.Partnid)
                                {
                                    found = true;
                                    itemsup.Delstat = 0;
                                    itemsup.Def = 1;
                                }
                                else
                                {
                                    itemsup.Def = 0;
                                }
                                itemsup.Save();
                            }

                            if (!found)
                            {
                                result.ItemSup.Entity[ItemSup.FieldSuppartnid.Name] = p.Partnid;
                                result.ItemSup.Entity[ItemSup.FieldItemid.Name] = result.Item.Entity[BItem.FieldItemid.Name];
                                result.ItemSup.Entity[ItemSup.FieldDef.Name] = 1;

                                var ismap = itemSupBL.CreateBLObjects();
                                ismap.Default = result.ItemSup.Entity;
                                itemSupBL.Save(ismap);
                            }
                        } else
                        {
                            logText.AppendLine("$missingpartneroldcode".eLogTransl(sp));
                        }
                    }

                    var ebl = ItemExtBL.New();
                    var exts = ItemExts.LoadDefByItemid(itemid);
                    foreach (ItemExt item in exts.AllRows)
                    {
                        item.Def = 0;
                    }
                    exts.Save(ebl);
                    var ek = new Key
                {
                    { ItemExt.FieldItemid.Name, itemid },
                    { ItemExt.FieldExtcode.Name, result.ItemExt.Entity[ItemExt.FieldExtcode.Name] }
                };

                    var ext = ItemExt.Load(ek);
                    if (ext != null)
                    {
                        ext.MergeTo(result.ItemExt.Entity);
                        result.ItemExt.Entity.State = DataRowState.Modified;
                    }

                    result.ItemExt.Entity[ItemExt.FieldItemid.Name] = itemid;

                    var emap = ebl.CreateBLObjects();
                    emap.Default = result.ItemExt.Entity;
                    ebl.Save(emap);

                    var pk = new Key
                {
                    { OlcPrctable.FieldPrc.Name, result.PrcTable.Entity[OlcPrctable.FieldPrc.Name] },
                    { OlcPrctable.FieldPtid.Name, result.PrcTable.Entity[OlcPrctable.FieldPtid.Name] },
                    { OlcPrctable.FieldCurid.Name, result.PrcTable.Entity[OlcPrctable.FieldCurid.Name] },
                    { OlcPrctable.FieldStartdate.Name, result.PrcTable.Entity[OlcPrctable.FieldStartdate.Name] },
                    { OlcPrctable.FieldEnddate.Name, result.PrcTable.Entity[OlcPrctable.FieldEnddate.Name] },
                    { OlcPrctable.FieldImid.Name, imid.Imid }
                };

                    if (ConvertUtils.ToInt32(result.OlcItem.Entity[OlcItem.FieldIscollectionarticlenumber]) == 0)
                    {
                        var pt = OlcPrctable.Load(pk);
                        if (pt == null)
                        {
                            var pbl = OlcPrctableBL.New();
                            var pmap = pbl.CreateBLObjects();
                            result.PrcTable.Entity[OlcPrctable.FieldImid.Name] = imid.Imid;
                            pmap.Default = result.PrcTable.Entity;
                            try
                            {
                                pbl.Save(pmap);
                            }
                            catch (Exception e)
                            {
                                logText.AppendLine("$missing_prcerror".eLogTransl(e.Message));
                            }
                            
                        }
                    }
                    return true;
                }

                if (result.TypeHeadColor != null)
                {

                    //var tbl = TypeLineBL3.New();
                    //var map = tbl.CreateBLObjects();

                    var ck = new Key
                    {
                        { TypeLine.FieldName.Name, ConvertUtils.ToString(result.TypeHeadColor.Entity[TypeLine.FieldName]) },
                        { TypeLine.FieldValue.Name, ConvertUtils.ToString(result.TypeHeadColor.Entity[TypeLine.FieldValue]) },
                    };

                    var origItem = TypeLine.Load(ck);
                    //map.Default = result.TypeHeadColor.Entity;
                    if (origItem != null)
                    {
                        this.logger.Log(" [modify] ");
                        origItem.MergeTo(result.TypeHeadColor.Entity);

                        result.TypeHeadColor.Entity.State = DataRowState.Modified;
                        //map.SysParams.ActionID = ActionID.Modify;
                    }
                    try
                    {
                        Session.Current[ItemImportService.ItemImportRuning] = true;
                        //tbl.Save(map);
                        result.TypeHeadColor.Entity.Save();
                        Session.Current[ItemImportService.ItemImportRuning] = false;
                    }
                    catch (Exception e)
                    {
                        throw new ItemImportMessageException("$colorsaveerror".eLogTransl(e.Message));
                    }

                    return true;
                }

                if (result.ItemAssortment != null)
                {

                    var tbl = OlcItemAssortmentBL.New();
                    var map = tbl.CreateBLObjects();

                    var ck = new Key
                    {
                        { OlcItemAssortment.FieldAssortmentitemid.Name,
                            ConvertUtils.ToInt32(
                                result.ItemAssortment.Entity[OlcItemAssortment.FieldAssortmentitemid]) },
                        { OlcItemAssortment.FieldItemid.Name,
                            ConvertUtils.ToInt32(
                                result.ItemAssortment.Entity[OlcItemAssortment.FieldItemid]) },
                    };

                    var origItem = OlcItemAssortment.Load(ck);
                    map.Default = result.ItemAssortment.Entity;
                    
                    if (origItem != null)
                    {
                        this.logger.Log(" [modify] ");
                        origItem.MergeTo(result.ItemAssortment.Entity);

                        result.ItemAssortment.Entity.State = DataRowState.Modified;
                        map.SysParams.ActionID = ActionID.Modify;
                    }
                    try
                    {
                        Session.Current[ItemImportService.ItemImportRuning] = true;
                        tbl.Save(map);
                        Session.Current[ItemImportService.ItemImportRuning] = false;
                    }
                    catch (Exception e)
                    {
                        throw new ItemImportMessageException("$itemassortmenterror".eLogTransl(e.Message));
                    }

                    return true;
                }

                if (result.ImpColorException != null)
                {
                    var ck = new Key
                    {
                        { ImpColorException.FieldModelnumber.Name,
                            ConvertUtils.ToString(result.ImpColorException.Entity[ImpColorException.FieldModelnumber]) },
                        { ImpColorException.FieldColorbalance.Name, 
                            ConvertUtils.ToString(result.ImpColorException.Entity[ImpColorException.FieldColorbalance]) },
                        { ImpColorException.FieldSeason.Name, 
                            ConvertUtils.ToString(result.ImpColorException.Entity[ImpColorException.FieldSeason]) },
                    };

                    var origItem = ImpColorException.Load(ck);
                    if (origItem != null)
                    {
                        this.logger.Log(" [modify] ");
                        origItem.MergeTo(result.ImpColorException.Entity);
                        result.ImpColorException.Entity.State = DataRowState.Modified;
                    }
                    try
                    {
                        Session.Current[ItemImportService.ItemImportRuning] = true;

                        var mn = ConvertUtils.ToString(result.ImpColorException.Entity[ImpColorException.FieldModelnumber]);

                        if (mn != null)
                        {
                            mn = mn.Replace("-", "");
                        }

                        result.ImpColorException.Entity[ImpColorException.FieldModelnumber] = mn;
                        
                         result.ImpColorException.Entity.Save();

                        var ice = ConvertUtils.ToInt32(result.ImpColorException.Entity[ImpColorException.FieldIce]);

                        var ok = true;
                        var found = false;
                        var errorfield = "";

                        foreach (var row in SqlDataAdapter.Query(String.Format(@"
select color1, color2, color3, sample1, sample2, 
	tl1.value c1, tl2.value c2, tl3.value c3, tl4.value s1, tl5.value s2
  from imp_colorexception e
  left join ols_typeline tl1 on tl1.typegrpid=507 and tl1.name=e.color1
  left join ols_typeline tl2 on tl2.typegrpid=507 and tl2.name=e.color2
  left join ols_typeline tl3 on tl3.typegrpid=507 and tl3.name=e.color3
  left join ols_typeline tl4 on tl4.typegrpid=501 and tl4.str1=e.sample1
  left join ols_typeline tl5 on tl5.typegrpid=501 and tl5.str1=e.sample2
  where e.ice={0}
", ice)).AllRows)
                        {
                            found = true;
                             
                            var color1 = ConvertUtils.ToString(row["color1"]);
                            var color2 = ConvertUtils.ToString(row["color2"]);
                            var color3 = ConvertUtils.ToString(row["color3"]);
                            var sample1 = ConvertUtils.ToString(row["sample1"]);
                            var sample2 = ConvertUtils.ToString(row["sample2"]);
                            var c1 = ConvertUtils.ToInt32(row["c1"]);
                            var c2 = ConvertUtils.ToInt32(row["c2"]);
                            var c3 = ConvertUtils.ToInt32(row["c3"]);
                            var s1 = ConvertUtils.ToInt32(row["s1"]);
                            var s2 = ConvertUtils.ToInt32(row["s2"]);

                            CheckValue(color1, c1, ref ok, ref errorfield, "Szín 1");
                            CheckValue(color2, c2, ref ok, ref errorfield, "Szín 2");
                            CheckValue(color3, c3, ref ok, ref errorfield, "Szín 3");
                            CheckValue(sample1, s1, ref ok, ref errorfield, "Minta 1");
                            CheckValue(sample2, s2, ref ok, ref errorfield, "Minta 2");

                        }
                        if (!found || !ok)
                        {
                            throw new ItemImportMessageException(Translator.Translate("$impcolorexceptionmissingtypeline", errorfield));
                        }


                        Session.Current[ItemImportService.ItemImportRuning] = false;
                    }
                    catch (Exception e)
                    {
                        throw new ItemImportMessageException("$impcolorexceptionsaveerror".eLogTransl(e.Message));
                    }

                    return true;
                }

                return null;
            }
        }

        private void CheckValue(string str, int? typelinevalie, ref bool ok, ref string errorfield, string name)
        {
            if (!string.IsNullOrEmpty(str))
            {
                if (!typelinevalie.HasValue)
                {
                    ok = false;
                    errorfield = name;
                }
            }
        }

        private OlcPrctable CreatePrc(decimal? prc, int ptid, string curid, PrcType prcType)
        {
            return CreatePrc(prc, ptid, curid, prcType, null, null, null);
        }

        private OlcPrctable CreatePrc(decimal? prc, int ptid, string curid, PrcType prcType, WebshopType? wt, int? gross, int? round)
        { 
            if (gross.HasValue && prc.HasValue && round.HasValue)
            {
                prc = prc / 100 * (100 + gross);
                prc = decimal.Round(prc.Value, round.Value);
            }          
            
            
            var pt = OlcPrctable.CreateNew();

            pt.Startdate = new DateTime(2001, 1, 1);
            pt.Enddate = new DateTime(2099, 12, 31);
            pt.Curid = curid; 
            pt.Prc = prc;
            pt.Ptid = ptid;
            pt.Prctype = (int)prcType;
            if (wt.HasValue)
            {
                pt.Wid = wt.ToString();
            }




            return pt;
        }

        protected override ItemImportResultSet CreateResultSet(ItemRowContext rowContext)
        {
            return new ItemImportResultSet
            {
                Item = rowContext.Item,
                OlcItem = rowContext.OlcItem,
                ItemCmps = rowContext.ItemCmps.ToList(),
                ItemExt = rowContext.ItemExt,
                ItemModel = rowContext.ItemModel,
                ItemSeason = rowContext.ItemSeason,
                ItemSup = rowContext.ItemSup,
                PrcTable = rowContext.PrcTable,
                MultiplePrcTable = rowContext.MultiplePrcTable,
                ItemMainGroup = rowContext.ItemMainGroup,
                TypeHeadColor= rowContext.TypeHeadColor,
                ImpColorException = rowContext.ImpColorException,
                ItemAssortment= rowContext.ItemAssortment,
                LogText = rowContext.LogText,
                Row = rowContext.Row,
                LogCol = rowContext.LogCol,
            };
        }

        protected override void CreateEntity(ItemRowContext rowContext)
        {
            var alias = rowContext.CurrentTable.Alias;
            if (string.IsNullOrWhiteSpace(alias))
            {
                alias = rowContext.CurrentTable.Table;
            }

            TableEntry entry;
            switch (rowContext.CurrentTable.Table.ToLowerInvariant())
            {
                case "ols_item":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = Base.Masters.Item.Item.GetSchema(),
                        Entity = Base.Masters.Item.Item.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.Item = entry;
                    break;
                case "olc_item":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = OlcItem.GetSchema(),
                        Entity = OlcItem.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.OlcItem = entry;
                    break;
                case "ols_itemcmp":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = Base.Masters.Item.ItemCmp.GetSchema(),
                        Entity = Base.Masters.Item.ItemCmp.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.ItemCmps.Add(entry);
                    break;
                case "ols_itemext":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = Base.Masters.Item.ItemExt.GetSchema(),
                        Entity = Base.Masters.Item.ItemExt.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.ItemExt = entry;
                    break;
                case "olc_itemmodel":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = OlcItemModel.GetSchema(),
                        Entity = OlcItemModel.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.ItemModel = entry;
                    break;
                case "olc_itemseason":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = OlcItemSeason.GetSchema(),
                        Entity = OlcItemSeason.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.ItemSeason = entry;
                    break;
                case "olc_prctable":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = OlcPrctable.GetSchema(),
                        Entity = OlcPrctable.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.PrcTable = entry;
                    break;
                case "ols_itemsup":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = ItemSup.GetSchema(),
                        Entity = ItemSup.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.ItemSup = entry;
                    break;
                case "olc_multipleprctable":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = OlcMultiplePrcTable.GetSchema(),
                        Entity = OlcMultiplePrcTable.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.MultiplePrcTable = entry;
                    break;
                case "olc_itemmaingroup":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = OlcItemMainGroup.GetSchema(),
                        Entity = OlcItemMainGroup.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.ItemMainGroup = entry;
                    break;
                case "ols_typeline":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = TypeLine.GetSchema(),
                        Entity = TypeLine.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.TypeHeadColor = entry;
                    break;
                case "imp_colorexception":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = ImpColorException.GetSchema(),
                        Entity = ImpColorException.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.ImpColorException = entry;
                    break;  
                case "olc_itemassortment":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = OlcItemAssortment.GetSchema(),
                        Entity = OlcItemAssortment.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.ItemAssortment = entry;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rowContext.CurrentTable.Table), rowContext.CurrentTable.Table, nameof(this.CreateEntity));
            }
        }

        protected override void RemoveEntity(ItemRowContext rowContext)
        {
            switch (rowContext.CurrentTable.Table.ToLowerInvariant())
            {
                case "ols_item": rowContext.Item = null; break;
                case "olc_item": rowContext.OlcItem = null; break;
                case "ols_itemcmp": rowContext.ItemCmps.Clear(); break;
                case "ols_itemext": rowContext.ItemExt = null; break;
                case "olc_itemmodel": rowContext.ItemModel = null; break;
                case "olc_itemseason": rowContext.ItemSeason = null; break;
                case "olc_prctable": rowContext.PrcTable = null; break;
                case "ols_itemsup": rowContext.ItemSup = null; break;
                case "olc_multipleprctable": rowContext.MultiplePrcTable = null; break;
                case "olc_itemmaingroup": rowContext.ItemMainGroup = null; break;
                case "imp_colorexception": rowContext.ImpColorException = null; break;
                case "ols_typeline": rowContext.TypeHeadColor = null; break;
                case "olc_itemassortment": rowContext.ItemAssortment = null; break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(rowContext.CurrentTable.Table), rowContext.CurrentTable.Table, nameof(this.RemoveEntity));
            }
        }

        protected override object DetermineSelfLookupValue(object value, ItemRowContext rowContext)
        {
            return base.DetermineSelfLookupValue(value, rowContext);
        }

        protected override bool ProcessCustomConditionals(ItemRowContext rowContext, ImportConditional cond, object value)
        {
            return base.ProcessCustomConditionals(rowContext, cond, value);
        }

        protected override void ResolveColumnNames(XlsxWorksheet worksheet, IEnumerable<Tuple<string, string>> cellObjs, ImportConditional cond)
        {
            base.ResolveColumnNames(worksheet, cellObjs, cond);

            if (cond.Valid && cond is ItemImportConditional itemCond)
            {
                if (string.IsNullOrWhiteSpace(itemCond.RefColumn) && !string.IsNullOrWhiteSpace(itemCond.RefColumnName))
                {
                    var obj = cellObjs.FirstOrDefault(c => string.Equals(c.Item1, itemCond.RefColumnName, StringComparison.InvariantCultureIgnoreCase));
                    itemCond.RefColumn = obj?.Item2;
                    itemCond.Valid = !string.IsNullOrWhiteSpace(itemCond.RefColumn);
                }

                if (!string.IsNullOrWhiteSpace(itemCond.RefColumn))
                {
                    itemCond.RefColumnIndex = XlsxWorksheet.GetColumnIndex(itemCond.RefColumn);
                }
            }
        }
    }
}
