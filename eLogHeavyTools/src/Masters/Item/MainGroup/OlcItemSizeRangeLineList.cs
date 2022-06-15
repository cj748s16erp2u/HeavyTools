using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item.MainGroup
{
    public class OlcItemSizeRangeLineList : DefaultListProvider
    {
        public static readonly string ID = typeof(OlcItemSizeRangeLineList).FullName;

        protected static string m_queryString = @"select isrlid, size, delstat from olc_itemsizerangeline l where delstat=0  and $$sqlwhere$$ order by ordernum";

        protected static ListColumn[] m_columns = new ListColumn[] {
            new ListColumn("isrlid", 0),
            new ListColumn("size", 180),
            new ListColumn("delstat", 0),
        };

        public OlcItemSizeRangeLineList()
            : base(m_queryString, m_columns)
        {
            ValueFieldName = "isrlid";
            TextFieldName = "size";
            ShowCodeFieldName = "";
            SearchFieldNames = "code,name";
        }

        private static QueryArg[] m_argsTemplate = new QueryArg[] {
            new QueryArg("isrhid", FieldType.Integer, QueryFlags.Equals)
        };

        protected override string GetQuerySql(Dictionary<string, object> args)
        {
            if (args == null)
            {
                args = new Dictionary<string, object>();
            }

            int isrhid = -1;
            if (args.ContainsKey("isrhid"))
            {
                isrhid = ConvertUtils.ToInt32(args["isrhid"]).Value; 
            }
            args["isrhid"] = isrhid;
            var sqlwhere = QueryArg.BuildQueryString(m_argsTemplate, args);


            return base.GetQuerySql(args).Replace("$$sqlwhere$$", sqlwhere);
        }
    }
}
