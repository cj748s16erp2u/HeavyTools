using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item.MainGroup
{
    class OlcItemSizeRangeHeadList : DefaultListProvider
    {
        public static readonly string ID = typeof(OlcItemSizeRangeHeadList).FullName;

        protected static string m_queryString = @"select isrhid, code, name, delstat from olc_itemsizerangehead where delstat=0";

        protected static ListColumn[] m_columns = new ListColumn[] {
            new ListColumn("isrhid", 0),
            new ListColumn("code", 180),
            new ListColumn("name", 180),
            new ListColumn("delstat", 0),
        };

        public OlcItemSizeRangeHeadList()
            : base(m_queryString, m_columns)
        {
            ValueFieldName = "isrhid";
            TextFieldName = "name";
            ShowCodeFieldName = "code";
            SearchFieldNames = "code,name";
        }
    }
}
