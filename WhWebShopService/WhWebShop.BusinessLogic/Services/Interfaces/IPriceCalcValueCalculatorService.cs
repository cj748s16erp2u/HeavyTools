using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;

public interface IPriceCalcValueCalculatorService
{
    Task CalculateCartAsync(CalcJsonResultDto res, CancellationToken cancellationtoken = default);
}
