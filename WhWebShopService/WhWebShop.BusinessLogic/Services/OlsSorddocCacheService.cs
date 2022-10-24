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


[RegisterDI(Interface = typeof(IOlsSorddocCacheService))]
internal class OlsSorddocCacheService : LogicServiceBase<OlsSorddoc>, IOlsSorddocCacheService
{
    public static ConcurrentDictionary<string, OlsSorddoc> cacheSordDoc = new ConcurrentDictionary<string, OlsSorddoc>();



    public OlsSorddocCacheService(IValidator<OlsSorddoc> validator,
                                  IRepository<OlsSorddoc> repository,
                                  IUnitOfWork unitOfWork,
                                  IEnvironmentService environmentService) : base(validator, repository, unitOfWork, environmentService)
    {
    }

    public async Task<OlsSorddoc> GetSorddocAsync(string? sorddoc, CancellationToken cancellationToken = default)
    {
        if (sorddoc != null)
        {
            if (cacheSordDoc.TryGetValue(sorddoc, out var i))
            {
                return i;
            }
            var item = await this.Repository.Entities.FirstOrDefaultAsync(p => p.Sorddocid == sorddoc, cancellationToken);
            if (item is not null)
            {
                cacheSordDoc.TryAdd(item.Sorddocid, item);
                return item;
            }
        }
        throw new ArgumentException("invalid sorddoc", sorddoc);
    }
}
