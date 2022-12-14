using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Masters.Partner;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.PageParts;

namespace eLog.HeavyTools.Masters.Import.Partner
{
    public class PartnerImportTab : eProjectWeb.Framework.UI.PageParts.TabPage2
    {
        public static PartnerImportTab New()
        {
            var t = eProjectWeb.Framework.ObjectFactory.New<PartnerImportTab>();
            t.Initialize("partnerimport");
            return t;
        }

        protected PartnerImportTab() : base() { }

        protected UploadButton partnerImportButton;
        protected DialogBox dlgSimpleMessage;

        protected override void Initialize(string labelID)
        {
            base.Initialize(labelID);

            this.CreateControls();
        }

        protected override void CreateControls()
        {
            base.CreateControls();

            this.partnerImportButton = this.AddCmd(new UploadButton("partnerimport", 950));
            this.SetButtonAction(this.partnerImportButton.ID, this.PartnerImportButtonClicked);

            this.dlgSimpleMessage = new DialogBox(DialogBoxType.Ok);
            this.RegisterDialog(this.dlgSimpleMessage);
        }

        private void PartnerImportButtonClicked(PageUpdateArgs args)
        {
            var uploadInfo = this.partnerImportButton.GetUploadData(args);

            var partnerBL = PartnerBL3.New3();
            var processResult = partnerBL.PartnerImport(uploadInfo);

            var message = this.FormatImportResult(processResult?.ImportProcessResults);
            if (!string.IsNullOrWhiteSpace(message))
            {
                args.ShowDialog(this.dlgSimpleMessage, "$import_title", message);
            }

            var fileName = processResult?.ResultFileName;
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var ext = System.IO.Path.GetExtension(fileName);

                var partnerImportTransl = "$partnerimport".eLogTransl();
                var realFileName = $"{partnerImportTransl}_{DateTime.Now:yyyyMMddHhmmss}";
                realFileName = $"{realFileName}{ext}";

                fileName = System.IO.Path.GetFileName(fileName);
                realFileName = System.Web.HttpUtility.UrlEncode(realFileName);

                if (string.Equals(ext, ".txt", StringComparison.InvariantCultureIgnoreCase))
                {
                    var sett = new eProjectWeb.Framework.UI.Script.OpenPopupSettings
                    {
                        ServicePrefix = "Services/DownloadService.ashx",
                        RealFileName = realFileName,
                        StoredFileName = fileName,
                        ContentType = "text/plain",
                    };

                    args.AddExecCommand(new eProjectWeb.Framework.UI.Script.OpenPopupStep(sett));
                }
                else
                {
                    var sett = new eProjectWeb.Framework.UI.Script.DownloadSettings
                    {
                        ServicePrefix = "Services/DownloadService.ashx",
                        RealFileName = realFileName,
                        StoredFileName = fileName,
                        ContentType = string.Equals(ext, ".xlsx", StringComparison.InvariantCultureIgnoreCase)
                            ? "application/vnd.ms-excel"
                            : "application/zip",
                    };

                    args.AddExecCommand(new eProjectWeb.Framework.UI.Script.DownloadStep(sett));
                }
            }
        }

        private string FormatImportResult(IEnumerable<ImportBase.ImportResult.ImportProcessResult> result)
        {
            if (result == null)
            {
                return null;
            }

            var texts = new List<string>();

            foreach (var r in result)
            {
                var importFileName = r.FileName;
                var importResult = r.ProcessResult;

                var msgs = new List<string>();
                if (importResult != null)
                {
                    if (importResult.TotalRows > importResult.ProcessedRows)
                    {
                        msgs.Add("$importresult_processedrows".eLogTransl(importResult.TotalRows, importResult.ProcessedRows));
                    }

                    if (importResult.ProcessedRows > importResult.SavedRows)
                    {
                        msgs.Add("$importresult_savedrows".eLogTransl(importResult.ProcessedRows, importResult.SavedRows));
                    }
                }

                if (!string.IsNullOrWhiteSpace(r.Message))
                {
                    msgs.Add(r.Message);
                }

                string msg;
                if (msgs.Count > 1)
                {
                    msg = string.Join("", msgs.Select(m => $"<li>{m}</li>"));
                    msg = $"<ul>{msg}</ul>";
                }
                else
                {
                    msg = msgs.FirstOrDefault() ?? "$importresult_ok".eLogTransl(importResult?.SavedRows);
                }

                msg = "$importresult_message".eLogTransl(importFileName, msg);

                texts.Add(msg);
            }

            var ret = string.Join("", texts.Select(t => $"<li>{t}</li>"));
            if (!string.IsNullOrWhiteSpace(ret))
            {
                return $"<div><ul>{ret}</ul></div>";
            }

            return "OK";
        }
    }
}
