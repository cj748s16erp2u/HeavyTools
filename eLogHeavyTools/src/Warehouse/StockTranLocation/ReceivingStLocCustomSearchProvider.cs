using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Warehouse.StockTranLocation
{
    public class ReceivingStLocCustomSearchProvider : MemorySearchProvider<ReceivingStLocCustomDto>
    {
        public static readonly string ID = typeof(ReceivingStLocCustomSearchProvider).FullName;

        protected static QueryArg[] argsTemplate = new QueryArg[]
        {
        };

        protected ReceivingStLocCustomSearchProvider() : base(string.Empty, argsTemplate, SearchProviderType.Default) { }

        protected override IList<ReceivingStLocCustomDto> PrepareList(string sql, MSPCreateListArgs args)
        {
            return new List<ReceivingStLocCustomDto>();
        }
    }
}
