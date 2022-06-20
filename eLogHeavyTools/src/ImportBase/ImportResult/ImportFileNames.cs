using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.ImportBase.ImportResult
{
    /// <summary>
    /// Import során feldolgozott és létrejött fájl nevek tárolója
    /// </summary>
    public class ImportFileNames
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
}
