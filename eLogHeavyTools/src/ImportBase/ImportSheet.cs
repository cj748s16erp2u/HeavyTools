using System.Collections.Generic;

namespace eLog.HeavyTools.ImportBase
{
    public class ImportSheet
    {
        public string Sheet { get; set; }
        public string HeaderRow { get; set; }
        public int? ColumnNameRow { get; set; }
        public bool? Ignore { get; set; }
        public IEnumerable<ImportTable> Tables { get; set; }
        public IEnumerable<ImportConditional> Conditionals { get; set; }
        public IEnumerable<ImportDictionary> Dictionaries { get; set; }
    }
}
