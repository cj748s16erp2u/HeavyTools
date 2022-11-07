using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhZone.DataAccess.Repositories.Interfaces;
using eLog.HeavyTools.Services.WhZone.Test.Base;
using eLog.HeavyTools.Services.WhZone.Test.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace eLog.HeavyTools.Services.WhZone.Test.Validators;

public class OlcWhzstockmapValidatorTest : TestBase<OlcWhzstockmap, IWhZStockMapService>
{
    private readonly int itemId;
    private readonly string whIdNoZoneNoLoc;
    private readonly string whIdNoZoneWithLoc;
    private readonly string whIdWithZoneNoLoc;
    private readonly string whIdWithZoneWithLoc;
    private readonly int whZoneIdNoLoc;
    private readonly int whZoneIdWithLoc;
    private readonly int whLocId;
    private readonly int whLocIdNoZone;

    public OlcWhzstockmapValidatorTest(
        ITestOutputHelper testOutputHelper,
        TestFixture fixture) : base(testOutputHelper, fixture)
    {
        this.itemId = this.GetFirstItemIdAsync().GetAwaiter().GetResult() ?? throw new InvalidOperationException();

        this.whIdNoZoneNoLoc = this.GetFirstWarehouseIdAsync(false, false).GetAwaiter().GetResult() ?? throw new InvalidOperationException();
        this.whIdNoZoneWithLoc = this.GetFirstWarehouseIdAsync(false, true).GetAwaiter().GetResult() ?? throw new InvalidOperationException();
        this.whIdWithZoneNoLoc = this.GetFirstWarehouseIdAsync(true, false).GetAwaiter().GetResult() ?? throw new InvalidOperationException();
        this.whIdWithZoneWithLoc = this.GetFirstWarehouseIdAsync(true, true).GetAwaiter().GetResult() ?? throw new InvalidOperationException();

        this.whZoneIdNoLoc = this.GetFirstWhZoneIdAsync(this.whIdWithZoneNoLoc, true).GetAwaiter().GetResult() ?? throw new InvalidOperationException();
        this.whZoneIdWithLoc = this.GetFirstWhZoneIdAsync(this.whIdWithZoneWithLoc, true).GetAwaiter().GetResult() ?? throw new InvalidOperationException();

        this.whLocId = this.GetFirstWhLocIdAsync(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc).GetAwaiter().GetResult() ?? throw new InvalidOperationException();
        this.whLocIdNoZone = this.GetFirstWhLocIdAsync(this.whIdNoZoneWithLoc, null).GetAwaiter().GetResult() ?? throw new InvalidOperationException();
    }

    #region Add...

    #region Generic tests

