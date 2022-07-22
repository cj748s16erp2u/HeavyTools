using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eLog.Base.Masters.Item;
using eLog.Base.Masters.Partner;
using eLog.Base.Purchase.Pord;
using eLog.HeavyTools.ImportBase;
using eLog.HeavyTools.Masters.Partner.Import;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.Lang;
using eProjectWeb.Framework.Rules;

namespace eLog.HeavyTools.Purchase.Pord.Import
{
    public class PordHeadImportService : ImportServiceBase<PordHeadImportResultSet, PordHeadImportResultSets, PordHeadRowContext>
    {
        private PordHeadBL PordHeadBL;

        public override ProcessResult Import(string importDescrFileName, string importXlsxFileName)
        {
            return this.Import("PordHead", importDescrFileName, importXlsxFileName);
        }

        protected override int SaveImport(PordHeadImportResultSets results)
        {
            //PordHeadRules.ValidateExtcodeUnique = false;

            this.PordHeadBL = PordHeadBL.New();
            Dictionary<int, int> partnerPordHeadIds = new Dictionary<int, int>();

            var success = 0;

            using (new NS(typeof(PordHead).Namespace))
            {
                var count = results.Count();
                var i = 0;
                foreach (var r in results)
                {
                    var ret = this.SaveImport(r, partnerPordHeadIds, ++i, count);
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

        protected override PordHeadImportResultSet CreateResultSet(PordHeadRowContext rowContext)
        {
            return new PordHeadImportResultSet
            {
                PordHead = rowContext.PordHead,
                PordLine = rowContext.PordLine,

                LogCol = rowContext.LogCol,
                LogText = rowContext.LogText,
                Row = rowContext.Row
            };
        }

        protected override void CreateEntity(PordHeadRowContext rowContext)
        {
            var alias = rowContext.CurrentTable.Alias;
            if (string.IsNullOrWhiteSpace(alias))
            {
                alias = rowContext.CurrentTable.Table;
            }

            TableEntry entry;

            switch (rowContext.CurrentTable.Table.ToLowerInvariant())
            {
                case "ols_pordhead":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = PordHead.GetSchema(),
                        Entity = PordHead.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.PordHead = entry;
                    break;
                case "ols_pordline":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = PordLine.GetSchema(),
                        Entity = PordLine.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.PordLine = entry;
                    break;
                default:
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                    throw new ArgumentOutOfRangeException(nameof(rowContext.CurrentTable.Table), rowContext.CurrentTable.Table, nameof(this.CreateEntity));
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
            }
        }

        protected override void RemoveEntity(PordHeadRowContext rowContext)
        {
            switch (rowContext.CurrentTable.Table.ToLowerInvariant())
            {
                case "ols_pordhead":
                    rowContext.PordHead = null;
                    break;
                case "ols_pordline":
                    rowContext.PordLine = null;
                    break;
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                    throw new ArgumentOutOfRangeException(nameof(rowContext.CurrentTable.Table), rowContext.CurrentTable.Table, nameof(this.RemoveEntity));
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
            }
        }


        private bool SaveImport(PordHeadImportResultSet result, Dictionary<int, int> partnerPordHeadIds, int? pos = null, int? count = null)
        {
            var partnCode = result.PordHead?.Entity["partnid"]
                            ?? result.PordHead?.Entity["partnid"];

            if (pos != null && count != null)
            {
                var percnt = Math.Round(pos.Value / (count.Value * 1M) * 100, 2, MidpointRounding.AwayFromZero);
                this.logger.Log($"Saving partner [{pos} / {count} ({percnt:#.00}%)]: {partnCode} ");
            }
            else
            {
                this.logger.Log($"Saving partner: {partnCode} ");
            }

            var logText = new StringBuilder(result.LogText);

            string entityType = null;
            try
            {
                using (var db = DB.GetConn(DB.Main, Transaction.Use))
                {
                    entityType = typeof(PordHead).Name;
                    this.SavePordHeadAndPordLine(result, partnerPordHeadIds);
                    
                    db.Commit();

                    this.logger.LogLine("...done");

                    return true;
                }
            }
            catch (eProjectWeb.Framework.Rules.ValidateException ex)
            {
                this.logger.LogLine();
                this.logger.LogErrorLine($"Vevoi rendeles: {partnCode}, {"$entity".eLogTransl()}: {this.GetEntityName(entityType)}");
                var message = this.ToTextValidate(ex);
                this.logger.LogErrorLine(message);

                logText.AppendLine(message);
            }
            catch (Exception ex)
            {
                this.logger.LogLine();
                this.logger.LogErrorLine($"Vevoi rendeles: {partnCode}, {"$entity".eLogTransl()}: {this.GetEntityName(entityType)}");
                var message = ex.Message;
                this.logger.LogErrorLine(message);

                logText.AppendLine(message);
            }

            result.LogText = logText.ToString();

            return false;
        }

        private void SavePordHeadAndPordLine(PordHeadImportResultSet result, Dictionary<int, int> partnerPordHeadIds)
        {
            if (result.PordHead != null)
            {
                var map = new BLObjectMap();
                map.SysParams.ActionID = ActionID.New;

                // PordHead
                var pordhead = PordHead.CreateNew();
                var pordline = PordLine.CreateNew();

                var addrKey = new Key
                {
                    [Base.Masters.Partner.PartnAddr.FieldPartnid.Name] = ConvertUtils.ToInt32(result.PordHead.Entity[PordHead.FieldPartnid.Name]),
                    [Base.Masters.Partner.PartnAddr.FieldDef.Name] = 1,
                };

                pordhead.Cmpid = ConvertUtils.ToInt32(result.PordHead.Entity[PordHead.FieldCmpid.Name]);
                pordhead.Porddocid = result.PordHead.Entity[PordHead.FieldPorddocid.Name]?.ToString();
                pordhead.Porddate = DateTime.Now;
                pordhead.Partnid = ConvertUtils.ToInt32(result.PordHead.Entity[PordHead.FieldPartnid.Name]);

                var defAddr = Base.Masters.Partner.PartnAddr.Load(addrKey);
                if (defAddr != null)
                {
                    pordhead.Addrid = defAddr.Addrid;
                }

                pordhead.Curid = CustomSettings.GetString("PordHeadImportDefaultCurid");
                //pordhead.Paymid = result.PordHead.Entity[PordHead.FieldPaymid.Name]?.ToString();
                pordhead.Ref1 = result.PordHead.Entity[PordHead.FieldRef1.Name]?.ToString();
                pordhead.Note = result.PordHead.Entity[PordHead.FieldNote.Name]?.ToString();

                // ha ezt a fejet mar lementettuk, akkor azt hasznaljuk inkabb
                if (pordhead.Partnid.HasValue)
                {
                    if (partnerPordHeadIds.ContainsKey(pordhead.Partnid.Value))
                    {
                        var sordId = partnerPordHeadIds[pordhead.Partnid.Value];
                        pordhead = PordHead.Load(sordId);
                    }
                }

                map.Default = pordhead;

                // PordLine
                pordline.Reqdate = ConvertUtils.ToDateTime(result.PordLine.Entity[PordLine.FieldReqdate.Name]);
                pordline.Confreqdate = ConvertUtils.ToDateTime(result.PordLine.Entity[PordLine.FieldConfreqdate.Name]);
                pordline.Itemid = ConvertUtils.ToInt32(result.PordLine.Entity[PordLine.FieldItemid.Name]);

                pordline.Ref2 = result.PordLine.Entity[PordLine.FieldRef2.Name].ToString();

                pordline.Note = result.PordLine.Entity[PordLine.FieldNote.Name].ToString();

                if (!pordline.Itemid.HasValue)
                {
                    var ve = new List<ValidationError>
                    {
                        new ValidationError("$error_import_item_not_found")
                    };

                    throw new ValidateException(ve);
                }

                pordline.Ordqty = ConvertUtils.ToInt32(result.PordLine.Entity[PordLine.FieldOrdqty.Name]);
                pordline.Purchprc  = ConvertUtils.ToInt32(result.PordLine.Entity[PordLine.FieldPurchprc.Name]);
                pordline.Confqty = ConvertUtils.ToInt32(result.PordLine.Entity[PordLine.FieldConfqty.Name]);

                //var taxidSql = $@"SELECT tt.taxid FROM ols_taxtrans (nolock) tt
                //                JOIN ols_itemgroup (nolock) itmgrp on itmgrp.taxid = tt.taxid
                //                JOIN ols_item (nolock) itm on itm.itemgrpid = itmgrp.itemgrpid
                //                JOIN ols_sorddoc (nolock) sd on sd.bustypeid = tt.bustypeid
                //                WHERE itm.itemid = {Utils.SqlToString(pordline.Itemid)} AND sd.sorddocid = {Utils.SqlToString(pordhead.Sorddocid)}";

                //pordline.Taxid = SqlDataAdapter.ExecuteSingleValue(DB.Main, taxidSql).ToString();

                map.Add(pordline);

                this.PordHeadBL.Save(map);

                if (pordhead.Partnid.HasValue)
                {
                    if (!partnerPordHeadIds.ContainsKey(pordhead.Partnid.Value))
                    {
                        partnerPordHeadIds.Add(pordhead.Partnid.Value, pordhead.Pordid.Value);
                    }
                }
            }
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

        protected override object DetermineSelfLookupValue(object value, PordHeadRowContext rowContext)
        {
            
            Entity entity = null;

            if (rowContext.CurrentField.Field.Equals(Partner.FieldPartnid.Name))
            {
                var sql = $"SELECT * FROM ols_partner ols LEFT JOIN olc_partner olc on ols.partnid = olc.partnid WHERE ols.partncode = '{Utils.SqlToString(value)}' or olc.oldcode = '{Utils.SqlToString(value)}'";
                var partners = SqlDataAdapter.GetList<Partner>(sql);

                entity = partners.FirstOrDefault();
            }

            if (rowContext.CurrentField.Field.Equals(Item.FieldItemid.Name))
            {
                var sql = $"SELECT * FROM ols_item  WHERE itemcode = {Utils.SqlToString(value)}";

                var items = SqlDataAdapter.GetList<Item>(sql);

                entity = items.FirstOrDefault();
            }

            if (entity != null)
            {
                return entity[rowContext.CurrentField.Lookup.ValueField];
            }
            else
            {
                return null;
            }
        }
    }
}