using System;
using System.Collections.Generic;
using System.IO;
using eLog.Base.Purchase.Pord;
using eLog.HeavyTools.ImportBase.ImportResult;
using eLog.HeavyTools.Purchase.Pord.Import;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.UI.Controls;

namespace eLog.HeavyTools.Purchase.Pord
{
    public class PordHeadBL3 : PordHeadBL
    {
        ImportBase.ImportUtils utils;

        protected override bool PreSave(BLObjectMap objects, Entity e)
        {
            if (e is OlcPordHead olc)
            {
                var ols = objects.Get<PordHead>();

                olc.Pordid = ols.Pordid;
            }

            return base.PreSave(objects, e);
        }

        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);

            var olc = objects.Get<OlcPordHead>();
            if (olc != null)
            {
                RuleServer.Validate<OlcPordHead, OlcPordHeadRules>(objects);
            }
        }

        public override void Delete(Key k)
        {
            var olc = OlcPordHead.Load(k);
            if (olc != null)
            {
                olc.State = DataRowState.Deleted;
                olc.Save();
            }

            base.Delete(k);
        }

        public ImportResult PordHeadImport(UploadData uploadInfo)
        {
            utils = new ImportBase.ImportUtils();
            string error = null;

            var importDescrFileName = CustomSettings.GetString("PordHeadImportDescrFileName");
            if (string.IsNullOrWhiteSpace(importDescrFileName))
            {
                error = "$err_pordheadimport_missingsettings".eLogTransl("PordHeadImportDescrFileName");
            }

            var descrDirs = CustomSettings.GetString("ImportDescrFolders");
            if (string.IsNullOrWhiteSpace(descrDirs))
            {
                error = "$err_pordheadimport_missingsettings".eLogTransl("ImportDescrFolders");
            }
            else
            {
                descrDirs = utils.AddSiteRoot(descrDirs);
            }

            if (uploadInfo?.Process == true && string.IsNullOrWhiteSpace(error))
            {
                return this.ImportPartnerXlsxFiles(uploadInfo.Files, importDescrFileName, descrDirs);
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
        private ImportResult ImportPartnerXlsxFiles(IEnumerable<eProjectWeb.Framework.UI.Controls.UploadFileInfo> uploadFiles, string importDescrFileName, string descrDirs)
        {
            importDescrFileName = eProjectWeb.Framework.Utils.GetFirstFileFromDirs(descrDirs, importDescrFileName);

            var result = new List<ImportFileNames>();
            string resultFileName;

            var importResultFolder = Path.Combine(Globals.WritableRoot, "PordHeadImport");
            if (!Directory.Exists(importResultFolder))
            {
                Directory.CreateDirectory(importResultFolder);
            }

            var processResults = new List<ImportProcessResult>();

            using (new eProjectWeb.Framework.Lang.NS(typeof(PordHeadBL3).Namespace))
            {
                foreach (var f in uploadFiles)
                {
                    var res = this.ImportPartnerXlsxFile(importDescrFileName, importResultFolder, f.FileName, f.StoredFileName);
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
        private ImportXlsxResult ImportPartnerXlsxFile(string importDescrFileName, string importResultFolder, string fileName, string storedFileName)
        {
            var importService = new PordHeadImportService();
            var sFileName = Path.Combine(Globals.ReportsTempFolder, storedFileName);
            var realFileName = Path.Combine(Globals.ReportsTempFolder, fileName);

            try { File.Move(sFileName, realFileName); }
            catch { realFileName = sFileName; }

            var executeId = Guid.NewGuid();

            try
            {
                Log.Info($"{executeId} - {Session.UserID} executing pordhead import: {fileName} ({storedFileName})");

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