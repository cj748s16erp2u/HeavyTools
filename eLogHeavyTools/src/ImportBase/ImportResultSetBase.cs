using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.ImportBase
{
    public abstract class ImportResultSetBase
    {
        public string LogText { get; set; }
        public int Row { get; set; }
        public int? LogCol { get; set; }
    }
}
