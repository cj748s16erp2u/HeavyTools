using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item.Color
{
    public class OlcItemColorList : DefaultListProvider
    {
        public static readonly string ID = typeof(OlcItemColorList).FullName;

        protected static string m_queryString = @"select icid, name, delstat from olc_itemcolor where delstat=0 order by icid desc";

        protected static ListColumn[] m_columns = new ListColumn[] {
            new ListColumn("icid", 0),
            new ListColumn("name", 180),
            new ListColumn("delstat", 0),
        };

        public OlcItemColorList()
            : base(m_queryString, m_columns)
        {
            ValueFieldName = "icid";
            TextFieldName = "name";
            ShowCodeFieldName = "icid";
            SearchFieldNames = "icid,name";
        }
    }
}
