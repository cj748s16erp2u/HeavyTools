namespace eLog.HeavyTools.Sales.Sord
{
    public class OlcSordHeadRules : eLog.Base.Common.TypedBaseRuleSet<OlcSordHead>
    {
        public OlcSordHeadRules() : base(true, false)
        {
            this.ERules[OlcSordHead.FieldSordid.Name].Mandatory = false;
            this.ERules[OlcSordHead.FieldSordapprovalstat.Name].Mandatory = false;
        }
    }
}