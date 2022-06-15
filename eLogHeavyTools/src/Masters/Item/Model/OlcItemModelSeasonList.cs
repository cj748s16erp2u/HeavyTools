using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item.Model
{
    public class OlcItemModelSeasonList : DefaultListProvider
    {
        public static readonly string ID = typeof(OlcItemModelSeasonList).FullName;

        protected static string m_queryString = @"select imsid, s.isid+ ' - ' +ss.name+ ' / ' +s.icid+ ' - ' +c.name name, s.delstat
  from olc_itemmodelseason s
  join olc_itemcolor c on c.icid=s.icid
  join olc_itemseason ss on ss.isid=s.isid
  where s.delstat=0
    and c.delstat=0
	and ss.delstat=0
    and $$sqlwhere$$";

        protected static ListColumn[] m_columns = new ListColumn[] {
            new ListColumn("imsid", 0), 
            new ListColumn("name", 250), 
            new ListColumn("delstat", 0),
        };

        public OlcItemModelSeasonList()
            : base(m_queryString, m_columns)
        {
            ValueFieldName = "imsid";
            TextFieldName = "name";
            ShowCodeFieldName = "";
            SearchFieldNames = "name";
        }

        private static QueryArg[] m_argsTemplate = new QueryArg[] {
            new QueryArg("imid", FieldType.Integer, QueryFlags.Equals)
        };

        protected override string GetQuerySql(Dictionary<string, object> args)
        {
            if (args == null)
            {
                args = new Dictionary<string, object>();
            }

            int imid = -1;
            if (args.ContainsKey("imid"))
            {
                imid = ConvertUtils.ToInt32(args["imid"]).Value;

            }
            args["imid"] = imid;
            
            var sqlwhere = QueryArg.BuildQueryString(m_argsTemplate, args);

            return base.GetQuerySql(args).Replace("$$sqlwhere$$", sqlwhere);
        }

    }
}

 
