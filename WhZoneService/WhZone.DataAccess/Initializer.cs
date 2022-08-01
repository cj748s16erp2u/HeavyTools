using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        //services.AddScoped<eLog.HeavyTools.Services.WhZone.DataAccess.Context.WhZoneDbContext>();
        services.AddDbContext<eLog.HeavyTools.Services.WhZone.DataAccess.Context.WhZoneDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString(nameof(eLog.HeavyTools.Services.WhZone.DataAccess.Context.WhZoneDbContext));
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<eLog.HeavyTools.Services.WhZone.DataAccess.Repositories.Interfaces.IUnitOfWork, eLog.HeavyTools.Services.WhZone.DataAccess.Repositories.Base.UnitOfWork>();
        services.AddScoped(typeof(eLog.HeavyTools.Services.WhZone.DataAccess.Repositories.Interfaces.IRepository<>), typeof(eLog.HeavyTools.Services.WhZone.DataAccess.Repositories.Base.RepositoryBase<>));

        return services;
    }
}
