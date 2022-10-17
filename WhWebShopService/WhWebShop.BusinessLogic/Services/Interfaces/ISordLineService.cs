using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using Newtonsoft.Json.Linq;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;

public interface ISordLineService
{
    Task<SordLineDto> SordLineDeleteAsync(JObject value, CancellationToken cancellationToken = default);
}
