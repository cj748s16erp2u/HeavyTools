using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Extensions;
using Interf;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocLink
{
    public class OlcWhLocLinkSearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(OlcWhLocLinkSearchProvider).FullName;

        protected static string queryString = @"
select [wll].*, [whz].[whzonecode], [whz].[name] [whzname], [whl].[whloccode], [wlll].[volume], [cnt].[cnt], [f_el].[el]
from [olc_whloclink] [wll] (nolock)
  left join [olc_whzone] [whz] (nolock) on [whz].[whzoneid] = [wll].[whzoneid]
  join [olc_whlocation] [whl] (nolock) on [whl].[whlocid] = [wll].[whlocid]
  join [ols_warehouse] [wh] (nolock) on [wh].[whid] = [wll].[whid]
  outer apply (select sum([wl].[volume]) [volume]
    from [olc_whloclinkline] [wlll] (nolock)
      join [olc_whlocation] [wl] (nolock) on [wl].[whlocid] = [wlll].[whlocid]
    where [wlll].[whllid] = [wll].[whllid]) [wlll]
	outer apply (select count(0) [cnt] from [olc_whloclinkline] [wlll] (nolock) where [wlll].[whllid] = [wll].[whllid]) [cnt]
  outer apply (select top 1 [wl].[whloccode] [el]
    from [olc_whloclinkline] [wlll] (nolock)
      join [olc_whlocation] [wl] (nolock) on [wl].[whlocid] = [wlll].[whlocid]
    where [wlll].[whllid] = [wll].[whllid]
      and [wlll].[whllinktype] <> 1
    order by [wlll].[whlllineid]) [f_el]
";

        /// <summary>
        /// Lista a szűrendő adatokra. Milyen tíousú adtok ezek és egyéb szabályok a szűrésre.
        /// </summary>
        protected static QueryArg[] argsTemplate = new QueryArg[]
        {
            new QueryArg("whllid", "wll", FieldType.Integer, QueryFlags.MultipleAllowed),
            new QueryArg("whid", "wll", FieldType.Integer, QueryFlags.MultipleAllowed),
            new QueryArg("whzoneid", "wll", FieldType.Integer, QueryFlags.MultipleAllowed),
            new QueryArg("whloccode", "whl", FieldType.String, QueryFlags.Like),
            new QueryArg("startdate", "enddate", "wll", FieldType.DateTime, QueryFlags.Greater | QueryFlags.Equals),
            new QueryArg("enddate", "startdate", "wll", FieldType.DateTime, QueryFlags.Less | QueryFlags.Equals)
        };

        protected OlcWhLocLinkSearchProvider() : base(queryString, argsTemplate, SearchProviderType.Default)
        {
            SetCustomFunc(argsTemplate, "whloccode", LoccodeFilter);
        }

        /// <summary>
        /// Csak az egyik dátumadat jelenlétében a másik kiegészül arra, hogy az aznapi napra történjen szűrés.
        /// </summary>
        /// <param name="args"></param>
        protected override void PreSearch(Dictionary<string, object> args)
        {
            base.PreSearch(args);
            if (!args.ContainsKey("enddate") && args.ContainsKey("startdate"))
            {
                args["enddate"] = args["startdate"];
            }
            else if (!args.ContainsKey("startdate") && args.ContainsKey("enddate"))
            {
                args["startdate"] = args["enddate"];
            }
        }

        /// <summary>
        /// A Helykódok egyszerűbb szűréséhez kiegészítő filter metódus.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="arg"></param>
        /// <param name="quotedFieldName"></param>
        /// <param name="argValue"></param>
        private static void LoccodeFilter(StringBuilder sb, QueryArg arg, string quotedFieldName, object argValue)
        {
            
            string whloccode = ConvertUtils.ToString(argValue);
            if (!string.IsNullOrWhiteSpace(whloccode))
            {
                var sb2 = new StringBuilder();
                QueryArg.BuildQueryArgString(sb2, arg, quotedFieldName, whloccode);
                if (sb2.Length > 0)
                {
                    var sql = $@"exists (select [whl].[whloccode] from [olc_whloclinkline] [wlll] (nolock) 
join [olc_whlocation] [whl] (nolock) on [whl].[whlocid] = [wlll].[whlocid] 
where [wlll].[whllid] = [wll].[whllid] and {sb2})";

                    sb.Append(sql);
                }                   
            }
        }
    }
}
