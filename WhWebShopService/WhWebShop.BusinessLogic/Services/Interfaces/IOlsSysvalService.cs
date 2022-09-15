using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;

public interface IOlsSysvalService
{
    void ClearCache();
    ValueTask<OlsSysval?> GetAsync(string? sysvalid, CancellationToken cancellationToken = default);
}
