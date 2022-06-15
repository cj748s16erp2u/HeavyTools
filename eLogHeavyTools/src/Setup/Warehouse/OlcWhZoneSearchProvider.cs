using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Setup.Warehouse
{
    public class OlcWhZoneSearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(OlcWhZoneSearchProvider).FullName;

        protected static string queryString = $"select top 1000 * from [{OlcWhZone._TableName}] [t] (nolock) ";

        protected static QueryArg[] argsTemplate = new[]
        {
            new QueryArg("whzoneid", "t", FieldType.Integer, QueryFlags.MultipleAllowed),
            new QueryArg("whid", "t", FieldType.String, QueryFlags.MultipleAllowed),
            new QueryArg("whzonecode", "t", FieldType.String, QueryFlags.Like),
            new QueryArg("name", "t", FieldType.String, QueryFlags.Like),
        };

        protected OlcWhZoneSearchProvider() : base(queryString, argsTemplate, SearchProviderType.Default) { }
    }
}
