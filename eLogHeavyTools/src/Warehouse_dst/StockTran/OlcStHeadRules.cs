using eLog.Base.Common; 
using eLog.Base.Warehouse.StockTran;
using eLog.HeavyTools.Common;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Rules;
using System;

namespace eLog.HeavyTools.Warehouse.StockTran
{
    internal class OlcStHeadRules : TypedBaseRuleSet<OlcStHead>
    {
        public OlcStHeadRules()
            : base(true)
        {
            ERules["stid"].Mandatory = false; //fk

            AddCustomRule(OnroadtowhidRule);

        }

        private RuleParentAccess _ruleParentAccess;

        protected override void BeforeCustomRules(RuleValidateContext ctx, OlcStHead value)
        { 
            base.BeforeCustomRules(ctx, value);
            _ruleParentAccess = new RuleParentAccess(ctx);
            _ruleParentAccess.AddTable(typeof(StHead).Name);
        }



        private void OnroadtowhidRule(RuleValidateContext ctx, OlcStHead olchead)
        {
            if (string.IsNullOrEmpty(olchead.Onroadtowhid))
            {
                var sh = _ruleParentAccess.GetParent<StHead>();
                if (sh != null)
                {
                    if (sh.Towhid == TransferingHeadBL3.OnRoadWarehouse)
                    {
                        throw new MessageException("$cannotuseonroadwhidfrom");
                    }
                }
            }
        }
    }
}
