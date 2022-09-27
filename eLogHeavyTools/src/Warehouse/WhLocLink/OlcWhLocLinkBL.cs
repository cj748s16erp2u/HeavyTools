using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocLink
{
    public class OlcWhLocLinkBL : DefaultBL1<OlcWhLocLink, OlcWhLocLinkRules>
    {
        public static readonly string ID = typeof(OlcWhLocLinkBL).FullName;
        public static OlcWhLocLinkBL New()
        {
            return ObjectFactory.New<OlcWhLocLinkBL>();
        }

        /// <summary>
        /// Ellenőrzés arra, hogy a kapcsolás időintervalluma nem foglalt-e.
        /// </summary>
        /// <param name="whlocid"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="whllid"></param>
        /// <returns>bool</returns>
        public bool CheckActiveLinkPeriod(int? whlocid, DateTime? startdate, DateTime? enddate, int? whllid)
        {
            var sql = $@"select top 1 1
from [olc_whloclinkline] [wlll] (nolock)
  join [olc_whloclink] [wll] (nolock) on [wll].[whlocid] = [wlll].[whllid]
where [wlll].[whlocid] = {Utils.SqlToString(whlocid)}
  and [wll].[startdate] <= {Utils.SqlToString(enddate)}
  and [wll].[enddate] >= {Utils.SqlToString(startdate)}";

            if (whllid.HasValue)
            {
                sql += $" and wll.whllid <> {Utils.SqlToString(whllid)}";
            }

            var obj = SqlDataAdapter.ExecuteSingleValue(DB.Main, sql);

            return ConvertUtils.ToInt32(obj).GetValueOrDefault() != 0;
        }

        /// <summary>
        /// Mentés után beállítandó adatok a helykód kapcsolatoknál.
        /// </summary>
        /// <param name="objects"></param>
        /// <param name="e"></param>
        protected override void PostSave(BLObjectMap objects, Entity e)
        {
            base.PostSave(objects, e);

            var whloclink = e as OlcWhLocLink;

            if (objects.SysParams.ActionID == ActionID.New && whloclink != null)
            {
                var whloclinkline = OlcWhLocLinkLine.CreateNew();

                whloclinkline.Whllinktype = (int)OlcWhLocLinkLine_WhlLinkType.Master;
                whloclinkline.Whlocid = whloclink.Whlocid;
                whloclinkline.Whllid = whloclink.Whllid;

                var map = new BLObjectMap();
                map.SysParams.ActionID = ActionID.New;
                map.Default = whloclinkline;
                var linebl = OlcWhLocLinkLineBL.New();
                linebl.Save(map);
            }
        }

        /// <summary>
        /// Adatszerzés a második tabon lévő infopanel-hez.
        /// </summary>
        /// <param name="args"></param>
        /// <returns>DataRow</returns>
        public override DataRow GetInfo(IDictionary<string, object> args)
        {
            var num = args.FirstOrDefault().Value;

            var sql = $@"
 select [wll].*, [whz].[whzonecode], [whz].[name] [whzname], [whl].[whloccode], [whl].[name] [whlname], [wh].[name] [whname], wlll.*
from [olc_whloclink] [wll] (nolock)
  left join [olc_whzone] [whz] (nolock) on [whz].[whzoneid] = [wll].[whzoneid]
  join [olc_whlocation] [whl] (nolock) on [whl].[whlocid] = [wll].[whlocid]
  join [ols_warehouse] [wh] (nolock) on [wh].[whid] = [wll].[whid] 
outer apply (select sum([wl].[volume]) [volume]
    from [olc_whloclinkline] [wlll] (nolock)
      join [olc_whlocation] [wl] (nolock) on [wl].[whlocid] = [wlll].[whlocid]
    where [wlll].[whllid] = [wll].[whllid]) [wlll]
where {Utils.SqlToString(num)} = [wll].[whllid]";

            var schema = SqlDataAdapter.GetSchema(DB.Main, sql, sql);
            var row = new DataRow(schema);
            SqlDataAdapter.FillSingleRow(DB.Main, row, sql);

            return row;
        }
    }
}
