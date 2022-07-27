namespace eLog.HeavyTools.Purchase.Pord
{
    public class OlcPordHeadRules : eLog.Base.Common.TypedBaseRuleSet<OlcPordHead>
    {
        public OlcPordHeadRules() : base(true, false)
        {
            this.ERules[OlcPordHead.FieldPordid.Name].Mandatory = false;
        }
    }
}