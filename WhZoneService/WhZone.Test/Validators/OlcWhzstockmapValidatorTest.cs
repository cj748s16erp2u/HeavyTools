using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhZone.DataAccess.Repositories.Interfaces;
using eLog.HeavyTools.Services.WhZone.Test.Base;
using eLog.HeavyTools.Services.WhZone.Test.Fixtures;
using Xunit;

namespace eLog.HeavyTools.Services.WhZone.Test.Validators;

public class OlcWhzstockmapValidatorTest : TestBase<OlcWhzstockmap, IWhZStockMapService>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IWhZStockService stockService;

    private readonly int itemId;
    private readonly string whId;
    private readonly int whZoneId;
    private readonly int whLocId;

    public OlcWhzstockmapValidatorTest(
        ITestOutputHelper testOutputHelper,
        TestFixture fixture) : base(testOutputHelper, fixture)
    {
        this.unitOfWork = this._fixture.GetService<IUnitOfWork>(this._testOutputHelper) ?? throw new InvalidOperationException($"{nameof(IUnitOfWork)} is not found.");
        this.stockService = this._fixture.GetService<IWhZStockService>(this._testOutputHelper) ?? throw new InvalidOperationException($"{nameof(IWhZStockService)} is not found.");

        this.itemId = this.GetFirstItemIdAsync().GetAwaiter().GetResult() ?? throw new InvalidOperationException();
        this.whId = this.GetFirstWarehouseIdAsync(true).GetAwaiter().GetResult() ?? throw new InvalidOperationException();
        this.whZoneId = this.GetFirstWhZoneIdAsync(this.whId).GetAwaiter().GetResult() ?? throw new InvalidOperationException();
        this.whLocId = this.GetFirstWhLocIdAsync(this.whId, this.whZoneId).GetAwaiter().GetResult() ?? throw new InvalidOperationException();
    }

    [Fact]
    public async Task AddWrongItemIdTest()
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = -1,
            Whid = this.whId,
            Whzoneid = this.whZoneId,
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
    public async Task AddWrongZoneTest()
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Whzoneid = -1,
            Whlocid = this.whLocId,
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The zone doesn't exists in the given warehouse: '{entity.Whid}' (zone: {entity.Whzoneid})\r\nThe location doesn't exists in the given warehouse/zone: '{entity.Whid}'/{entity.Whzoneid} (location: {entity.Whlocid})";
        Assert.Contains(message, ex.Message);
    }

    [Fact]
    public async Task AddWrongLocation01Test()
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Whzoneid = this.whZoneId,
            Whlocid = -1,
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
    public async Task AddWrongLocation02Test()
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whId,
            //Whzoneid = this.whZoneId,
            Whlocid = this.whLocId,
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
    public async Task AddWrongLocation03Test()
    {
        var whLocIdNoZone = await this.GetFirstWhLocIdAsync(this.whId, null);
        Assert.NotNull(whLocIdNoZone);

        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Whzoneid = this.whZoneId,
            Whlocid = whLocIdNoZone!.Value,
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
    public async Task Add001Test()
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Whzoneid = this.whZoneId,
            Whlocid = this.whLocId,
            Recqty = 0
        };

        var stockRequest = new WhZStockDto
        {
            Itemid = entity.Itemid,
            Whid = entity.Whid,
            Whzoneid = entity.Whzoneid,
            Qty = 5
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        using var tran = await this.unitOfWork.BeginTransactionAsync();
        try
        {
            var context = this.stockService.CreateContext();

            await this.stockService.AddReceivingAsync(context, stockRequest);
            await this.stockService.RemoveReceivingAsync(context, stockRequest);
            await this.stockService.StoreAsync(context);

            await this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Fact]
    public async Task Add002Test()
    {
        var whLocIdNoZone = await this.GetFirstWhLocIdAsync(this.whId, null);
        Assert.NotNull(whLocIdNoZone);

        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whId,
            //Whzoneid = this.whZoneId,
            Whlocid = whLocIdNoZone!.Value,
        };


        var stockRequest = new WhZStockDto
        {
            Itemid = entity.Itemid,
            Whid = entity.Whid,
            Whzoneid = entity.Whzoneid,
            Qty = 5
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        using var tran = await this.unitOfWork.BeginTransactionAsync();
        try
        {
            var context = this.stockService.CreateContext();

            await this.stockService.AddReceivingAsync(context, stockRequest);
            await this.stockService.RemoveReceivingAsync(context, stockRequest);
            await this.stockService.StoreAsync(context);

            await this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    public async Task AddRecQtyTest(double actQty)
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Whzoneid = this.whZoneId,
            Whlocid = this.whLocId,
            Actqty = (decimal)actQty,
            Recqty = 5
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        using var tran = await this.unitOfWork.BeginTransactionAsync();
        try
        {
            var context = this.stockService.CreateContext();

            WhZStockDto stockRequest;
            if (entity.Actqty != 0)
            {
                stockRequest = new WhZStockDto
                {
                    Itemid = entity.Itemid,
                    Whid = entity.Whid,
                    Whzoneid = entity.Whzoneid,
                    Qty = entity.Actqty,
                };

                await this.stockService.AddReceivingAsync(context, stockRequest);
                await this.stockService.CommitReceivingAsync(context, stockRequest);
                await this.stockService.StoreAsync(context);
            }

            stockRequest = new WhZStockDto
            {
                Itemid = entity.Itemid,
                Whid = entity.Whid,
                Whzoneid = entity.Whzoneid,
                Qty = entity.Recqty,
            };

            await this.stockService.AddReceivingAsync(context, stockRequest);
            await this.stockService.StoreAsync(context);

            await this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    public async Task AddResQtyTest(double actQty)
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Whzoneid = this.whZoneId,
            Whlocid = this.whLocId,
            Actqty = (decimal)actQty,
            Recqty = Math.Max(5 - (decimal)actQty, 0),
            Resqty = 5
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        using var tran = await this.unitOfWork.BeginTransactionAsync();
        try
        {
            var context = this.stockService.CreateContext();

            WhZStockDto stockRequest;
            if (entity.Actqty != 0)
            {
                stockRequest = new WhZStockDto
                {
                    Itemid = entity.Itemid,
                    Whid = entity.Whid,
                    Whzoneid = entity.Whzoneid,
                    Qty = entity.Actqty,
                };

                await this.stockService.AddReceivingAsync(context, stockRequest);
                await this.stockService.CommitReceivingAsync(context, stockRequest);
                await this.stockService.StoreAsync(context);
            }

            if (entity.Recqty != 0)
            {
                stockRequest = new WhZStockDto
                {
                    Itemid = entity.Itemid,
                    Whid = entity.Whid,
                    Whzoneid = entity.Whzoneid,
                    Qty = entity.Recqty,
                };

                await this.stockService.AddReceivingAsync(context, stockRequest);
                await this.stockService.StoreAsync(context);
            }

            stockRequest = new WhZStockDto
            {
                Itemid = entity.Itemid,
                Whid = entity.Whid,
                Whzoneid = entity.Whzoneid,
                Qty = entity.Resqty,
            };

            await this.stockService.AddReservedAsync(context, stockRequest);
            await this.stockService.StoreAsync(context);

            await this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    public async Task AddResQtyFailedTest(double actQty)
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Whzoneid = this.whZoneId,
            Whlocid = this.whLocId,
            Actqty = (decimal)actQty,
            Resqty = 15
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        using var tran = await this.unitOfWork.BeginTransactionAsync();
        try
        {
            var context = this.stockService.CreateContext();

            WhZStockDto stockRequest;
            if (entity.Actqty != 0)
            {
                stockRequest = new WhZStockDto
                {
                    Itemid = entity.Itemid,
                    Whid = entity.Whid,
                    Whzoneid = entity.Whzoneid,
                    Qty = entity.Actqty,
                };

                await this.stockService.AddReceivingAsync(context, stockRequest);
                await this.stockService.CommitReceivingAsync(context, stockRequest);
                await this.stockService.StoreAsync(context);
            }

            if (entity.Recqty != 0)
            {
                stockRequest = new WhZStockDto
                {
                    Itemid = entity.Itemid,
                    Whid = entity.Whid,
                    Whzoneid = entity.Whzoneid,
                    Qty = entity.Recqty,
                };

                await this.stockService.AddReceivingAsync(context, stockRequest);
                await this.stockService.StoreAsync(context);
            }

            var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
            var message = $"The provisioned value is less than 0 (actQty: {entity.Actqty}, recQty: {entity.Recqty}, resQty: {entity.Resqty})";
            Assert.Contains(message, ex.Message);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    public async Task AddActQtyTest(double actQty)
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Whzoneid = this.whZoneId,
            Whlocid = this.whLocId,
            Actqty = (decimal)actQty
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        using var tran = await this.unitOfWork.BeginTransactionAsync();
        try
        {
            var context = this.stockService.CreateContext();

            WhZStockDto stockRequest;
            if (entity.Actqty != 0)
            {
                stockRequest = new WhZStockDto
                {
                    Itemid = entity.Itemid,
                    Whid = entity.Whid,
                    Whzoneid = entity.Whzoneid,
                    Qty = entity.Actqty,
                };

                await this.stockService.AddReceivingAsync(context, stockRequest);
                await this.stockService.CommitReceivingAsync(context, stockRequest);
                await this.stockService.StoreAsync(context);
            }
            else
            {
                stockRequest = new WhZStockDto
                {
                    Itemid = entity.Itemid,
                    Whid = entity.Whid,
                    Whzoneid = entity.Whzoneid,
                    Qty = 1,
                };

                await this.stockService.AddReceivingAsync(context, stockRequest);
                await this.stockService.CommitReceivingAsync(context, stockRequest);
                await this.stockService.AddReservedAsync(context, stockRequest);
                await this.stockService.CommitReservedAsync(context, stockRequest);
                await this.stockService.StoreAsync(context);
            }

            await this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(-10)]
    public async Task AddActQtyFailed01Test(double actQty)
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Whzoneid = this.whZoneId,
            Whlocid = this.whLocId,
            Actqty = (decimal)actQty
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The actual value is less than 0 (actQty: {entity.Actqty}){Environment.NewLine}The provisioned value is less than 0 (actQty: {entity.Actqty}, recQty: {entity.Recqty}, resQty: {entity.Resqty})";
        Assert.Contains(message, ex.Message);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(-10)]
    public async Task AddActQtyFailed02Test(double actQty)
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Whzoneid = this.whZoneId,
            Whlocid = this.whLocId,
            Recqty = (decimal)Math.Abs(actQty * 5),
            Actqty = (decimal)actQty
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The actual value is less than 0 (actQty: {entity.Actqty})";
        Assert.Contains(message, ex.Message);
    }

    [Fact]
    public async Task UpdateWrongItemIdTest()
    {
        var origEntity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Whzoneid = this.whZoneId,
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
            Whid = this.whId,
            Whzoneid = this.whZoneId,
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
            Whid = this.whId,
            Whzoneid = this.whZoneId,
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
            Whid = this.whId,
            Whzoneid = this.whZoneId,
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

    [Fact]
    public async Task Update0Test()
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Whzoneid = this.whZoneId,
            Whlocid = this.whLocId,
        };

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        using var tran = await this.unitOfWork.BeginTransactionAsync();
        try
        {
            var context = this.stockService.CreateContext();

            var stockRequest = new WhZStockDto
            {
                Itemid = entity.Itemid,
                Whid = entity.Whid,
                Whzoneid = entity.Whzoneid,
                Qty = 1,
            };

            await this.stockService.AddReceivingAsync(context, stockRequest);
            await this.stockService.RemoveReceivingAsync(context, stockRequest);
            await this.stockService.StoreAsync(context);

            await this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    public async Task UpdateRecQtyTest(double actQty)
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Whzoneid = this.whZoneId,
            Whlocid = this.whLocId,
            Actqty = (decimal)actQty,
            Recqty = 5
        };

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        using var tran = await this.unitOfWork.BeginTransactionAsync();
        try
        {
            var context = this.stockService.CreateContext();

            WhZStockDto stockRequest;
            if (entity.Actqty != 0)
            {
                stockRequest = new WhZStockDto
                {
                    Itemid = entity.Itemid,
                    Whid = entity.Whid,
                    Whzoneid = entity.Whzoneid,
                    Qty = entity.Actqty,
                };

                await this.stockService.AddReceivingAsync(context, stockRequest);
                await this.stockService.CommitReceivingAsync(context, stockRequest);
                await this.stockService.StoreAsync(context);
            }

            stockRequest = new WhZStockDto
            {
                Itemid = entity.Itemid,
                Whid = entity.Whid,
                Whzoneid = entity.Whzoneid,
                Qty = entity.Recqty,
            };

            await this.stockService.AddReceivingAsync(context, stockRequest);
            await this.stockService.StoreAsync(context);

            await this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    public async Task UpdateResQtyTest(double actQty)
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Whzoneid = this.whZoneId,
            Whlocid = this.whLocId,
            Actqty = (decimal)actQty,
            Recqty = Math.Max(5 - (decimal)actQty, 0),
            Resqty = 5
        };

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        using var tran = await this.unitOfWork.BeginTransactionAsync();
        try
        {
            var context = this.stockService.CreateContext();

            WhZStockDto stockRequest;
            if (entity.Actqty != 0)
            {
                stockRequest = new WhZStockDto
                {
                    Itemid = entity.Itemid,
                    Whid = entity.Whid,
                    Whzoneid = entity.Whzoneid,
                    Qty = entity.Actqty,
                };

                await this.stockService.AddReceivingAsync(context, stockRequest);
                await this.stockService.CommitReceivingAsync(context, stockRequest);
                await this.stockService.StoreAsync(context);
            }

            if (entity.Recqty != 0)
            {
                stockRequest = new WhZStockDto
                {
                    Itemid = entity.Itemid,
                    Whid = entity.Whid,
                    Whzoneid = entity.Whzoneid,
                    Qty = entity.Recqty,
                };

                await this.stockService.AddReceivingAsync(context, stockRequest);
                await this.stockService.StoreAsync(context);
            }

            stockRequest = new WhZStockDto
            {
                Itemid = entity.Itemid,
                Whid = entity.Whid,
                Whzoneid = entity.Whzoneid,
                Qty = entity.Resqty,
            };

            await this.stockService.AddReservedAsync(context, stockRequest);
            await this.stockService.StoreAsync(context);

            await this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    public async Task UpdateResQtyFailedTest(double actQty)
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Whzoneid = this.whZoneId,
            Whlocid = this.whLocId,
            Actqty = (decimal)actQty,
            Resqty = 15
        };

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        using var tran = await this.unitOfWork.BeginTransactionAsync();
        try
        {
            var context = this.stockService.CreateContext();

            WhZStockDto stockRequest;
            if (entity.Actqty != 0)
            {
                stockRequest = new WhZStockDto
                {
                    Itemid = entity.Itemid,
                    Whid = entity.Whid,
                    Whzoneid = entity.Whzoneid,
                    Qty = entity.Actqty,
                };

                await this.stockService.AddReceivingAsync(context, stockRequest);
                await this.stockService.CommitReceivingAsync(context, stockRequest);
                await this.stockService.StoreAsync(context);
            }

            if (entity.Recqty != 0)
            {
                stockRequest = new WhZStockDto
                {
                    Itemid = entity.Itemid,
                    Whid = entity.Whid,
                    Whzoneid = entity.Whzoneid,
                    Qty = entity.Recqty,
                };

                await this.stockService.AddReceivingAsync(context, stockRequest);
                await this.stockService.StoreAsync(context);
            }

            var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets));
            var message = $"The provisioned value is less than 0 (actQty: {entity.Actqty}, recQty: {entity.Recqty}, resQty: {entity.Resqty})";
            Assert.Contains(message, ex.Message);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    public async Task UpdateActQtyTest(double actQty)
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Whzoneid = this.whZoneId,
            Whlocid = this.whLocId,
            Actqty = (decimal)actQty
        };

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        using var tran = await this.unitOfWork.BeginTransactionAsync();
        try
        {
            var context = this.stockService.CreateContext();

            WhZStockDto stockRequest;
            if (entity.Actqty != 0)
            {
                stockRequest = new WhZStockDto
                {
                    Itemid = entity.Itemid,
                    Whid = entity.Whid,
                    Whzoneid = entity.Whzoneid,
                    Qty = entity.Actqty,
                };

                await this.stockService.AddReceivingAsync(context, stockRequest);
                await this.stockService.CommitReceivingAsync(context, stockRequest);
                await this.stockService.StoreAsync(context);
            }
            else
            {
                stockRequest = new WhZStockDto
                {
                    Itemid = entity.Itemid,
                    Whid = entity.Whid,
                    Whzoneid = entity.Whzoneid,
                    Qty = 1,
                };

                await this.stockService.AddReceivingAsync(context, stockRequest);
                await this.stockService.CommitReceivingAsync(context, stockRequest);
                await this.stockService.AddReservedAsync(context, stockRequest);
                await this.stockService.CommitReservedAsync(context, stockRequest);
                await this.stockService.StoreAsync(context);
            }

            await this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(-10)]
    public async Task UpdateActQtyFailed01Test(double actQty)
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Whzoneid = this.whZoneId,
            Whlocid = this.whLocId,
            Actqty = (decimal)actQty
        };

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets));
        var message = $"The actual value is less than 0 (actQty: {entity.Actqty}){Environment.NewLine}The provisioned value is less than 0 (actQty: {entity.Actqty}, recQty: {entity.Recqty}, resQty: {entity.Resqty})";
        Assert.Contains(message, ex.Message);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(-10)]
    public async Task UpdateActQtyFailed02Test(double actQty)
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Whzoneid = this.whZoneId,
            Whlocid = this.whLocId,
            Recqty = (decimal)(Math.Abs(actQty * 5)),
            Actqty = (decimal)actQty
        };

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets));
        var message = $"The actual value is less than 0 (actQty: {entity.Actqty})";
        Assert.Contains(message, ex.Message);
    }

    [Fact]
    public async Task Delete0Test()
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Whzoneid = this.whZoneId,
            Whlocid = this.whLocId,
        };

        var ruleSets = new[]
        {
            "Default",
            "Delete"
        };

        using var tran = await this.unitOfWork.BeginTransactionAsync();
        try
        {
            var context = this.stockService.CreateContext();

            var stockRequest = new WhZStockDto
            {
                Itemid = entity.Itemid,
                Whid = entity.Whid,
                Whzoneid = entity.Whzoneid,
                Qty = 1,
            };

            await this.stockService.AddReceivingAsync(context, stockRequest);
            await this.stockService.RemoveReceivingAsync(context, stockRequest);
            await this.stockService.StoreAsync(context);

            await this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Fact]
    public async Task DeleteActQtyFailedTest()
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Whzoneid = this.whZoneId,
            Whlocid = this.whLocId,
            Actqty = 5
        };

        var ruleSets = new[]
        {
            "Default",
            "Delete"
        };

        using var tran = await this.unitOfWork.BeginTransactionAsync();
        try
        {
            var context = this.stockService.CreateContext();

            var stockRequest = new WhZStockDto
            {
                Itemid = entity.Itemid,
                Whid = entity.Whid,
                Whzoneid = entity.Whzoneid,
                Qty = 1,
            };

            await this.stockService.AddReceivingAsync(context, stockRequest);
            await this.stockService.RemoveReceivingAsync(context, stockRequest);
            await this.stockService.StoreAsync(context);

            var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets));
            Assert.Contains($"Unable to delete stock if actual value is not 0 (actQty: {entity.Actqty})", ex.Message);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Fact]
    public async Task DeleteRecQtyFailedTest()
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Whzoneid = this.whZoneId,
            Whlocid = this.whLocId,
            Recqty = 5
        };

        var ruleSets = new[]
        {
            "Default",
            "Delete"
        };

        using var tran = await this.unitOfWork.BeginTransactionAsync();
        try
        {
            var context = this.stockService.CreateContext();

            var stockRequest = new WhZStockDto
            {
                Itemid = entity.Itemid,
                Whid = entity.Whid,
                Whzoneid = entity.Whzoneid,
                Qty = 1,
            };

            await this.stockService.AddReceivingAsync(context, stockRequest);
            await this.stockService.RemoveReceivingAsync(context, stockRequest);
            await this.stockService.StoreAsync(context);

            var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets));
            Assert.Contains($"Unable to delete stock if receiving value is not 0 (recQty: {entity.Recqty})", ex.Message);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Fact]
    public async Task CheckStockActQtyFailedTest()
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Whzoneid = this.whZoneId,
            Whlocid = this.whLocId,
            Actqty = 5
        };

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        using var tran = await this.unitOfWork.BeginTransactionAsync();
        try
        {
            var context = this.stockService.CreateContext();

            var stockRequest = new WhZStockDto
            {
                Itemid = entity.Itemid,
                Whid = entity.Whid,
                Whzoneid = entity.Whzoneid,
                Qty = 1,
            };

            await this.stockService.AddReceivingAsync(context, stockRequest);
            await this.stockService.RemoveReceivingAsync(context, stockRequest);
            await this.stockService.StoreAsync(context);

            var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets));
            var message = $"The actual value and the stock value are different";
            Assert.Contains(message, ex.Message);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Fact]
    public async Task CheckStockRecQtyFailedTest()
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Whzoneid = this.whZoneId,
            Whlocid = this.whLocId,
            Recqty = 5
        };

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        using var tran = await this.unitOfWork.BeginTransactionAsync();
        try
        {
            var context = this.stockService.CreateContext();

            var stockRequest = new WhZStockDto
            {
                Itemid = entity.Itemid,
                Whid = entity.Whid,
                Whzoneid = entity.Whzoneid,
                Qty = 1,
            };

            await this.stockService.AddReceivingAsync(context, stockRequest);
            await this.stockService.RemoveReceivingAsync(context, stockRequest);
            await this.stockService.StoreAsync(context);

            var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets));
            var message = $"The receiving value and the stock value are different";
            Assert.Contains(message, ex.Message);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Fact]
    public async Task CheckStockResQtyFailedTest()
    {
        var entity = new OlcWhzstockmap
        {
            Itemid = this.itemId,
            Whid = this.whId,
            Whzoneid = this.whZoneId,
            Whlocid = this.whLocId,
            Resqty = 5
        };

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        using var tran = await this.unitOfWork.BeginTransactionAsync();
        try
        {
            var context = this.stockService.CreateContext();

            var stockRequest = new WhZStockDto
            {
                Itemid = entity.Itemid,
                Whid = entity.Whid,
                Whzoneid = entity.Whzoneid,
                Qty = 1,
            };

            await this.stockService.AddReceivingAsync(context, stockRequest);
            await this.stockService.RemoveReceivingAsync(context, stockRequest);
            await this.stockService.StoreAsync(context);

            var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, entity, ruleSets: ruleSets));
            var message = $"The reserved value and the stock value are different";
            Assert.Contains(message, ex.Message);
        }
        finally
        {
            tran.Rollback();
        }
    }
}
