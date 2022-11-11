using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;

public interface IItemGroupCacheService
{
    Task<OlsItemgroup> GetAsync(string itemgrpid, CancellationToken cancellationToken);
}
