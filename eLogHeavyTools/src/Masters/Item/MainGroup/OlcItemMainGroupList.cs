using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item.MainGroup
{
    public class OlcItemMainGroupList : DefaultListProvider
    {
        public static readonly string ID = typeof(OlcItemMainGroupList).FullName;

        protected static string m_queryString = @"select imgid, code, name, delstat from olc_itemmaingroup where delstat=0";

        protected static ListColumn[] m_columns = new ListColumn[] {
            new ListColumn("imgid", 0),
            new ListColumn("code", 180),
            new ListColumn("name", 180),
            new ListColumn("delstat", 0),
        };

        public OlcItemMainGroupList()
            : base(m_queryString, m_columns)
        {
            ValueFieldName = "imgid";
            TextFieldName = "name";
            ShowCodeFieldName = "code";
            SearchFieldNames = "code,name";
        }
    }
}
