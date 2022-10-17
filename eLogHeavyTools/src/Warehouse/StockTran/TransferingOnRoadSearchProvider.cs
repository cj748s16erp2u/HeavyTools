using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.StockTran
{
    internal class TransferingOnRoadSearchProvider : eProjectWeb.Framework.Data.DefaultSearchProvider
    {
        public static string ID = typeof(TransferingOnRoadSearchProvider).FullName;

        protected static string queryString =
@"
select stl.stid, stl.linenum, stl.stlineid, stl.origid, stl.unitid2, stl.ordqty2, 
       stl.movqty2 - isnull(x.movqty2,0) as dispqty2, stl.movqty2 - isnull(x.movqty2,0) as movqty2,
       stl.itemid, itm.itemcode, itm.name01, itm.name02, itm.name03, itm.unitid unitid
  from ols_stline stl (nolock)
  join ols_item itm (nolock) on itm.itemid = stl.itemid 
 outer apply (select sum(stl2.movqty2) movqty2
                from ols_stline stl2 (nolock)
                join ols_sthead sth (nolock) on sth.stid = stl2.stid 
                join olc_stline ostl (nolock) on ostl.stlineid=stl2.stlineid and ostl.origstlineid=stl.stlineid) x
";

        protected static eProjectWeb.Framework.Data.QueryArg[] argsTemplate = new eProjectWeb.Framework.Data.QueryArg[]
        {
            new QueryArg("stid", "stl", FieldType.Integer),                        // ID
            new QueryArg("stlineid", "stl", FieldType.Integer),                    // Line ID
            new QueryArg("linenum", "stl", FieldType.Integer),                     // Position
            new QueryArg("itemid", "stl", FieldType.Integer),                      // Item ID
            new QueryArg("itemcode", "itm", FieldType.String, QueryFlags.Like),    // Itemcode
            new QueryArg("name01", "itm", FieldType.String, QueryFlags.Like),      // item name
            new QueryArg("linenumfrom", "linenum", "stl", FieldType.Integer, QueryFlags.Greater | QueryFlags.Equals),
            new QueryArg("linenumto", "linenum", "stl", FieldType.Integer, QueryFlags.Less | QueryFlags.Equals),
        };

        public TransferingOnRoadSearchProvider()
            : base(queryString, argsTemplate, eProjectWeb.Framework.Data.SearchProviderType.Default, 200)
        {
        }

        protected override string CreateQueryString(Dictionary<string, object> args, bool fmtonly)
        {
            var s = base.CreateQueryString(args, fmtonly);

            if (!fmtonly)
            {
                if (args.Count == 0)
                    s += " where ";
                else
                    s += " and ";
                s += " stl.movqty2 - isnull(x.movqty2,0)>0 ";
            }

            return s;
        }
    }
}
