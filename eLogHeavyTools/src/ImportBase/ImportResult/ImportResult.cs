using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.ImportBase.ImportResult
{
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

}
