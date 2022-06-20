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
}
