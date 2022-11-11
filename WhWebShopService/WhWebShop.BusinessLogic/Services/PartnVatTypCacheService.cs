using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
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

[RegisterDI(Interface = typeof(IPartnVatTypCacheService))]
internal class PartnVatTypCacheService : OlsPartnvattypService, IPartnVatTypCacheService
{

    public static ConcurrentDictionary<string, OlsPartnvattyp> Cache = new ConcurrentDictionary<string, OlsPartnvattyp>();

    public PartnVatTypCacheService(IValidator<OlsPartnvattyp> validator,
                                   IRepository<OlsPartnvattyp> repository,
                                   IUnitOfWork unitOfWork,
                                   IEnvironmentService environmentService) : base(validator, repository, unitOfWork, environmentService)
    {
    }

    public async Task<OlsPartnvattyp> GetAsync(string? ptvattypid, CancellationToken cancellationToken = default)
    {
        if (ptvattypid != null)
        {
            if (Cache.TryGetValue(ptvattypid, out var i))
            {
                return i;
            }

            var e = await this.Repository.Entities.FirstOrDefaultAsync(p => p.Ptvattypid == ptvattypid, cancellationToken);
            if (e is not null)
            {
                Cache.TryAdd(ptvattypid, e);
                return e;
            }
        }
        throw new ArgumentException("Invalid value: " + ptvattypid, nameof(ptvattypid));
    }
}
