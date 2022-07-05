using System.Collections.Generic;
using eLog.HeavyTools.ImportBase;

namespace eLog.HeavyTools.Masters.Item.Import
{
    class ItemImportResultSet : ImportResultSetBase
    {
        public TableEntry Item { get; set; }
        public TableEntry OlcItem { get; set; }
        public IEnumerable<TableEntry> ItemCmps { get; set; }
        public TableEntry ItemModel { get; set; }
        public TableEntry PrcTable { get; set; }
        public TableEntry ItemExt { get; set; }
        public TableEntry ItemSeason { get; set; }
        public TableEntry ItemSup { get; set; }
        public TableEntry MultiplePrcTable { get; set; }

        public TableEntry ItemMainGroup { get; set; }
    }
}
