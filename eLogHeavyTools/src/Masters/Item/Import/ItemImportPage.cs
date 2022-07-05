using eLog.HeavyTools.ImportBase;
using eLog.HeavyTools.ImportBase.ImportResult;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.PageParts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eLog.HeavyTools.Masters.Item.Import
{
    class ItemImportPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(ItemImportPage).FullName;
         
        public ItemImportPage()
            : base("ItemImport")
        {
            Tabs.AddTab(delegate { return ItemImportTab.New(); });
        }
    }
    internal class ItemImportTab : TabPage2
    {
        protected DialogBox dlgSimpleMessage;

        public static ItemImportTab New()
        {
            var t = (ItemImportTab)ObjectFactory.New(typeof(ItemImportTab));
            t.Initialize("ItemImport");
            return t;
        }
        protected override void Initialize(string labelID)
        {
            base.Initialize(labelID);
            CreateControls();
            itemImportButton = this.AddCmd(new UploadButton("itemimport", 950));
            this.SetButtonAction(this.itemImportButton.ID, this.OnImport);
            dlgSimpleMessage = new DialogBox(DialogBoxType.Ok);
            RegisterDialog(dlgSimpleMessage);
        }

        UploadButton itemImportButton;
        private void OnImport(PageUpdateArgs args)
        {
            var uploadInfo = this.itemImportButton.GetUploadData(args);

            var itemBL = (ItemBL3)ItemBL3.New();
            var processResult = itemBL.ItemImport(uploadInfo);

            var message = this.FormatImportResult(processResult?.ImportProcessResults);
            if (!string.IsNullOrWhiteSpace(message))
            {
                args.ShowDialog(this.dlgSimpleMessage, "$import_title", message);
            }

            var fileName = processResult?.ResultFileName;
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var ext = System.IO.Path.GetExtension(fileName);

                var itemImportTransl = "$itemimport".eLogTransl();
                var realFileName = $"{itemImportTransl}_{DateTime.Now:yyyyMMddHhmmss}";
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

        private string FormatImportResult(IEnumerable<ImportProcessResult> result)
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
