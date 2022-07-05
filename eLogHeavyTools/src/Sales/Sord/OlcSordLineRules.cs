namespace eLog.HeavyTools.Sales.Sord
{
    public class OlcSordLineRules : eLog.Base.Common.TypedBaseRuleSet<OlcSordLine>
    {
        public OlcSordLineRules() : base(true, false)
        {
            this.ERules[OlcSordLine.FieldSordlineid.Name].Mandatory = false;
        }
    }
}