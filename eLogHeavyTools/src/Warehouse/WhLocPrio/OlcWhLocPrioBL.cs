using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocPrio
{
    public class OlcWhLocPrioBL:DefaultBL1<OlcWhLocPrio,OlcWhLocPrioRules>
    {
        public static readonly string ID = typeof(OlcWhLocPrioBL).FullName;

        public static T New<T>() where T: OlcWhLocPrioBL
        { 
            return ObjectFactory.New<T>();
        }

        public static OlcWhLocPrioBL New()
        {
            return New<OlcWhLocPrioBL>();
        }

        protected OlcWhLocPrioBL():base(DefaultBLFunctions.Basic)
        {

        }

        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);
        }

        /// <summary>
        /// Leellenőrzi, hogy a megadott cikkhez nem tartozik-e már olyan érvényességi idő ami ütközne a most megadottal, 
        /// úgy hogy a helykód típusa ugyanaz.
        /// </summary>
        /// <param name="itemid">Cikk id</param>
        /// <param name="startdate">Kezdő dátum</param>
        /// <param name="enddate">Befejező dátum</param>
        /// <param name="priotype">Helykód típusa </param>
        /// <param name="whlpid">WhLocPrio id</param>
        /// <returns></returns>

        public bool CheckItemActivePeriod(int? itemid, DateTime? startdate, DateTime? enddate, int? priotype, int? whlpid)
        {
            var sql = $@"
select top 1 1 
from [olc_whlocprio] [p] (nolock) 
where [p].[enddate] >= {Utils.SqlToString(startdate)}
  and [p].[startdate] <= {Utils.SqlToString(enddate)}
  and [p].[whpriotype] = {Utils.SqlToString(priotype)} and p.itemid = {Utils.SqlToString(itemid)}";
            if (whlpid.HasValue)
            {
                sql += $" and p.whlpid<>{Utils.SqlToString(whlpid)}";
            }
            var obj = SqlDataAdapter.ExecuteSingleValue(DB.Main, sql);
            return ConvertUtils.ToInt32(obj).GetValueOrDefault()!=0;
        }


        /// <summary>
        /// Leellenőrzi, hogy a helykódhoz csak 1 cikk tartozhat vagy több is 
        /// (ha ismulti 0 akkor több is tartozhat ellenkező esetben csak 1) 
        /// és ha csak 1 tartozhat és már van ilyen cikk létrehozva akkor hibát ír ki. 
        /// Ha nincs megadva ismulti, akkor úgy veszi, hogy csak 1 tartozhat hozzá.
        /// </summary>
        /// <param name="whlocid">Helykód id</param>
        /// <param name="itemid">Cikk id</param>
        /// <param name="startdate">Kezdő dátum</param>
        /// <param name="enddate">Befejező dátum</param>
        /// <param name="whlpid">WhLocPrio id</param>
        /// <returns></returns>
        public bool CheckItemIsMulti(int? whlocid, int? itemid, DateTime? startdate, DateTime? enddate, int? whlpid)
        {
            var sql = $@"select top 1 1 
from [olc_whlocprio] [p] (nolock) 
join [olc_whlocation] [l] (nolock) on [l].[whlocid] = [p].[whlocid] 
where isnull([l].[ismulti],1) = 1 
and [p].[whlocid] = {Utils.SqlToString(whlocid)} 
and [p].[itemid] <> {Utils.SqlToString(itemid)} 
and [p].[enddate] >= {Utils.SqlToString(startdate)} 
and [p].[startdate] <= {Utils.SqlToString(enddate)} ";
            if (whlpid.HasValue)
            {
                sql += $" and p.whlpid<>{Utils.SqlToString(whlpid)}";
            }
            var obj = SqlDataAdapter.ExecuteSingleValue(DB.Main, sql);
            return ConvertUtils.ToInt32(obj).GetValueOrDefault() != 0;
        }
    }
}
