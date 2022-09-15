using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;

public interface IPriceCalcOriginalService
{
    Task<CalcJsonResultDto> GetOriginalPrice(CalcJsonParamsDto? cart, CancellationToken cancellationToken = default); 
}
