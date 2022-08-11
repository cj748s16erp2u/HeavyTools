using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.Test.Base;
using eLog.HeavyTools.Services.WhWebShop.Test.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace eLog.HeavyTools.Services.WhWebShop.Test.Services;

public class OrderTest : TestBase2<OlsSordhead, IOrderService>
{
    public OrderTest(ITestOutputHelper testOutputHelper, TestFixture fixture) : base(testOutputHelper, fixture)
    {
    } 
 

    [Fact]
    public void Test01()
    {
        var f = File.ReadAllText(@"c:\Munka\workspaces\eLogHeavyTools\eLogHeavyTools\order.txt");

        var res = this.service.CreateAsync(new Newtonsoft.Json.Linq.JObject(""), new OlcApilogger());
        Assert.True(res.Result.Success);

    }
}
