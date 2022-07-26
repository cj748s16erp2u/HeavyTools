using eLog.HeavyTools.ImportBase;

namespace eLog.HeavyTools.Purchase.Pord.Import
{
    public class PordHeadImportResultSet : ImportResultSetBase
    {
        public TableEntry PordHead { get; set; }

        public TableEntry PordLine { get; set; }
    }
}