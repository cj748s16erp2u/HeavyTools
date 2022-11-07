using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Microsoft.Extensions.DependencyInjection;

public static class InitializerE
{
    public static IServiceCollection AddBusinessEntities(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        RegisterAutoMapper(services);

        return services;
    }

    private static void RegisterAutoMapper(IServiceCollection services)
    {
        var asm = typeof(InitializerE).Assembly;
        //var config = new MapperConfiguration(cfg => cfg.AddMaps(asm));
        //config.AssertConfigurationIsValid();
        //config.CompileMappings();
        services.AddAutoMapper(asm);
    }
}
