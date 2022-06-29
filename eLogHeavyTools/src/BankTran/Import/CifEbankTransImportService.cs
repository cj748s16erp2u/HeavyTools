using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.ImportBase;
using eLog.HeavyTools.ImportBase.Xlsx;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.Lang;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace eLog.HeavyTools.BankTran.Import
{
    internal class CifEbankTransImportService : ImportServiceBase<CifEbankTransImportResultSet, CifEbankTransImportResultSets, CifEbankTransRowContext>
    {
        private U4Ext.Bank.Base.Transaction.CifEbankTransBL cifEbankBL;

        private IDictionary<int, int> addrIdTranslateDict = new Dictionary<int, int>();

        public class ImportData
        {
            public U4Ext.Bank.Base.Transaction.CifEbankTrans cifTrans { get; set; }
        }

        public CifEbankTransImportService() : base()
        {
        }

        public override ProcessResult Import(string importDescrFileName, string importXlsxFileName)
        {
            return this.Import("CifEbankTrans", importDescrFileName, importXlsxFileName);
            
        }

        protected override int SaveImport(CifEbankTransImportResultSets results)
        {
            this.cifEbankBL = U4Ext.Bank.Base.Transaction.CifEbankTransBL.New();

            var success = 0;

            using (new NS(typeof(U4Ext.Bank.Base.Transaction.CifEbankTrans).Namespace))
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

        private bool SaveImport(CifEbankTransImportResultSet result, int? pos = null, int? count = null)
        {

            //var partnCode = result.OlcPartner?.Entity["oldcode"]
            //    ?? result.Partner?.Entity["partnid"]
            //    ?? result.PartnAddrs?.FirstOrDefault()?.Entity["partnid"]
            //    ?? result.PartnBanks?.FirstOrDefault()?.Entity["partnid"]
            //    ?? result.Employees?.FirstOrDefault()?.Entity["partnid"];

            var partnCode = "PPA";

            if (pos != null && count != null)
            {
                var percnt = Math.Round(pos.Value / (count.Value * 1M) * 100, 2, MidpointRounding.AwayFromZero);
                this.logger.Log($"Saving cif_ebank_trans [{pos} / {count} ({percnt:#.00}%)]: {partnCode} ");
            }
            else
            {
                this.logger.Log($"Saving cif_ebank_trans: {partnCode} ");
            }

            var logText = new StringBuilder(result.LogText);

            string entityType = null;
            try
            {
                using (var db = DB.GetConn(DB.Main, Transaction.Use))
                {
                    entityType = typeof(U4Ext.Bank.Base.Transaction.CifEbankTrans).Name;
                    this.SaveCifEbankTrans(result);

                    //db.Commit();

                    this.logger.LogLine("...done");

                    return true;
                }
            }
            catch (eProjectWeb.Framework.Rules.ValidateException ex)
            {
                this.logger.LogLine();
                this.logger.LogErrorLine($"Partner: {partnCode}, {"$entity".eLogTransl()}: {this.GetEntityName(entityType)}");
                var message = ToTextValidate(ex);
                this.logger.LogErrorLine(message);

                logText.AppendLine(message);
            }
            catch (Exception ex)
            {
                this.logger.LogLine();
                this.logger.LogErrorLine($"Partner: {partnCode}, {"$entity".eLogTransl()}: {this.GetEntityName(entityType)}");
                var message = ex.Message;
                this.logger.LogErrorLine(message);

                logText.AppendLine(message);
            }

            result.LogText = logText.ToString();

            return false;
        }

        private bool? SaveCifEbankTrans(CifEbankTransImportResultSet result)
        {
            if (result.CifEbankTrans != null)
            { 
            }

            return null;
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

        protected override CifEbankTransImportResultSet CreateResultSet(CifEbankTransRowContext rowContext)
        {
            return new CifEbankTransImportResultSet
            {
                CifEbankTrans = rowContext.CifEbankTrans,

                LogText = rowContext.LogText,
                Row = rowContext.Row,
                LogCol = rowContext.LogCol,
            };
        }

        protected override void CreateEntity(CifEbankTransRowContext rowContext)
        {
            var alias = rowContext.CurrentTable.Alias;
            if (string.IsNullOrWhiteSpace(alias))
            {
                alias = rowContext.CurrentTable.Table;
            }

            TableEntry entry;
            switch (rowContext.CurrentTable.Table.ToLowerInvariant())
            {
                case "cif_ebank_trans":
                    entry = new TableEntry
                    {
                        Alias = alias,
                        Table = rowContext.CurrentTable,
                        Schema = U4Ext.Bank.Base.Transaction.CifEbankTrans.GetSchema(),
                        Entity = U4Ext.Bank.Base.Transaction.CifEbankTrans.CreateNew()
                    };
                    rowContext.CurrentEntry = entry;
                    rowContext.CifEbankTrans = entry;
                    break;

                default:
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                    throw new ArgumentOutOfRangeException(nameof(rowContext.CurrentTable.Table), rowContext.CurrentTable.Table, nameof(this.CreateEntity));
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
            }
        }

        protected override void RemoveEntity(CifEbankTransRowContext rowContext)
        {
            switch (rowContext.CurrentTable.Table.ToLowerInvariant())
            {
                case "cif_ebank_trans":
                    rowContext.CifEbankTrans = null;
                    break;

                default:
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                    throw new ArgumentOutOfRangeException(nameof(rowContext.CurrentTable.Table), rowContext.CurrentTable.Table, nameof(this.RemoveEntity));
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
            }
        }

    }
}
