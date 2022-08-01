using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.Test.Fixtures;
using Xunit;

namespace eLog.HeavyTools.Services.WhWebShop.Test.Helpers;

public class UtilsTest : TestBed<TestFixture>
{
    public UtilsTest(ITestOutputHelper testOutputHelper, TestFixture fixture) : base(testOutputHelper, fixture)
    {
    }

    [Theory]
    [InlineData(null)]
    [InlineData("q")]
    public void ToSqlStringTest(string prefix)
    {
        var testObj = new
        {
            Name = "TestName",
        };

        var sql = Utils.ToSql(testObj, t => t.Name, prefix);
        if (!string.IsNullOrWhiteSpace(prefix))
        {
            prefix = $"[{prefix}].";
        }

        Assert.Equal($"{prefix}[{nameof(testObj.Name).ToLowerInvariant()}] = '{testObj.Name}'", sql);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("q")]
    public void ToSqlDateTimeTest(string prefix)
    {
        var testObj = new
        {
            TranDate = DateTime.Now,
        };

        var sql = Utils.ToSql(testObj, t => t.TranDate, prefix);
        if (!string.IsNullOrWhiteSpace(prefix))
        {
            prefix = $"[{prefix}].";
        }

        Assert.Equal($"{prefix}[{nameof(testObj.TranDate).ToLowerInvariant()}] = '{testObj.TranDate:yyyyMMdd HH:mm:ss}'", sql);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("q")]
    public void ToSqlIntTest(string prefix)
    {
        var testObj = new
        {
            Qty = 1,
        };

        var sql = Utils.ToSql(testObj, t => t.Qty, prefix);
        if (!string.IsNullOrWhiteSpace(prefix))
        {
            prefix = $"[{prefix}].";
        }

        Assert.Equal($"{prefix}[{nameof(testObj.Qty).ToLowerInvariant()}] = {testObj.Qty}", sql);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("q")]
    public void ToSqlDecimalTest(string prefix)
    {
        var testObj = new
        {
            Price = 1.25M,
        };

        var sql = Utils.ToSql(testObj, t => t.Price, prefix);
        if (!string.IsNullOrWhiteSpace(prefix))
        {
            prefix = $"[{prefix}].";
        }

        Assert.Equal($"{prefix}[{nameof(testObj.Price).ToLowerInvariant()}] = {testObj.Price}", sql);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("q")]
    public void ToSqlBoolTest(string prefix)
    {
        var testObj = new
        {
            Delstat = false,
        };

        var sql = Utils.ToSql(testObj, t => t.Delstat, prefix);
        if (!string.IsNullOrWhiteSpace(prefix))
        {
            prefix = $"[{prefix}].";
        }

        Assert.Equal($"{prefix}[{nameof(testObj.Delstat).ToLowerInvariant()}] = {Convert.ToInt16(testObj.Delstat)}", sql);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("q")]
    public void ToSqlKeyTest(string prefix)
    {
        var testObj = new
        {
            Name = "TestName",
            TranDate = DateTime.Now,
            Qty = 1,
            Price = 1.25M,
            Delstat = false,
        };

        var sql = Utils.ToSql(testObj, t => new { t.Name, t.TranDate, t.Qty, t.Price, t.Delstat }, prefix);
        if (!string.IsNullOrWhiteSpace(prefix))
        {
            prefix = $"[{prefix}].";
        }

        Assert.Equal($"{prefix}[{nameof(testObj.Name).ToLowerInvariant()}] = '{testObj.Name}' and " +
            $"{prefix}[{nameof(testObj.TranDate).ToLowerInvariant()}] = '{testObj.TranDate:yyyyMMdd HH:mm:ss}' and " +
            $"{prefix}[{nameof(testObj.Qty).ToLowerInvariant()}] = {testObj.Qty} and " +
            $"{prefix}[{nameof(testObj.Price).ToLowerInvariant()}] = {testObj.Price} and " +
            $"{prefix}[{nameof(testObj.Delstat).ToLowerInvariant()}] = {Convert.ToInt16(testObj.Delstat)}", sql);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("q")]
    public void ToSqlAllTest(string prefix)
    {
        var testObj = new
        {
            Name = "TestName",
            TranDate = DateTime.Now,
            Qty = 1,
            Price = 1.25M,
            Delstat = false,
        };

        var sql = Utils.ToSql(testObj, prefix);
        if (!string.IsNullOrWhiteSpace(prefix))
        {
            prefix = $"[{prefix}].";
        }

        Assert.Equal($"{prefix}[{nameof(testObj.Name).ToLowerInvariant()}] = '{testObj.Name}' and " +
            $"{prefix}[{nameof(testObj.TranDate).ToLowerInvariant()}] = '{testObj.TranDate:yyyyMMdd HH:mm:ss}' and " +
            $"{prefix}[{nameof(testObj.Qty).ToLowerInvariant()}] = {testObj.Qty} and " +
            $"{prefix}[{nameof(testObj.Price).ToLowerInvariant()}] = {testObj.Price} and " +
            $"{prefix}[{nameof(testObj.Delstat).ToLowerInvariant()}] = {Convert.ToInt16(testObj.Delstat)}", sql);
    }
}
