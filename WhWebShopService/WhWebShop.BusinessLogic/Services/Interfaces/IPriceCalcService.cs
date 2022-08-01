using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;

public interface IPriceCalcService : ILogicService<OlcPriceCalcResult>
{
    int Calc(int? x, int? y);
    Task<CalcResultDto> CalcAsync(CalcParamsDto parms, CancellationToken cancellationToken = default);
}
