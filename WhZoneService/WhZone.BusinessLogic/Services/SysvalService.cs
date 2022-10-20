using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhZone.DataAccess.Repositories.Interfaces;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Services;

[RegisterDI(Interface = typeof(ISysvalService))]
internal class SysvalService : ISysvalService
{
    private readonly IRepository<OlsSysval> repository;

    private static readonly IDictionary<string, SysvalCacheItem> cache = new System.Collections.Concurrent.ConcurrentDictionary<string, SysvalCacheItem>();

    public SysvalService(
        IRepository<OlsSysval> repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async ValueTask<OlsSysval?> GetAsync(string? sysvalid, CancellationToken cancellationToken = default)
    {
        this.ValidateGetParameters(sysvalid);

        if (!cache.TryGetValue(sysvalid!, out var cacheItem) || cacheItem?.Expires < DateTime.Now)
        {
            if (cacheItem is not null)
            {
                cache.Remove(sysvalid!);
            }

            var sysval = await this.repository.FindAsync(new object[] { sysvalid! }, cancellationToken);
            if (sysval is not null)
            {
                cacheItem = new SysvalCacheItem
                {
                    Expires = DateTime.Now.AddMinutes(30),
                    Value = sysval
                };

                cache.TryAdd(sysvalid!, cacheItem);
            }
        }

        return cacheItem?.Value;
    }

    public void ClearCache()
    {
        cache.Clear();
    }

    private void ValidateGetParameters(string? sysvalid)
    {
        if (string.IsNullOrWhiteSpace(sysvalid))
        {
            throw new ArgumentNullException(nameof(sysvalid));
        }
    }

    class SysvalCacheItem
    {
        public DateTime Expires { get; set; }
        public OlsSysval Value { get; set; } = null!;
    }
}
