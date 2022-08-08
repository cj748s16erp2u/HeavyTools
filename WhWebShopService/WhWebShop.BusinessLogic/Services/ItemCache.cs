using System;
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

[RegisterDI(Interface = typeof(IItemCache))]
public class ItemCache : LogicServiceBase<OlsItem>, IItemCache
{
    public static Dictionary<string, int> cache = new Dictionary<string, int>();
    private IRepository<OlsItem> repository;

    public ItemCache(IValidator<OlsItem> validator, IRepository<OlsItem> repository, IUnitOfWork unitOfWork, IEnvironmentService environmentService) : base(validator, repository, unitOfWork, environmentService)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<int?> GetAsync(string? itemcode, CancellationToken cancellationToken = default)
    {
        if (itemcode != null)
        {
            if (cache.ContainsKey(itemcode))
            {
                return cache[itemcode];
            } 
            var item = await this.repository.Entities.FirstOrDefaultAsync(p => p.Itemcode == itemcode, cancellationToken);
            if (item is not null)
            {
                cache.Add(itemcode, item.Itemid);
                return item.Itemid;
            }
        }
        throw new ArgumentException("invalid itemcode", itemcode);
    }
}
