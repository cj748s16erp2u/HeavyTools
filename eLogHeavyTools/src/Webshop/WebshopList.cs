using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Webshop
{
    internal class WebshopList : DefaultListProvider
    {
        public static readonly string ID = typeof(WebshopList).FullName;

        private static string m_queryString = "select * from olc_webshop (nolock) where delstat=0";

        private static ListColumn[] AllColumns = new ListColumn[] { new ListColumn("wid", 30), new ListColumn("name", 150) };

        public WebshopList()
            : base(m_queryString, AllColumns)
        {
            DBConnID = CodaInt.Base.Module.CodaDBConnID;

            ValueFieldName = "wid";
            TextFieldName = "name";
            ShowCodeFieldName = "wid";
            SearchFieldNames = "wid,name";
        }
    }
}
