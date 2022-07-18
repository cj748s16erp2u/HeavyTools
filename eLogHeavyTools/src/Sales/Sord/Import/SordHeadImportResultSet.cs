using eLog.HeavyTools.ImportBase;

namespace eLog.HeavyTools.Sales.Sord.Import
{
    public class SordHeadImportResultSet : ImportResultSetBase
    {
        public TableEntry SordHead { get; set; }

        public TableEntry SordLine { get; set; }
    }
}