using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Setup.Warehouse
{
    public class OlcWhZoneList : DefaultListProvider
    {
        public static readonly string ID = typeof(OlcWhZoneList).FullName;

        protected static string queryString = $@"select *
from [olc_whzone] [z] (nolock)
";

        protected static QueryArg[] argsTemplate = new[]
        {
            new QueryArg("whid", "z", FieldType.String, QueryFlags.MultipleAllowed),
        };

        protected static ListColumn[] columns = new[]
        {
            new ListColumn("whzoneid", 0),
            new ListColumn("whzonecode", 120),
            new ListColumn("name", 300),
            new ListColumn("delstat", 0),
        };

        public OlcWhZoneList():base(queryString, columns)
        {
            this.ValueFieldName = OlcWhZone.FieldWhzoneid.Name;
            this.TextFieldName = OlcWhZone.FieldName.Name;
            this.ShowCodeFieldName = OlcWhZone.FieldWhzonecode.Name;
            this.SearchFieldNames = string.Join(",", new[] { this.TextFieldName, this.ShowCodeFieldName});
        }

        protected override string GetQuerySql(Dictionary<string, object> args)
        {
            return base.GetQuerySqlArgs(args, argsTemplate);
        }
    }
}
