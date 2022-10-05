using eLog.HeavyTools.Setup.Warehouse;
using eProjectWeb.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocLink
{
    public class OlcWhLocLinkLineRules : eLog.Base.Common.TypedBaseRuleSet<OlcWhLocLinkLine>
    {
        public OlcWhLocLinkLineRules() : base(true, true)
        {
            this.AddCustomRule(CheckActiveLineRule);
            this.AddCustomRule(CheckSameWarehouseAndZoneRule);
        }

        /// <summary>
        /// Validálja, máshol az a helykód ne legyen használatban.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="loclinkline"></param>
        private void CheckActiveLineRule(RuleValidateContext ctx, OlcWhLocLinkLine loclinkline)
        {
            if (loclinkline.Whlocid != null)
            {
                var bl = OlcWhLocLinkLineBL.New();
                var whloc = OlcWhLocation.Load(loclinkline.Whlocid);

                if (bl.CheckActiveLinkByIDs(loclinkline.Whlocid, loclinkline.Whllid))
                { 
                    ctx.AddErrorField(OlcWhLocLink.FieldWhlocid, "$err_OlcWhLocLinkLine_WhLocIdHasActivePeriod", whloc?.Whloccode);
                }
            }
        }

        /// <summary>
        /// Validálás arra, hogy a fő és az alárendelt helykód ugyanabban a raktárban és zónában legyen.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="locline"></param>
        private void CheckSameWarehouseAndZoneRule(RuleValidateContext ctx, OlcWhLocLinkLine locline)
        {
            var loc1 = OlcWhLocation.Load(locline.Whlocid);
            var temp = OlcWhLocLink.Load(locline.Whllid);

            if (locline.Whlocid != null)
            {
                if(temp.Whid.Value != loc1.Whid.Value)
                {
                    ctx.AddErrorField(OlcWhLocLink.FieldWhlocid, "$err_OlcWhLocLink_WhidIsDifferent", loc1.Whloccode);
                }
                if(temp.Whzoneid == null && loc1.Whzoneid != null || temp.Whzoneid != null && loc1.Whzoneid == null || temp.Whid.Value != loc1.Whid.Value)
                {
                    ctx.AddErrorField(OlcWhLocLink.FieldWhlocid, "$err_OlcWhLocLink_ZoneIsDifferent", loc1.Whloccode);
                }
            }
        }
    }
}
