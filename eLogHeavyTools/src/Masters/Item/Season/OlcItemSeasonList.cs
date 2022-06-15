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

        protected static string m_queryString = @"select isid, name, delstat from olc_itemseason where delstat=0 order by isid desc";

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
    }
}
