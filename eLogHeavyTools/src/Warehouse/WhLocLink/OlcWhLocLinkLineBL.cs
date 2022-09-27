using eLog.HeavyTools.Setup.Warehouse;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
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

        /// <summary>
        /// Ellenőrzés arra ID alapján, hogy dátumot is figyelembe véve, kapcsolt-e valahová a helykód.
        /// </summary>
        /// <param name="whlocid"></param>
        /// <param name="whllid"></param>
        /// <returns></returns>
        public bool CheckActiveLinkByIDs(int? whlocid, int? whllid)
        {
            var sql = $@"select top 1 1
from [olc_whloclinkline] [wlll] (nolock)
  join [olc_whloclink] [wll] (nolock) on [wll].[whlocid] = [wlll].[whllid]
  join [olc_whloclink] [wllm] (nolock) on [wllm].[startdate] <= [wll].[enddate] and [wllm].[enddate] >= [wll].[startdate]
where [wlll].[whlocid] = {Utils.SqlToString(whlocid)}
  and [wllm].[whllid] = {Utils.SqlToString(whllid)}";

            var obj = SqlDataAdapter.ExecuteSingleValue(DB.Main, sql);

            return ConvertUtils.ToInt32(obj).GetValueOrDefault() != 0;
        }

        /// <summary>
        /// Megnézi, hogy a master hely és a detail hely raktárai és zónái megegyeznek-e.
        /// </summary>
        /// <param name="wh1"></param>
        /// <param name="wh2"></param>
        /// <param name="zone1"></param>
        /// <param name="zone2"></param>
        /// <returns></returns>
        public bool CheckWarehouseAndZone(string wh1, string wh2, int? zone1, int? zone2)
        {
            var sql = $@"select top 1 1 from [olc_whloclinkline] [wlll] (nolock)
join [olc_whloclink] [wll] (nolock) on [wll].[whlocid] = [wlll].[whllid]
where [wll].[whid] = {Utils.SqlToString(wh1)} and {Utils.SqlToString(wh1)} = {Utils.SqlToString(wh2)}
and [wll].[whzoneid] = {Utils.SqlToString(zone1)} and {Utils.SqlToString(zone1)} = {Utils.SqlToString(zone2)}";

            var obj = SqlDataAdapter.ExecuteSingleValue(DB.Main, sql);

            return ConvertUtils.ToInt32(obj).GetValueOrDefault() != 0;
        }
    }
}
