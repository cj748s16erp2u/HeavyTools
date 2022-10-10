using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eLog.Base.Masters.Item;
using eLog.Base.Masters.Partner;
using eLog.Base.Sales.Sord;
using eLog.Base.Setup.Company;
using eLog.Base.Setup.ItemGroup;
using eLog.HeavyTools.ImportBase;
using eLog.HeavyTools.Masters.Partner;
using eLog.HeavyTools.Masters.Partner.Import;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.Lang;
using eProjectWeb.Framework.Rules;
using eProjectWeb.Framework.UI.Script;

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
            List<SordHeadImportUnique> savedSordHeads = new List<SordHeadImportUnique>();

            var success = 0;

            using (new NS(typeof(Base.Sales.Sord.SordHead).Namespace))
            {
                var count = results.Count();
                var i = 0;
                foreach (var r in results)
                {
                    var ret = this.SaveImport(r, savedSordHeads, ++i, count);
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


        private bool SaveImport(SordHeadImportResultSet result, List<SordHeadImportUnique> savedSordHeads, int? pos = null, int? count = null)
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
                    this.SaveSordHeadAndSordLine(result, savedSordHeads);
                    
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

        private void SaveSordHeadAndSordLine(SordHeadImportResultSet result, List<SordHeadImportUnique> savedSordHeads)
        {
            if (result.SordHead != null)
            {
                var map = new BLObjectMap();
                map.SysParams.ActionID = ActionID.New;

                // sordhead
                var sordHead = SordHead.CreateNew();
                var sordLine = SordLine.CreateNew();
                
                sordHead.Cmpid = ConvertUtils.ToInt32(result.SordHead.Entity[SordHead.FieldCmpid.Name]);
                sordHead.Sorddocid = result.SordHead.Entity[SordHead.FieldSorddocid.Name]?.ToString();
                sordHead.Sorddate = DateTime.Now;
                sordHead.Partnid = ConvertUtils.ToInt32(result.SordHead.Entity[SordHead.FieldPartnid.Name]);
                
                // get the default payment details for the partner if it's not included in the excel
                var paymid = result.SordHead.Entity[SordHead.FieldPaymid.Name]?.ToString();
                if (string.IsNullOrWhiteSpace(paymid))
                {
                    paymid = SqlDataAdapter.ExecuteSingleValue(DB.Main, $"SELECT paymid FROM ols_partncmp WHERE partnid = {Utils.SqlToString(sordHead.Partnid)} AND cmpid = {Utils.SqlToString(sordHead.Cmpid)}")?.ToString();
                    sordHead.Paycid = ConvertUtils.ToInt32(SqlDataAdapter.ExecuteSingleValue(DB.Main, $"SELECT paycid FROM ols_partncmp WHERE partnid = {Utils.SqlToString(sordHead.Partnid)} AND cmpid = {Utils.SqlToString(sordHead.Cmpid)}"));
                }

                // alapertelmezett cim kivalasztasa, ha nem jott be excelbol
                if (!sordHead.Addrid.HasValue)
                {
                    // partnerhez tartozo alapertelmezett cim megadasa ha van
                    var addrKey = new Key
                    {
                        [Base.Masters.Partner.PartnAddr.FieldPartnid.Name] =
                            ConvertUtils.ToInt32(result.SordHead.Entity[SordHead.FieldPartnid.Name]),
                        [Base.Masters.Partner.PartnAddr.FieldDef.Name] = 1,
                    };

                    var defAddr = Base.Masters.Partner.PartnAddr.Load(addrKey);
                    if (defAddr != null)
                    {
                        sordHead.Addrid = defAddr.Addrid;
                    }
                }

                if (!sordHead.Addrid.HasValue)
                {
                    var ve = new List<ValidationError>
                    {
                        new ValidationError("$error_import_address_not_found")
                    };

                    throw new ValidateException(ve);
                }

                if (string.IsNullOrWhiteSpace(result.SordHead.Entity[SordHead.FieldCurid.Name]?.ToString()))
                {
                    sordHead.Curid = CustomSettings.GetString("SordheadImportDefaultCurid");
                }
                else
                {
                    sordHead.Curid = result.SordHead.Entity[SordHead.FieldCurid.Name].ToString();
                }

                sordHead.Paymid = paymid;
                sordHead.Ref1 = result.SordHead.Entity[SordHead.FieldRef1.Name]?.ToString();
                sordHead.Note = result.SordHead.Entity[SordHead.FieldNote.Name]?.ToString();

                // ha ezt a fejet mar lementettuk, akkor azt hasznaljuk inkabb
                var uniqueSh = new SordHeadImportUnique(sordHead);
                if (sordHead.Partnid.HasValue)
                {
                    if (savedSordHeads.Contains(uniqueSh))
                    {
                        var savedBefore = savedSordHeads.First(x => x.Equals(uniqueSh));
                        sordHead = SordHead.Load(savedBefore.SordHeadId);
                    }
                }

                map.Default = sordHead;

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

                // csak egesz szam lehet
                var validationErrors = new List<ValidationError>();
                if (!sordLine.Ordqty.ToString().Equals(result.SordLine.Entity[SordLine.FieldOrdqty.Name].ToString()))
                {
                    validationErrors.Add(new ValidationError("$error_import_ordqty_notint"));
                }

                if (!sordLine.Selprc.ToString().Equals(result.SordLine.Entity[SordLine.FieldSelprc.Name].ToString()))
                {
                    validationErrors.Add(new ValidationError("$error_import_selprc_notint"));
                }

                if (validationErrors.Count > 0)
                {
                    throw new ValidateException(validationErrors);
                }

                var taxidSql = $@"SELECT tt.taxid FROM ols_taxtrans (nolock) tt
                                JOIN ols_itemgroup (nolock) itmgrp on itmgrp.taxid = tt.taxid
                                JOIN ols_item (nolock) itm on itm.itemgrpid = itmgrp.itemgrpid
                                JOIN ols_sorddoc (nolock) sd on sd.bustypeid = tt.bustypeid
                                WHERE itm.itemid = {Utils.SqlToString(sordLine.Itemid)} AND sd.sorddocid = {Utils.SqlToString(sordHead.Sorddocid)}";

                sordLine.Taxid = SqlDataAdapter.ExecuteSingleValue(DB.Main, taxidSql).ToString();

                map.Add(sordLine);

                this.sordHeadBL.Save(map);

                if (savedSordHeads.All(x => !x.Equals(uniqueSh)))
                {
                    uniqueSh.SordHeadId = sordHead.Sordid ?? 0;
                    savedSordHeads.Add(uniqueSh);
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
            var stringVal = value?.ToString(); // nem tudom, hogy int, vagy string, vagy mi es az Utils.SqlToString tipushoz merten general sql-t

            if (rowContext.CurrentField.Field.Equals(Partner.FieldPartnid.Name))
            {
                var sql = $"SELECT * FROM ols_partner ols LEFT JOIN olc_partner olc on ols.partnid = olc.partnid WHERE ols.partncode = {Utils.SqlToString(stringVal)} or olc.oldcode = {Utils.SqlToString(stringVal)}";
                var partners = SqlDataAdapter.GetList<Partner>(sql);

                entity = partners.FirstOrDefault();

                if (entity == null)
                {
                    var ve = new List<ValidationError>
                    {
                        new ValidationError("$error_import_partner_not_found", stringVal)
                    };

                    throw new ValidateException(ve);
                }
            }else if (rowContext.CurrentField.Field.Equals(Item.FieldItemid.Name))
            {
                var sql = $"SELECT * FROM ols_item  WHERE itemcode = {Utils.SqlToString(stringVal)}";

                var items = SqlDataAdapter.GetList<Item>(sql);

                entity = items.FirstOrDefault();

                if (entity == null)
                {
                    var ve = new List<ValidationError>
                    {
                        new ValidationError("$error_import_item_not_found", stringVal)
                    };

                    throw new ValidateException(ve);
                }
            }
            else if (rowContext.CurrentField.Field.Equals(Partner.FieldCmpid.Name))
            {
                if (string.IsNullOrWhiteSpace(stringVal))
                {
                    stringVal = CustomSettings.GetString("SordheadImportDefaultCompany");
                }

                if (!string.IsNullOrWhiteSpace(stringVal))
                {
                    var sql = $"SELECT * FROM ols_company WHERE cmpcode = {Utils.SqlToString(stringVal)}";
                    var companies = SqlDataAdapter.GetList<Company>(sql);

                    entity = companies.FirstOrDefault();
                }

                if (entity == null)
                {
                    var ve = new List<ValidationError>
                    {
                        new ValidationError("$error_import_company_not_found", stringVal)
                    };

                    throw new ValidateException(ve);
                }
            } else if (rowContext.CurrentField.Field.Equals(SordHead.FieldAddrid.Name))
            {
                if (!string.IsNullOrWhiteSpace(stringVal))
                {
                    var opaQuery = $"SELECT * FROM olc_partnaddr opa (nolock) WHERE opa.oldcode = {Utils.SqlToString(stringVal)}";
                    var opaResult = SqlDataAdapter.GetList<OlcPartnAddr>(opaQuery);

                    if (opaResult != null && opaResult.Count > 0)
                    {
                        var opa = opaResult.FirstOrDefault();
                        entity = opa;
                    }
                }
            }

            if (entity != null)
            {
                return entity[rowContext.CurrentField.Lookup.ValueField];
            }

            return null;
        }
    }
}