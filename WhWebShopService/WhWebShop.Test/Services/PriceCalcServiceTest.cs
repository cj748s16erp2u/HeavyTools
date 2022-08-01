using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.Test.Base;
using eLog.HeavyTools.Services.WhWebShop.Test.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace eLog.HeavyTools.Services.WhWebShop.Test.Services;

public class PriceCalcServiceTest : TestBase<OlcPriceCalcResult, IPriceCalcService>
{
    public PriceCalcServiceTest(ITestOutputHelper testOutputHelper, TestFixture fixture) : base(testOutputHelper, fixture)
    {
    }

    [Theory]
    [InlineData(3, 6)]
    [InlineData(-3, 6)]
    [InlineData(3, -6)]
    [InlineData(0, 6)]
    [InlineData(3, 1)]
    public void Test01(int x, int y)
    {
        var res = this.service.Calc(x, y);
        Assert.Equal(x * y * 5, res);
    }

    [Theory]
    [InlineData(3, 6)]
    [InlineData(-3, 6)]
    [InlineData(3, -6)]
    [InlineData(0, 6)]
    [InlineData(3, 1)]
    public async Task Test02Async(int x, int y)
    {
        var res = await this.TestIntl02Async(this.service, x, y);

        if (res is not null)
        {
            var entity = await this.dbContext.OlcPriceCalcResults.FirstOrDefaultAsync(r => r.Id == res.Id);
            Assert.NotNull(entity);
            Assert.Equal(res.Result, entity!.Value);
        }
    }

    private async Task<CalcResultDto?> TestIntl02Async(IPriceCalcService service, int x, int y)
    {
        var parms = new CalcParamsDto
        {
            X = x,
            Y = y
        };

        if (x > y)
        {
            var ex = await Assert.ThrowsAnyAsync<Exception>(() => service.CalcAsync(parms));
            Assert.Contains("Parameter 'y less than x'", ex.Message);
            return null;
        }
        else
        {
            var res = await service.CalcAsync(parms);
            Assert.NotNull(res);
            Assert.NotNull(res.Id);
            return res;
        }
    }

    [Fact]
    public void StressTest01()
    {
        var scopeFactory = this._fixture.GetService<IServiceScopeFactory>(this._testOutputHelper);

        var list = new int[20].Select(i =>
        {
            var scope = scopeFactory.CreateScope();
            return new Worker<OlcPriceCalcResult, IPriceCalcService>
            {
                Scope = scope,
                Service = scope.ServiceProvider.GetService<IPriceCalcService>()
            };
        }).ToList();

        var parOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = 4
        };

        var rnd = new Random();
        var arr = new int[5000];
        var result = Parallel.ForEach(arr, parOptions, (i, state) =>
        {
            var task = Task.Run(async () =>
            {
                Worker<OlcPriceCalcResult, IPriceCalcService>? s = null!;
                while (s is null)
                {
                    lock (list)
                    {
                        s = list.FirstOrDefault(l => !l.Used);
                        if (s is not null)
                        {
                            s.Used = true;
                        }
                    }

                    if (s is null)
                    {
                        await Task.Delay(30);
                    }
                }

                try
                {
                    var x = rnd.Next(255);
                    var y = rnd.Next(255);

                    if (x > y)
                    {
                        (x, y) = (y, x);
                    }

                    await this.TestIntl02Async(s.Service, x, y);
                }
                finally
                {
                    s.Used = false;
                }
            });

            task.Wait();
        });

        foreach (var w in list)
        {
            w.Scope.Dispose();
        }
    }

    class Worker<TEntity, TService>
        where TEntity : class, IEntity
        where TService : ILogicService<TEntity>
    {
        public Task? Task { get; set; }
        public IServiceScope? Scope { get; set; }
        public TService? Service { get; set; }
        public bool Used { get; set; } = false;
    }
}
