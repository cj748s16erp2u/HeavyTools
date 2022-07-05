using eLog.HeavyTools.ImportBase.ImportResult;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Package;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.ImportBase
{
    public class ImportUtils
    {

        /// <summary>
        /// Feltöltött Xlsx fájlok törlése
        /// </summary>
        /// <param name="uploadFiles">Fájlok listája</param>
        public void RemoveFiles(IEnumerable<eProjectWeb.Framework.UI.Controls.UploadFileInfo> uploadFiles)
        {
            foreach (var f in uploadFiles)
            {
                var storedFileName = Path.Combine(Globals.ReportsTempFolder, f.StoredFileName);
                try { File.Delete(storedFileName); }
                catch (Exception ex) { Log.Error(ex); }
            }
        }

        /// <summary>
        /// Feldolgozás során létrejök fájlok törlése
        /// </summary>
        /// <param name="result">Feldolgozás eredmények</param>
        public void RemoveTemporaryFiles(IEnumerable<ImportFileNames> result)
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
        public IEnumerable<string> GetResultFileInfo(IEnumerable<ImportFileNames> result)
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
        public string AddSiteRoot(string descrDirs)
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
        public void ZipResult(IEnumerable<string> result, string targetFileName)
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
        public void ZipResult(ImportFileNames result, string targetFileName)
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
        private string GetTempFileName()
        {
            var rnd = new Random();
            var s4 = new Func<string>(() => Convert.ToInt64(Math.Floor((1 + rnd.NextDouble()) * 0x10000)).ToString("x"));
            return s4() + s4();
        }


        /// <summary>
        /// Feldolgozás eredmény fájl létrehozása (zip-be tömörítés)
        /// </summary>
        /// <param name="result">Feldolgozás eredmények</param>
        public string CreateResultFile(IEnumerable<ImportFileNames> result)
        {
            string resultFileName;
            var resultFileInfo = GetResultFileInfo(result);

            var tempFileName = GetTempFileName();
            if (resultFileInfo?.Count() > 1)
            {
                resultFileName = Path.Combine(Globals.ReportsTempFolder, $"{tempFileName}.zip");
                ZipResult(resultFileInfo, resultFileName);
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
    }
}
