using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eLog.Base.Masters.Item;
using eLog.HeavyTools.ImportBase;
using eLog.HeavyTools.ImportBase.Xlsx;
using eLog.HeavyTools.Masters.Item.MainGroup;
using eLog.HeavyTools.Masters.Item.Model;
using eLog.HeavyTools.Masters.Item.Season;
using eLog.HeavyTools.Masters.Partner;
using eLog.HeavyTools.Masters.PriceTable;
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
        private Base.Masters.Item.ItemBL ItemBL;
        private Base.Masters.Item.ItemExtBL ItemExtBL; 

        public ItemImportService() : base()
        {
        }

        public override ProcessResult Import(string importDescrFileName, string importXlsxFileName)
        {
            return this.Import("Item", importDescrFileName, importXlsxFileName);
        }

        protected override int SaveImport(ItemImportResultSets results)
        { 
            this.ItemBL = Base.Masters.Item.ItemBL.New();
            this.ItemExtBL = Base.Masters.Item.ItemExtBL.New(); 

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

                    entityType = typeof(Base.Masters.Item.Item).Name;
                    this.SaveItem(result);
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
          
        private bool? SaveItem(ItemImportResultSet result)
        {
            if (result.Item != null)
            {
                var itemSupBL = ItemSupBL.New();

                var map = new BLObjectMap();
                map.SysParams.ActionID = ActionID.New;
                
                BItem origItem = null;
                OlcItem origOlcItem = null;

                using (eProjectWeb.Framework.Lang.NS ns = new eProjectWeb.Framework.Lang.NS("eLog.HeavyTools.Masters.Item.Import"))
                { 
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
                        origOlcItem = OlcItem.Load(origItem.Itemid);
                        origItem.MergeTo(result.OlcItem.Entity);

                        result.Item.Entity.State = DataRowState.Modified;
                        map.SysParams.ActionID = ActionID.Modify;
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

                        result.OlcItem[OlcItem.] =???;

                        result.OlcItem[OlcItem.FieldColortype1] =???;

                        result.OlcItem[OlcItem.FieldIsrlid] =???;

                        map.Add(result.OlcItem.Entity);
                    }

                    var origItemCmps = origItem != null ? ItemCmps.Load(origItem.PK) : null;
                    var itemCmps = ItemCmps.New();
                    foreach (var pc in result.ItemCmps)
                    {
                        var origPc = origItemCmps?.FirstOrDefault(x => Utils.Equals(x["cmpid"], pc.Entity["cmpid"]));
                        if (origPc != null)
                        {
                            origPc.MergeTo(pc.Entity);
                            pc.Entity["partnid"] = origPc["partnid"];
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
                        var isrlidCode = Item.Itemcode.Value.Substring(11);

                        var img = OlcItemMainGroup.Load(new Key() { { OlcItemMainGroup.FieldOldcode.Name, imgOldCode } });
                        if (img == null)
                        {
                            throw new MessageException("$missing_imgOldCode".eLogTransl(imgOldCode));
                        }
                        var isid = OlcItemSeason.Load(isidCode);
                        if (isid == null)
                        {
                            throw new MessageException("$missing_season".eLogTransl(isidCode));
                        }

                        tesztelni, hogy bekerűl-e a model kódva a főcsoport is  imgOldCode + imidCode ,DB törölni

                        var imid = OlcItemModel.Load(new Key { { OlcItemModel.FieldImgid.Name, img.Imgid }, { OlcItemModel.FieldCode.Name, imgOldCode + imidCode } });
                        if (imid == null)
                        {
                            try
                            {
                                imid = OlcItemModel.CreateNew();
                                imid.Code = imgOldCode + imidCode;
                                imid.Imgid = img.Imgid;
                                imid.Name = ConvertUtils.ToString(result.ItemModel.Entity["name"]);
                                imid.Grossweight = ConvertUtils.ToDecimal(result.ItemModel.Entity["grossweight"]);
                                imid.Netweight = ConvertUtils.ToDecimal(result.ItemModel.Entity["netweight"]);
                                imid.Volume = ConvertUtils.ToDecimal(result.ItemModel.Entity["volume"]);
                                imid.Unitid = "db";
                                imid.Save();
                            }
                            catch (Exception e)
                            {
                                throw new MessageException("$itemmodelcreateerror".eLogTransl(e.Message));
                            }

                        }


                        var colorOldCode = ConvertUtils.ToInt32(result.OlcItem.Entity[OlcItem.FieldColortype1.Name]);

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
  order by c.icid", Utils.SqlToString(imid.Imid), Utils.SqlToString(isid.Isid))));
                            if (string.IsNullOrEmpty(icid))
                            {
                                throw new MessageException("$nonoreicid");
                            }

                            ims = OlcItemModelSeason.CreateNew();
                            ims.Imid = imid.Imid;
                            ims.Isid = isid.Isid;
                            ims.Oldcode = colorOldCode;
                            ims.Icid = icid;

                            ims.Save();
                        }

                        result.OlcItem.Entity[OlcItem.FieldImsid.Name] = ims.Imsid;
                    }

                    catch (Exception e)
                    {
                        throw new MessageException("$itemcoderror".eLogTransl(e.Message));
                    }

                }
                this.ItemBL.Save(map);

                var sp = ConvertUtils.ToString(result.ItemSup.Entity[ItemSup.FieldSuppartnid.Name]);

                if (!string.IsNullOrEmpty(sp))
                {
                    var p = OlcPartner.Load(new Key { { OlcPartner.FieldOldcode.Name, sp } });
                    if (p == null)
                    {
                        throw new MessageException("$missingpartneroldcode".eLogTransl(sp));
                    }

                    var found = false;
                    var iss = ItemSups.LoadByItemid(ConvertUtils.ToInt32(result.Item.Entity[BItem.FieldItemid.Name]));
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
                }

                //TODO: itemext


                return true; 
            }

            return null;
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
                        Schema = OlcItemModelSeason.GetSchema(),
                        Entity = OlcItemModelSeason.CreateNew()
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
