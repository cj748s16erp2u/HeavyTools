using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;

public interface IOrderService : ILogicService<OlsSordhead>
{
    Task<OrderResultDto> CreateAsync(Newtonsoft.Json.Linq.JObject value, OlcApilogger apilogger, CancellationToken cancellationToken = default);
}
