using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Setup.PriceTable
{
    public class OlcPrcTypeList : DefaultListProvider
    {
        public static readonly string ID = typeof(OlcPrcTypeList).FullName;

        protected static string m_queryString = @"select ptid, name+'  - '+ case when isnet=1 then 'Nettó' else 'Bruttó' end name, delstat from olc_prctype where delstat=0";

        protected static ListColumn[] m_columns = new ListColumn[] {
            new ListColumn("ptid", 0),
            new ListColumn("name", 180),
            new ListColumn("delstat", 0),
        };

        public OlcPrcTypeList()
            : base(m_queryString, m_columns)
        {
            ValueFieldName = "ptid";
            TextFieldName = "name";
            ShowCodeFieldName = "icid";
            SearchFieldNames = "name";
        }
    }
}
