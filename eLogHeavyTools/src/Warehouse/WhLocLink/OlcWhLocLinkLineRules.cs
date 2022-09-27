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
            this.AddCustomRule(CheckSameWarehouseandZoneRule);
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

                if (bl.CheckActiveLinkByIDs(loclinkline.Whlocid, loclinkline.Whllid))
                {
                    var whloc = OlcWhLocation.Load(loclinkline.Whlocid);
                    ctx.AddErrorField(OlcWhLocLink.FieldWhlocid, "$err_OlcWhLocLinkLine_WhLocIdHasActivePeriod", whloc?.Whloccode );
                }
            }
        }

        /// <summary>
        /// Validálás arra, hogy a fő és az alárendelt helykód ugyanabban a raktárban és zónában legyen.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="locline"></param>
        private void CheckSameWarehouseandZoneRule(RuleValidateContext ctx, OlcWhLocLinkLine locline)
        {
            var loc1 = OlcWhLocation.Load(locline.Whlocid);
            var temp = OlcWhLocLink.Load(locline.Whllid);
            var loc2 = OlcWhLocation.Load(temp.Whlocid);

            if (loc1.Whlocid != null && loc2.Whlocid != null)
            {
                var bl = OlcWhLocLinkLineBL.New();

                if(!bl.CheckWarehouseAndZone(loc1.Whid, loc2.Whid, loc1.Whzoneid, loc2.Whzoneid))
                {
                    ctx.AddErrorField(OlcWhLocLink.FieldWhlocid, "$err_OlcWhLocLink_WhidOrZoneIsDifferent", loc1.Whloccode, loc2.Whloccode);
                }
            }
        }
    }
}
