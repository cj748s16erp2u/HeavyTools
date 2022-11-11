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



[RegisterDI(Interface = typeof(ICountryCacheService))]
internal class CountryCacheService : OlsCountryService, ICountryCacheService
{
    public static ConcurrentDictionary<string, OlsCountry> Cache = new ConcurrentDictionary<string, OlsCountry>();
     
    public CountryCacheService(IValidator<OlsCountry> validator,
                               IRepository<OlsCountry> repository,
                               IUnitOfWork unitOfWork,
                               IEnvironmentService environmentService) : base(validator, repository, unitOfWork, environmentService)
    {
    }

    public async Task<OlsCountry> GetCountryAsync(string countryid, CancellationToken cancellationToken)
    {
        if (countryid != null)
        {
            if (Cache.TryGetValue(countryid, out var i))
            {
                return i;
            }

            var e = await this.Repository.Entities.FirstOrDefaultAsync(p => p.Countryid == countryid, cancellationToken);
            if (e is not null)
            {
                Cache.TryAdd(countryid, e);
                return e;
            }
        }
        throw new ArgumentException("Invalid value: " + countryid, nameof(countryid));
    }
}
