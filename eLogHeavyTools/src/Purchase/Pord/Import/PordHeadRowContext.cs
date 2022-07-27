using eLog.HeavyTools.ImportBase;

namespace eLog.HeavyTools.Purchase.Pord.Import
{
    public class PordHeadRowContext : RowContextBase
    {
        public TableEntry PordHead { get; internal set; }
        public TableEntry PordLine { get; internal set; }

        public override void Dispose()
        {
            base.Dispose();

            this.PordHead = null;
            this.PordLine = null;
        }
    }
}