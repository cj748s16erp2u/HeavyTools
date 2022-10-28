using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;

public interface IOlsSorddocCacheService
{
    Task<OlsSorddoc> GetSorddocAsync(string? sorddoc, CancellationToken cancellationToken = default);

}
