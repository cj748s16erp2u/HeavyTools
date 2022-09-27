using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Base;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;

[RegisterDI(Interface = typeof(IOlsCurrencyCacheService))]
public class OlsCurrencyCacheService : LogicServiceBase<OlsCurrency>, IOlsCurrencyCacheService  
{ 

    public static ConcurrentDictionary<string, int> Cache = new ConcurrentDictionary<string, int>();

    public OlsCurrencyCacheService(IValidator<OlsCurrency> validator, IRepository<OlsCurrency> repository, IUnitOfWork unitOfWork, IEnvironmentService environmentService) : base(validator, repository, unitOfWork, environmentService)
    {
    }

    public async Task<int> GetRound(string curid, CancellationToken cancellationToken = default)
    {
        int i;
        if (Cache.TryGetValue(curid, out i))
        {
            return i;
        }

        var item = await this.Repository.Entities.FirstOrDefaultAsync(p => p.Curid == curid, cancellationToken);
        if (item is not null)
        {
            Cache.TryAdd(curid, item.Decnum!.Value);
            return item.Decnum!.Value;
        }
        throw new ArgumentException("Invalid value: " + curid, nameof(curid));
    }
}
