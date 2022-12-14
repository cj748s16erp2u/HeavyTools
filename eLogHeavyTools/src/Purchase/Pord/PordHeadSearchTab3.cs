using System;
using System.Collections.Generic;
using System.Linq;
using eLog.Base.Purchase.Pord;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.PageParts;

namespace eLog.HeavyTools.Purchase.Pord
{
    public class PordHeadSearchTab3 : PordHeadSearchTab, eProjectWeb.Framework.Xml.IXmlObjectName
    {
        #region IXmlObjectName
        protected static Type baseType = typeof(PordHeadSearchTab);

        public override string GetNamespaceName()
        {
            return baseType.Namespace;
        }

        protected override string GetPageXmlFileName()
        {
            return $"{this.GetNamespaceName()}.{this.XmlObjectName}";
        }

        public string XmlObjectName => baseType.Name;
        #endregion

        protected UploadButton pordHeadImportButton;
        protected DialogBox dlgSimpleMessage3;

        protected override void CreateControls()
        {
            base.CreateControls();

            this.pordHeadImportButton = this.AddCmd(new UploadButton("pordheadimport", 950));
            this.SetButtonAction(this.pordHeadImportButton.ID, this.PordHeadImportButtonClicked);

            this.dlgSimpleMessage3 = new DialogBox(DialogBoxType.Ok);
            this.RegisterDialog(this.dlgSimpleMessage3);
        }

        private void PordHeadImportButtonClicked(PageUpdateArgs args)
        {
            var uploadInfo = this.pordHeadImportButton.GetUploadData(args);

            var pordHeadBL = (PordHeadBL3)PordHeadBL.New();
            var processResult = pordHeadBL.PordHeadImport(uploadInfo);

            var message = this.FormatImportResult(processResult?.ImportProcessResults);
            if (!string.IsNullOrWhiteSpace(message))
            {
                args.ShowDialog(this.dlgSimpleMessage3, "$import_title", message);
            }

            var fileName = processResult?.ResultFileName;
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var ext = System.IO.Path.GetExtension(fileName);

                var pordHeadImportTransl = "$pordheadimport".eLogTransl();
                var realFileName = $"{pordHeadImportTransl}_{DateTime.Now:yyyyMMddHhmmss}";
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