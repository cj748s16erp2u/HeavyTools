using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocPrio
{
    public class OlcWhLocPrioAllSearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(OlcWhLocPrioAllSearchProvider).FullName;

        protected static string queryString =
            @"select prio.*,item.itemcode,item.name01 itemname01,wh.name whname,whzone.name whzonename, 
whzone.whzonecode, loc.name whlocname,loc.whloccode --$$moreFields$$ 
from olc_whlocprio prio (nolock) 
left join ols_item item (nolock) on prio.itemid=item.itemid 
left join olc_whlocation loc (nolock) on prio.whlocid=loc.whlocid 
left join olc_whzone whzone (nolock) on prio.whzoneid=whzone.whzoneid 
left join ols_warehouse wh (nolock) on prio.whid=wh.whid --$$moreJoins$$
";

        protected static QueryArg[] argsTemplate = new QueryArg[]
        {
            new QueryArg("whlpid","prio",FieldType.Integer,QueryFlags.MultipleAllowed),
            new QueryArg("itemname01","name01","item",FieldType.String,QueryFlags.Like),
            new QueryArg("itemcode","item",FieldType.String,QueryFlags.Like),
            new QueryArg("whid","prio",FieldType.String,QueryFlags.Equals),
            new QueryArg("whzonecode","whzone",FieldType.String,QueryFlags.Equals),
            new QueryArg("whloccode","loc",FieldType.String,QueryFlags.Equals),
            new QueryArg("enddate","startdate","prio",FieldType.DateTime,QueryFlags.Equals | QueryFlags.Less),
            new QueryArg("startdate","enddate","prio",FieldType.DateTime,QueryFlags.Equals | QueryFlags.Greater)
        };

        protected OlcWhLocPrioAllSearchProvider() : base(queryString, argsTemplate, SearchProviderType.Default)
        {
        }

        protected override void PreSearch(Dictionary<string, object> args)
        {
            base.PreSearch(args);
            if (args.ContainsKey("enddate"))
            {
                args["startdate"] = args["enddate"];
            }
        }

        protected override string CreateQueryString(Dictionary<string, object> args, bool fmtonly)
        {
            var s = base.CreateQueryString(args, fmtonly);
            return s;
        }

    }
}
