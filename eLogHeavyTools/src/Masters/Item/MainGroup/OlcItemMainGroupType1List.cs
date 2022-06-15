using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Masters.Item.MainGroup
{
    class OlcItemMainGroupType1List : DefaultListProvider
    {
        public static readonly string ID = typeof(OlcItemMainGroupType1List).FullName;

        protected static string m_queryString = @"select imgt1id, groupname, delstat from olc_itemmaingrouptype1 (nolock) where delstat=0";
         
        protected static ListColumn[] m_columns = new ListColumn[] {
            new ListColumn("imgt1id", 80),
            new ListColumn("groupname", 180),
            new ListColumn("delstat", 0),
        };

        public OlcItemMainGroupType1List()
            : base(m_queryString, m_columns)
        {
            ValueFieldName = "imgt1id";
            TextFieldName = "groupname";
            ShowCodeFieldName = "imgt1id";
            SearchFieldNames = "imgt1id,groupname";
        }
    }
}
