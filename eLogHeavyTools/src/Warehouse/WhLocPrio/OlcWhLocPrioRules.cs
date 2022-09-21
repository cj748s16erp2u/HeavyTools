using eLog.HeavyTools.Setup.Warehouse;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Rules;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocPrio
{
    public class OlcWhLocPrioRules : eLog.Base.Common.TypedBaseRuleSet<OlcWhLocPrio>
    {
        public OlcWhLocPrioRules() : base(true, false)
        {
            this.AddCustomRule(this.IsNormalLocTypeRule);
            this.AddCustomRule(this.CheckItemActivePeriodRule);
            this.AddCustomRule(this.CheckItemIsMultiRule);
            this.AddCustomRule(this.IsDateIntervalValidRule);
        }

        protected const string OLCWHLOC = nameof(OlcWhLocation);

        protected override void BeforeCustomRules(RuleValidateContext ctx, OlcWhLocPrio value)
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

        /// <summary>
        /// Leelenőrzi, hogy a helykód típusa normál típus.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="olcWhLocPrio"></param>
        private void IsNormalLocTypeRule(RuleValidateContext ctx, OlcWhLocPrio olcWhLocPrio)
        {
            if (olcWhLocPrio?.Whlocid != null && olcWhLocPrio.IsFieldChanged2(OlcWhLocPrio.FieldWhlocid))
            {
                var loc = GetOlcWhLocation(ctx);
                if (loc.Loctype != (int)OlcWhLocation_LocType.Normal)
                {
                    ctx.AddErrorField(OlcWhLocPrio.FieldWhlocid.Name,"$err_olcwhlocprio_whlocid", olcWhLocPrio.Whid.Value);
                }
            }
        }

        /// <summary>
        /// Leelenőrzi, hogy a megadott érvényességi idő kezdete az nincs-e az érvényességi idő vége után.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="olcWhLocPrio"></param>
        private void IsDateIntervalValidRule(RuleValidateContext ctx, OlcWhLocPrio olcWhLocPrio)
        {
            if (olcWhLocPrio?.Startdate != null && olcWhLocPrio?.Enddate != null &&
                olcWhLocPrio.IsFieldChanged2(OlcWhLocPrio.FieldStartdate) &&
                olcWhLocPrio.IsFieldChanged2(OlcWhLocPrio.FieldEnddate))
            {
                if (olcWhLocPrio.Startdate > olcWhLocPrio.Enddate)
                {
                    ctx.AddError("$err_olcwhlocprio_startdate_enddate", olcWhLocPrio.Startdate, olcWhLocPrio.Enddate);
                }
            }
        }

        private void CheckItemActivePeriodRule(RuleValidateContext ctx, OlcWhLocPrio olcWhLocPrio)
        {
            var bl = OlcWhLocPrioBL.New();
            
            var checkResult = bl.CheckItemActivePeriod(olcWhLocPrio.Itemid,olcWhLocPrio.Startdate, olcWhLocPrio.Enddate, olcWhLocPrio.Whpriotype, olcWhLocPrio.Whlpid);
            if (checkResult)
            {
                var item = Base.Masters.Item.Item.Load(olcWhLocPrio.Itemid);
                ctx.AddErrorField(OlcWhLocPrio.FieldItemid.Name, "$err_olcwhlocprio_itemid", item?.Itemcode,olcWhLocPrio.Startdate, olcWhLocPrio.Enddate);
            }
        }

        private void CheckItemIsMultiRule(RuleValidateContext ctx, OlcWhLocPrio olcWhLocPrio)
        {
            var bl = OlcWhLocPrioBL.New();
            var checkResult = bl.CheckItemIsMulti(olcWhLocPrio.Whlocid,olcWhLocPrio.Itemid,olcWhLocPrio.Startdate,olcWhLocPrio.Enddate,olcWhLocPrio.Whlpid);
            if (checkResult)
            {
                var loc = GetOlcWhLocation(ctx);
                ctx.AddErrorField(OlcWhLocPrio.FieldWhlocid.Name, "$err_olcwhlocprio_location_ismulti", loc?.Whlocid, olcWhLocPrio.Startdate, olcWhLocPrio.Enddate);
            }
        }
    }
}
