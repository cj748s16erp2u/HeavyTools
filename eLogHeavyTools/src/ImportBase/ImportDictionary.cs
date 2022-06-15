using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.ImportBase
{
    public class ImportDictionary
    {
        public string Name { get; set; }
        public ImportDictionaryType? Type { get; set; }
        public string Table { get; set; }
        public string KeyField { get; set; }
        public IEnumerable<ImportFieldBase> ValueFields { get; set; }
    }

    public enum ImportDictionaryType
    {
        None = 0,
        FlagType,
    }
}
