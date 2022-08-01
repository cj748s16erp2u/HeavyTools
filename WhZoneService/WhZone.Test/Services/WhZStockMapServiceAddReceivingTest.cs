using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Enums;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services;
using eLog.HeavyTools.Services.WhZone.Test.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace eLog.HeavyTools.Services.WhZone.Test.Services;

public class WhZStockMapServiceAddReceivingTest : Base.WhZStockMapServiceTestBase
{
    public WhZStockMapServiceAddReceivingTest(ITestOutputHelper testOutputHelper, TestFixture fixture) : base(testOutputHelper, fixture)
    {
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public async Task AddReceivingWhStockTestAsync(double? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
        Assert.NotNull(whid);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
        Assert.NotNull(whLocId);

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whlocid = whLocId.Value,
            Qty = (decimal?)qty
        };

        await this.AddReceivingAsync(context, request, ct);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public async Task AddReceivingWhZStockTestAsync(double? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(true, ct);
        Assert.NotNull(whid);

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
        Assert.NotNull(whZoneId);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
        Assert.NotNull(whLocId);

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId.Value,
            Whlocid = whLocId.Value,
            Qty = (decimal?)qty
        };

        await this.AddReceivingAsync(context, request, ct);
    }

    [Fact]
    public async Task AddReceivingWhStockNullFailedTestAsync()
    {
        var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.AddReceivingWhStockTestAsync(null));
        Assert.Equal(WhZStockExceptionType.InvalidRequestQty, ex.Type);
        Assert.Equal("The add qty is not set", ex.Message);
    }

    [Fact]
    public async Task AddReceivingWhZStockNullFailedTestAsync()
    {
        var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.AddReceivingWhZStockTestAsync(null));
        Assert.Equal(WhZStockExceptionType.InvalidRequestQty, ex.Type);
        Assert.Equal("The add qty is not set", ex.Message);
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-15.0)]
    [InlineData(-0.25)]
    [InlineData(-5.25)]
    public async Task AddReceivingWhStockFailedTestAsync(double? qty)
    {
        var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.AddReceivingWhStockTestAsync(qty));
        Assert.Equal(WhZStockExceptionType.InvalidRequestQty, ex.Type);
        Assert.Equal("The add qty cannot be less or equal to 0", ex.Message);
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-15.0)]
    [InlineData(-0.25)]
    [InlineData(-5.25)]
    public async Task AddReceivingWhZStockFailedTestAsync(double? qty)
    {
        var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.AddReceivingWhZStockTestAsync(qty));
        Assert.Equal(WhZStockExceptionType.InvalidRequestQty, ex.Type);
        Assert.Equal("The add qty cannot be less or equal to 0", ex.Message);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public async Task AddReceivingAndStoreWhStockTestAsync(double? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
        Assert.NotNull(whid);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
        Assert.NotNull(whLocId);

        var originalEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whlocid = whLocId.Value,
            Qty = (decimal?)qty
        };

        await this.AddReceivingAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal((originalEntity?.Recqty).GetValueOrDefault() + request.Qty, currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
        Assert.Equal((originalEntity?.Resqty).GetValueOrDefault(), currentEntity!.Resqty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public async Task AddReceivingAndStoreWhZStockTestAsync(double? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(true, ct);
        Assert.NotNull(whid);

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
        Assert.NotNull(whZoneId);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
        Assert.NotNull(whLocId);

        var originalEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whlocid == whLocId, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Whlocid = whLocId.Value,
            Qty = (decimal?)qty
        };

        await this.AddReceivingAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal((originalEntity?.Recqty).GetValueOrDefault() + request.Qty, currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
        Assert.Equal((originalEntity?.Resqty).GetValueOrDefault(), currentEntity!.Resqty);
    }


    [Fact]
    public async Task DeleteWhStock01TestAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
        Assert.NotNull(whid);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
        Assert.NotNull(whLocId);

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whlocid = whLocId.Value,
            Qty = 10
        };


        var data = await this.AddReceivingAsync(context, request, ct);
        Assert.Contains(context.MovementList, l => l == data);
        this.Delete(context, data!);
        Assert.DoesNotContain(context.MovementList, l => l == data);
    }

    [Fact]
    public async Task DeleteWhStock02TestAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
        Assert.NotNull(whid);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
        Assert.NotNull(whLocId);

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whlocid = whLocId.Value,
            Qty = 10
        };

        var data = await this.AddReceivingAsync(context, request, ct);
        Assert.Contains(context.MovementList, l => l == data);
        await this.AddReceivingAsync(context, request, ct);
        await this.AddReservedAsync(context, request, ct);
        this.Delete(context, data!);
        Assert.DoesNotContain(context.MovementList, l => l == data);
    }

    [Fact]
    public async Task DeleteWhStock03TestAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
        Assert.NotNull(whid);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
        Assert.NotNull(whLocId);

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whlocid = whLocId.Value,
            Qty = 10
        };

        await this.AddReceivingAsync(context, request, ct);
        var data = await this.CommitReceivingAsync(context, request, ct);
        Assert.Contains(context.MovementList, l => l == data);
        this.Delete(context, data!);
        Assert.DoesNotContain(context.MovementList, l => l == data);
    }

    [Fact]
    public async Task DeleteWhStockFailedTestAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
        Assert.NotNull(whid);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
        Assert.NotNull(whLocId);

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whlocid = whLocId.Value,
            Qty = 10
        };

        var data = await this.AddReceivingAsync(context, request, ct);
        await this.AddReservedAsync(context, request, ct);
        var ex = Assert.Throws<WhZStockMapServiceException>(() => this.Delete(context, data!));
        Assert.Equal(WhZStockExceptionType.DeleteNotEnoughQty, ex.Type);
        Assert.Equal("Unable to remove this request, cause the further requests uses its quantity", ex.Message);
        Assert.Contains(context.MovementList, l => l == data);
    }

    [Fact]
    public async Task DeleteWhZStock01TestAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(true, ct);
        Assert.NotNull(whid);

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
        Assert.NotNull(whZoneId);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
        Assert.NotNull(whLocId);

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Whlocid = whLocId.Value,
            Qty = 10
        };


        var data = await this.AddReceivingAsync(context, request, ct);
        Assert.Contains(context.MovementList, l => l == data);
        this.Delete(context, data!);
        Assert.DoesNotContain(context.MovementList, l => l == data);
    }

    [Fact]
    public async Task DeleteWhZStock02TestAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(true, ct);
        Assert.NotNull(whid);

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
        Assert.NotNull(whZoneId);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
        Assert.NotNull(whLocId);

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Whlocid = whLocId.Value,
            Qty = 10
        };

        var data = await this.AddReceivingAsync(context, request, ct);
        Assert.Contains(context.MovementList, l => l == data);
        await this.AddReceivingAsync(context, request, ct);
        await this.AddReservedAsync(context, request, ct);
        this.Delete(context, data!);
        Assert.DoesNotContain(context.MovementList, l => l == data);
    }

    [Fact]
    public async Task DeleteWhZStock03TestAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(true, ct);
        Assert.NotNull(whid);

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
        Assert.NotNull(whZoneId);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
        Assert.NotNull(whLocId);

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Whlocid = whLocId.Value,
            Qty = 10
        };

        await this.AddReceivingAsync(context, request, ct);
        var data = await this.CommitReceivingAsync(context, request, ct);
        Assert.Contains(context.MovementList, l => l == data);
        this.Delete(context, data!);
        Assert.DoesNotContain(context.MovementList, l => l == data);
    }

    [Fact]
    public async Task DeleteWhZStockFailedTestAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(true, ct);
        Assert.NotNull(whid);

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
        Assert.NotNull(whZoneId);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
        Assert.NotNull(whLocId);

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Whlocid = whLocId.Value,
            Qty = 10
        };

        var data = await this.AddReceivingAsync(context, request, ct);
        await this.AddReservedAsync(context, request, ct);
        var ex = Assert.Throws<WhZStockMapServiceException>(() => this.Delete(context, data!));
        Assert.Equal(WhZStockExceptionType.DeleteNotEnoughQty, ex.Type);
        Assert.Equal("Unable to remove this request, cause the further requests uses its quantity", ex.Message);
        Assert.Contains(context.MovementList, l => l == data);
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-15.0)]
    [InlineData(-0.25)]
    [InlineData(-5.25)]
    public async Task RemoveWhStockFailedAsync(double? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
        Assert.NotNull(whid);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
        Assert.NotNull(whLocId);

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whlocid = whLocId.Value,
            Qty = (decimal?)qty
        };

        var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.RemoveReceivingAsync(context, request, ct));
        Assert.Equal(WhZStockExceptionType.InvalidRequestQty, ex.Type);
        Assert.Equal("The remove qty cannot be less or equal to 0", ex.Message);
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-15.0)]
    [InlineData(-0.25)]
    [InlineData(-5.25)]
    public async Task RemoveWhZStockFailedAsync(double? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(true, ct);
        Assert.NotNull(whid);

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
        Assert.NotNull(whZoneId);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
        Assert.NotNull(whLocId);

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Whlocid = whLocId.Value,
            Qty = (decimal?)qty
        };

        var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.RemoveReceivingAsync(context, request, ct));
        Assert.Equal(WhZStockExceptionType.InvalidRequestQty, ex.Type);
        Assert.Equal("The remove qty cannot be less or equal to 0", ex.Message);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public async Task AddAndRemoveAndStoreWhStockTest01Async(double? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
        Assert.NotNull(whid);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
        Assert.NotNull(whLocId);

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);

        using var context = this.service.CreateContext();

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whlocid = whLocId.Value,
            Qty = (decimal?)qty
        };

        await this.AddReceivingAsync(context, request, ct);

        var removeRequest = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whlocid = whLocId.Value,
            Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)(currentEntity!.Recqty + request.Qty.GetValueOrDefault()), Precision, MidpointRounding.AwayFromZero))
        };

        await this.RemoveReceivingAsync(context, removeRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal((originalEntity?.Recqty).GetValueOrDefault() + request.Qty - removeRequest.Qty, currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
        Assert.Equal((originalEntity?.Resqty).GetValueOrDefault(), currentEntity!.Resqty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public async Task AddAndRemoveAndStoreWhZStockTest01Async(double? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(true, ct);
        Assert.NotNull(whid);

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
        Assert.NotNull(whZoneId);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
        Assert.NotNull(whLocId);

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);

        using var context = this.service.CreateContext();

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Whlocid = whLocId.Value,
            Qty = (decimal?)qty
        };

        await this.AddReceivingAsync(context, request, ct);

        var removeRequest = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Whlocid = whLocId.Value,
            Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)(currentEntity.Recqty + request.Qty.GetValueOrDefault()), Precision, MidpointRounding.AwayFromZero))
        };

        await this.RemoveReceivingAsync(context, removeRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal((originalEntity?.Recqty).GetValueOrDefault() + request.Qty - removeRequest.Qty, currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
        Assert.Equal((originalEntity?.Resqty).GetValueOrDefault(), currentEntity!.Resqty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public async Task AddAndRemoveAndStoreWhStockTest02Async(double? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
        Assert.NotNull(whid);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
        Assert.NotNull(whLocId);

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whlocid = whLocId.Value,
            Qty = (decimal?)qty
        };

        await this.AddReceivingAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var removeRequest = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whlocid = whLocId.Value,
            Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)currentEntity.Recqty, Precision, MidpointRounding.AwayFromZero))
        };

        await this.RemoveReceivingAsync(context, removeRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal((originalEntity?.Recqty).GetValueOrDefault() + request.Qty - removeRequest.Qty, currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
        Assert.Equal((originalEntity?.Resqty).GetValueOrDefault(), currentEntity!.Resqty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public async Task AddAndRemoveAndStoreWhZStockTest02Async(double? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(true, ct);
        Assert.NotNull(whid);

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
        Assert.NotNull(whZoneId);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
        Assert.NotNull(whLocId);

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Whlocid = whLocId.Value,
            Qty = (decimal?)qty
        };

        await this.AddReceivingAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var removeRequest = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Whlocid = whLocId.Value,
            Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)currentEntity.Recqty, Precision, MidpointRounding.AwayFromZero))
        };

        await this.RemoveReceivingAsync(context, removeRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal((originalEntity?.Recqty).GetValueOrDefault() + request.Qty - removeRequest.Qty, currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
        Assert.Equal((originalEntity?.Resqty).GetValueOrDefault(), currentEntity!.Resqty);
    }

    [Fact]
    public async Task AddAndRemoveAndStoreWhStockTo0TestAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
        Assert.NotNull(whid);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
        Assert.NotNull(whLocId);

        var originalEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whlocid = whLocId.Value,
            Qty = 15.75M
        };

        await this.AddReceivingAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var removeRequest = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whlocid = whLocId.Value,
            Qty = currentEntity.Recqty
        };

        await this.RemoveReceivingAsync(context, removeRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal(0M, currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
        Assert.Equal((originalEntity?.Resqty).GetValueOrDefault(), currentEntity!.Resqty);
    }

    [Fact]
    public async Task AddAndRemoveAndStoreWhZStockTo0TestAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(true, ct);
        Assert.NotNull(whid);

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
        Assert.NotNull(whZoneId);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
        Assert.NotNull(whLocId);

        var originalEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whlocid == whLocId, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Whlocid = whLocId.Value,
            Qty = 15.75M
        };

        await this.AddReceivingAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whlocid == whLocId, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var removeRequest = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Whlocid = whLocId.Value,
            Qty = currentEntity.Recqty
        };

        await this.RemoveReceivingAsync(context, removeRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whlocid == whLocId, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal(0M, currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
        Assert.Equal((originalEntity?.Resqty).GetValueOrDefault(), currentEntity!.Resqty);
    }

    [Fact]
    public async Task AddAndRemoveAndStoreWhStockFailedTestAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
        Assert.NotNull(whid);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
        Assert.NotNull(whLocId);

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whlocid = whLocId.Value,
            Qty = 15.75M
        };

        await this.AddReceivingAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var removeRequest = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whlocid = whLocId.Value,
            Qty = currentEntity.Recqty * 1.75M
        };

        var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.RemoveReceivingAsync(context, removeRequest, ct));
        Assert.Equal(WhZStockExceptionType.RemoveReceivingQty, ex.Type);
        Assert.Equal("Not enough receiving quantity to fulfill the remove request", ex.Message);
    }

    [Fact]
    public async Task AddAndRemoveAndStoreWhZStockFailedTestAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(true, ct);
        Assert.NotNull(whid);

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
        Assert.NotNull(whZoneId);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
        Assert.NotNull(whLocId);

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Whlocid = whLocId.Value,
            Qty = 15.75M
        };

        await this.AddReceivingAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whlocid == whLocId, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var removeRequest = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Whlocid = whLocId.Value,
            Qty = currentEntity.Recqty * 1.75M
        };

        var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.RemoveReceivingAsync(context, removeRequest, ct));
        Assert.Equal(WhZStockExceptionType.RemoveReceivingQty, ex.Type);
        Assert.Equal("Not enough receiving quantity to fulfill the remove request", ex.Message);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public async Task AddAndCommitWhStockTest01Async(double? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
        Assert.NotNull(whid);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
        Assert.NotNull(whLocId);

        var originalEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);

        using var context = this.service.CreateContext();

        var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);
        Assert.NotNull(currentEntity);

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whlocid = whLocId.Value,
            Qty = (decimal?)qty
        };

        await this.AddReceivingAsync(context, request, ct);

        var commitRequest = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whlocid = whLocId.Value,
            Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)(currentEntity!.Recqty + request.Qty.GetValueOrDefault()), Precision, MidpointRounding.AwayFromZero))
        };

        await this.CommitReceivingAsync(context, commitRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal((originalEntity?.Recqty).GetValueOrDefault() + request.Qty - commitRequest.Qty, currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault() + commitRequest.Qty, currentEntity!.Actqty);
        Assert.Equal((originalEntity?.Resqty).GetValueOrDefault(), currentEntity!.Resqty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public async Task AddAndCommitWhZStockTest01Async(double? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(true, ct);
        Assert.NotNull(whid);

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
        Assert.NotNull(whZoneId);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
        Assert.NotNull(whLocId);

        var originalEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whlocid == whLocId, ct);

        using var context = this.service.CreateContext();

        var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whlocid == whLocId, ct);
        Assert.NotNull(currentEntity);

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Whlocid = whLocId.Value,
            Qty = (decimal?)qty
        };

        await this.AddReceivingAsync(context, request, ct);

        var commitRequest = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Whlocid = whLocId.Value,
            Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)(currentEntity!.Recqty + request.Qty.GetValueOrDefault()), Precision, MidpointRounding.AwayFromZero))
        };

        await this.CommitReceivingAsync(context, commitRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whlocid == whLocId, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal((originalEntity?.Recqty).GetValueOrDefault() + request.Qty - commitRequest.Qty, currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault() + commitRequest.Qty, currentEntity!.Actqty);
        Assert.Equal((originalEntity?.Resqty).GetValueOrDefault(), currentEntity!.Resqty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public async Task AddAndCommitWhStockTest02Async(double? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
        Assert.NotNull(whid);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
        Assert.NotNull(whLocId);

        var originalEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whlocid = whLocId.Value,
            Qty = (decimal?)qty
        };

        await this.AddReceivingAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var commitRequest = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whlocid = whLocId.Value,
            Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)currentEntity!.Recqty, Precision, MidpointRounding.AwayFromZero))
        };

        await this.CommitReceivingAsync(context, commitRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal((originalEntity?.Recqty).GetValueOrDefault() + request.Qty - commitRequest.Qty, currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault() + commitRequest.Qty, currentEntity!.Actqty);
        Assert.Equal((originalEntity?.Resqty).GetValueOrDefault(), currentEntity!.Resqty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public async Task AddAndCommitWhZStockTest02Async(double? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(true, ct);
        Assert.NotNull(whid);

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
        Assert.NotNull(whZoneId);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
        Assert.NotNull(whLocId);

        var originalEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whlocid == whLocId, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Whlocid = whLocId.Value,
            Qty = (decimal?)qty
        };

        await this.AddReceivingAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whlocid == whLocId, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var commitRequest = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Whlocid = whLocId.Value,
            Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)currentEntity!.Recqty, Precision, MidpointRounding.AwayFromZero))
        };

        await this.CommitReceivingAsync(context, commitRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whlocid == whLocId, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal((originalEntity?.Recqty).GetValueOrDefault() + request.Qty - commitRequest.Qty, currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault() + commitRequest.Qty, currentEntity!.Actqty);
        Assert.Equal((originalEntity?.Resqty).GetValueOrDefault(), currentEntity!.Resqty);
    }

    [Fact]
    public async Task AddAndCommitWhStockTo0TestAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
        Assert.NotNull(whid);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
        Assert.NotNull(whLocId);

        var originalEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whlocid = whLocId.Value,
            Qty = 15.75M
        };

        await this.AddReceivingAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var commitRequest = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whlocid = whLocId.Value,
            Qty = currentEntity.Recqty
        };

        await this.CommitReceivingAsync(context, commitRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal(0M, currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault() + commitRequest.Qty, currentEntity!.Actqty);
        Assert.Equal((originalEntity?.Resqty).GetValueOrDefault(), currentEntity!.Resqty);
    }

    [Fact]
    public async Task AddAndCommitWhZStockTo0TestAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(true, ct);
        Assert.NotNull(whid);

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
        Assert.NotNull(whZoneId);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
        Assert.NotNull(whLocId);

        var originalEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whlocid == whLocId, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Whlocid = whLocId.Value,
            Qty = 15.75M
        };

        await this.AddReceivingAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whlocid == whLocId, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var commitRequest = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Whlocid = whLocId.Value,
            Qty = currentEntity.Recqty
        };

        await this.CommitReceivingAsync(context, commitRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whlocid == whLocId, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal(0M, currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault() + commitRequest.Qty, currentEntity!.Actqty);
        Assert.Equal((originalEntity?.Resqty).GetValueOrDefault(), currentEntity!.Resqty);
    }

    [Fact]
    public async Task AddAndCommitWhStockFailedTestAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
        Assert.NotNull(whid);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
        Assert.NotNull(whLocId);

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whlocid = whLocId.Value,
            Qty = 15.75M
        };

        await this.AddReceivingAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var commitRequest = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whlocid = whLocId.Value,
            Qty = currentEntity.Recqty * 1.75M
        };

        var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.CommitReceivingAsync(context, commitRequest, ct));
        Assert.Equal(WhZStockExceptionType.CommitReceivingQty, ex.Type);
        Assert.Equal("Not enough receiving quantity to fulfill the commit request", ex.Message);
    }

    [Fact]
    public async Task AddAndCommitWhZStockFailedTestAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(true, ct);
        Assert.NotNull(whid);

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
        Assert.NotNull(whZoneId);

        var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
        Assert.NotNull(whLocId);

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Whlocid = whLocId.Value,
            Qty = 15.75M
        };

        await this.AddReceivingAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whlocid == whLocId, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var commitRequest = new WhZStockMapDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Whlocid = whLocId.Value,
            Qty = currentEntity.Recqty * 1.75M
        };

        var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.CommitReceivingAsync(context, commitRequest, ct));
        Assert.Equal(WhZStockExceptionType.CommitReceivingQty, ex.Type);
        Assert.Equal("Not enough receiving quantity to fulfill the commit request", ex.Message);
    }

    [Theory]
    [InlineData(5, 7)]
    public async Task AddMultipleItemsWhStockTestAsync(int numberOfItems, int numberOfLocations)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
        Assert.NotNull(whid);

        using var context = this.service.CreateContext();

        var list = new List<(int itemId, int whLocId, decimal qty, OlcWhzstockmap? stockMap)>();

        var random = new Random();
        for (var i = 0; i < numberOfItems; i++)
        {
            var itemId = await this.GetRandomItemIdAsync(ct);
            Assert.NotNull(itemId);

            for (var j = 0; j < numberOfLocations; j++)
            {
                var whLocId = await this.GetRandomWhLocIdAsync(whid!, null, ct);
                Assert.NotNull(whLocId);

                var request = new WhZStockMapDto
                {
                    Itemid = itemId.Value,
                    Whid = whid!,
                    Whlocid = whLocId.Value,
                    Qty = Convert.ToDecimal(Math.Round(random.NextDouble() * random.NextDouble() * 1000, 2, MidpointRounding.AwayFromZero))
                };

                var entity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(i => i.Itemid == itemId && i.Whid == whid && i.Whzoneid == null && i.Whlocid == whLocId, ct);
                list.Add((itemId.Value, whLocId.Value, request.Qty.Value, entity));

                await this.AddReceivingAsync(context, request, ct);
            }
        }

        await this.StoreAsync(context, ct);

        foreach (var (itemId, whLocId, qty, stockMap) in list.GroupBy(l => new { l.itemId, l.whLocId }).Select(l => (l.Key.itemId, l.Key.whLocId, l.Sum(l1 => l1.qty), l.First().stockMap)))
        {
            var entity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(i => i.Itemid == itemId && i.Whid == whid && i.Whzoneid == null && i.Whlocid == whLocId, ct);
            Assert.NotNull(entity);
            Assert.Equal((stockMap?.Recqty).GetValueOrDefault() + qty, entity!.Recqty);
        }
    }

    [Theory]
    [InlineData(5, 3, 7)]
    //[InlineData(50, 30, 70, Skip = "need optimization")]
    public async Task AddMultipleItemsWhZStockTestAsync(int numberOfItems, int numberOfZones, int numberOfLocations)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var whid = await this.GetFirstWarehouseIdAsync(true, ct);
        Assert.NotNull(whid);

        using var context = this.service.CreateContext();

        var list = new List<(int itemId, int whZoneId, int whLocId, decimal qty, OlcWhzstockmap? stockMap)>();

        var random = new Random();
        for (var i = 0; i < numberOfItems; i++)
        {
            var itemId = await this.GetRandomItemIdAsync(ct);
            Assert.NotNull(itemId);

            for (var j = 0; j < numberOfZones; j++)
            {
                var whZoneId = await this.GetRandomWhZoneIdAsync(whid!, true, ct);
                Assert.NotNull(whZoneId);

                for (var k = 0; k < numberOfLocations; k++)
                {
                    var whLocId = await this.GetRandomWhLocIdAsync(whid!, whZoneId, ct);
                    Assert.NotNull(whLocId);

                    var request = new WhZStockMapDto
                    {
                        Itemid = itemId.Value,
                        Whid = whid!,
                        Whzoneid = whZoneId,
                        Whlocid = whLocId.Value,
                        Qty = Convert.ToDecimal(Math.Round(random.NextDouble() * random.NextDouble() * 1000, 2, MidpointRounding.AwayFromZero))
                    };

                    var entity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(i => i.Itemid == itemId && i.Whid == whid && i.Whzoneid == whZoneId && i.Whlocid == whLocId, ct);
                    list.Add((itemId.Value, whZoneId.Value, whLocId.Value, request.Qty.Value, entity));

                    await this.AddReceivingAsync(context, request, ct);
                }
            }
        }

        await this.StoreAsync(context, ct);

        var group = list
            .GroupBy(l => new { l.itemId, l.whZoneId, l.whLocId })
            .Select(l => (l.Key.itemId, l.Key.whZoneId, l.Key.whLocId, l.Sum(l1 => l1.qty), l.First().stockMap));
        foreach (var (itemId, whZoneId, whLocId, qty, stockMap) in group)
        {
            var entity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(i => i.Itemid == itemId && i.Whid == whid && i.Whzoneid == whZoneId && i.Whlocid == whLocId, ct);
            Assert.NotNull(entity);
            Assert.Equal((stockMap?.Recqty).GetValueOrDefault() + qty, entity!.Recqty);
        }
    }

    // CommitMultipleItemsWhStockTestAsync
}
