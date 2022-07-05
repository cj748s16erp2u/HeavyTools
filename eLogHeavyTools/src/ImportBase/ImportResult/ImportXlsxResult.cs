using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.ImportBase.ImportResult
{
    /// <summary>
    /// Xlsx import eredmény és létrejött fájlok nevei
    /// </summary>
    public class ImportXlsxResult
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
