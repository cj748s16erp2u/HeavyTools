using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework.Rules;
using eProjectWeb.Framework.Extensions;

namespace eLog.HeavyTools.Purchase.Pinv
{
    public class OlcCostLineRules : Base.Common.TypedBaseRuleSet<OlcCostLine>
    {
        protected OlcCostLineRules() : base(true, false)
        {
            ERules[OlcCostLine.FieldCostlineid.Name].Mandatory = false;

            AddCustomRule(CostlineidRule);
        }

        protected const string COSTLINE = "COSTLINE";

        protected override void BeforeCustomRules(RuleValidateContext ctx, OlcCostLine olcCostLine)
        {
            base.BeforeCustomRules(ctx, olcCostLine);

            var dict = new Dictionary<string, object>();

            Base.Purchase.Pinv.CostLine costLine = null;
            object parent = ctx.GetParentObject();
            if (parent is eProjectWeb.Framework.INamedObjectCollection)
            {
                var iparent = (eProjectWeb.Framework.INamedObjectCollection)parent;
                costLine = iparent.Get("CostLine") as Base.Purchase.Pinv.CostLine;
            }

            if (costLine == null && costLine.Costlineid.HasValue)
                costLine = Base.Purchase.Pinv.CostLine.Load(costLine.Costlineid);

            if (costLine != null)
                dict[COSTLINE] = costLine;

            if (costLine != null && olcCostLine.Costlineid == null)
                olcCostLine.Costlineid = costLine.Costlineid;

            ctx.InternalCustomData = dict;
        }

        protected Base.Purchase.Pinv.CostLine GetCostLine(RuleValidateContext ctx)
        {
            if (ctx.InternalCustomData is Dictionary<string, object>)
            {
                Dictionary<string, object> dict = (Dictionary<string, object>)ctx.InternalCustomData;
                object x;
                if (dict.TryGetValue(COSTLINE, out x))
                    return x as Base.Purchase.Pinv.CostLine;
            }
            return null;
        }

        protected void CostlineidRule(RuleValidateContext ctx, OlcCostLine olcCostLine)
        {
            var costLine = GetCostLine(ctx);
            if (costLine == null)
                CheckMandatory(ctx, olcCostLine, OlcCostLine.FieldCostlineid);
        }

    }
}
