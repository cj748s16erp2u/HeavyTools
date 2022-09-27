using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;

public interface IOlsRecidService : ILogicService<OlsRecid>
{
    public Task<OlsRecid?> GetNewIdAsync(string riid, CancellationToken cancellationtoken);
    public Task<OlsRecid?> GetCurrentAsync(string riid, CancellationToken cancellationtoken);
}
