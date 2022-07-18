using eLog.HeavyTools.ImportBase;

namespace eLog.HeavyTools.Sales.Sord.Import
{
    public class SordHeadRowContext : RowContextBase
    {
        public TableEntry SordHead { get; internal set; }
        public TableEntry SordLine { get; internal set; }

        public override void Dispose()
        {
            base.Dispose();

            SordHead = null;
            SordLine = null;
        }
    }
}