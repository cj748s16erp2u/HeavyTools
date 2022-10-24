using eLog.HeavyTools.Reports.Financials.ReminderLetter;
using eLog.HeavyTools.Setup.Warehouse;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocLink
{
    public class OlcWhLocLinkLineBL : DefaultBL1<OlcWhLocLinkLine, OlcWhLocLinkLineRules>
    {
        public static readonly string ID = typeof(OlcWhLocLinkLineBL).FullName;

        public static OlcWhLocLinkLineBL New()
        {
            return ObjectFactory.New<OlcWhLocLinkLineBL>();
        }

        protected static readonly string SQL_DeleteCheck = $@"
select [whllinktype] from [dbo].[olc_whloclinkline] as [wlll] where [wlll].[whlllineid] = {{0}}";

        /// <summary>
        /// Ellenőrzés arra ID alapján, hogy dátumot is figyelembe véve, kapcsolt-e valahová a helykód.
        /// </summary>
        /// <param name="whlocid"></param>
        /// <param name="whllid"></param>
        /// <returns>bool</returns>
        public bool CheckActiveLinkByIDs(int? whlocid, int? whllid)
        {
            var sql = $@"select top 1 1
from [olc_whloclinkline] [wlll] (nolock)
  join [olc_whloclink] [wll] (nolock) on [wll].[whllid] = [wlll].[whllid]
  join [olc_whloclink] [wllm] (nolock) on [wllm].[startdate] <= [wll].[enddate] and [wllm].[enddate] >= [wll].[startdate]
where [wlll].[whlocid] = {Utils.SqlToString(whlocid)}
  and [wllm].[whllid] = {Utils.SqlToString(whllid)}";

            var obj = SqlDataAdapter.ExecuteSingleValue(DB.Main, sql);

            return ConvertUtils.ToInt32(obj).GetValueOrDefault() != 0;
        }

        /// <summary>
        /// A fő helykódok nem törölhetőek a Kapcsolt helykódok felületen.
        /// </summary>
        /// <param name="k"></param>
        /// <param name="reason"></param>
        /// <returns>bool</returns>
        public override bool IsDeletePossible(Key k, out string reason)
        {
            var b = base.IsDeletePossible(k, out reason);

            if (b)
            {
                var sql = string.Format(SQL_DeleteCheck, k.Values.First().ToString());
                var num = SqlDataAdapter.ExecuteSingleValue(DB.Main, sql);
                if ((int)num == 1)
                {
                    b = false;
                    reason = "$err_masterloclinkline".eLogTransl();
                }
            }
            return b;
        }
    }
}
