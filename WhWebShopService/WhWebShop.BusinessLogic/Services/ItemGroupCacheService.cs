using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Base;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;


[RegisterDI(Interface = typeof(IItemGroupCacheService))]
public class ItemGroupCacheService : LogicServiceBase<OlsItemgroup>, IItemGroupCacheService
{
    public static ConcurrentDictionary<string, OlsItemgroup> cacheItemGroup = new ConcurrentDictionary<string, OlsItemgroup>();


    public ItemGroupCacheService(IValidator<OlsItemgroup> validator,
                          IRepository<OlsItemgroup> repository,
                          IUnitOfWork unitOfWork,
                          IEnvironmentService environmentService) : base(validator, repository, unitOfWork, environmentService)
    {
    }

    public async Task<OlsItemgroup> GetAsync(string itemgrpid, CancellationToken cancellationToken)
    { 
        if (itemgrpid != null)
        {
            if (cacheItemGroup.TryGetValue(itemgrpid, out var i))
            {
                return i;
            }
            var ig = await this.Repository.Entities.FirstOrDefaultAsync(p => p.Itemgrpid == itemgrpid, cancellationToken);
            if (ig is not null)
            {
                cacheItemGroup.TryAdd(ig.Itemgrpid, ig);
                return ig;
            }
        }


        throw new ArgumentException("invalid itemgrpid", itemgrpid);
    }
}
