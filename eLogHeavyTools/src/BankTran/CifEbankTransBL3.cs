using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodaInt.Base.Masters.Partner;
using eLog.Base.Masters.Partner;
using eLog.HeavyTools.ImportBase;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.Lang;
using eProjectWeb.Framework.Package;
using U4Ext.Bank.Base.Transaction;

namespace eLog.HeavyTools.BankTran
{
    public class CifEbankTransBL3 : U4Ext.Bank.Base.Transaction.CifEbankTransBL
    {
        public const int CONST_InvalidImportBankType = 61; // 61: import not possible for larger type

        public static CifEbankTransBL3 New3()
        {
            return (CifEbankTransBL3)New();
        }

        #region FoxPost HUF kivonat import
        /// <summary>
        /// Feltöltött Xlsx fájlok importálása
        /// </summary>
        /// <param name="uploadInfo">Feltöltött fájlok tárolója</param>
        /// <param name="cifTransId">Kivonat tétel id, amihez beolvassuk az állományt</param>
        public ImportResult FoxPostBankStatementImport(eProjectWeb.Framework.UI.Controls.UploadData uploadInfo, int? cifTransId)
        {
            string error = null;

            var importDescrFileName = CustomSettings.GetString("FoxPostBankStatementImportDescrFileName");
            if (string.IsNullOrWhiteSpace(importDescrFileName))
            {
                error = "$err_bankstamentimport_missingsettings".eLogTransl("BankStatementImportDescrFileName");
            }

            var descrDirs = CustomSettings.GetString("ImportDescrFolders");
            if (string.IsNullOrWhiteSpace(descrDirs))
            {
                error = "$err_bankstatementimport_missingsettings".eLogTransl("ImportDescrFolders");
            }
            else
            {
                descrDirs = this.AddSiteRoot(descrDirs);
            }

            if (uploadInfo?.Process == true && string.IsNullOrWhiteSpace(error))
            {
                return this.FoxPostBankStatementImportXlsxFiles(uploadInfo.Files, importDescrFileName, descrDirs, cifTransId);
            }
            else if (uploadInfo != null)
            {
                this.RemoveFiles(uploadInfo.Files);
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
        /// <param name="cifTransId">Kivonat tétel id, amihez beolvassuk az állományt</param>
        /// <returns>Feldolgozás eredménye</returns>
        private ImportResult FoxPostBankStatementImportXlsxFiles(IEnumerable<eProjectWeb.Framework.UI.Controls.UploadFileInfo> uploadFiles, string importDescrFileName, string descrDirs, int? cifTransId)
        {
            importDescrFileName = Utils.GetFirstFileFromDirs(descrDirs, importDescrFileName);

            var result = new List<ImportFileNames>();
            string resultFileName;

            var importResultFolder = Path.Combine(Globals.WritableRoot, "FoxPostBankStatementImport");
            if (!Directory.Exists(importResultFolder))
            {
                Directory.CreateDirectory(importResultFolder);
            }

            var processResults = new List<ImportProcessResult>();

            using (new eProjectWeb.Framework.Lang.NS(typeof(CifEbankTransBL3).Namespace))
            {
                foreach (var f in uploadFiles)
                {
                    var res = this.FoxPostBankStatementImportXlsxFiles(importDescrFileName, importResultFolder, f.FileName, f.StoredFileName, cifTransId);
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

            resultFileName = this.CreateResultFile(result);

            this.RemoveTemporaryFiles(result);

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
        /// <param name="cifTransId">Kivonat tétel id, amihez beolvassuk az állományt</param>
        /// <param name="storedFileName">Feldolgozandó fájl fizikai neve</param>
        private ImportXlsxResult FoxPostBankStatementImportXlsxFiles(string importDescrFileName, string importResultFolder, string fileName, string storedFileName, int? cifTransId)
        {
            var importService = new Import.CifEbankTransImportService();
            if (importService.cifTrans == null)
                importService.cifTrans = U4Ext.Bank.Base.Transaction.CifEbankTrans.Load(cifTransId);
            if (importService.importFileName == null)
                importService.importFileName = fileName;

            var sFileName = Path.Combine(Globals.ReportsTempFolder, storedFileName);
            var realFileName = Path.Combine(Globals.ReportsTempFolder, fileName);

            try { File.Move(sFileName, realFileName); }
            catch { realFileName = sFileName; }

            var executeId = Guid.NewGuid();

            try
            {
                Log.Info($"{executeId} - {Session.UserID} executing FOXPOST import: {fileName} ({storedFileName})");

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

                this.ZipResult(fileNames, resultFileName);

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
        #endregion FoxPost HUF kivonat import

        #region GLS HUF kivonat import
        /// <summary>
        /// Feltöltött Xlsx fájlok importálása
        /// </summary>
        /// <param name="uploadInfo">Feltöltött fájlok tárolója</param>
        /// <param name="cifTransId">Kivonat tétel id, amihez beolvassuk az állományt</param>
        public ImportResult GLSHUFBankStatementImport(eProjectWeb.Framework.UI.Controls.UploadData uploadInfo, int? cifTransId)
        {
            string error = null;

            var importDescrFileName = CustomSettings.GetString("GLSHUFBankStatementImportDescrFileName");
            if (string.IsNullOrWhiteSpace(importDescrFileName))
            {
                error = "$err_bankstamentimport_missingsettings".eLogTransl("BankStatementImportDescrFileName");
            }

            var descrDirs = CustomSettings.GetString("ImportDescrFolders");
            if (string.IsNullOrWhiteSpace(descrDirs))
            {
                error = "$err_bankstatementimport_missingsettings".eLogTransl("ImportDescrFolders");
            }
            else
            {
                descrDirs = this.AddSiteRoot(descrDirs);
            }

            if (uploadInfo?.Process == true && string.IsNullOrWhiteSpace(error))
            {
                return this.GLSHUFBankStatementImportXlsxFiles(uploadInfo.Files, importDescrFileName, descrDirs, cifTransId);
            }
            else if (uploadInfo != null)
            {
                this.RemoveFiles(uploadInfo.Files);
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
        /// <param name="cifTransId">Kivonat tétel id, amihez beolvassuk az állományt</param>
        /// <returns>Feldolgozás eredménye</returns>
        private ImportResult GLSHUFBankStatementImportXlsxFiles(IEnumerable<eProjectWeb.Framework.UI.Controls.UploadFileInfo> uploadFiles, string importDescrFileName, string descrDirs, int? cifTransId)
        {
            importDescrFileName = Utils.GetFirstFileFromDirs(descrDirs, importDescrFileName);

            var result = new List<ImportFileNames>();
            string resultFileName;

            var importResultFolder = Path.Combine(Globals.WritableRoot, "GLSBankStatementImport");
            if (!Directory.Exists(importResultFolder))
            {
                Directory.CreateDirectory(importResultFolder);
            }

            var processResults = new List<ImportProcessResult>();

            using (new eProjectWeb.Framework.Lang.NS(typeof(CifEbankTransBL3).Namespace))
            {
                foreach (var f in uploadFiles)
                {
                    var res = this.GLSHUFBankStatementImportXlsxFiles(importDescrFileName, importResultFolder, f.FileName, f.StoredFileName, cifTransId);
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

            resultFileName = this.CreateResultFile(result);

            this.RemoveTemporaryFiles(result);

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
        /// <param name="cifTransId">Kivonat tétel id, amihez beolvassuk az állományt</param>
        /// <param name="storedFileName">Feldolgozandó fájl fizikai neve</param>
        private ImportXlsxResult GLSHUFBankStatementImportXlsxFiles(string importDescrFileName, string importResultFolder, string fileName, string storedFileName, int? cifTransId)
        {
            var importService = new Import.CifEbankTransImportService();
            //var importData = new Import.CifEbankTransImportService.ImportData();
            //if (importData.cifTrans == null)
            //    importData.cifTrans = U4Ext.Bank.Base.Transaction.CifEbankTrans.Load(cifTransId);
            var sFileName = Path.Combine(Globals.ReportsTempFolder, storedFileName);
            var realFileName = Path.Combine(Globals.ReportsTempFolder, fileName);

            try { File.Move(sFileName, realFileName); }
            catch { realFileName = sFileName; }

            var executeId = Guid.NewGuid();

            try
            {
                Log.Info($"{executeId} - {Session.UserID} executing GLS HUF import: {fileName} ({storedFileName})");

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

                this.ZipResult(fileNames, resultFileName);

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
        #endregion GLS HUF kivonat import

        #region GLS EUR kivonat import
        /// <summary>
        /// Feltöltött Xlsx fájlok importálása
        /// </summary>
        /// <param name="uploadInfo">Feltöltött fájlok tárolója</param>
        /// <param name="cifTransId">Kivonat tétel id, amihez beolvassuk az állományt</param>
        public ImportResult GLSEURBankStatementImport(eProjectWeb.Framework.UI.Controls.UploadData uploadInfo, int? cifTransId)
        {
            string error = null;

            var importDescrFileName = CustomSettings.GetString("GLSEURBankStatementImportDescrFileName");
            if (string.IsNullOrWhiteSpace(importDescrFileName))
            {
                error = "$err_bankstamentimport_missingsettings".eLogTransl("BankStatementImportDescrFileName");
            }

            var descrDirs = CustomSettings.GetString("ImportDescrFolders");
            if (string.IsNullOrWhiteSpace(descrDirs))
            {
                error = "$err_bankstatementimport_missingsettings".eLogTransl("ImportDescrFolders");
            }
            else
            {
                descrDirs = this.AddSiteRoot(descrDirs);
            }

            if (uploadInfo?.Process == true && string.IsNullOrWhiteSpace(error))
            {
                return this.GLSEURBankStatementImportXlsxFiles(uploadInfo.Files, importDescrFileName, descrDirs, cifTransId);
            }
            else if (uploadInfo != null)
            {
                this.RemoveFiles(uploadInfo.Files);
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
        /// <param name="cifTransId">Kivonat tétel id, amihez beolvassuk az állományt</param>
        /// <returns>Feldolgozás eredménye</returns>
        private ImportResult GLSEURBankStatementImportXlsxFiles(IEnumerable<eProjectWeb.Framework.UI.Controls.UploadFileInfo> uploadFiles, string importDescrFileName, string descrDirs, int? cifTransId)
        {
            importDescrFileName = Utils.GetFirstFileFromDirs(descrDirs, importDescrFileName);

            var result = new List<ImportFileNames>();
            string resultFileName;

            var importResultFolder = Path.Combine(Globals.WritableRoot, "GLSBankStatementImport");
            if (!Directory.Exists(importResultFolder))
            {
                Directory.CreateDirectory(importResultFolder);
            }

            var processResults = new List<ImportProcessResult>();

            using (new eProjectWeb.Framework.Lang.NS(typeof(CifEbankTransBL3).Namespace))
            {
                foreach (var f in uploadFiles)
                {
                    var res = this.GLSEURBankStatementImportXlsxFiles(importDescrFileName, importResultFolder, f.FileName, f.StoredFileName, cifTransId);
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

            resultFileName = this.CreateResultFile(result);

            this.RemoveTemporaryFiles(result);

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
        /// <param name="cifTransId">Kivonat tétel id, amihez beolvassuk az állományt</param>
        /// <param name="storedFileName">Feldolgozandó fájl fizikai neve</param>
        private ImportXlsxResult GLSEURBankStatementImportXlsxFiles(string importDescrFileName, string importResultFolder, string fileName, string storedFileName, int? cifTransId)
        {
            var importService = new Import.CifEbankTransImportService();
            //var importData = new Import.CifEbankTransImportService.ImportData();
            //if (importData.cifTrans == null)
            //    importData.cifTrans = U4Ext.Bank.Base.Transaction.CifEbankTrans.Load(cifTransId);
            var sFileName = Path.Combine(Globals.ReportsTempFolder, storedFileName);
            var realFileName = Path.Combine(Globals.ReportsTempFolder, fileName);

            try { File.Move(sFileName, realFileName); }
            catch { realFileName = sFileName; }

            var executeId = Guid.NewGuid();

            try
            {
                Log.Info($"{executeId} - {Session.UserID} executing GLS EUR import: {fileName} ({storedFileName})");

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

                this.ZipResult(fileNames, resultFileName);

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
        #endregion GLS EUR kivonat import

        /// <summary>
        /// Feltöltött Xlsx fájlok törlése
        /// </summary>
        /// <param name="uploadFiles">Fájlok listája</param>
        private void RemoveFiles(IEnumerable<eProjectWeb.Framework.UI.Controls.UploadFileInfo> uploadFiles)
        {
            foreach (var f in uploadFiles)
            {
                var storedFileName = Path.Combine(Globals.ReportsTempFolder, f.StoredFileName);
                try { File.Delete(storedFileName); }
                catch (Exception ex) { Log.Error(ex); }
            }
        }

        /// <summary>
        /// Feldolgozás eredmény fájl létrehozása (zip-be tömörítés)
        /// </summary>
        /// <param name="result">Feldolgozás eredmények</param>
        private string CreateResultFile(IEnumerable<ImportFileNames> result)
        {
            string resultFileName;
            var resultFileInfo = this.GetResultFileInfo(result);

            var tempFileName = GetTempFileName();
            if (resultFileInfo?.Count() > 1)
            {
                resultFileName = Path.Combine(Globals.ReportsTempFolder, $"{tempFileName}.zip");
                this.ZipResult(resultFileInfo, resultFileName);
            }
            else
            {
                resultFileName = resultFileInfo.FirstOrDefault();
                tempFileName = Path.Combine(Globals.ReportsTempFolder, $"{tempFileName}{Path.GetExtension(resultFileName)}");
                try
                {
                    File.Move(resultFileName, tempFileName);
                    resultFileName = tempFileName;
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }

            return resultFileName;
        }

        /// <summary>
        /// Feldolgozás során létrejök fájlok törlése
        /// </summary>
        /// <param name="result">Feldolgozás eredmények</param>
        private void RemoveTemporaryFiles(IEnumerable<ImportFileNames> result)
        {
            foreach (var f in result)
            {
                try
                {
                    var storedFileName = Path.Combine(Globals.ReportsTempFolder, f.RealFileName);
                    if (File.Exists(storedFileName))
                    {
                        File.Delete(storedFileName);
                    }

                    if (File.Exists(f.LogFileName))
                    {
                        File.Delete(f.LogFileName);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
        }

        /// <summary>
        /// A feldolgozási folyamatok által létrejött hiba fájlok kigyűjtése 1 listába
        /// </summary>
        private IEnumerable<string> GetResultFileInfo(IEnumerable<ImportFileNames> result)
        {
            if (result == null)
            {
                return Enumerable.Empty<string>();
            }

            var fileList = new List<string>();

            foreach (var r in result)
            {
                if (!string.IsNullOrWhiteSpace(r.LogXlsxFileName) && File.Exists(r.LogXlsxFileName))
                {
                    var importFileName = Path.GetFileName(r.FileName);
                    var logXlsxFileName = Path.Combine(Globals.ReportsTempFolder, $"{Path.GetFileNameWithoutExtension(importFileName)}_result.xlsx");
                    try { File.Move(r.LogXlsxFileName, logXlsxFileName); }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                        logXlsxFileName = r.LogXlsxFileName;
                    }

                    fileList.Add(logXlsxFileName);
                }
                else if (!string.IsNullOrWhiteSpace(r.ErrorFileName))
                {
                    var fileInfo = new FileInfo(r.ErrorFileName);
                    if (fileInfo.Exists && fileInfo.Length > 3)
                    {
                        var importFileName = Path.GetFileName(r.FileName);
                        var errFileName = Path.Combine(Globals.ReportsTempFolder, $"{Path.GetFileNameWithoutExtension(importFileName)}.txt");
                        try { File.Move(r.ErrorFileName, errFileName); }
                        catch (Exception ex)
                        {
                            Log.Error(ex);
                            errFileName = r.ErrorFileName;
                        }

                        fileList.Add(errFileName);
                    }
                }
            }

            return fileList;
        }

        /// <summary>
        /// A paraméterben megadott könyvtárhoz hozzáadja a SiteRoot könyvtárat
        /// </summary>
        private string AddSiteRoot(string descrDirs)
        {
            var descrDirsList = descrDirs.Split(new[] { ';' });
            for (var i = 0; i < descrDirsList.Length; i++)
            {
                var l = descrDirsList[i];
                if (!Path.IsPathRooted(l))
                {
                    l = Path.Combine(Globals.SiteRoot, l);
                }

                descrDirsList[i] = l;
            }

            descrDirs = string.Join(";", descrDirsList);

            return descrDirs;
        }

        /// <summary>
        /// A feldolgozási folyamat során létrejött hiba fájlok tömörítése 1 zip fájlba
        /// </summary>
        /// <param name="result">Fájlok listája</param>
        /// <param name="targetFileName">Zip fájl neve</param>
        private void ZipResult(IEnumerable<string> result, string targetFileName)
        {
            if (result?.Any() != true)
            {
                return;
            }

            var packager = PackageUtils.GetPackage(PackageUtils.ZipPackageType, "ZipPackager");
            if (packager == null)
            {
                throw new PackageException("ZipPackager is not found!");
            }

            var entries = new List<PackageEntry>();
            foreach (var m in result)
            {
                var fileName = m;
                entries.Add(new PackageEntry(fileName, m));
            }

            packager.Compress(targetFileName, entries);

            foreach (var m in result)
            {
                var fileName = m;
                try { File.Delete(fileName); }
                catch (Exception ex) { Log.Error(ex); }
            }
        }

        /// <summary>
        /// Feldolgozás során létrejött fájlokat 1 zip-be tömöríti
        /// </summary>
        /// <param name="result">Fájl nevek tárolója</param>
        /// <param name="targetFileName">Zip fájl neve</param>
        private void ZipResult(ImportFileNames result, string targetFileName)
        {
            if (result == null)
            {
                return;
            }

            var packager = PackageUtils.GetPackage(PackageUtils.ZipPackageType, "ZipPackager");
            if (packager == null)
            {
                throw new PackageException("ZipPackager is not found!");
            }

            var entries = new List<PackageEntry>();

            var importFileName = Path.GetFileName(result.FileName);
            var storedFileName = Path.Combine(Globals.ReportsTempFolder, result.RealFileName);
            if (!string.IsNullOrWhiteSpace(storedFileName))
            {
                entries.Add(new PackageEntry(storedFileName, importFileName));
            }

            var logFileName = $"{Path.GetFileNameWithoutExtension(importFileName)}_log.txt";
            if (!string.IsNullOrWhiteSpace(result.LogFileName))
            {
                entries.Add(new PackageEntry(result.LogFileName, logFileName));
            }

            var errFileName = $"{Path.GetFileNameWithoutExtension(importFileName)}.txt";
            if (!string.IsNullOrWhiteSpace(result.ErrorFileName))
            {
                entries.Add(new PackageEntry(result.ErrorFileName, errFileName));
            }

            var logXlsxFileName = $"{Path.GetFileNameWithoutExtension(importFileName)}_result.xlsx";
            if (!string.IsNullOrWhiteSpace(result.LogXlsxFileName))
            {
                entries.Add(new PackageEntry(result.LogXlsxFileName, logXlsxFileName));
            }

            packager.Compress(targetFileName, entries);
        }

        /// <summary>
        /// Ideiglenes fájl név előállítás
        /// </summary>
        private static string GetTempFileName()
        {
            var rnd = new Random();
            var s4 = new Func<string>(() => Convert.ToInt64(Math.Floor((1 + rnd.NextDouble()) * 0x10000)).ToString("x"));
            return s4() + s4();
        }

        /// <summary>
        /// Feldolgozási folyamat eredmény tároló
        /// </summary>
        public class ImportProcessResult
        {
            /// <summary>
            /// Feldolgozott fájl neve
            /// </summary>
            public string FileName { get; internal set; }
            /// <summary>
            /// Feldolgozási folyamat eredmény tároló
            /// </summary>
            public ProcessResult ProcessResult { get; internal set; }
            /// <summary>
            /// Hibaüzenetek
            /// </summary>
            public string Message { get; internal set; }
        }

        /// <summary>
        /// Import során feldolgozott és létrejött fájl nevek tárolója
        /// </summary>
        protected class ImportFileNames
        {
            /// <summary>
            /// Feldolgozandó fájl neve
            /// </summary>
            public string RealFileName { get; internal set; }
            /// <summary>
            /// Fájl neve
            /// </summary>
            public string FileName { get; internal set; }
            /// <summary>
            /// Feldolgozási folyamat log fájl neve
            /// </summary>
            public string LogFileName { get; internal set; }
            /// <summary>
            /// Hiba fájl neve
            /// </summary>
            public string ErrorFileName { get; internal set; }
            /// <summary>
            /// Feldolgozás eredményét tartalmazó Xlsx fájl neve
            /// </summary>
            public string LogXlsxFileName { get; internal set; }
        }

        /// <summary>
        /// Feldolgozási folyamat eredmény tároló
        /// </summary>
        public class ImportResult
        {
            /// <summary>
            /// Feldolgozás eredményeként létrejött, letöltendő fájl neve
            /// </summary>
            public string ResultFileName { get; set; }
            /// <summary>
            /// Feldolgozási folyamat eredménye
            /// </summary>
            public IEnumerable<ImportProcessResult> ImportProcessResults { get; set; }
        }

        /// <summary>
        /// Xlsx import eredmény és létrejött fájlok nevei
        /// </summary>
        protected class ImportXlsxResult
        {
            /// <summary>
            /// Feldolgozási folyamat eredmény tároló
            /// </summary>
            public ImportProcessResult ImportResult { get; internal set; }
            /// <summary>
            /// Import során feldolgozott és létrejött fájl nevek tárolója
            /// </summary>
            public ImportFileNames FileNames { get; internal set; }
        }

        public virtual string ImportRecords(int? cifTransId, string importText, System.Globalization.NumberFormatInfo numberFormat, int? fieldListId, out List<Key> generatedRecords)
        {
            generatedRecords = null;
            List<string> wrongLines = null;
            var list = new List<CifEbankTrans>();

            var parentCifTrans = U4Ext.Bank.Base.Transaction.CifEbankTrans.Load(cifTransId);
            if (parentCifTrans == null)
            {
                throw new MessageException("$import_exception_ciftransmising");
            }

            if (!fieldListId.HasValue)
                throw new MessageException("$import_exception_fieldsunknown");

            string[] fieldList = ImportRecordsGetFields(fieldListId.Value);
            if (fieldList == null || fieldList.Length == 0)
                throw new MessageException("$import_exception_fieldsinvalid");

            var k = new Key(U4Ext.Bank.Base.Setup.Bank.EfxBank.FieldInterfaceid.Name, parentCifTrans.Interfaceid);
            U4Ext.Bank.Base.Setup.Bank.EfxBank b = U4Ext.Bank.Base.Setup.Bank.EfxBank.Load(k);
            if (b.Banktype > CONST_InvalidImportBankType)
                throw new MessageException("$import_exception_banktypeinvalid");

            using (var ns = new eProjectWeb.Framework.Lang.NS(typeof(CifEbankTransBL3).Namespace))
            // create transaction
            using (var db = DB.GetConn(DB.Main, Transaction.Use))
            {
                if (cifTransId.HasValue)
                {
                    var origCifTrans = CifEbankTrans.Load(cifTransId);

                    Dictionary<CifEbankTrans, string> imported;

                    wrongLines = ProcessImportLines(fieldListId, importText, numberFormat, origCifTrans, ref list, out imported);

                    BeforeSaveImportedLines(origCifTrans, ref list, imported);

                    generatedRecords = SaveImportedLines(list, imported, wrongLines);

                    if (wrongLines.Count() == 0)
                    {
                        var bankHeadList = U4Ext.Bank.Base.Common.U4ExtSqlFunctions.CreateEfxBankTranHeadFromCifTrans();
                        db.Commit();
                    }
                    else
                    {
                        db.Rollback();

                        generatedRecords = null;

                        // returns the unprocessable lines
                        return string.Join("\n", wrongLines.ToArray());
                    }
                }

                return importText;
            }
        }

        protected virtual List<string> ProcessImportLines(int? fieldListId, string importText, System.Globalization.NumberFormatInfo numberFormat, CifEbankTrans origCifTrans, ref List<CifEbankTrans> list, out Dictionary<CifEbankTrans, string> imported)
        {
            CifEbankTrans newCifTrans = null;
            imported = new Dictionary<CifEbankTrans, string>();
            var wrongLines = new List<string>();

            var impLines = importText.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            ImportCifEbankData importCifData = new ImportCifEbankData();
            importCifData.origCifTrans = origCifTrans;
            importCifData.extRef1 = origCifTrans.Extref1;
            importCifData.extRef2 = origCifTrans.Extref2;
            importCifData.origCur = origCifTrans.Origcur;
            importCifData.origValue = origCifTrans.Origvalue;
            importCifData.valueAcc = origCifTrans.Valueacc;
            importCifData.statementId = origCifTrans.Id;

            // process each line
            foreach (var il in impLines)
            {
                string wrongLine = null;
                switch (fieldListId)
                {
                    case 1:
                        wrongLine = ImportGLS(il, fieldListId, numberFormat, importCifData, out newCifTrans);
                        break;
                    case 2:
                        wrongLine = ImportFoxPost(il, fieldListId, numberFormat, importCifData, out newCifTrans);
                        break;
                    case 3:
                        wrongLine = ImportPPP(il, fieldListId, numberFormat, importCifData, out newCifTrans);
                        break;
                    case 4:
                        wrongLine = ImportHervis(il, fieldListId, numberFormat, importCifData, out newCifTrans);
                        break;
                    case 5:
                        wrongLine = ImportInterSport(il, fieldListId, numberFormat, importCifData, out newCifTrans);
                        break;
                    default:
                        throw new eProjectWeb.Framework.MessageException("$import_invalidtype", fieldListId);
                }
                if (!string.IsNullOrEmpty(wrongLine))
                {
                    wrongLines.Add(wrongLine);
                }
                if (newCifTrans != null)
                {
                    list.Add(newCifTrans);
                    imported.Add(newCifTrans, il);
                }
            }

            return wrongLines;
        }

        protected virtual string ImportGLS(string importLine, int? fieldListId, System.Globalization.NumberFormatInfo numberFormat, ImportCifEbankData importCifData, out CifEbankTrans newCifTrans)
        {
            newCifTrans = null;

            string[] parts = importLine.Split(new char[] { '\t' });

            if (parts.Length != 3)
            {
                return importLine + "\t" + eProjectWeb.Framework.Lang.Translator.Translate("$import_error_numberofcolumns");
            }

            string[] fieldList = ImportRecordsGetFields(fieldListId.Value);
            int? ref1NameFieldIndex = null;
            int? ref2NameFieldIndex = null;
            int? origvalueFieldIndex = null;
            int? origcurFieldIndex = null;
            int? valueaccFieldIndex = null;

            int idx = 0;
            foreach (string f in fieldList)
            {
                if (f.Equals("ref1", StringComparison.OrdinalIgnoreCase))
                    ref1NameFieldIndex = idx;
                if (f.Equals("ref2", StringComparison.OrdinalIgnoreCase))
                    ref2NameFieldIndex = idx;
                if (f.Equals("origvalue", StringComparison.OrdinalIgnoreCase))
                    origvalueFieldIndex = idx;
                if (f.Equals("origcur", StringComparison.OrdinalIgnoreCase))
                    origcurFieldIndex = idx;
                if (f.Equals("valueacc", StringComparison.OrdinalIgnoreCase))
                    valueaccFieldIndex = idx;
                idx++;
            }

            if (!ref1NameFieldIndex.HasValue)
                throw new MessageException("$import_exception_ref1expected");

            if (!origvalueFieldIndex.HasValue)
                throw new MessageException("$import_exception_origvalueexpected");

            if (!origcurFieldIndex.HasValue)
                throw new MessageException("$import_exception_origcurexpected");

            var interfaceId = CustomSettings.GetString("GLSBankStatementImportInterfaceId");
            if (string.IsNullOrWhiteSpace(interfaceId))
            {
                throw new MessageException("$import_exception_interfaceid_missing");
            }

            string error = null;
            decimal decOrigValue = 0.0M;

            string dVal = parts[1].ToString();
            if (string.IsNullOrEmpty(dVal))
            {
                return importLine + "\t" + eProjectWeb.Framework.Lang.Translator.Translate("$import_error_missingamount");
            }
            else
            {
                if (!decimal.TryParse(dVal, System.Globalization.NumberStyles.Any, numberFormat, out decOrigValue))
                {
                    return importLine + "\t" + eProjectWeb.Framework.Lang.Translator.Translate("$import_error_wrongvalue");
                }
            }

            importCifData.interfaceId = interfaceId;
            importCifData.fileId = interfaceId + "_" + DateTime.Now.ToString();
            importCifData.extRef1 = parts[0].ToString().Length > 32 ? parts[0].ToString().Substring(0, 32) : parts[0].ToString();
            importCifData.origValue = decOrigValue;
            importCifData.origCur = parts[2].ToString().Length > 3 ? parts[2].ToString().Substring(0, 3) : parts[2].ToString();

            if (importCifData.origCifTrans.Origcur != importCifData.origCur)
            {
                return importLine + "\t" + eProjectWeb.Framework.Lang.Translator.Translate("$import_error_wrongcurrency");
            }

            error = ImportLine(importCifData, out newCifTrans);

            if (!string.IsNullOrEmpty(error))
            {
                return importLine + "\t" + error;
            }
            return string.Empty;
        }

        protected virtual string ImportFoxPost(string importLine, int? fieldListId, System.Globalization.NumberFormatInfo numberFormat, ImportCifEbankData importCifData, out CifEbankTrans newCifTrans)
        {
            newCifTrans = null;

            string[] parts = importLine.Split(new char[] { '\t' });

            if (parts.Length != 3)
            {
                return importLine + "\t" + eProjectWeb.Framework.Lang.Translator.Translate("$import_error_numberofcolumns");
            }

            string[] fieldList = ImportRecordsGetFields(fieldListId.Value);
            int? ref1NameFieldIndex = null;
            int? ref2NameFieldIndex = null;
            int? origvalueFieldIndex = null;
            int? origcurFieldIndex = null;
            int? valueaccFieldIndex = null;

            int idx = 0;
            foreach (string f in fieldList)
            {
                if (f.Equals("ref1", StringComparison.OrdinalIgnoreCase))
                    ref1NameFieldIndex = idx;
                if (f.Equals("ref2", StringComparison.OrdinalIgnoreCase))
                    ref2NameFieldIndex = idx;
                if (f.Equals("origvalue", StringComparison.OrdinalIgnoreCase))
                    origvalueFieldIndex = idx;
                if (f.Equals("origcur", StringComparison.OrdinalIgnoreCase))
                    origcurFieldIndex = idx;
                if (f.Equals("valueacc", StringComparison.OrdinalIgnoreCase))
                    valueaccFieldIndex = idx;
                idx++;
            }

            if (!ref1NameFieldIndex.HasValue)
                throw new MessageException("$import_exception_ref1expected");

            if (!ref2NameFieldIndex.HasValue)
                throw new MessageException("$import_exception_ref2expected");

            if (!origvalueFieldIndex.HasValue)
                throw new MessageException("$import_exception_origvalueexpected");

            var interfaceId = CustomSettings.GetString("FoxPostBankStatementImportInterfaceId");
            if (string.IsNullOrWhiteSpace(interfaceId))
            {
                throw new MessageException("$import_exception_interfaceid_missing");
            }

            string error = null;
            decimal decOrigValue = 0.0M;

            string dVal = parts[2].ToString();
            if (string.IsNullOrEmpty(dVal))
            {
                return importLine + "\t" + eProjectWeb.Framework.Lang.Translator.Translate("$import_error_missingamount");
            }
            else
            {
                if (!decimal.TryParse(dVal, System.Globalization.NumberStyles.Any, numberFormat, out decOrigValue))
                {
                    return importLine + "\t" + eProjectWeb.Framework.Lang.Translator.Translate("$import_error_wrongvalue");
                }
            }

            importCifData.interfaceId = interfaceId;
            importCifData.fileId = interfaceId + "_" + DateTime.Now.ToString();
            importCifData.extRef1 = parts[0].ToString().Length > 32 ? parts[0].ToString().Substring(0, 32) : parts[0].ToString();
            importCifData.extRef2 = parts[1].ToString().Length > 32 ? parts[1].ToString().Substring(0, 32) : parts[1].ToString();
            importCifData.origValue = decOrigValue;

            error = ImportLine(importCifData, out newCifTrans);

            if (!string.IsNullOrEmpty(error))
            {
                return importLine + "\t" + error;
            }
            return string.Empty;
        }

        protected virtual string ImportPPP(string importLine, int? fieldListId, System.Globalization.NumberFormatInfo numberFormat, ImportCifEbankData importCifData, out CifEbankTrans newCifTrans)
        {
            newCifTrans = null;

            string[] parts = importLine.Split(new char[] { '\t' });

            if (parts.Length != 3)
            {
                return importLine + "\t" + eProjectWeb.Framework.Lang.Translator.Translate("$import_error_numberofcolumns");
            }

            string[] fieldList = ImportRecordsGetFields(fieldListId.Value);
            int? ref1NameFieldIndex = null;
            int? ref2NameFieldIndex = null;
            int? origvalueFieldIndex = null;
            int? origcurFieldIndex = null;
            int? valueaccFieldIndex = null;

            int idx = 0;
            foreach (string f in fieldList)
            {
                if (f.Equals("ref1", StringComparison.OrdinalIgnoreCase))
                    ref1NameFieldIndex = idx;
                if (f.Equals("ref2", StringComparison.OrdinalIgnoreCase))
                    ref2NameFieldIndex = idx;
                if (f.Equals("origvalue", StringComparison.OrdinalIgnoreCase))
                    origvalueFieldIndex = idx;
                if (f.Equals("origcur", StringComparison.OrdinalIgnoreCase))
                    origcurFieldIndex = idx;
                if (f.Equals("valueacc", StringComparison.OrdinalIgnoreCase))
                    valueaccFieldIndex = idx;
                idx++;
            }

            if (!ref1NameFieldIndex.HasValue)
                throw new MessageException("$import_exception_ref1expected");

            if (!origvalueFieldIndex.HasValue)
                throw new MessageException("$import_exception_origvalueexpected");

            if (!origcurFieldIndex.HasValue)
                throw new MessageException("$import_exception_origcurexpected");

            var interfaceId = CustomSettings.GetString("SprinterBankStatementImportInterfaceId");
            if (string.IsNullOrWhiteSpace(interfaceId))
            {
                throw new MessageException("$import_exception_interfaceid_missing");
            }

            string error = null;
            decimal decOrigValue = 0.0M;

            string dVal = parts[1].ToString();
            if (string.IsNullOrEmpty(dVal))
            {
                return importLine + "\t" + eProjectWeb.Framework.Lang.Translator.Translate("$import_error_missingamount");
            }
            else
            {
                if (!decimal.TryParse(dVal, System.Globalization.NumberStyles.Any, numberFormat, out decOrigValue))
                {
                    return importLine + "\t" + eProjectWeb.Framework.Lang.Translator.Translate("$import_error_wrongvalue");
                }
            }

            importCifData.interfaceId = interfaceId;
            importCifData.fileId = interfaceId + "_" + DateTime.Now.ToString();
            importCifData.extRef1 = parts[0].ToString().Length > 32 ? parts[0].ToString().Substring(0, 32) : parts[0].ToString();
            importCifData.origValue = decOrigValue;
            importCifData.origCur = parts[2].ToString().Length > 3 ? parts[2].ToString().Substring(0, 3) : parts[2].ToString();

            if (importCifData.origCifTrans.Origcur != importCifData.origCur)
            {
                return importLine + "\t" + eProjectWeb.Framework.Lang.Translator.Translate("$import_error_wrongcurrency");
            }

            error = ImportLine(importCifData, out newCifTrans);

            if (!string.IsNullOrEmpty(error))
            {
                return importLine + "\t" + error;
            }
            return string.Empty;
        }

        protected virtual string ImportHervis(string importLine, int? fieldListId, System.Globalization.NumberFormatInfo numberFormat, ImportCifEbankData importCifData, out CifEbankTrans newCifTrans)
        {
            newCifTrans = null;

            string[] parts = importLine.Split(new char[] { '\t' });

            if (parts.Length != 3)
            {
                return importLine + "\t" + eProjectWeb.Framework.Lang.Translator.Translate("$import_error_numberofcolumns");
            }

            string[] fieldList = ImportRecordsGetFields(fieldListId.Value);
            int? ref1NameFieldIndex = null;
            int? ref2NameFieldIndex = null;
            int? origvalueFieldIndex = null;
            int? origcurFieldIndex = null;
            int? valueaccFieldIndex = null;

            int idx = 0;
            foreach (string f in fieldList)
            {
                if (f.Equals("ref1", StringComparison.OrdinalIgnoreCase))
                    ref1NameFieldIndex = idx;
                if (f.Equals("ref2", StringComparison.OrdinalIgnoreCase))
                    ref2NameFieldIndex = idx;
                if (f.Equals("origvalue", StringComparison.OrdinalIgnoreCase))
                    origvalueFieldIndex = idx;
                if (f.Equals("origcur", StringComparison.OrdinalIgnoreCase))
                    origcurFieldIndex = idx;
                if (f.Equals("valueacc", StringComparison.OrdinalIgnoreCase))
                    valueaccFieldIndex = idx;
                idx++;
            }

            if (!ref1NameFieldIndex.HasValue)
                throw new MessageException("$import_exception_ref1expected");

            if (!valueaccFieldIndex.HasValue)
                throw new MessageException("$import_exception_valueaccexpected");

            if (!origvalueFieldIndex.HasValue)
                throw new MessageException("$import_exception_origvalueexpected");

            var interfaceId = CustomSettings.GetString("HervisBankStatementImportInterfaceId");
            if (string.IsNullOrWhiteSpace(interfaceId))
            {
                throw new MessageException("$import_exception_interfaceid_missing");
            }

            string error = null;
            decimal decOrigValue = 0.0M;
            decimal decValueAcc = 0.0M;

            string dVal = parts[1].ToString();
            if (string.IsNullOrEmpty(dVal))
            {
                return importLine + "\t" + eProjectWeb.Framework.Lang.Translator.Translate("$import_error_missingamount");
            }
            else
            {
                if (!decimal.TryParse(dVal, System.Globalization.NumberStyles.Any, numberFormat, out decValueAcc))
                {
                    return importLine + "\t" + eProjectWeb.Framework.Lang.Translator.Translate("$import_error_wrongvalue");
                }
            }

            dVal = parts[2].ToString();
            if (string.IsNullOrEmpty(dVal))
            {
                return importLine + "\t" + eProjectWeb.Framework.Lang.Translator.Translate("$import_error_missingamount");
            }
            else
            {
                if (!decimal.TryParse(dVal, System.Globalization.NumberStyles.Any, numberFormat, out decOrigValue))
                {
                    return importLine + "\t" + eProjectWeb.Framework.Lang.Translator.Translate("$import_error_wrongvalue");
                }
            }

            importCifData.interfaceId = interfaceId;
            importCifData.fileId = interfaceId + "_" + DateTime.Now.ToString();
            importCifData.extRef1 = parts[0].ToString().Length > 32 ? parts[0].ToString().Substring(0, 32) : parts[0].ToString();
            importCifData.valueAcc = decValueAcc;
            importCifData.origValue = decOrigValue;

            error = ImportLine(importCifData, out newCifTrans);

            if (!string.IsNullOrEmpty(error))
            {
                return importLine + "\t" + error;
            }
            return string.Empty;
        }

        protected virtual string ImportInterSport(string importLine, int? fieldListId, System.Globalization.NumberFormatInfo numberFormat, ImportCifEbankData importCifData, out CifEbankTrans newCifTrans)
        {
            newCifTrans = null;

            string[] parts = importLine.Split(new char[] { '\t' });

            if (parts.Length != 3)
            {
                return importLine + "\t" + eProjectWeb.Framework.Lang.Translator.Translate("$import_error_numberofcolumns");
            }

            string[] fieldList = ImportRecordsGetFields(fieldListId.Value);
            int? ref1NameFieldIndex = null;
            int? ref2NameFieldIndex = null;
            int? origvalueFieldIndex = null;
            int? origcurFieldIndex = null;
            int? valueaccFieldIndex = null;

            int idx = 0;
            foreach (string f in fieldList)
            {
                if (f.Equals("ref1", StringComparison.OrdinalIgnoreCase))
                    ref1NameFieldIndex = idx;
                if (f.Equals("ref2", StringComparison.OrdinalIgnoreCase))
                    ref2NameFieldIndex = idx;
                if (f.Equals("origvalue", StringComparison.OrdinalIgnoreCase))
                    origvalueFieldIndex = idx;
                if (f.Equals("origcur", StringComparison.OrdinalIgnoreCase))
                    origcurFieldIndex = idx;
                if (f.Equals("valueacc", StringComparison.OrdinalIgnoreCase))
                    valueaccFieldIndex = idx;
                idx++;
            }

            if (!ref1NameFieldIndex.HasValue)
                throw new MessageException("$import_exception_ref1expected");

            if (!valueaccFieldIndex.HasValue)
                throw new MessageException("$import_exception_valueaccexpected");

            if (!origvalueFieldIndex.HasValue)
                throw new MessageException("$import_exception_origvalueexpected");

            var interfaceId = CustomSettings.GetString("InterSportBankStatementImportInterfaceId");
            if (string.IsNullOrWhiteSpace(interfaceId))
            {
                throw new MessageException("$import_exception_interfaceid_missing");
            }

            string error = null;
            decimal decOrigValue = 0.0M;
            decimal decValueAcc = 0.0M;

            string dVal = parts[1].ToString();
            if (string.IsNullOrEmpty(dVal))
            {
                return importLine + "\t" + eProjectWeb.Framework.Lang.Translator.Translate("$import_error_missingamount");
            }
            else
            {
                if (!decimal.TryParse(dVal, System.Globalization.NumberStyles.Any, numberFormat, out decOrigValue))
                {
                    return importLine + "\t" + eProjectWeb.Framework.Lang.Translator.Translate("$import_error_wrongvalue");
                }
            }

            dVal = parts[2].ToString();
            if (string.IsNullOrEmpty(dVal))
            {
                return importLine + "\t" + eProjectWeb.Framework.Lang.Translator.Translate("$import_error_missingamount");
            }
            else
            {
                if (!decimal.TryParse(dVal, System.Globalization.NumberStyles.Any, numberFormat, out decValueAcc))
                {
                    return importLine + "\t" + eProjectWeb.Framework.Lang.Translator.Translate("$import_error_wrongvalue");
                }
            }

            importCifData.interfaceId = interfaceId;
            importCifData.fileId = interfaceId + "_" + DateTime.Now.ToString();
            importCifData.extRef1 = parts[0].ToString().Length > 32 ? parts[0].ToString().Substring(0, 32) : parts[0].ToString();
            importCifData.valueAcc = decValueAcc;
            importCifData.origValue = decOrigValue;

            error = ImportLine(importCifData, out newCifTrans);

            if (!string.IsNullOrEmpty(error))
            {
                return importLine + "\t" + error;
            }
            return string.Empty;
        }

        private static string ImportLine(ImportCifEbankData importCifData, out CifEbankTrans newCifTrans)
        {
            newCifTrans = null;

            newCifTrans = CifEbankTrans.CreateNew();
            importCifData.origCifTrans.CopyTo(newCifTrans);
            newCifTrans.ApplyChanges();
            newCifTrans.Id = null;
            newCifTrans.Interfaceid = importCifData.interfaceId;
            newCifTrans.Fileid = importCifData.fileId;
            newCifTrans.Extref1 = importCifData.extRef1;
            newCifTrans.Extref2 = importCifData.extRef2;
            newCifTrans.Origcur = importCifData.origCur;
            newCifTrans.Origvalue = importCifData.origValue;
            newCifTrans.Valueacc = importCifData.valueAcc;
            newCifTrans.Statement = importCifData.statementId;
            newCifTrans.Openbalacc = null;
            newCifTrans.Closebalacc = null;
            newCifTrans.Extvalueacc = null;
            newCifTrans.Createdate = DateTime.Now;
            newCifTrans.State = DataRowState.Added;

            return string.Empty;
        }

        protected virtual void BeforeSaveImportedLines(CifEbankTrans origCifTrans, ref List<CifEbankTrans> list, Dictionary<CifEbankTrans, string> imported)
        {
            EfxBankTranHead tranHead = EfxBankTranHead.New();
            var sql = eProjectWeb.Framework.Utils.SqlToStringFormat(
@"select TOP 1 h.*
from efx_banktranhead h (nolock)
    left outer join efx_bank b(nolock) on b.bankid = h.bankid
where b.banktype > {0} and h.ref1 = {1}", CONST_InvalidImportBankType, origCifTrans.Id);

            try
            {
                SqlDataAdapter.FillSingleRow(U4Ext.Bank.Base.Module.eBankDBConnID, tranHead, sql);
            }
            catch (RecordNotFoundException)
            {
                tranHead = null;
            }

            if (tranHead != null)
            {
                var msg = eProjectWeb.Framework.Lang.Translator.Translate("$import_exception_translineinvalid", tranHead.Filename);
                throw new MessageException(msg);
            }
            else
            {
                decimal? importedValue = list.Sum(l => ConvertUtils.ToDecimal(l.Origvalue)).GetValueOrDefault(0);
                if (importedValue != origCifTrans.Origvalue)
                {
                    var msg = eProjectWeb.Framework.Lang.Translator.Translate("$import_exception_translinevalue", importedValue, origCifTrans.Origvalue);
                    throw new MessageException(msg);
                }                
            }

        }

        private static List<Key> SaveImportedLines(List<CifEbankTrans> list, Dictionary<CifEbankTrans, string> imported, List<string> wrongLines)
        {
            var generatedRecords = new List<Key>();

            var bl = CifEbankTransBL3.New();

            foreach (var item in list)
            {
                if (item.State == DataRowState.Added)
                {
                    // save
                    var blObjectMap = new BLObjectMap();
                    blObjectMap.Default = item;
                    var pk = bl.Save(blObjectMap);
                    generatedRecords.Add(pk);
                }
            }

            // returns the saved new records PK
            return generatedRecords;
        }

        protected string[] ImportRecordsGetFields(int id)
        {
            string typekey = new CifEbankTransImportFieldList().Key;

            Key k = new Key(new Field[] { eLog.Base.Setup.Type.TypeHead.FieldTypekey }, new object[] { typekey });
            eLog.Base.Setup.Type.TypeHead th = eLog.Base.Setup.Type.TypeHead.Load(k);
            if (th != null)
            {
                eLog.Base.Setup.Type.TypeLine tl = eLog.Base.Setup.Type.TypeLine.Load(th.Typegrpid, id);
                if (tl != null)
                {
                    string fieldsStr = StringN.ConvertToString(tl.Str1);
                    if (fieldsStr != null)
                        return fieldsStr.Split(',');
                }
            }
            return null;
        }

        protected class ImportCifEbankData
        {
            public CifEbankTrans origCifTrans { get; set; }
            public string interfaceId { get; set; }
            public string fileId { get; set; }
            public string extRef1 { get; set; }
            public string extRef2 { get; set; }
            public string origCur { get; set; }
            public decimal? origValue { get; set; }
            public decimal? valueAcc { get; set; }
            public int? statementId { get; set; }
        }

    }
}
