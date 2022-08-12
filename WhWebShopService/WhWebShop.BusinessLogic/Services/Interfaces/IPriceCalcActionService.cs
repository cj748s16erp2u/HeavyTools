
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;

public interface IPriceCalcActionService
{
    Task<CalcJsonResultDto> CalculateActionPrice(CalcJsonResultDto cart, CancellationToken cancellationToken = default);
}
