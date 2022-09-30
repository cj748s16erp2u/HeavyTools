using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit.Microsoft.DependencyInjection;

namespace eLog.HeavyTools.Services.WhZone.Test.Fixtures;

public class TestFixture : TestBedFixture
{
    internal const int TimeoutSeconds = 300 * 1000; // 300s = 5m

    protected override void AddServices(IServiceCollection services, IConfiguration? configuration)
    {
        services
            .AddBusinessEntities()
            .AddDataAccess(configuration!)
            .AddBusinessLogic(configuration!);

        var descriptor = new ServiceDescriptor(
            typeof(BusinessLogic.Services.Interfaces.IEnvironmentService),
            typeof(EnvironmentService),
            ServiceLifetime.Scoped);
        services.Replace(descriptor);
    }

    protected override ValueTask DisposeAsyncCore() => new();

    protected override IEnumerable<TestAppSettings> GetTestAppSettings()
    {
        yield return new() { Filename = "appsettings.json", IsOptional = false };
    }
}
