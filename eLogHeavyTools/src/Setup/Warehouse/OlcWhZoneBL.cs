using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Setup.Warehouse
{
    public class OlcWhZoneBL : DefaultBL1<OlcWhZone, OlcWhZoneRules>
    {
        public static readonly string ID = typeof(OlcWhZoneBL).FullName;

        public static T New<T>()
            where T : OlcWhZoneBL
        {
            return ObjectFactory.New<T>();
        }

        public static OlcWhZoneBL New()
        {
            return New<OlcWhZoneBL>();
        }

        protected OlcWhZoneBL() : base(DefaultBLFunctions.Basic)
        {
        }

        public override DataRow GetInfo(IDictionary<string, object> args)
        {
            var sql = $@"select [wh].[whid], [wh].[name] [whname]--$$moreFields$$
from [ols_warehouse] [wh] (nolock)--$$moreJoins$$
";

            var sep = "where ";
            if (args.ContainsKey("whid"))
            {
                sql += $"{sep}[wh].[whid] = {Utils.SqlToString(args["whid"])}";
                sep = "  and ";
            }

            if (args.ContainsKey("whzoneid"))
            {
                sql = sql
                    .Replace("--$$moreFields$$", ", [z].[whzonecode], [z].[name] [whzonename], [z].[locdefvolume], [z].[locdefoverfillthreshold], [z].[locdefismulti]--$$moreFields$$")
                    .Replace("--$$moreJoins$$", $"{Environment.NewLine}  left join [olc_whzone] [z] (nolock) on [z].[whid] = [wh].[whid]--$$moreJoins$$");
                sql += $"{sep}[z].[whzoneid] = {Utils.SqlToString(args["whzoneid"])}";
                sep = "  and ";
            }

            var schema = SqlDataAdapter.GetSchema(DB.Main, sql, sql);
            var row = new DataRow(schema);
            SqlDataAdapter.FillSingleRow(DB.Main, row, sql);

            return row;
        }

        public bool CheckZoneIsBackgroundUniqueInWh(string whid, int? whzoneid)
        {
            var sql = $@"select top 1 1
from [{OlcWhZone._TableName}] (nolock)
where [{OlcWhZone.FieldWhid.Name}] = {Utils.SqlToString(whid)}
  and [{OlcWhZone.FieldIsbackground.Name}] <> 0
";

            if (whzoneid != null)
            {
                sql += $"  and [{OlcWhZone.FieldWhzoneid.Name}] <> {Utils.SqlToString(whzoneid)}";
            }

            return ConvertUtils.ToInt32(SqlDataAdapter.ExecuteSingleValue(DB.Main, sql)).GetValueOrDefault() != 1;
        }

        public bool CheckZoneIsPufferUniqueInWh(string whid, int? whzoneid)
        {
            var sql = $@"select top 1 1
from [{OlcWhZone._TableName}] (nolock)
where [{OlcWhZone.FieldWhid.Name}] = {Utils.SqlToString(whid)}
  and [{OlcWhZone.FieldIspuffer.Name}] <> 0
";

            if (whzoneid != null)
            {
                sql += $"  and [{OlcWhZone.FieldWhzoneid.Name}] <> {Utils.SqlToString(whzoneid)}";
            }

            return ConvertUtils.ToInt32(SqlDataAdapter.ExecuteSingleValue(DB.Main, sql)).GetValueOrDefault() != 1;
        }

        public bool CheckHasLocationWOSpecValue(int? whzoneid, Field field, OlcWhLocation_LocType? locType = null)
        {
            var sql = $@"select top 1 1
from [{OlcWhLocation._TableName}] (nolock)
where [{OlcWhLocation.FieldWhzoneid.Name}] = {Utils.SqlToString(whzoneid)}
  and [{field.Name}] is null
";

            if (locType != null)
            {
                sql += $"  and [{OlcWhLocation.FieldLoctype.Name}] = {Utils.SqlToString((int)locType)}";
            }

            return ConvertUtils.ToInt32(SqlDataAdapter.ExecuteSingleValue(DB.Main, sql)).GetValueOrDefault() == 1;
        }
    }
}
