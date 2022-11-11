using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Base;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Validators.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;

[RegisterDI(Interface = typeof(IItemCacheService))]
public class ItemCacheService : LogicServiceBase<OlsItem>, IItemCacheService
{
    public static ConcurrentDictionary<string, int> cacheItemid = new ConcurrentDictionary<string, int>();
    public static ConcurrentDictionary<int, string> cacheItemCode = new ConcurrentDictionary<int, string>();
    public static ConcurrentDictionary<int, OlsItem> cacheItem = new ConcurrentDictionary<int, OlsItem>();


    private IRepository<OlsItem> repository;

    public ItemCacheService(IValidator<OlsItem> validator, IRepository<OlsItem> repository, IUnitOfWork unitOfWork, IEnvironmentService environmentService) : base(validator, repository, unitOfWork, environmentService)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<int?> GetAsync(string? itemcode, CancellationToken cancellationToken = default)
    {
        if (itemcode != null)
        { 
            if (cacheItemid.TryGetValue(itemcode,out var i))
            {
                return i;
            } 
            var item = await this.repository.Entities.FirstOrDefaultAsync(p => p.Itemcode == itemcode, cancellationToken);
            if (item is not null)
            {
                cacheItemid.TryAdd(item.Itemcode, item.Itemid);
                cacheItemCode.TryAdd(item.Itemid, item.Itemcode);
                cacheItem.TryAdd(item.Itemid, item);
                return item.Itemid;
            }
        }
        throw new ArgumentException("invalid itemcode", itemcode);
    }

    public async Task<string?> GetAsync(int? itemid, CancellationToken cancellationToken = default)
    {
        if (itemid != null)
        {
            if (cacheItemCode.TryGetValue(itemid.Value, out var i))
            {
                return i;
            }
            var item = await this.repository.Entities.FirstOrDefaultAsync(p => p.Itemid == itemid, cancellationToken);
            if (item is not null)
            {
                cacheItemid.TryAdd(item.Itemcode, item.Itemid);
                cacheItemCode.TryAdd(item.Itemid, item.Itemcode);
                cacheItem.TryAdd(item.Itemid, item);
                return item.Itemcode;
            }
        }
        throw new ArgumentException("invalid itemid", itemid.ToString());
    }

    public async Task<OlsItem?> GetItemAsync(int? itemid, CancellationToken cancellationToken = default)
    {
        if (itemid != null)
        {
            if (cacheItem.TryGetValue(itemid.Value, out var i))
            {
                return i;
            }
            var item = await this.repository.Entities.FirstOrDefaultAsync(p => p.Itemid == itemid, cancellationToken);
            if (item is not null)
            {
                cacheItemid.TryAdd(item.Itemcode, item.Itemid);
                cacheItemCode.TryAdd(item.Itemid, item.Itemcode);
                cacheItem.TryAdd(item.Itemid, item);
                return item;
            }
        }
        throw new ArgumentException("invalid itemid", itemid.ToString());
    }
}
