using System.Collections.Generic;
using eLog.HeavyTools.ImportBase;

namespace eLog.HeavyTools.Masters.Item.Import
{
    class ItemRowContext : RowContextBase
    {
        public TableEntry Item { get; internal set; }
        public TableEntry OlcItem { get; internal set; }
        public List<TableEntry> ItemCmps { get; internal set; } = new List<TableEntry>();
        public TableEntry ItemModel { get; internal set; }
        public TableEntry PrcTable { get; internal set; }
        public TableEntry ItemExt { get; internal set; }
        public TableEntry ItemSeason { get; internal set; }
        public TableEntry ItemSup { get; internal set; }
        public TableEntry MultiplePrcTable { get; internal set; }
        public TableEntry ItemMainGroup { get; internal set; }
        public TableEntry TypeHeadColor { get; internal set; }
        public TableEntry ImpColorException { get; internal set; }

        public override void Dispose()
        {
            base.Dispose();

            this.ItemCmps?.Clear();
            this.ItemCmps = null;
            
            this.Item = null; 
            this.OlcItem = null;
            this.ItemModel = null;
            this.PrcTable = null;
            this.ItemExt = null;
            this.ItemSeason = null;
            this.ItemSup = null;
            this.MultiplePrcTable = null;
            this.ItemMainGroup = null;
            this.TypeHeadColor = null;
            this.ImpColorException = null;
        }
    }
}
