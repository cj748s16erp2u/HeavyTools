using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Interfaces; 
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.Test.Base;
using eLog.HeavyTools.Services.WhWebShop.Test.CartJsonGenerator;
using eLog.HeavyTools.Services.WhWebShop.Test.Fixtures;
using eLog.HeavyTools.Services.WhWebShop.Test.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace eLog.HeavyTools.Services.WhWebShop.Test.Services;

public class PriceCalcServiceTest : TestBase<OlcPriceCalcResult, IPriceCalcService>
{

    private IPriceCalcService? service;
    public PriceCalcServiceTest(ITestOutputHelper testOutputHelper, TestFixture fixture) : base(testOutputHelper, fixture)
    {
        
    }
     
      
    [Theory]
    [InlineData(3, 6)]
    [InlineData(-3, 6)]
    [InlineData(3, -6)]
    [InlineData(0, 6)]
    [InlineData(3, 1)]
    public void Test01Test(int x, int y)
    {
        var res = this.service.Calc(x, y);
        Assert.Equal(x * y * 5, res);
    }

    private static string JSON= @"
{ 
    ""Cart"": {
        ""Wid"": ""com"",
        ""Curid"": ""EUR"",
        ""FirstPurchase"": ""false"",
		""LoyaltyCardNo"": ""ABC12345678"",
        ""CountryId"": ""HU"",
		""Cupons"": [

            ""Cupon1"",
			""Cupon2""],
        ""Items"": [
            {
                ""CartId"": ""1"",
                ""ItemCode"": ""A2W22282NA.L"",
				""Quantity"": 4
            },{
                ""CartId"": ""2"",
                ""ItemCode"": ""C6S22145ST.L"",
				""Quantity"": 3
            },{
                ""CartId"": ""3"",
                ""ItemCode"": ""G3S20529RT.L"",
				""Quantity"": 4
            }]
	}
}
 ";

    private static string JSON2 = @"
{ 
    ""Cart"": {
        ""Wid"": ""com"",
        ""Curid"": ""EUR"",
        ""FirstPurchase"": ""false"",
		""LoyaltyCardNo"": ""ABC12345678"",
        ""CountryId"": ""HU"",
		""Cupons"": [

            ""Cupon1"",
			""Cupon2""],
        ""Items"": [
            {
                ""CartId"": ""1"",
                ""ItemCode"": ""A2W22282NA.L"",
				""Quantity"": 4
            }]
	}
}
 ";
 
    [Fact]
    public async void Test01Fix()
    {
        var jo = Newtonsoft.Json.Linq.JObject.Parse(JSON2);
        var res = await this.service.CalcJsonAsync(jo);            
        Assert.True(res.Success);
    }
 
    [Fact]
    public async void Test01Random()
    {
        var jo = Newtonsoft.Json.Linq.JObject.Parse(CartGenerator.GetRandomCart());
        var res = await this.service.CalcJsonAsync(jo);
        Assert.True(res.Success);
    }

    private async Task<CalcJsonResultDto?> TestIntl02Async(IPriceCalcService service, string json)
    {
        var jo = Newtonsoft.Json.Linq.JObject.Parse(json);
        var res = await service.CalcJsonAsync(jo);
        return res;
 
    }

    [Fact]
    public void StressTest01()
    {
        var scopeFactory = this._fixture.GetService<IServiceScopeFactory>(this._testOutputHelper);

        var list = new int[20].Select(i =>
        {
            var scope = scopeFactory!.CreateScope();
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

                    var res = await this.TestIntl02Async(s.Service!, CartGenerator.GetRandomCart());
                    Assert.True(res.Success);
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
