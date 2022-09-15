using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class InitializerBL
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services, IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.Configure<eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Options.CryptoOptions>(options => configuration
            .GetSection(eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Options.CryptoOptions.NAME)
            .Bind(options));

        services.Configure<eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Options.SordOptions>(options => configuration
            .GetSection(eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Options.SordOptions.NAME)
            .Bind(options));

        services.Configure<eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Options.OSSOptions>(options => configuration
        .GetSection(eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Options.OSSOptions.NAME)
        .Bind(options));

        RegisterImplementations(services);

        services.AddTransient(typeof(eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Validators.Base.EntityValidator<>));
        services.AddTransient(typeof(eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Validators.Base.BusinessEntityValidator<>));
        services.AddTransient(typeof(eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Validators.Base.BusinessMasterEntityValidator<>));

        return services;
    }

    private static void RegisterImplementations(IServiceCollection services)
    {
        var attrType = typeof(RegisterDIAttribute);
        var types = typeof(InitializerBL).Assembly.GetTypes();
        var typesWithAttr = types
            .Select(t => new { type = t, attrs = t.GetCustomAttributes(attrType, true) })
            .Where(t => t.attrs?.Any() == true);

        foreach (var t in typesWithAttr)
        {
            var attr = t.attrs.First() as RegisterDIAttribute;

            if (attr?.Interface is null)
            {
                throw new InvalidOperationException($"Invalid attribute implementation on type: {t.type.FullName}");
            }

            var descr = new ServiceDescriptor(attr.Interface, t.type, attr.Lifetime);
            services.Add(descr);
        }
    }
}