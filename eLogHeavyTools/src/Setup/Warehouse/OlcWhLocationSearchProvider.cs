using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Setup.Warehouse
{
    public class OlcWhLocationSearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(OlcWhLocationSearchProvider).FullName;

        protected static string queryString = $@"select top 1000 [t].*, [z].[whzonecode]
from [{OlcWhLocation._TableName}] [t] (nolock)
  left join [{OlcWhZone._TableName}] [z] (nolock) on [z].[whzoneid] = [t].[whzoneid]
";

        protected static QueryArg[] argsTemplate = new[]
        {
            new QueryArg("whlocid", "t", FieldType.Integer, QueryFlags.MultipleAllowed),
            new QueryArg("whid", "t", FieldType.String, QueryFlags.MultipleAllowed),
            new QueryArg("whzoneid", "t", FieldType.Integer, QueryFlags.MultipleAllowed),
            new QueryArg("code", "t", FieldType.String, QueryFlags.Like),
            new QueryArg("loctype", "t", FieldType.Integer, QueryFlags.MultipleAllowed),
            new QueryArg("movloctype", "t", FieldType.Integer, QueryFlags.MultipleAllowed),
            new QueryArg("fromcrawlorder", "crawlorder", "t", FieldType.Integer, QueryFlags.Greater | QueryFlags.Equals),
            new QueryArg("tocrawlorder", "crawlorder", "t", FieldType.Integer, QueryFlags.Less | QueryFlags.Equals),
        };

        protected OlcWhLocationSearchProvider() : base(queryString, argsTemplate, SearchProviderType.Default) { }
    }
}
