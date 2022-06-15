using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item.Model
{
    public class OlcItemModelAllList : DefaultListProvider
    {
        public static readonly string ID = typeof(OlcItemModelAllList).FullName;

        protected static string m_queryString = @"select imid, code, name, delstat from olc_itemmodel where delstat=0";

        protected static ListColumn[] m_columns = new ListColumn[] {
            new ListColumn("imid", 0),
            new ListColumn("code", 180),
            new ListColumn("name", 180),
            new ListColumn("delstat", 0), 
        };

        public OlcItemModelAllList()
            : base(m_queryString, m_columns)
        {
            ValueFieldName = "imid";
            TextFieldName = "name";
            ShowCodeFieldName = "code";
            SearchFieldNames = "code,name";
        }

        private static QueryArg[] m_argsTemplate = new QueryArg[] {
            new QueryArg("imgid", FieldType.Integer, QueryFlags.Equals)
        }; 
    }
}
