
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;

public interface IPriceCalcActionService
{
    Task CalculateActionPriceAsync(CalcJsonParamsDto cart, CalcJsonResultDto cartRes, PriceCalcActionResultDto pricecalcactionresult, CancellationToken cancellationToken);
}
