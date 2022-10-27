using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Warehouse.WhZone.WhZTranService;
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
            IList<ReceivingStLocCustomDto> list = null;

            var queryParams = this.CreateQueryParams(args);
            if (queryParams != null)
            {
                var tranService = WhZone.Common.WhZTranUtils.CreateTranLocService();
                var response = tranService.Query(queryParams);
                list = this.ConvertResponse(response);
            }

            return list;
        }

        private IList<ReceivingStLocCustomDto> ConvertResponse(IEnumerable<WhZTranLocDto> response)
        {
            var bl = ReceivingStLocCustomBL.New();
            return response?
                .Select(r => bl.ConvertServiceResponseToDto(r))
                .ToList();
        }

        private WhZTranLocQueryDto CreateQueryParams(MSPCreateListArgs args)
        {
            WhZTranLocQueryDto param = null;

            if (args?.Filters?.Any() == true)
            {
                param = new WhZTranLocQueryDto();

                var type = typeof(WhZTranLocQueryDto);
                var props = type.GetProperties();
                foreach (var p in props)
                {
                    var propName = p.Name.ToLowerInvariant();
                    if (args.Filters.TryGetValue(propName, out var o))
                    {
                        var propType = p.PropertyType;
                        if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            propType = Nullable.GetUnderlyingType(propType);
                        }

                        o = ConvertUtils.ChangeType(o, propType);

                        p.SetValue(param, o);
                    }
                }
            }

            return param;
        }
    }
}
