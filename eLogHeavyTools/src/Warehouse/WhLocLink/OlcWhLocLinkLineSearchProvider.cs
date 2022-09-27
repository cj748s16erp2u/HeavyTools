using eProjectWeb.Framework.Data;
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

        protected static string queryString = @"select whlll.*, whl.whloccode, whl.name, whl.volume from [dbo].[olc_whlocation] (nolock) as whl
 join[dbo].[olc_whloclinkline] (nolock) as whlll on whl.whlocid = whlll.whlocid
 join[dbo].[olc_whloclink] (nolock) as whll on whll.whllid = whlll.whllid";

        protected static QueryArg[] argsTemplate = new QueryArg[]
        {
            new QueryArg("whlllineid", "whlll", FieldType.Integer, QueryFlags.MultipleAllowed),
            new QueryArg("whllid", "whll", FieldType.Integer, QueryFlags.Like),
            new QueryArg("whlocid", "whl", FieldType.Integer, QueryFlags.MultipleAllowed),
            new QueryArg("whlinktype", "whlll", FieldType.Integer, QueryFlags.MultipleAllowed)
        };

        protected OlcWhLocLinkLineSearchProvider() : base(queryString, argsTemplate, SearchProviderType.Default)
        {

        }
    }
}
