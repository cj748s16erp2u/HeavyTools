using eLog.Base.Masters.Item;
using eLog.HeavyTools.ImportBase;
using eLog.HeavyTools.ImportBase.ImportResult;
using eLog.HeavyTools.Masters.Partner;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static eLog.HeavyTools.Masters.Partner.PartnerBL3;

namespace eLog.HeavyTools.Masters.Item
{
    public class ItemBL2: ItemBL
    {
        protected override bool PreSave(eProjectWeb.Framework.BL.BLObjectMap objects, eProjectWeb.Framework.Data.Entity e)
        {
            bool b = base.PreSave(objects, e);

            eLog.Base.Masters.Item.Item it = (eLog.Base.Masters.Item.Item)objects.Default;

            if (e is OlcItem)
            {
                OlcItem it2 = (OlcItem)e;
                if (it2.State == eProjectWeb.Framework.Data.DataRowState.Added)
                    it2.Itemid = it.Itemid;
            }
            return b;
        }

        public override void Validate(eProjectWeb.Framework.BL.BLObjectMap objects)
        {
            base.Validate(objects);

            OlcItem it2 = objects[typeof(OlcItem).Name] as OlcItem;
            if (it2 != null)
                eProjectWeb.Framework.RuleServer.Validate(objects, typeof(OlcItemRules));
        }

        ImportBase.ImportUtils utils;

        /// <summary>
        /// Feltöltött Xlsx fájlok importálása
        /// </summary>
        /// <param name="uploadInfo">Feltöltött fájlok tárolója</param>
        public ImportResult ItemImport(eProjectWeb.Framework.UI.Controls.UploadData uploadInfo)
        {
            utils = new ImportBase.ImportUtils();

            string error = null;

            var importDescrFileName = CustomSettings.GetString("ItemImportDescrFileName");
            if (string.IsNullOrWhiteSpace(importDescrFileName))
            {
                error = "$err_partnerimport_missingsettings".eLogTransl("PartnerImportDescrFileName");
            }

            var descrDirs = CustomSettings.GetString("ImportDescrFolders");
            if (string.IsNullOrWhiteSpace(descrDirs))
            {
                error = "$err_partnerimport_missingsettings".eLogTransl("ImportDescrFolders");
            }
            else
            {
                descrDirs = utils.AddSiteRoot(descrDirs);
            }

            if (uploadInfo?.Process == true && string.IsNullOrWhiteSpace(error))
            {
                return this.ImportItemXlsxFiles(uploadInfo.Files, importDescrFileName, descrDirs);
            }
            else if (uploadInfo != null)
            {
                utils.RemoveFiles(uploadInfo.Files);
            }

            if (!string.IsNullOrWhiteSpace(error))
            {
                throw new MessageException(error);
            }

            return null;
        }
        /// <summary>
        /// Feltöltött Xlsx fájlok feldolgozása
        /// </summary>
        /// <param name="uploadFiles">Fájlok listája</param>
        /// <param name="importDescrFileName">Import leíró fájl neve</param>
        /// <param name="descrDirs">Import leíró keresési könyvtárok</param>
        /// <returns>Feldolgozás eredménye</returns>
        private ImportResult ImportItemXlsxFiles(IEnumerable<eProjectWeb.Framework.UI.Controls.UploadFileInfo> uploadFiles, string importDescrFileName, string descrDirs)
        {
            importDescrFileName = eProjectWeb.Framework.Utils.GetFirstFileFromDirs(descrDirs, importDescrFileName);

            var result = new List<ImportFileNames>();
            string resultFileName;

            var importResultFolder = Path.Combine(Globals.WritableRoot, "ParnterImport");
            if (!Directory.Exists(importResultFolder))
            {
                Directory.CreateDirectory(importResultFolder);
            }

            var processResults = new List<ImportProcessResult>();

            using (new eProjectWeb.Framework.Lang.NS(typeof(PartnerBL3).Namespace))
            {
                foreach (var f in uploadFiles)
                {
                    var res = this.ImportItemXlsxFile(importDescrFileName, importResultFolder, f.FileName, f.StoredFileName);
                    if (res.FileNames != null)
                    {
                        result.Add(res.FileNames);
                    }

                    if (res.ImportResult != null)
                    {
                        processResults.Add(res.ImportResult);
                    }
                }
            }

            resultFileName = utils.CreateResultFile(result);

            utils.RemoveTemporaryFiles(result);

            return new ImportResult
            {
                ResultFileName = resultFileName,
                ImportProcessResults = processResults
            };
        }

        /// <summary>
        /// Xlsx fájl feldolgozása, tárolása későbbi elemzés céljából
        /// </summary>
        /// <param name="importDescrFileName">Import leíró fájl neve</param>
        /// <param name="importResultFolder">Végeredményt tároló könyvtár neve</param>
        /// <param name="fileName">Feldolgozandó fájl neve</param>
        /// <param name="storedFileName">Feldolgozandó fájl fizikai neve</param>
        private ImportXlsxResult ImportItemXlsxFile(string importDescrFileName, string importResultFolder, string fileName, string storedFileName)
        {
            var importService = new Import.ItemImportService();
            var sFileName = Path.Combine(Globals.ReportsTempFolder, storedFileName);
            var realFileName = Path.Combine(Globals.ReportsTempFolder, fileName);

            try { File.Move(sFileName, realFileName); }
            catch { realFileName = sFileName; }

            var executeId = Guid.NewGuid();

            try
            {
                Log.Info($"{executeId} - {Session.UserID} executing partner import: {fileName} ({storedFileName})");

                var processResult = importService.Import(importDescrFileName, realFileName);
                var importResult = new ImportProcessResult
                {
                    FileName = fileName,
                    ProcessResult = processResult
                };

                var fileNames = new ImportFileNames
                {
                    RealFileName = realFileName,
                    FileName = fileName,
                    LogFileName = processResult.LogFileName,
                    ErrorFileName = processResult.ErrorFileName,
                    LogXlsxFileName = processResult.LogXlsxFileName,
                };

                var resultFileName = Path.Combine(importResultFolder, $"{Path.GetFileNameWithoutExtension(fileName)}-{DateTime.Now:yyyyMMddHHmmss}.zip");

                utils.ZipResult(fileNames, resultFileName);

                Log.Info($"{executeId} - finished...");

                return new ImportXlsxResult
                {
                    FileNames = fileNames,
                    ImportResult = importResult
                };
            }
            catch (Exception ex)
            {
                Log.Error(executeId, ex);

                return new ImportXlsxResult
                {
                    FileNames = new ImportFileNames
                    {
                        RealFileName = realFileName,
                        FileName = fileName,
                    },
                    ImportResult = new ImportProcessResult
                    {
                        Message = ex.Message
                    }
                };
            }
        }

    }
}
