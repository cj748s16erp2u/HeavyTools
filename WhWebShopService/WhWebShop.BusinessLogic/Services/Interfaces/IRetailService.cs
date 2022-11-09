using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;

public interface IRetailService
{
    Task<RetailOrderResultDto> CreateOrderAsync(JObject value, CancellationToken cancellationToken = default);
}
