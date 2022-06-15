using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item.MainGroup
{
    class OlcItemMainGroupType2List : DefaultListProvider
    {
        public static readonly string ID = typeof(OlcItemMainGroupType2List).FullName;

        protected static string m_queryString = @"select imgt2id, groupname, delstat from olc_itemmaingrouptype2 where delstat=0";

        protected static ListColumn[] m_columns = new ListColumn[] {
            new ListColumn("imgt2id", 80),
            new ListColumn("groupname", 180),
            new ListColumn("delstat", 0),
        };

        public OlcItemMainGroupType2List()
            : base(m_queryString, m_columns)
        {
            ValueFieldName = "imgt2id";
            TextFieldName = "groupname";
            ShowCodeFieldName = "";
            SearchFieldNames = "groupname";
        }
    }
}
