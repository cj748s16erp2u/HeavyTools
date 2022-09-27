using eLog.HeavyTools.Setup.Warehouse;
using eProjectWeb.Framework.Rules;
using System.Collections.Generic;


namespace eLog.HeavyTools.Warehouse.WhLocLink
{
    public class OlcWhLocLinkRules : Base.Common.TypedBaseRuleSet<OlcWhLocLink>
    {
        protected const string OLCWHZONE = nameof(OlcWhZone);
        protected const string OLCWHLOC = nameof(OlcWhZone);
        public OlcWhLocLinkRules() : base(true, true)
        {
            this.AddCustomRule(CheckActiveLinkRule);
            this.AddCustomRule(DateValidateRule);
            this.AddCustomRule(IsNormalLocTypeRule);
        }

        /// <summary>
        /// Validálja, hogy a kapcsolat aktív-e.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="loclink"></param>
        private void CheckActiveLinkRule(RuleValidateContext ctx, OlcWhLocLink loclink)
        {
            if (loclink.Whlocid != null)
            {
                var bl = OlcWhLocLinkBL.New();

                if(bl.CheckActiveLinkPeriod(loclink.Whlocid, loclink.Startdate, loclink.Enddate, loclink.Whllid))
                {
                    var whloc = OlcWhLocation.Load(loclink.Whlocid);
                    ctx.AddErrorField(OlcWhLocLink.FieldWhlocid, "$err_OlcWhLocLink_WhLocIdHasActivePeriod", whloc?.Whloccode, loclink.Startdate, loclink.Enddate);
                }
            }
        }

        /// <summary>
        /// Validálja, hogy a paraméterben kapott kapcsolat kezdő dátuma ne legyen nagyobb az lezáró dátumától!
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="loclink"></param>
        private void DateValidateRule(RuleValidateContext ctx, OlcWhLocLink loclink)
        {
            if (loclink.Startdate.GetValueOrDefault() > loclink.Enddate.GetValueOrDefault())
            {
                ctx.AddErrorField(OlcWhLocLink.FieldEnddate, "$err_OlcWhLocLink_Enddate", loclink.Startdate, loclink.Enddate);
            }
        }

        /// <summary>
        /// Leellenőrzi, hogy a helykód típusa normál típus.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="loclink"></param>
        private void IsNormalLocTypeRule(RuleValidateContext ctx, OlcWhLocLink loclink)
        {
            if (loclink?.Whlocid != null && loclink.IsFieldChanged2(OlcWhLocLink.FieldWhlocid))
            {
                var loc = GetOlcWhLocation(ctx);

                if (loc.Loctype != (int)OlcWhLocation_LocType.Normal)
                {
                    ctx.AddErrorField(OlcWhLocLink.FieldWhlocid.Name, "$err_olcwhloclink_whlocid", loclink.Whid.Value);
                }
            }
        }

        protected override void BeforeCustomRules(RuleValidateContext ctx, OlcWhLocLink value)
        {
            base.BeforeCustomRules(ctx, value);

            var dict = new Dictionary<string, object>();

            if (value.Whlocid != null)
            {
                var loc = OlcWhLocation.Load(value.Whlocid);
                if (loc != null)
                {
                    dict[OLCWHLOC] = loc;
                }
            }

            ctx.InternalCustomData = dict;
        }

        protected object GetInternalCustomData(RuleValidateContext ctx, string id)
        {
            if (ctx.InternalCustomData is IDictionary<string, object>)
            {
                var dict = (IDictionary<string, object>)ctx.InternalCustomData;

                object obj;
                if (dict.TryGetValue(id, out obj))
                {
                    return obj;
                }
            }
            return null;
        }

        protected OlcWhLocation GetOlcWhLocation(RuleValidateContext ctx)
        {
            return this.GetInternalCustomData(ctx, OLCWHLOC) as OlcWhLocation;
        }
    }
}
