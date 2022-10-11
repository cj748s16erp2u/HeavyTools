using eLog.Base.Warehouse.StockTran;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.StockTran
{
    internal class TransferingHeadSearchProvider3 : TransferingHeadSearchProvider {

        protected static QueryArg[] argsTemplate2 = new QueryArg[]
            {
                new QueryArg("towh2whid", "whid", "towh2", FieldType.String, QueryFlags.Like)
            };

        public TransferingHeadSearchProvider3():base()
        {
            ArgsTemplate = MergeQueryArgs(ArgsTemplate, argsTemplate2);
        }

        protected override string CreateQueryString(Dictionary<string, object> args, bool fmtonly)
        {
            var sql = base.CreateQueryString(args, fmtonly);

            return sql.
                Replace("--$$morejoins$$",
                    @"left join olc_sthead ch (nolock) on ch.stid=sth.stid 
                      left join ols_warehouse towh2 (nolock) on towh2.whid = isnull(ch.onroadtowhid, whs.towhid)").
                Replace("--$$morefields$$", ",towh2.whid towh2whid, towh2.name towh2name");

        }
    }
}
