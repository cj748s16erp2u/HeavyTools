using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;

public interface IOlcActionCacheService
{
    Task RefreashCacheAsync(int aid, CancellationToken cancellationToken);
    Task<ActionDataGroupDto> GetCuponByIdAsync(string couponnumber, CancellationToken cancellationToken);
    Task UpdateAsync(ActionDataGroupDto ca, CancellationToken cancellationToken);
    Task<OlcActioncouponnumber> GetCoupoNumberByIdAsync(string couponnumber, CancellationToken cancellationToken);
    Task UpdateAsync(OlcActioncouponnumber an, CancellationToken cancellationToken);
    Task<ActionDataGroupDto[]> GetActiveActionsAsync(string[] cupons, CancellationToken cancellationToken);
    Task<bool> Reset(int? aid, CancellationToken cancellationToken);
}
