using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Sales.Action
{
    public class ActionCountryList : DefaultListProvider
    {
        public static readonly string ID = typeof(ActionCountryList).FullName;

        protected static string m_queryString = @"select countryid, name, delstat from ols_country (nolock) where delstat=0";

        protected static ListColumn[] m_columns = new ListColumn[] {
            new ListColumn("countryid", 80),
            new ListColumn("name", 180),
            new ListColumn("delstat", 0),
        };

        public ActionCountryList()
            : base(m_queryString, m_columns)
        {
            ValueFieldName = "countryid";
            TextFieldName = "name";
            ShowCodeFieldName = "countryid";
            SearchFieldNames = "countryid,countryid";
        }
    }
 

    public class ActionRetailList : DefaultListProvider
    {
        public static readonly string ID = typeof(ActionRetailList).FullName;

        protected static string m_queryString = @"select whid, name, delstat from ols_warehouse (nolock) where delstat=0";

        protected static ListColumn[] m_columns = new ListColumn[] {
            new ListColumn("whid", 80),
            new ListColumn("name", 180),
            new ListColumn("delstat", 0),
        };

        public ActionRetailList()
            : base(m_queryString, m_columns)
        {
            ValueFieldName = "whid";
            TextFieldName = "name";
            ShowCodeFieldName = "whid";
            SearchFieldNames = "whid,countryid";
        }
    } 

}
