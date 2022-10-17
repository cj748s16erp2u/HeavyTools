using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using Newtonsoft.Json.Linq;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;

public interface IReserveService
{
    Task<ReserveDto> ReserveAsync(JObject value, CancellationToken cancellationToken = default);
    Task<ReserveDto> ReserveDeleteAsync(JObject value, CancellationToken cancellationToken = default);
    Task<ReserveDto> ReserveDeleteAsync(int resid, CancellationToken cancellationToken);
    Task<ReserveDto> ReserveSetQtyAsync(int resid,  decimal qty, CancellationToken cancellationToken);
}
