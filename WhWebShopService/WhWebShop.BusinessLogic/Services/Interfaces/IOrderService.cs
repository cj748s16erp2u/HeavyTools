using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;

public interface IOrderService : ILogicService<OlsSordhead>
{
    Task<OrderResultDto> CreateAsync(OrderParamsDto parms, CancellationToken cancellationToken = default);
}
