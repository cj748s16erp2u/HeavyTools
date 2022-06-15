﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodaInt.Base.Masters.Partner;
using eLog.Base.Masters.Partner;
using eLog.HeavyTools.ImportBase;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.Package;

namespace eLog.HeavyTools.Masters.Partner
{
    public class PartnerBL3 : PartnerBL2
    {
        public static new PartnerBL3 New()
        {
            return (PartnerBL3)PartnerBL.New();
        }

        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);

            var olcPartner = objects.Get<OlcPartner>();
            if (olcPartner != null)
            {
                RuleServer.Validate<OlcPartner, OlcPartnerRules>(objects);
            }

            var olcPartnCmps = objects.Get<OlcPartnCmps>();
            if (olcPartnCmps != null)
            {
                foreach (var olcPartnCmp in olcPartnCmps.AllRows)
                {
                    var map = new BLObjectMap();
                    map.SysParams.ActionID = objects.SysParams.ActionID;
                    map.Default = olcPartnCmp;

                    RuleServer.Validate<OlcPartnCmp, OlcPartnCmpRules>(map);
                }
            }
        }

        protected override bool PreSave(BLObjectMap objects, Entity e)
        {
            var partner = (Base.Masters.Partner.Partner)objects.Default;

            if (e is OlcPartner olc && olc.State == DataRowState.Added)
            {
                olc.Partnid = partner.Partnid;
            }

            if (e is OlcPartnCmp pc && pc.State == DataRowState.Added)
            {
                pc.Partnid = partner.Partnid;
            }

            return base.PreSave(objects, e);
        }

        public override void Delete(Key k)
        {
            var olc = OlcPartner.Load(k);
            olc.State = DataRowState.Deleted;
            olc.Save();

            base.Delete(k);
        }

        /// <summary>
        /// Feltöltött Xlsx fájlok importálása
        /// </summary>
        /// <param name="uploadInfo">Feltöltött fájlok tárolója</param>
        public ImportResult PartnerImport(eProjectWeb.Framework.UI.Controls.UploadData uploadInfo)
        {
            string error = null;

            var importDescrFileName = CustomSettings.GetString("PartnerImportDescrFileName");
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
                descrDirs = this.AddSiteRoot(descrDirs);
            }

            if (uploadInfo?.Process == true && string.IsNullOrWhiteSpace(error))
            {
                return this.ImportPartnerXlsxFiles(uploadInfo.Files, importDescrFileName, descrDirs);
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
        /// Feltöltött Xlsx fájlok feldolgozása
        /// </summary>
        /// <param name="uploadFiles">Fájlok listája</param>
        /// <param name="importDescrFileName">Import leíró fájl neve</param>
        /// <param name="descrDirs">Import leíró keresési könyvtárok</param>
        /// <returns>Feldolgozás eredménye</returns>
        private ImportResult ImportPartnerXlsxFiles(IEnumerable<eProjectWeb.Framework.UI.Controls.UploadFileInfo> uploadFiles, string importDescrFileName, string descrDirs)
        {
            importDescrFileName = Utils.GetFirstFileFromDirs(descrDirs, importDescrFileName);

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
        /// <param name="storedFileName">Feldolgozandó fájl fizikai neve</param>
        private ImportXlsxResult ImportPartnerXlsxFile(string importDescrFileName, string importResultFolder, string fileName, string storedFileName)
        {
            var importService = new Import.PartnerImportService();
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
    }
}