using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocLink
{
    public class OlcWhLocLinkLineSearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(OlcWhLocLinkLineSearchProvider).FullName;

        protected static string queryString = @"select whlll.*, whl.whloccode whlocid_whloccode, whl.name whlocid_name, whl.volume from [dbo].[olc_whlocation] (nolock) as whl
 join[dbo].[olc_whloclinkline] (nolock) as whlll on whl.whlocid = whlll.whlocid
 join[dbo].[olc_whloclink] (nolock) as whll on whll.whllid = whlll.whllid";

        /// <summary>
        /// Lista a szűrendő adatokra. Milyen tíousú adtok ezek és egyéb szabályok a szűrésre.
        /// </summary>
        protected static QueryArg[] argsTemplate = new QueryArg[]
        {
            new QueryArg("whlllineid", "whlll", FieldType.Integer, QueryFlags.MultipleAllowed),
            new QueryArg("whllid", "whll", FieldType.Integer, QueryFlags.Like),
            new QueryArg("whloccode", "whl", FieldType.String, QueryFlags.Like),
            new QueryArg("whlname","name", "whl", FieldType.String, QueryFlags.Like),
            new QueryArg("whlinktype", "whlll", FieldType.Integer, QueryFlags.MultipleAllowed)
        };

        protected OlcWhLocLinkLineSearchProvider() : base(queryString, argsTemplate, SearchProviderType.Default)
        {
            SetCustomFunc(argsTemplate, "whloccode", LoccodeFilter);
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
                    var sql = $@"exists (select [whloc].[whloccode] from [olc_whloclinkline] [wllline] (nolock) 
join [olc_whlocation] [whloc] (nolock) on [whloc].[whlocid] = [wllline].[whlocid] 
where [wllline].[whllid] = [whll].[whllid] and {sb2})";

                    sb.Append(sql);
                }
            }
        }
    }
}
