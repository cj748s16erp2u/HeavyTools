using System.Collections.Generic;
using System.Linq;
using eLog.HeavyTools.ImportBase;

namespace eLog.HeavyTools.ImportBase
{
    [System.Diagnostics.DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class ImportTable
    {
        public string Alias { get; set; }
        public string Table { get; set; }
        public bool? Optional { get; set; }
        public IEnumerable<ImportField> Fields { get; set; }
        public IEnumerable<ImportSplit> Splits { get; set; }
        public IEnumerable<ImportConditional> Conditionals { get; set; }

        public IEnumerable<ImportField> ValidFields => this.Fields?.Where(f => f.Valid);
        public IEnumerable<ImportConditional> ValidConditionals => this.Conditionals?.Where(c => c.Valid);

        private string DebuggerDisplay => $"{this.Table} [{this.Alias}]";
    }
}
