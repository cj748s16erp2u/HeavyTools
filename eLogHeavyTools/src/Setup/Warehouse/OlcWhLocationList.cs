using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Setup.Warehouse
{
    public class OlcWhLocationList : DefaultListProvider
    {
        public static readonly string ID = typeof(OlcWhLocationList).FullName;

        protected static string queryString = "select * from olc_whlocation loc (nolock)";

        protected static QueryArg[] argsTemplate = new[]
        {
            new QueryArg("whlocid", "loc", FieldType.Integer, QueryFlags.MultipleAllowed),
            new QueryArg("whid", "loc", FieldType.String, QueryFlags.MultipleAllowed),
            new QueryArg("whzoneid", "loc", FieldType.Integer, QueryFlags.MultipleAllowed)
        };

        protected static ListColumn[] columns = new[]
        {
            new ListColumn("whlocid", 0),
            new ListColumn("whloccode", 120),
            new ListColumn("name", 300),
            new ListColumn("delstat", 0),
        };

        public OlcWhLocationList() : base(queryString, columns)
        {
            this.ValueFieldName = OlcWhLocation.FieldWhlocid.Name;
            this.TextFieldName = OlcWhLocation.FieldName.Name;
            this.ShowCodeFieldName = OlcWhLocation.FieldWhloccode.Name;
            this.SearchFieldNames = string.Join(",", new[] { this.TextFieldName, this.ShowCodeFieldName });
        }

        protected override string GetQuerySql(Dictionary<string, object> args)
        {
            return base.GetQuerySqlArgs(args, argsTemplate);
        }
    }
}
