using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item.Season
{
    public class OlcItemSeasonList : DefaultListProvider
    {
        public static readonly string ID = typeof(OlcItemSeasonList).FullName;

        protected static string m_queryString = @"select * from (
	select distinct its.isid, name, its.delstat 
	  from olc_itemseason its 
	  join olc_itemmodelseason ims on ims.isid=its.isid
	 where its.delstat=0
	   --where
) x order by substring(isid,2,2) desc , substring(isid,1,1) 
";

        protected static ListColumn[] m_columns = new ListColumn[] {
            new ListColumn("isid", 0),
            new ListColumn("name", 180),
            new ListColumn("delstat", 0),
        };

        
        public OlcItemSeasonList()
            : base(m_queryString, m_columns)
        {
            ValueFieldName = "isid";
            TextFieldName = "name";
            ShowCodeFieldName = "isid";
            SearchFieldNames = "isid,name";
        }

        protected override string GetQuerySql(Dictionary<string, object> args)
        {
            var sql = base.GetQuerySqlArgs(args, new QueryArg[0]);

            if (args != null)
            {
                if (args.ContainsKey("imid"))
                {
                    sql = sql.Replace("--where", " and imid= " + args["imid"]);
                }
            }

            return sql;
        }
    }
}