    [Fact]
    public async Task AddWrongItemIdTest()
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = -1,
            Whid = this.whIdWithZoneNoLoc,
            Whzoneid = this.whZoneIdWithLoc,
            Whlocid = this.whLocId,
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The item doesn't exists (item: {entity.Itemid})";
        Assert.Contains(message, ex.Message);
    }

    [Fact]
    public async Task AddWrongWhIdTest()
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = "NONE",
            //Whzoneid = this.whZoneId,
            Whlocid = this.whLocId,
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The warehouse doesn't exists (warehouse: {entity.Whid})\r\nThe location doesn't exists in the given warehouse/zone: '{entity.Whid}'/{entity.Whzoneid} (location: {entity.Whlocid})";
        Assert.Contains(message, ex.Message);
    }

    [Fact]
    public async Task AddWhTest()
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whIdNoZoneNoLoc,
            Recqty = 0
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        await this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets);
    }

    [Fact]
    public async Task AddWhFailed01Test()
    {
        var whLocId = await this.GetFirstWhLocIdAsync(this.whIdNoZoneNoLoc, null);
        Assert.NotNull(whLocId);

        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whIdNoZoneNoLoc,
            Whlocid = whLocId!.Value,
            Recqty = 0,
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The warehouse doesn't allow to handle location (warehouse: {entity.Whid})";
        Assert.Contains(message, ex.Message);
    }

    [Fact]
    public async Task AddWhFailed02Test()
    {
        var whLocId = await this.GetFirstWhLocIdAsync(this.whIdNoZoneWithLoc, null);
        Assert.NotNull(whLocId);

        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whIdNoZoneNoLoc,
            Whlocid = whLocId!.Value,
            Recqty = 0,
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The location doesn't exists in the given warehouse/zone: '{entity.Whid}'/{entity.Whzoneid} (location: {entity.Whlocid})";
        Assert.Contains(message, ex.Message);
    }

    [Fact]
    public async Task AddWhLocTest()
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whIdNoZoneWithLoc,
            Whlocid = this.whLocIdNoZone,
            Recqty = 0
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        await this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets);
    }

    [Fact]
    public async Task AddWhLocFailed01Test()
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whIdNoZoneWithLoc,
            Recqty = 0
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The warehouse requires to handle location (warehouse: {entity.Whid})";
        Assert.Contains(message, ex.Message);
    }

    [Fact]
    public async Task AddWhLocFailed02Test()
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whIdNoZoneWithLoc,
            Whlocid = this.whLocId,
            Recqty = 0
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The location doesn't exists in the given warehouse/zone: '{entity.Whid}'/{entity.Whzoneid} (location: {entity.Whlocid})";
        Assert.Contains(message, ex.Message);
    }

    [Fact]
    public async Task AddWhZoneTest()
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whIdWithZoneNoLoc,
            Whzoneid = this.whZoneIdNoLoc,
            Recqty = 0
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        await this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets);
    }

    [Fact]
    public async Task AddWhZoneFailed01Test()
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whIdWithZoneNoLoc,
            Whzoneid = this.whZoneIdWithLoc,
            Recqty = 0
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The zone doesn't exists in the given warehouse: '{entity.Whid}' (zone: {entity.Whzoneid})";
        Assert.Contains(message, ex.Message);
    }

    [Fact]
    public async Task AddWhZoneFailed02Test()
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whIdWithZoneNoLoc,
            Whzoneid = this.whZoneIdNoLoc,
            Whlocid = this.whLocIdNoZone,
            Recqty = 0
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The location doesn't exists in the given warehouse/zone: '{entity.Whid}'/{entity.Whzoneid} (location: {entity.Whlocid})";
        Assert.Contains(message, ex.Message);

        message = $"The warehouse doesn't allow to handle location (warehouse: {entity.Whid})";
        Assert.Contains(message, ex.Message);
    }

    [Fact]
    public async Task AddWhZoneLocTest()
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whIdWithZoneWithLoc,
            Whzoneid = this.whZoneIdWithLoc,
            Whlocid = this.whLocId,
            Recqty = 0
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        await this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets);
    }

    [Fact]
    public async Task AddWhZoneLocFailed01Test()
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whIdWithZoneWithLoc,
            Whzoneid = this.whZoneIdWithLoc,
            Recqty = 0
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The warehouse requires to handle location (warehouse: {entity.Whid})";
        Assert.Contains(message, ex.Message);
    }

    [Fact]
    public async Task AddWhZoneLocFailed02Test()
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whIdWithZoneWithLoc,
            Whzoneid = this.whZoneIdWithLoc,
            Whlocid = this.whLocIdNoZone,
            Recqty = 0
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The location doesn't exists in the given warehouse/zone: '{entity.Whid}'/{entity.Whzoneid} (location: {entity.Whlocid})";
        Assert.Contains(message, ex.Message);
    }

    #endregion

    #region ActQty tests

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    public Task AddActQty01Test(double actQty)
    {
        return this.AddActQtyTest(this.whIdNoZoneNoLoc, null, null, (decimal)actQty);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    public Task AddActQty02Test(double actQty)
    {
        return this.AddActQtyTest(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal)actQty);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    public Task AddActQty03Test(double actQty)
    {
        return this.AddActQtyTest(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal)actQty);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    public Task AddActQty04Test(double actQty)
    {
        return this.AddActQtyTest(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal)actQty);
    }

    private async Task AddActQtyTest(string whId, int? whZoneId, int? whLocId, decimal actQty)
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Actqty = actQty
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
    public Task AddActQtyFailed01Test(double actQty)
    {
        return this.AddActQtyFailedTest(this.whIdNoZoneNoLoc, null, null, (decimal)actQty);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(-10)]
    public Task AddActQtyFailed02Test(double actQty)
    {
        return this.AddActQtyFailedTest(this.whIdWithZoneNoLoc, this.whZoneIdWithLoc, null, (decimal)actQty);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(-10)]
    public Task AddActQtyFailed03Test(double actQty)
    {
        return this.AddActQtyFailedTest(this.whIdNoZoneWithLoc, null, this.whLocId, (decimal)actQty);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(-10)]
    public Task AddActQtyFailed04Test(double actQty)
    {
        return this.AddActQtyFailedTest(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal)actQty);
    }

    private async Task AddActQtyFailedTest(string whId, int? whZoneId, int? whLocId, decimal actQty)
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Actqty = actQty
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The actual value is less than 0 (actQty: {entity.Actqty})";
        Assert.Contains(message, ex.Message);

        message = $"{Environment.NewLine}The provisioned value is less than 0 (actQty: {entity.Actqty}, recQty: {entity.Recqty}, resQty: {entity.Resqty})";
        Assert.Contains(message, ex.Message);
    }

    #endregion

    #region RecQty tests

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(10.55)]
    public Task AddRecQty01Test(double actQty)
    {
        return this.AddRecQtyTest(this.whIdNoZoneNoLoc, null, null, (decimal)actQty);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(10.55)]
    public Task AddRecQty02Test(double actQty)
    {
        return this.AddRecQtyTest(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal)actQty);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(10.55)]
    public Task AddRecQty03Test(double actQty)
    {
        return this.AddRecQtyTest(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal)actQty);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(10.55)]
    public Task AddRecQty04Test(double actQty)
    {
        return this.AddRecQtyTest(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal)actQty);
    }

    private async Task AddRecQtyTest(string whId, int? whZoneId, int? whLocId, decimal actQty)
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Actqty = actQty,
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
    [InlineData(10.55)]
    public Task AddRecQtyFailed01Test(double actQty)
    {
        return this.AddRecQtyFailedTest(this.whIdNoZoneNoLoc, null, null, (decimal)actQty);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(10.55)]
    public Task AddRecQtyFailed02Test(double actQty)
    {
        return this.AddRecQtyFailedTest(this.whIdWithZoneNoLoc, this.whZoneIdWithLoc, null, (decimal)actQty);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(10.55)]
    public Task AddRecQtyFailed03Test(double actQty)
    {
        return this.AddRecQtyFailedTest(this.whIdNoZoneWithLoc, null, this.whLocId, (decimal)actQty);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(10.55)]
    public Task AddRecQtyFailed04Test(double actQty)
    {
        return this.AddRecQtyFailedTest(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal)actQty);
    }

    private async Task AddRecQtyFailedTest(string whId, int? whZoneId, int? whLocId, decimal actQty)
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Actqty = actQty,
            Recqty = -5
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The receiving value is less than 0 (recQty: {(decimal)entity.Recqty})";
        Assert.Contains(message, ex.Message);
    }

    #endregion

    #region ResQty tests

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(10.55)]
    public Task AddResQty01Test(double actQty)
    {
        return this.AddResQtyTest(this.whIdNoZoneNoLoc, null, null, (decimal)actQty);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(10.55)]
    public Task AddResQty02Test(double actQty)
    {
        return this.AddResQtyTest(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal)actQty);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(10.55)]
    public Task AddResQty03Test(double actQty)
    {
        return this.AddResQtyTest(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal)actQty);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(10.55)]
    public Task AddResQty04Test(double actQty)
    {
        return this.AddResQtyTest(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal)actQty);
    }

    private async Task AddResQtyTest(string whId, int? whZoneId, int? whLocId, decimal actQty)
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Actqty = actQty,
            Recqty = Math.Max(5 - actQty, 0),
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
    [InlineData(10.55)]
    public Task AddResQtyFailed01Test(double actQty)
    {
        return this.AddResQtyFailedTest01(this.whIdNoZoneNoLoc, null, null, (decimal)actQty);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(10.55)]
    public Task AddResQtyFailed02Test(double actQty)
    {
        return this.AddResQtyFailedTest01(this.whIdWithZoneNoLoc, this.whZoneIdWithLoc, null, (decimal)actQty);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(10.55)]
    public Task AddResQtyFailed03Test(double actQty)
    {
        return this.AddResQtyFailedTest01(this.whIdNoZoneWithLoc, null, this.whLocId, (decimal)actQty);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(10.55)]
    public Task AddResQtyFailed04Test(double actQty)
    {
        return this.AddResQtyFailedTest01(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal)actQty);
    }

    private async Task AddResQtyFailedTest01(string whId, int? whZoneId, int? whLocId, decimal actQty)
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Actqty = actQty,
            Resqty = 15
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The provisioned value is less than 0 (actQty: {entity.Actqty}, recQty: {entity.Recqty}, resQty: {entity.Resqty})";
        Assert.Contains(message, ex.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(10.55)]
    public Task AddResQtyFailed05Test(double actQty)
    {
        return this.AddResQtyFailedTest02(this.whIdNoZoneNoLoc, null, null, (decimal)actQty);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(10.55)]
    public Task AddResQtyFailed06Test(double actQty)
    {
        return this.AddResQtyFailedTest02(this.whIdWithZoneNoLoc, this.whZoneIdWithLoc, null, (decimal)actQty);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(10.55)]
    public Task AddResQtyFailed07Test(double actQty)
    {
        return this.AddResQtyFailedTest02(this.whIdNoZoneWithLoc, null, this.whLocId, (decimal)actQty);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(10.55)]
    public Task AddResQtyFailed08Test(double actQty)
    {
        return this.AddResQtyFailedTest02(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal)actQty);
    }

    private async Task AddResQtyFailedTest02(string whId, int? whZoneId, int? whLocId, decimal actQty)
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Actqty = actQty,
            Resqty = -15
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The reserved value is less than 0 (resQty: {entity.Resqty})";
        Assert.Contains(message, ex.Message);
    }

    #endregion

    #endregion

    #region Update...

    #region Generic tests

    [Fact]
    public async Task UpdateWrongItemIdTest()
    {
        var origEntity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whIdWithZoneWithLoc,
            Whzoneid = this.whZoneIdWithLoc,
            Whlocid = this.whLocId,
        };

        var entity = origEntity.Clone<OlcWhzstockmap>();
        entity.Itemid = -1;

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, origEntity, ruleSets: ruleSets));
        Assert.Contains("Unable to change the item", ex.Message);
    }

    [Fact]
    public async Task UpdateWrongWhIdTest()
    {
        var origEntity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whIdWithZoneWithLoc,
            Whzoneid = this.whZoneIdWithLoc,
            Whlocid = this.whLocId,
        };

        var entity = origEntity.Clone<OlcWhzstockmap>();
        entity.Whid = "NONE";

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, origEntity, ruleSets: ruleSets));
        Assert.Contains("Unable to change the warehouse", ex.Message);
    }

    [Fact]
    public async Task UpdateWrongZoneTest()
    {
        var origEntity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whIdWithZoneWithLoc,
            Whzoneid = this.whZoneIdWithLoc,
            Whlocid = this.whLocId,
        };

        var entity = origEntity.Clone<OlcWhzstockmap>();
        entity.Whzoneid = -1;

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, origEntity, ruleSets: ruleSets));
        Assert.Contains("Unable to change the zone", ex.Message);
    }

    [Fact]
    public async Task UpdateWrongLocationTest()
    {
        var origEntity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whIdWithZoneWithLoc,
            Whzoneid = this.whZoneIdWithLoc,
            Whlocid = this.whLocId,
        };

        var entity = origEntity.Clone<OlcWhzstockmap>();
        entity.Whlocid = -1;

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, origEntity, ruleSets: ruleSets));
        Assert.Contains("Unable to change the location", ex.Message);
    }

    #endregion

    #region nochange tests

    [Fact]
    public Task Update001Test()
    {
        return this.Update0Test(this.whIdNoZoneNoLoc, null, null);
    }

    [Fact]
    public Task Update002Test()
    {
        return this.Update0Test(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null);
    }

    [Fact]
    public Task Update003Test()
    {
        return this.Update0Test(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone);
    }

    [Fact]
    public Task Update004Test()
    {
        return this.Update0Test(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId);
    }

    private async Task Update0Test(string whId, int? whZoneId, int? whLocId)
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
        };

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        await this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets);
    }

    #endregion

    #region ActQty tests

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(10.55)]
    public Task UpdateActQty01Test(double actQty)
    {
        return this.UpdateActQtyTest(this.whIdNoZoneNoLoc, null, null, (decimal)actQty);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(10.55)]
    public Task UpdateActQty02Test(double actQty)
    {
        return this.UpdateActQtyTest(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal)actQty);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(10.55)]
    public Task UpdateActQty03Test(double actQty)
    {
        return this.UpdateActQtyTest(this.whIdNoZoneWithLoc, null, this.whLocId, (decimal)actQty);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(10.55)]
    public Task UpdateActQty04Test(double actQty)
    {
        return this.UpdateActQtyTest(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal)actQty);
    }

    private async Task UpdateActQtyTest(string whId, int? whZoneId, int? whLocId, decimal actQty)
    {
        var originalEntity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Actqty = 5
        };

        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Actqty = (decimal)actQty
        };

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        await this.service.ValidateAndThrowAsync(entity, originalEntity, ruleSets: ruleSets);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(-10)]
    [InlineData(-10.55)]
    public Task UpdateActQtyFailed01Test(double actQty)
    {
        return this.UpdateActQtyFailedTest01(this.whIdWithZoneWithLoc, null, null, (decimal)actQty);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(-10)]
    [InlineData(-10.55)]
    public Task UpdateActQtyFailed02Test(double actQty)
    {
        return this.UpdateActQtyFailedTest01(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal)actQty);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(-10)]
    [InlineData(-10.55)]
    public Task UpdateActQtyFailed03Test(double actQty)
    {
        return this.UpdateActQtyFailedTest01(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal)actQty);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(-10)]
    [InlineData(-10.55)]
    public Task UpdateActQtyFailed04Test(double actQty)
    {
        return this.UpdateActQtyFailedTest01(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal)actQty);
    }

    private async Task UpdateActQtyFailedTest01(string whId, int? whZoneId, int? whLocId, decimal actQty)
    {
        var originalEntity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Actqty = 5
        };

        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Actqty = actQty
        };

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, originalEntity, ruleSets: ruleSets));
        var message = $"The actual value is less than 0 (actQty: {entity.Actqty})";
        Assert.Contains(message, ex.Message);

        message = $"The provisioned value is less than 0 (actQty: {entity.Actqty}, recQty: {entity.Recqty}, resQty: {entity.Resqty})";
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(-10)]
    [InlineData(-10.55)]
    public Task UpdateActQtyFailed05Test(double actQty)
    {
        return this.UpdateActQtyFailedTest02(this.whIdNoZoneNoLoc, null, null, (decimal)actQty);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(-10)]
    [InlineData(-10.55)]
    public Task UpdateActQtyFailed06Test(double actQty)
    {
        return this.UpdateActQtyFailedTest02(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal)actQty);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(-10)]
    [InlineData(-10.55)]
    public Task UpdateActQtyFailed07Test(double actQty)
    {
        return this.UpdateActQtyFailedTest02(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal)actQty);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(-10)]
    [InlineData(-10.55)]
    public Task UpdateActQtyFailed08Test(double actQty)
    {
        return this.UpdateActQtyFailedTest02(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal)actQty);
    }

    private async Task UpdateActQtyFailedTest02(string whId, int? whZoneId, int? whLocId, decimal actQty)
    {
        var originalEntity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Recqty = Math.Abs(actQty * 5),
            Actqty = 5
        };

        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Recqty = Math.Abs(actQty * 5),
            Actqty = actQty
        };

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, originalEntity, ruleSets: ruleSets));
        var message = $"The actual value is less than 0 (actQty: {entity.Actqty})";
        Assert.Contains(message, ex.Message);
    }

    #endregion

    #region RecQty tests

    [Theory]
    [InlineData(0, 0, 5)]
    [InlineData(2, 0, 5)]
    [InlineData(10, 0, 5)]
    [InlineData(10.55, 0, 5)]
    [InlineData(0, 5, 0)]
    [InlineData(2, 5, 0)]
    [InlineData(10, 5, 0)]
    [InlineData(10.55, 5, 0)]
    [InlineData(0, 2, 5)]
    [InlineData(2, 2, 5)]
    [InlineData(10, 2, 5)]
    [InlineData(10.55, 2, 5)]
    [InlineData(0, 5, 2)]
    [InlineData(2, 5, 2)]
    [InlineData(10, 5, 2)]
    [InlineData(10.55, 5, 2)]
    public Task UpdateRecQty01Test(double actQty, double originalRecQty, double recQty)
    {
        return this.UpdateRecQtyTest(this.whIdNoZoneNoLoc, null, null, (decimal)actQty, (decimal)originalRecQty, (decimal)recQty);
    }

    [Theory]
    [InlineData(0, 0, 5)]
    [InlineData(2, 0, 5)]
    [InlineData(10, 0, 5)]
    [InlineData(10.55, 0, 5)]
    [InlineData(0, 5, 0)]
    [InlineData(2, 5, 0)]
    [InlineData(10, 5, 0)]
    [InlineData(10.55, 5, 0)]
    [InlineData(0, 2, 5)]
    [InlineData(2, 2, 5)]
    [InlineData(10, 2, 5)]
    [InlineData(10.55, 2, 5)]
    [InlineData(0, 5, 2)]
    [InlineData(2, 5, 2)]
    [InlineData(10, 5, 2)]
    [InlineData(10.55, 5, 2)]
    public Task UpdateRecQty02Test(double actQty, double originalRecQty, double recQty)
    {
        return this.UpdateRecQtyTest(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal)actQty, (decimal)originalRecQty, (decimal)recQty);
    }

    [Theory]
    [InlineData(0, 0, 5)]
    [InlineData(2, 0, 5)]
    [InlineData(10, 0, 5)]
    [InlineData(10.55, 0, 5)]
    [InlineData(0, 5, 0)]
    [InlineData(2, 5, 0)]
    [InlineData(10, 5, 0)]
    [InlineData(10.55, 5, 0)]
    [InlineData(0, 2, 5)]
    [InlineData(2, 2, 5)]
    [InlineData(10, 2, 5)]
    [InlineData(10.55, 2, 5)]
    [InlineData(0, 5, 2)]
    [InlineData(2, 5, 2)]
    [InlineData(10, 5, 2)]
    [InlineData(10.55, 5, 2)]
    public Task UpdateRecQty03Test(double actQty, double originalRecQty, double recQty)
    {
        return this.UpdateRecQtyTest(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal)actQty, (decimal)originalRecQty, (decimal)recQty);
    }

    [Theory]
    [InlineData(0, 0, 5)]
    [InlineData(2, 0, 5)]
    [InlineData(10, 0, 5)]
    [InlineData(10.55, 0, 5)]
    [InlineData(0, 5, 0)]
    [InlineData(2, 5, 0)]
    [InlineData(10, 5, 0)]
    [InlineData(10.55, 5, 0)]
    [InlineData(0, 2, 5)]
    [InlineData(2, 2, 5)]
    [InlineData(10, 2, 5)]
    [InlineData(10.55, 2, 5)]
    [InlineData(0, 5, 2)]
    [InlineData(2, 5, 2)]
    [InlineData(10, 5, 2)]
    [InlineData(10.55, 5, 2)]
    public Task UpdateRecQty04Test(double actQty, double originalRecQty, double recQty)
    {
        return this.UpdateRecQtyTest(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal)actQty, (decimal)originalRecQty, (decimal)recQty);
    }

    private async Task UpdateRecQtyTest(string whId, int? whZoneId, int? whLocId, decimal actQty, decimal originalRecQty, decimal recQty)
    {
        var originalEntity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Actqty = actQty,
            Recqty = originalRecQty
        };

        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Actqty = actQty,
            Recqty = recQty
        };

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        await this.service.ValidateAndThrowAsync(entity, originalEntity, ruleSets: ruleSets);
    }

    [Theory]
    [InlineData(0, 0, -5)]
    [InlineData(2, 0, -5)]
    [InlineData(10, 0, -5)]
    [InlineData(10.55, 0, -5)]
    [InlineData(0, 2, -5)]
    [InlineData(2, 2, -5)]
    [InlineData(10, 2, -5)]
    [InlineData(10.55, 2, -5)]
    [InlineData(0, 5, -2)]
    [InlineData(2, 5, -2)]
    [InlineData(10, 5, -2)]
    [InlineData(10.55, 5, -2)]
    public Task UpdateRecQtyFailed01Test(double actQty, double originalRecQty, double recQty)
    {
        return this.UpdateRecQtyFailedTest(this.whIdNoZoneNoLoc, null, null, (decimal)actQty, (decimal)originalRecQty, (decimal)recQty);
    }

    [Theory]
    [InlineData(0, 0, -5)]
    [InlineData(2, 0, -5)]
    [InlineData(10, 0, -5)]
    [InlineData(10.55, 0, -5)]
    [InlineData(0, 2, -5)]
    [InlineData(2, 2, -5)]
    [InlineData(10, 2, -5)]
    [InlineData(10.55, 2, -5)]
    [InlineData(0, 5, -2)]
    [InlineData(2, 5, -2)]
    [InlineData(10, 5, -2)]
    [InlineData(10.55, 5, -2)]
    public Task UpdateRecQtyFailed02Test(double actQty, double originalRecQty, double recQty)
    {
        return this.UpdateRecQtyFailedTest(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal)actQty, (decimal)originalRecQty, (decimal)recQty);
    }

    [Theory]
    [InlineData(0, 0, -5)]
    [InlineData(2, 0, -5)]
    [InlineData(10, 0, -5)]
    [InlineData(10.55, 0, -5)]
    [InlineData(0, 2, -5)]
    [InlineData(2, 2, -5)]
    [InlineData(10, 2, -5)]
    [InlineData(10.55, 2, -5)]
    [InlineData(0, 5, -2)]
    [InlineData(2, 5, -2)]
    [InlineData(10, 5, -2)]
    [InlineData(10.55, 5, -2)]
    public Task UpdateRecQtyFailed03Test(double actQty, double originalResQty, double resQty)
    {
        return this.UpdateRecQtyFailedTest(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal)actQty, (decimal)originalResQty, (decimal)resQty);
    }

    [Theory]
    [InlineData(0, 0, -5)]
    [InlineData(2, 0, -5)]
    [InlineData(10, 0, -5)]
    [InlineData(10.55, 0, -5)]
    [InlineData(0, 2, -5)]
    [InlineData(2, 2, -5)]
    [InlineData(10, 2, -5)]
    [InlineData(10.55, 2, -5)]
    [InlineData(0, 5, -2)]
    [InlineData(2, 5, -2)]
    [InlineData(10, 5, -2)]
    [InlineData(10.55, 5, -2)]
    public Task UpdateRecQtyFailed04Test(double actQty, double originalRecQty, double recQty)
    {
        return this.UpdateRecQtyFailedTest(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal)actQty, (decimal)originalRecQty, (decimal)recQty);
    }

    private async Task UpdateRecQtyFailedTest(string whId, int? whZoneId, int? whLocId, decimal actQty, decimal originalRecQty, decimal recQty)
    {
        var originalEntity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Actqty = actQty,
            Recqty = originalRecQty
        };

        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Actqty = actQty,
            Recqty = recQty
        };

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, originalEntity, ruleSets: ruleSets));
        var message = $"The receiving value is less than 0 (recQty: {entity.Recqty})";
        Assert.Contains(message, ex.Message);

        if (entity.Actqty + entity.Recqty - entity.Resqty < 0)
        {
            message = $"The provisioned value is less than 0 (actQty: {entity.Actqty}, recQty: {entity.Recqty}, resQty: {entity.Resqty})";
            Assert.Contains(message, ex.Message);
        }
    }

    #endregion

    #region ResQty tests

    [Theory]
    [InlineData(10, 0, 5)]
    [InlineData(10.55, 0, 5)]
    [InlineData(10, 5, 0)]
    [InlineData(10.55, 5, 0)]
    [InlineData(10, 2, 5)]
    [InlineData(10.55, 2, 5)]
    [InlineData(10, 5, 2)]
    [InlineData(10.55, 5, 2)]
    public Task UpdateResQty01Test(double actQty, double originalResQty, double resQty)
    {
        return this.UpdateResQtyTest01(this.whIdNoZoneNoLoc, null, null, (decimal)actQty, (decimal)originalResQty, (decimal)resQty);
    }

    [Theory]
    [InlineData(10, 0, 5)]
    [InlineData(10.55, 0, 5)]
    [InlineData(10, 5, 0)]
    [InlineData(10.55, 5, 0)]
    [InlineData(10, 2, 5)]
    [InlineData(10.55, 2, 5)]
    [InlineData(10, 5, 2)]
    [InlineData(10.55, 5, 2)]
    public Task UpdateResQty02Test(double actQty, double originalResQty, double resQty)
    {
        return this.UpdateResQtyTest01(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal)actQty, (decimal)originalResQty, (decimal)resQty);
    }

    [Theory]
    [InlineData(10, 0, 5)]
    [InlineData(10.55, 0, 5)]
    [InlineData(10, 5, 0)]
    [InlineData(10.55, 5, 0)]
    [InlineData(10, 2, 5)]
    [InlineData(10.55, 2, 5)]
    [InlineData(10, 5, 2)]
    [InlineData(10.55, 5, 2)]
    public Task UpdateResQty03Test(double actQty, double originalResQty, double resQty)
    {
        return this.UpdateResQtyTest01(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal)actQty, (decimal)originalResQty, (decimal)resQty);
    }

    [Theory]
    [InlineData(10, 0, 5)]
    [InlineData(10.55, 0, 5)]
    [InlineData(10, 5, 0)]
    [InlineData(10.55, 5, 0)]
    [InlineData(10, 2, 5)]
    [InlineData(10.55, 2, 5)]
    [InlineData(10, 5, 2)]
    [InlineData(10.55, 5, 2)]
    public Task UpdateResQty04Test(double actQty, double originalResQty, double resQty)
    {
        return this.UpdateResQtyTest01(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal)actQty, (decimal)originalResQty, (decimal)resQty);
    }

    private async Task UpdateResQtyTest01(string whId, int? whZoneId, int? whLocId, decimal actQty, decimal originalResQty, decimal resQty)
    {
        var originalEntity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Actqty = actQty,
            Resqty = originalResQty
        };

        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Actqty = actQty,
            Resqty = resQty
        };

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        await this.service.ValidateAndThrowAsync(entity, originalEntity, ruleSets: ruleSets);
    }

    [Theory]
    [InlineData(0, 0, 5)]
    [InlineData(2, 0, 5)]
    [InlineData(10, 0, 5)]
    [InlineData(10.55, 0, 5)]
    [InlineData(0, 5, 0)]
    [InlineData(2, 5, 0)]
    [InlineData(10, 5, 0)]
    [InlineData(10.55, 5, 0)]
    [InlineData(0, 2, 5)]
    [InlineData(2, 2, 5)]
    [InlineData(10, 2, 5)]
    [InlineData(10.55, 2, 5)]
    [InlineData(0, 5, 2)]
    [InlineData(2, 5, 2)]
    [InlineData(10, 5, 2)]
    [InlineData(10.55, 5, 2)]
    public Task UpdateResQty05Test(double actQty, double originalResQty, double resQty)
    {
        return this.UpdateResQtyTest02(this.whIdNoZoneNoLoc, null, null, (decimal)actQty, (decimal)originalResQty, (decimal)resQty);
    }

    [Theory]
    [InlineData(0, 0, 5)]
    [InlineData(2, 0, 5)]
    [InlineData(10, 0, 5)]
    [InlineData(10.55, 0, 5)]
    [InlineData(0, 5, 0)]
    [InlineData(2, 5, 0)]
    [InlineData(10, 5, 0)]
    [InlineData(10.55, 5, 0)]
    [InlineData(0, 2, 5)]
    [InlineData(2, 2, 5)]
    [InlineData(10, 2, 5)]
    [InlineData(10.55, 2, 5)]
    [InlineData(0, 5, 2)]
    [InlineData(2, 5, 2)]
    [InlineData(10, 5, 2)]
    [InlineData(10.55, 5, 2)]
    public Task UpdateResQty06Test(double actQty, double originalResQty, double resQty)
    {
        return this.UpdateResQtyTest02(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal)actQty, (decimal)originalResQty, (decimal)resQty);
    }

    [Theory]
    [InlineData(0, 0, 5)]
    [InlineData(2, 0, 5)]
    [InlineData(10, 0, 5)]
    [InlineData(10.55, 0, 5)]
    [InlineData(0, 5, 0)]
    [InlineData(2, 5, 0)]
    [InlineData(10, 5, 0)]
    [InlineData(10.55, 5, 0)]
    [InlineData(0, 2, 5)]
    [InlineData(2, 2, 5)]
    [InlineData(10, 2, 5)]
    [InlineData(10.55, 2, 5)]
    [InlineData(0, 5, 2)]
    [InlineData(2, 5, 2)]
    [InlineData(10, 5, 2)]
    [InlineData(10.55, 5, 2)]
    public Task UpdateResQty07Test(double actQty, double originalResQty, double resQty)
    {
        return this.UpdateResQtyTest02(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal)actQty, (decimal)originalResQty, (decimal)resQty);
    }

    [Theory]
    [InlineData(0, 0, 5)]
    [InlineData(2, 0, 5)]
    [InlineData(10, 0, 5)]
    [InlineData(10.55, 0, 5)]
    [InlineData(0, 5, 0)]
    [InlineData(2, 5, 0)]
    [InlineData(10, 5, 0)]
    [InlineData(10.55, 5, 0)]
    [InlineData(0, 2, 5)]
    [InlineData(2, 2, 5)]
    [InlineData(10, 2, 5)]
    [InlineData(10.55, 2, 5)]
    [InlineData(0, 5, 2)]
    [InlineData(2, 5, 2)]
    [InlineData(10, 5, 2)]
    [InlineData(10.55, 5, 2)]
    public Task UpdateResQty08Test(double actQty, double originalResQty, double resQty)
    {
        return this.UpdateResQtyTest02(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal)actQty, (decimal)originalResQty, (decimal)resQty);
    }

    private async Task UpdateResQtyTest02(string whId, int? whZoneId, int? whLocId, decimal actQty, decimal originalResQty, decimal resQty)
    {
        var originalEntity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Actqty = actQty,
            Recqty = resQty * 3,
            Resqty = originalResQty
        };

        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Actqty = actQty,
            Recqty = resQty * 3,
            Resqty = resQty
        };

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        await this.service.ValidateAndThrowAsync(entity, originalEntity, ruleSets: ruleSets);
    }

    [Theory]
    [InlineData(0, 0, -5)]
    [InlineData(2, 0, -5)]
    [InlineData(10, 0, -5)]
    [InlineData(10.55, 0, -5)]
    [InlineData(0, 2, -5)]
    [InlineData(2, 2, -5)]
    [InlineData(10, 2, -5)]
    [InlineData(10.55, 2, -5)]
    [InlineData(0, 5, -2)]
    [InlineData(2, 5, -2)]
    [InlineData(10, 5, -2)]
    [InlineData(10.55, 5, -2)]
    public Task UpdateResQtyFailed01Test(double actQty, double originalResQty, double resQty)
    {
        return this.UpdateResQtyFailedTest(this.whIdNoZoneNoLoc, null, null, (decimal)actQty, (decimal)originalResQty, (decimal)resQty);
    }

    [Theory]
    [InlineData(0, 0, -5)]
    [InlineData(2, 0, -5)]
    [InlineData(10, 0, -5)]
    [InlineData(10.55, 0, -5)]
    [InlineData(0, 2, -5)]
    [InlineData(2, 2, -5)]
    [InlineData(10, 2, -5)]
    [InlineData(10.55, 2, -5)]
    [InlineData(0, 5, -2)]
    [InlineData(2, 5, -2)]
    [InlineData(10, 5, -2)]
    [InlineData(10.55, 5, -2)]
    public Task UpdateResQtyFailed02Test(double actQty, double originalResQty, double resQty)
    {
        return this.UpdateResQtyFailedTest(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal)actQty, (decimal)originalResQty, (decimal)resQty);
    }

    [Theory]
    [InlineData(0, 0, -5)]
    [InlineData(2, 0, -5)]
    [InlineData(10, 0, -5)]
    [InlineData(10.55, 0, -5)]
    [InlineData(0, 2, -5)]
    [InlineData(2, 2, -5)]
    [InlineData(10, 2, -5)]
    [InlineData(10.55, 2, -5)]
    [InlineData(0, 5, -2)]
    [InlineData(2, 5, -2)]
    [InlineData(10, 5, -2)]
    [InlineData(10.55, 5, -2)]
    public Task UpdateResQtyFailed03Test(double actQty, double originalResQty, double resQty)
    {
        return this.UpdateResQtyFailedTest(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal)actQty, (decimal)originalResQty, (decimal)resQty);
    }

    [Theory]
    [InlineData(0, 0, -5)]
    [InlineData(2, 0, -5)]
    [InlineData(10, 0, -5)]
    [InlineData(10.55, 0, -5)]
    [InlineData(0, 2, -5)]
    [InlineData(2, 2, -5)]
    [InlineData(10, 2, -5)]
    [InlineData(10.55, 2, -5)]
    [InlineData(0, 5, -2)]
    [InlineData(2, 5, -2)]
    [InlineData(10, 5, -2)]
    [InlineData(10.55, 5, -2)]
    public Task UpdateResQtyFailed04Test(double actQty, double originalResQty, double resQty)
    {
        return this.UpdateResQtyFailedTest(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal)actQty, (decimal)originalResQty, (decimal)resQty);
    }

    private async Task UpdateResQtyFailedTest(string whId, int? whZoneId, int? whLocId, decimal actQty, decimal originalResQty, decimal resQty)
    {
        var originalEntity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Actqty = actQty,
            Recqty = originalResQty
        };

        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Actqty = actQty,
            Recqty = resQty
        };

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, originalEntity, ruleSets: ruleSets));
        var message = $"The receiving value is less than 0 (recQty: {entity.Recqty})";
        Assert.Contains(message, ex.Message);

        if (entity.Actqty + entity.Recqty - entity.Resqty < 0)
        {
            message = $"The provisioned value is less than 0 (actQty: {entity.Actqty}, recQty: {entity.Recqty}, resQty: {entity.Resqty})";
            Assert.Contains(message, ex.Message);
        }
    }

    #endregion

    #endregion

    #region Delete...

    #region empty test

    [Fact]
    public Task Delete001Test()
    {
        return this.Delete0Test(this.whIdNoZoneNoLoc, null, null);
    }

    [Fact]
    public Task Delete002Test()
    {
        return this.Delete0Test(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null);
    }

    [Fact]
    public Task Delete003Test()
    {
        return this.Delete0Test(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone);
    }

    [Fact]
    public Task Delete004Test()
    {
        return this.Delete0Test(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId);
    }

    private async Task Delete0Test(string whId, int? whZoneId, int? whLocId)
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
        };

        var ruleSets = new[]
        {
            "Default",
            "Delete"
        };

        await this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets);
    }

    #endregion

    #region ActQty test

    [Fact]
    public Task DeleteActQtyFailed01Test()
    {
        return this.DeleteActQtyFailedTest(this.whIdNoZoneNoLoc, null, null);
    }

    [Fact]
    public Task DeleteActQtyFailed02Test()
    {
        return this.DeleteActQtyFailedTest(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null);
    }

    [Fact]
    public Task DeleteActQtyFailed03Test()
    {
        return this.DeleteActQtyFailedTest(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone);
    }

    [Fact]
    public Task DeleteActQtyFailed04Test()
    {
        return this.DeleteActQtyFailedTest(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId);
    }

    private async Task DeleteActQtyFailedTest(string whId, int? whZoneId, int? whLocId)
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Actqty = 5
        };

        var ruleSets = new[]
        {
            "Default",
            "Delete"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets));
        var message = $"Unable to delete stock if actual value is not 0 (actQty: {entity.Actqty})";
        Assert.Contains(message, ex.Message);
    }

    #endregion

    #region RecQty test

    [Fact]
    public Task DeleteRecQtyFailed01Test()
    {
        return this.DeleteRecQtyFailedTest(this.whIdNoZoneNoLoc, null, null);
    }

    [Fact]
    public Task DeleteRecQtyFailed02Test()
    {
        return this.DeleteRecQtyFailedTest(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null);
    }

    [Fact]
    public Task DeleteRecQtyFailed03Test()
    {
        return this.DeleteRecQtyFailedTest(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone);
    }

    [Fact]
    public Task DeleteRecQtyFailed04Test()
    {
        return this.DeleteRecQtyFailedTest(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId);
    }

    private async Task DeleteRecQtyFailedTest(string whId, int? whZoneId, int? whLocId)
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Recqty = 5
        };

        var ruleSets = new[]
        {
            "Default",
            "Delete"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets));
        var message = $"Unable to delete stock if receiving value is not 0 (recQty: {entity.Recqty})";
        Assert.Contains(message, ex.Message);
    }

    #endregion

    #endregion
}
