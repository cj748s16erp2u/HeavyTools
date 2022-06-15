using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.ImportBase
{
    [System.Diagnostics.DebuggerDisplay("{Field,nq}")]
    public class ImportFieldBase
    {
        public string Field { get; set; }
        public ImportFieldBaseType? Type { get; set; }

        public ImportLookup Lookup { get; set; }
    }

    public enum ImportFieldBaseType
    {
        Unknown = 0,
        Current,
        Company,
        Type,
        Lookup,
        SelfLookup,
    }
}
