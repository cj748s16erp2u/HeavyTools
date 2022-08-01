using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhZone.Test.Base;
using eLog.HeavyTools.Services.WhZone.Test.Fixtures;
using Xunit;

namespace eLog.HeavyTools.Services.WhZone.Test.Validators;

public class OlcWhzstockValidatorTest : TestBase<OlcWhzstock, IWhZStockService>
{
    private readonly int itemId;
    private readonly string whId;

    public OlcWhzstockValidatorTest(
        ITestOutputHelper testOutputHelper,
        TestFixture fixture) : base(testOutputHelper, fixture)
    {
        this.itemId = this.GetFirstItemIdAsync().GetAwaiter().GetResult() ?? throw new InvalidOperationException();
        this.whId = this.GetFirstWarehouseIdAsync(true).GetAwaiter().GetResult() ?? throw new InvalidOperationException();
    }

    [Fact]
    public async Task AddWrongItemIdTest()
    {
        var entity = new OlcWhzstock
        {
            Itemid = -1,
            Whid = this.whId,
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        Assert.Equal($"The item doesn't exists (item: {entity.Itemid})", ex.Message);
    }

    [Fact]
    public async Task AddWrongWhIdTest()
    {
        var entity = new OlcWhzstock
        {
            Itemid = this.itemId,
            Whid = "NONE",
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        Assert.Equal($"The warehouse doesn't exists (warehouse: {entity.Whid})", ex.Message);
    }

    [Fact]
    public async Task AddGoodZoneTest()
    {
        var whZoneId = this.GetFirstWhZoneIdAsync(this.whId).Result;

        var entity = new OlcWhzstock
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Whzoneid = whZoneId
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        await this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets);
    }

    [Fact]
    public async Task AddWrongZoneTest()
    {
        var entity = new OlcWhzstock
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Whzoneid = -1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        Assert.Equal($"The zone doesn't exists in the given warehouse: '{entity.Whid}' (zone: {entity.Whzoneid})", ex.Message);
    }

    [Fact]
    public async Task Add0Test()
    {
        var entity = new OlcWhzstock
        {
            Itemid = this.itemId,
            Whid = this.whId,
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        await this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    public async Task AddRecQtyTest(double actQty)
    {
        var entity = new OlcWhzstock
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Actqty = (decimal)actQty,
            Recqty = 5
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        await this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    public async Task AddResQtyTest(double actQty)
    {
        var entity = new OlcWhzstock
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Actqty = (decimal)actQty,
            Recqty = Math.Max(5 - (decimal)actQty, 0),
            Resqty = 5
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        await this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    public async Task AddResQtyFailedTest(double actQty)
    {
        var entity = new OlcWhzstock
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Actqty = (decimal)actQty,
            Resqty = 15
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        Assert.Equal($"The provisioned value is less than 0 (actQty: {entity.Actqty}, recQty: {entity.Recqty}, resQty: {entity.Resqty})", ex.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    public async Task AddActQtyTest(double actQty)
    {
        var entity = new OlcWhzstock
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Actqty = (decimal)actQty
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        await this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(-10)]
    public async Task AddActQtyFailed01Test(double actQty)
    {
        var entity = new OlcWhzstock
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Actqty = (decimal)actQty
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        Assert.Equal($"The actual value is less than 0 (actQty: {entity.Actqty}){Environment.NewLine}The provisioned value is less than 0 (actQty: {entity.Actqty}, recQty: {entity.Recqty}, resQty: {entity.Resqty})", ex.Message);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(-10)]
    public async Task AddActQtyFailed02Test(double actQty)
    {
        var entity = new OlcWhzstock
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Recqty = (decimal)(Math.Abs(actQty * 5)),
            Actqty = (decimal)actQty
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        Assert.Equal($"The actual value is less than 0 (actQty: {entity.Actqty})", ex.Message);
    }

    [Fact]
    public async Task UpdateWrongItemIdTest()
    {
        var origEntity = new OlcWhzstock
        {
            Itemid = this.itemId,
            Whid = this.whId,
        };

        var entity = origEntity.Clone<OlcWhzstock>();
        entity.Itemid = -1;

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, origEntity, ruleSets: ruleSets));
        Assert.Equal("Unable to change the item", ex.Message);
    }

    [Fact]
    public async Task UpdateWrongWhIdTest()
    {
        var origEntity = new OlcWhzstock
        {
            Itemid = this.itemId,
            Whid = this.whId,
        };

        var entity = origEntity.Clone<OlcWhzstock>();
        entity.Whid = "NONE";

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, origEntity, ruleSets: ruleSets));
        Assert.Equal("Unable to change the warehouse", ex.Message);
    }

    [Fact]
    public async Task UpdateWrongZoneTest()
    {
        var origEntity = new OlcWhzstock
        {
            Itemid = this.itemId,
            Whid = this.whId,
        };

        var entity = origEntity.Clone<OlcWhzstock>();
        entity.Whzoneid = -1;

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, origEntity, ruleSets: ruleSets));
        Assert.Equal("Unable to change the zone", ex.Message);
    }

    [Fact]
    public async Task Update0Test()
    {
        var entity = new OlcWhzstock
        {
            Itemid = this.itemId,
            Whid = this.whId,
        };

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        await this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    public async Task UpdateRecQtyTest(double actQty)
    {
        var entity = new OlcWhzstock
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Actqty = (decimal)actQty,
            Recqty = 5
        };

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        await this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    public async Task UpdateResQtyTest(double actQty)
    {
        var entity = new OlcWhzstock
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Actqty = (decimal)actQty,
            Recqty = Math.Max(5 - (decimal)actQty, 0),
            Resqty = 5
        };

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        await this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    public async Task UpdateResQtyFailedTest(double actQty)
    {
        var entity = new OlcWhzstock
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Actqty = (decimal)actQty,
            Resqty = 15
        };

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets));
        Assert.Equal($"The provisioned value is less than 0 (actQty: {entity.Actqty}, recQty: {entity.Recqty}, resQty: {entity.Resqty})", ex.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    public async Task UpdateActQtyTest(double actQty)
    {
        var entity = new OlcWhzstock
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Actqty = (decimal)actQty
        };

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        await this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(-10)]
    public async Task UpdateActQtyFailed01Test(double actQty)
    {
        var entity = new OlcWhzstock
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Actqty = (decimal)actQty
        };

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets));
        Assert.Equal($"The actual value is less than 0 (actQty: {entity.Actqty}){Environment.NewLine}The provisioned value is less than 0 (actQty: {entity.Actqty}, recQty: {entity.Recqty}, resQty: {entity.Resqty})", ex.Message);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(-10)]
    public async Task UpdateActQtyFailed02Test(double actQty)
    {
        var entity = new OlcWhzstock
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Recqty = (decimal)(Math.Abs(actQty * 5)),
            Actqty = (decimal)actQty
        };

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets));
        Assert.Equal($"The actual value is less than 0 (actQty: {entity.Actqty})", ex.Message);
    }

    [Fact]
    public async Task Delete0Test()
    {
        var entity = new OlcWhzstock
        {
            Itemid = this.itemId,
            Whid = this.whId,
        };

        var ruleSets = new[]
        {
            "Default",
            "Delete"
        };

        await this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets);
    }

    [Fact]
    public async Task DeleteActQtyFailedTest()
    {
        var entity = new OlcWhzstock
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Actqty = 5
        };

        var ruleSets = new[]
        {
            "Default",
            "Delete"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets));
        Assert.Equal($"Unable to delete stock if actual value is not 0 (actQty: {entity.Actqty})", ex.Message);
    }

    [Fact]
    public async Task DeleteRecQtyFailedTest()
    {
        var entity = new OlcWhzstock
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Recqty = 5
        };

        var ruleSets = new[]
        {
            "Default",
            "Delete"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets));
        Assert.Equal($"Unable to delete stock if receiving value is not 0 (recQty: {entity.Recqty})", ex.Message);
    }
}
