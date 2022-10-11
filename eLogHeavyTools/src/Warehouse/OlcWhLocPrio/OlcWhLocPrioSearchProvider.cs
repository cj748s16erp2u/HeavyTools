using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocPrio
{
    public class OlcWhLocPrioSearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(OlcWhLocPrioSearchProvider).FullName;

        protected static string queryString =
            @"select prio.*,
(select line.abbr from ols_typeline line 
left join ols_typehead head on head.typegrpid=line.typegrpid 
where line.typegrpid=514 and prio.whpriotype=line.value) type,
item.itemcode, item.name01 itemname01,whzone.whzonecode,loc.whloccode --$$moreFields$$ 
from olc_whlocprio prio (nolock) 
left join ols_item item (nolock) on prio.itemid=item.itemid 
left join olc_whlocation loc (nolock) on prio.whlocid=loc.whlocid 
left join olc_whzone whzone (nolock) on prio.whzoneid=whzone.whzoneid 
 --$$moreJoins$$ 
";

        protected static QueryArg[] argsTemplate = new QueryArg[]
        {
            new QueryArg("whlpid","prio",FieldType.Integer,QueryFlags.MultipleAllowed),
            new QueryArg("itemname01","name01","item",FieldType.String,QueryFlags.Like),
            new QueryArg("itemcode","item",FieldType.String,QueryFlags.Like),
            new QueryArg("whid","prio",FieldType.String,QueryFlags.Equals),
            new QueryArg("whzonecode","whzone",FieldType.String,QueryFlags.Equals),
            new QueryArg("whloccode","loc",FieldType.String,QueryFlags.Like),
            new QueryArg("enddate","startdate","prio",FieldType.DateTime,QueryFlags.Equals | QueryFlags.Less),
            new QueryArg("startdate","enddate","prio",FieldType.DateTime,QueryFlags.Equals | QueryFlags.Greater),
            new QueryArg("__noresult",FieldType.Integer)
        };

        protected OlcWhLocPrioSearchProvider() : base(queryString, argsTemplate, SearchProviderType.Default)
        {
            SetCustomFunc("__noresult", NoResultCustomFunc);
        }

        private void NoResultCustomFunc(StringBuilder sb, QueryArg arg, string quotedFieldName, object argValue)
        {
            if (Utils.Equals(argValue, 1))
            {
                sb.Append("0=null");
            }
        }

        protected override void PreSearch(Dictionary<string, object> args)
        {
            base.PreSearch(args);
            if (!args.ContainsKey("startdate") && !args.ContainsKey("enddate") && !args.ContainsKey("whlpid"))
            {
                CustomClientMessage = "$err_datefilterrequired".eLogTransl();
                args["__noresult"] = 1;
            }
            else if (!args.ContainsKey("enddate") && args.ContainsKey("startdate"))
            {
                args["enddate"] = args["startdate"];
            }
            else if (!args.ContainsKey("startdate") && args.ContainsKey("enddate"))
            {
                args["startdate"] = args["enddate"];
            }
        }

        
        protected override string CreateQueryString(Dictionary<string, object> args, bool fmtonly)
        {
            var s = base.CreateQueryString(args, fmtonly);
            return s;
        }

        protected override string GetOrderByString(Dictionary<string, object> args)
        {
            return "\norder by itemname01,type";
        }
    }
}
