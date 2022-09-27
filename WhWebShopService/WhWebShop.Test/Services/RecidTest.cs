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

public class RecidTest : TestBase2<OlsRecid, IOlsRecidService>
{
    public RecidTest(ITestOutputHelper testOutputHelper, TestFixture fixture) : base(testOutputHelper, fixture)
    {
    }

    [Fact]
    public void GenerateNextRecid()
    {
        var riid = "SordHead.SordID";

        var c = this.service.GetCurrentAsync(riid, default);
        Assert.NotNull(c);
        Assert.NotNull(c.Result);
        var e = this.service.GetNewIdAsync(riid, default);
        Assert.NotNull(e);
        Assert.NotNull(e.Result);

        Assert.Equal(c.Result!.Lastid + 1, e.Result!.Lastid);

    }
}
