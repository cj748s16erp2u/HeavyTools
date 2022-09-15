using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class InitializerDA
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddDbContext<WhWebShopDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString(nameof(WhWebShopDbContext));
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Interfaces.IUnitOfWork, eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Base.UnitOfWork>();
        services.AddScoped(typeof(eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Interfaces.IRepository<>), typeof(eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Base.RepositoryBase<>));

        return services;
    }
}
