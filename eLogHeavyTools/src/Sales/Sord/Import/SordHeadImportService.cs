using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eLog.Base.Masters.Item;
using eLog.Base.Masters.Partner;
using eLog.Base.Sales.Sord;
using eLog.Base.Setup.ItemGroup;
using eLog.HeavyTools.ImportBase;
using eLog.HeavyTools.Masters.Partner.Import;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.Lang;
using eProjectWeb.Framework.Rules;

namespace eLog.HeavyTools.Sales.Sord.Import
{
    public class SordHeadImportService : ImportServiceBase<SordHeadImportResultSet, SordHeadImportResultSets, SordHeadRowContext>
    {
        private SordHeadBL sordHeadBL;

        public override ProcessResult Import(string importDescrFileName, string importXlsxFileName)
        {
            return this.Import("SordHead", importDescrFileName, importXlsxFileName);
        }

        protected override int SaveImport(SordHeadImportResultSets results)
        {
            //SordHeadRules.ValidateExtcodeUnique = false;

            this.sordHeadBL = SordHeadBL.New();
            Dictionary<int, int> partnerSordHeadIds = new Dictionary<int, int>();

            var success = 0;

            using (new NS(typeof(Base.Sales.Sord.SordHead).Namespace))
            {
                var count = results.Count();
                var i = 0;
                foreach (var r in results)
                {
                    var ret = this.SaveImport(r, partnerSordHeadIds, ++i, count);
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

        protected override SordHeadImportResultSet CreateResultSet(SordHeadRowContext rowContext)
        {
            return new SordHeadImportResultSet
            {
                SordHead = rowContext.SordHead,
                SordLine = rowContext.SordLine,

                LogCol = rowContext.LogCol,
                LogText = rowContext.LogText,
                Row = rowContext.Row
            };
        }

        protected override void CreateEntity(SordHeadRowContext rowContext)
        {
            var alias = rowContext.CurrentTable.Alias;
            if (string.IsNullOrWhiteSpace(alias))
            {
                alias = rowContext.CurrentTable.Table;
            }

            TableEntry entry;

            switch (rowContext.CurrentTable.Table.ToLowerInvariant())
            {
                case "ols_sordhead":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = SordHead.GetSchema(),
                        Entity = SordHead.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.SordHead = entry;
                    break;
                case "ols_sordline":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = SordLine.GetSchema(),
                        Entity = SordLine.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.SordLine = entry;
                    break;
                default:
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                    throw new ArgumentOutOfRangeException(nameof(rowContext.CurrentTable.Table), rowContext.CurrentTable.Table, nameof(this.CreateEntity));
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
            }
        }

        protected override void RemoveEntity(SordHeadRowContext rowContext)
        {
            switch (rowContext.CurrentTable.Table.ToLowerInvariant())
            {
                case "ols_sordhead":
                    rowContext.SordHead = null;
                    break;
                case "ols_sordline":
                    rowContext.SordLine = null;
                    break;
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                    throw new ArgumentOutOfRangeException(nameof(rowContext.CurrentTable.Table), rowContext.CurrentTable.Table, nameof(this.RemoveEntity));
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
            }
        }


        private bool SaveImport(SordHeadImportResultSet result, Dictionary<int, int> partnerSordHeadIds, int? pos = null, int? count = null)
        {
            var partnCode = result.SordHead?.Entity["partnid"]
                            ?? result.SordHead?.Entity["partnid"];

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
                    entityType = typeof(SordHead).Name;
                    this.SaveSordHeadAndSordLine(result, partnerSordHeadIds);
                    
                    db.Commit();

                    this.logger.LogLine("...done");

                    return true;
                }
            }
            catch (eProjectWeb.Framework.Rules.ValidateException ex)
            {
                this.logger.LogLine();
                this.logger.LogErrorLine($"Vevoi rendeles: {partnCode}, {"$entity".eLogTransl()}: {this.GetEntityName(entityType)}");
                var message = ToTextValidate(ex);
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

        private void SaveSordHeadAndSordLine(SordHeadImportResultSet result, Dictionary<int, int> partnerSordHeadIds)
        {
            if (result.SordHead != null)
            {
                var map = new BLObjectMap();
                map.SysParams.ActionID = ActionID.New;

                // sordhead
                var sordHead = SordHead.CreateNew();
                var sordLine = SordLine.CreateNew();

                map.Default = sordHead;

                var addrKey = new Key
                {
                    [Base.Masters.Partner.PartnAddr.FieldPartnid.Name] = ConvertUtils.ToInt32(result.SordHead.Entity[SordHead.FieldPartnid.Name]),
                    [Base.Masters.Partner.PartnAddr.FieldDef.Name] = 1,
                };

                sordHead.Cmpid = ConvertUtils.ToInt32(result.SordHead.Entity[SordHead.FieldCmpid.Name]);
                sordHead.Sorddocid = result.SordHead.Entity[SordHead.FieldSorddocid.Name]?.ToString();
                sordHead.Sorddate = DateTime.Now;
                sordHead.Partnid = ConvertUtils.ToInt32(result.SordHead.Entity[SordHead.FieldPartnid.Name]);

                var defAddr = Base.Masters.Partner.PartnAddr.Load(addrKey);
                if (defAddr != null)
                {
                    sordHead.Addrid = defAddr.Addrid;
                }

                sordHead.Curid = CustomSettings.GetString("SordheadImportDefaultCurid");
                sordHead.Paymid = result.SordHead.Entity[SordHead.FieldPaymid.Name]?.ToString();
                sordHead.Ref1 = result.SordHead.Entity[SordHead.FieldRef1.Name]?.ToString();
                sordHead.Note = result.SordHead.Entity[SordHead.FieldNote.Name]?.ToString();

                // ha ezt a fejet mar lementettuk, akkor azt hasznaljuk inkabb
                if (sordHead.Partnid.HasValue)
                {
                    if (partnerSordHeadIds.ContainsKey(sordHead.Partnid.Value))
                    {
                        var sordId = partnerSordHeadIds[sordHead.Partnid.Value];
                        sordHead = SordHead.Load(sordId);
                    }
                }

                // sordline
                sordLine.Reqdate = new DateTime(2022, 12, 01);
                sordLine.Itemid = ConvertUtils.ToInt32(result.SordLine.Entity[SordLine.FieldItemid.Name]);

                if (!sordLine.Itemid.HasValue)
                {
                    var ve = new List<ValidationError>
                    {
                        new ValidationError("$error_import_item_not_found")
                    };

                    throw new ValidateException(ve);
                }

                sordLine.Ordqty = ConvertUtils.ToInt32(result.SordLine.Entity[SordLine.FieldOrdqty.Name]);
                sordLine.Selprc = ConvertUtils.ToInt32(result.SordLine.Entity[SordLine.FieldSelprc.Name]);

                var taxidSql = $@"SELECT tt.taxid FROM ols_taxtrans (nolock) tt
                                JOIN ols_itemgroup (nolock) itmgrp on itmgrp.taxid = tt.taxid
                                JOIN ols_item (nolock) itm on itm.itemgrpid = itmgrp.itemgrpid
                                JOIN ols_sorddoc (nolock) sd on sd.bustypeid = tt.bustypeid
                                WHERE itm.itemid = {Utils.SqlToString(sordLine.Itemid)} AND sd.sorddocid = '{Utils.SqlToString(sordHead.Sorddocid)}'";

                sordLine.Taxid = SqlDataAdapter.ExecuteSingleValue(DB.Main, taxidSql).ToString();

                map.Add(sordLine);

                this.sordHeadBL.Save(map);

                if (sordHead.Partnid.HasValue)
                {
                    if (!partnerSordHeadIds.ContainsKey(sordHead.Partnid.Value))
                    {
                        partnerSordHeadIds.Add(sordHead.Partnid.Value, sordHead.Sordid.Value);
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

        protected override object DetermineSelfLookupValue(object value, SordHeadRowContext rowContext)
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