using eLog.Base.Common;

namespace eLog.HeavyTools.Warehouse.StockTran
{
    internal class OlcStHeadRules : TypedBaseRuleSet<OlcStHead>
    {
        public OlcStHeadRules()
            : base(true)
        {
            ERules["stid"].Mandatory = false; //fk
        }
    }

}
