using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using Newtonsoft.Json.Linq;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;

public interface IPriceCalcService : ILogicService<OlcPriceCalcResult>
{
    int Calc(int? x, int? y);
    Task<CalcResultDto> CalcAsync(CalcParamsDto parms, CancellationToken cancellationToken = default);

    Task<CalcJsonResultDto> CalcJsonAsync(Newtonsoft.Json.Linq.JObject parms, CancellationToken cancellationToken = default);
     
    Task<CalcJsonResultDto> CalculatePrice(CalcJsonParamsDto cart, PriceCalcActionResultDto priceCalcAction ,CancellationToken cancellationToken = default);
    Task<bool> ResetJsonAsync(JObject value, CancellationToken cancellationToken = default);
    void ResetCartCacheAsync(JObject value);
}
