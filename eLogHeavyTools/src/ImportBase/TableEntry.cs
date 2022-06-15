using System.Collections.Generic;

namespace eLog.HeavyTools.ImportBase
{
    [System.Diagnostics.DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class TableEntry
    {
        public string Alias { get; set; }
        public ImportTable Table { get; set; }
        public eProjectWeb.Framework.Data.Schema Schema { get; set; }
        public eProjectWeb.Framework.Data.Entity Entity { get; set; }
        public List<ImportField> DefaultIfExists { get; set; } = new List<ImportField>();
        public bool Valid { get; set; } = true;

        private string DebuggerDisplay => $"{this.Table} [{this.Alias}]";
    }
}
