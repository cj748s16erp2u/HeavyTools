using System.Collections.Generic;

namespace eLog.HeavyTools.ImportBase
{
    public class ImportLookup
    {
        public string Alias { get; set; }
        public string TypeKey { get; set; }
        public string Table { get; set; }
        public string KeyField { get; set; }
        public ImportFieldExpr KeyFieldExpr { get; set; }
        public string ValueField { get; set; }
        public IEnumerable<ImportFieldBase> DependentFields { get; set; }
    }
}
