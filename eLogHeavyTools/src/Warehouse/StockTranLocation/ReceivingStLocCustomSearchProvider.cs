using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Warehouse.StockTranLocation
{
    public class ReceivingStLocCustomSearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(ReceivingStLocCustomSearchProvider).FullName;

        protected static string queryString = $"select * from [{ReceivingLocCustom._TableName}] [t] (nolock) ";

        protected static QueryArg[] argsTemplate = new[]
        {
            new QueryArg("", FieldType.Integer)
        };

        protected ReceivingStLocCustomSearchProvider() : base(queryString, argsTemplate, SearchProviderType.Default) { }
    }
}
