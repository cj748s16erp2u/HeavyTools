using eLog.Base.Masters.Partner;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Setup.Warehouse
{
    public class OlcWhLocationSelectionProvider : DefaultSelectionProvider
    {
        public static readonly string ID = typeof(OlcWhLocationSelectionProvider).FullName;

        const string QueryString = @"select top 2 loc.*,zone.whzonecode /*morefields*/ from olc_whlocation loc (nolock)
 left join olc_whzone zone (nolock) on loc.whzoneid=zone.whzoneid/*morejoins*/";

        protected static QueryArg[] ArgsTemplate = new QueryArg[] {
            new QueryArg("whlocid", "loc", FieldType.Integer),
            new QueryArg("whid","loc",FieldType.String,QueryFlags.Equals),
            new QueryArg("whzoneid","loc",FieldType.Integer,QueryFlags.Equals),
            new QueryArg("whloccode","loc",FieldType.String,QueryFlags.Like)
        };

        public OlcWhLocationSelectionProvider() : base(QueryString,ArgsTemplate,OlcWhLocationSelectPage.ID,"whlocid")
        {

        }
    }
}
