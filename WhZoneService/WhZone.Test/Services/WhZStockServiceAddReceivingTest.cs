using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Containers.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Enums;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhZone.Test.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace eLog.HeavyTools.Services.WhZone.Test.Services;

public class WhZStockServiceAddReceivingTest : Base.WhZStockServiceTestBase
{
    public WhZStockServiceAddReceivingTest(ITestOutputHelper testOutputHelper, TestFixture fixture) : base(testOutputHelper, fixture)
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

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
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

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, cancellationToken: ct);
        Assert.NotNull(whZoneId);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId.Value,
            Qty = (decimal?)qty
        };

        await this.AddReceivingAsync(context, request, ct);
    }

    [Fact]
    public async Task AddReceivingWhStockNullFailedTestAsync()
    {
        var ex = await Assert.ThrowsAsync<WhZStockServiceException>(() => this.AddReceivingWhStockTestAsync(null));
        Assert.Equal(WhZStockExceptionType.InvalidRequestQty, ex.Type);
        Assert.Equal("The add qty is not set", ex.Message);
    }

    [Fact]
    public async Task AddReceivingWhZStockNullFailedTestAsync()
    {
        var ex = await Assert.ThrowsAsync<WhZStockServiceException>(() => this.AddReceivingWhZStockTestAsync(null));
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
        var ex = await Assert.ThrowsAsync<WhZStockServiceException>(() => this.AddReceivingWhStockTestAsync(qty));
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
        var ex = await Assert.ThrowsAsync<WhZStockServiceException>(() => this.AddReceivingWhZStockTestAsync(qty));
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

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
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

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, cancellationToken: ct);
        Assert.NotNull(whZoneId);

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
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

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
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

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
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

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
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

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = 10
        };

        var data = await this.AddReceivingAsync(context, request, ct);
        await this.AddReservedAsync(context, request, ct);
        var ex = Assert.Throws<WhZStockServiceException>(() => this.Delete(context, data!));
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

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, cancellationToken: ct);
        Assert.NotNull(whZoneId);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
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

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, cancellationToken: ct);
        Assert.NotNull(whZoneId);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
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

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, cancellationToken: ct);
        Assert.NotNull(whZoneId);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
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

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, cancellationToken: ct);
        Assert.NotNull(whZoneId);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = 10
        };

        var data = await this.AddReceivingAsync(context, request, ct);
        await this.AddReservedAsync(context, request, ct);
        var ex = Assert.Throws<WhZStockServiceException>(() => this.Delete(context, data!));
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

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = (decimal?)qty
        };

        var ex = await Assert.ThrowsAsync<WhZStockServiceException>(() => this.RemoveReceivingAsync(context, request, ct));
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

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, cancellationToken: ct);
        Assert.NotNull(whZoneId);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = (decimal?)qty
        };

        var ex = await Assert.ThrowsAsync<WhZStockServiceException>(() => this.RemoveReceivingAsync(context, request, ct));
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

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);

        using var context = this.service.CreateContext();

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = (decimal?)qty
        };

        await this.AddReceivingAsync(context, request, ct);

        var removeRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)(currentEntity.Recqty + request.Qty.GetValueOrDefault()), Precision, MidpointRounding.AwayFromZero))
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

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, cancellationToken: ct);
        Assert.NotNull(whZoneId);

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);

        using var context = this.service.CreateContext();

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = (decimal?)qty
        };

        await this.AddReceivingAsync(context, request, ct);

        var removeRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
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

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = (decimal?)qty
        };

        await this.AddReceivingAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var removeRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
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

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, cancellationToken: ct);
        Assert.NotNull(whZoneId);

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = (decimal?)qty
        };

        await this.AddReceivingAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var removeRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
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

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = 15.75M
        };

        await this.AddReceivingAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var removeRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = currentEntity.Recqty
        };

        await this.RemoveReceivingAsync(context, removeRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
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

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, cancellationToken: ct);
        Assert.NotNull(whZoneId);

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = 15.75M
        };

        await this.AddReceivingAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var removeRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = currentEntity.Recqty
        };

        await this.RemoveReceivingAsync(context, removeRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
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

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = 15.75M
        };

        await this.AddReceivingAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var removeRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = currentEntity.Recqty * 1.75M
        };

        var ex = await Assert.ThrowsAsync<WhZStockServiceException>(() => this.RemoveReceivingAsync(context, removeRequest, ct));
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

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, cancellationToken: ct);
        Assert.NotNull(whZoneId);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = 15.75M
        };

        await this.AddReceivingAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var removeRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = currentEntity.Recqty * 1.75M
        };

        var ex = await Assert.ThrowsAsync<WhZStockServiceException>(() => this.RemoveReceivingAsync(context, removeRequest, ct));
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

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);

        using var context = this.service.CreateContext();

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = (decimal?)qty
        };

        await this.AddReceivingAsync(context, request, ct);

        var commitRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)(currentEntity!.Recqty + request.Qty.GetValueOrDefault()), Precision, MidpointRounding.AwayFromZero))
        };

        await this.CommitReceivingAsync(context, commitRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
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

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, cancellationToken: ct);
        Assert.NotNull(whZoneId);

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);

        using var context = this.service.CreateContext();

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = (decimal?)qty
        };

        await this.AddReceivingAsync(context, request, ct);

        var commitRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)(currentEntity!.Recqty + request.Qty.GetValueOrDefault()), Precision, MidpointRounding.AwayFromZero))
        };

        await this.CommitReceivingAsync(context, commitRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
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

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = (decimal?)qty
        };

        await this.AddReceivingAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var commitRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)currentEntity!.Recqty, Precision, MidpointRounding.AwayFromZero))
        };

        await this.CommitReceivingAsync(context, commitRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
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

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, cancellationToken: ct);
        Assert.NotNull(whZoneId);

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = (decimal?)qty
        };

        await this.AddReceivingAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var commitRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)currentEntity!.Recqty, Precision, MidpointRounding.AwayFromZero))
        };

        await this.CommitReceivingAsync(context, commitRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
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

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = 15.75M
        };

        await this.AddReceivingAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var commitRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = currentEntity.Recqty
        };

        await this.CommitReceivingAsync(context, commitRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
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

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, cancellationToken: ct);
        Assert.NotNull(whZoneId);

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = 15.75M
        };

        await this.AddReceivingAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var commitRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = currentEntity.Recqty
        };

        await this.CommitReceivingAsync(context, commitRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
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

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = 15.75M
        };

        await this.AddReceivingAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var commitRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = currentEntity.Recqty * 1.75M
        };

        var ex = await Assert.ThrowsAsync<WhZStockServiceException>(() => this.CommitReceivingAsync(context, commitRequest, ct));
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

        var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, cancellationToken: ct);
        Assert.NotNull(whZoneId);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = 15.75M
        };

        await this.AddReceivingAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Recqty);

        var commitRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = currentEntity.Recqty * 1.75M
        };

        var ex = await Assert.ThrowsAsync<WhZStockServiceException>(() => this.CommitReceivingAsync(context, commitRequest, ct));
        Assert.Equal(WhZStockExceptionType.CommitReceivingQty, ex.Type);
        Assert.Equal("Not enough receiving quantity to fulfill the commit request", ex.Message);
    }

    [Theory]
    [InlineData(5)]
    public async Task AddMultipleItemsWhStockTestAsync(int numberOfItems)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
        Assert.NotNull(whid);

        using var context = this.service.CreateContext();

        var list = new List<(int itemId, decimal qty, OlcWhzstock? stock)>();

        var random = new Random();
        for (var i = 0; i < numberOfItems; i++)
        {
            var itemId = await this.GetRandomItemIdAsync(ct);
            Assert.NotNull(itemId);

            var request = new WhZStockDto
            {
                Itemid = itemId.Value,
                Whid = whid!,
                Qty = Convert.ToDecimal(Math.Round(random.NextDouble() * random.NextDouble() * 1000, 2, MidpointRounding.AwayFromZero))
            };

            var entity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(i => i.Itemid == itemId && i.Whid == whid && i.Whzoneid == null, ct);
            list.Add((itemId.Value, request.Qty.Value, entity));

            await this.AddReceivingAsync(context, request, ct);
        }

        await this.StoreAsync(context, ct);

        foreach (var (itemId, qty, stock) in list)
        {
            var entity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(i => i.Itemid == itemId && i.Whid == whid && i.Whzoneid == null, ct);
            Assert.NotNull(entity);
            Assert.Equal((stock?.Recqty).GetValueOrDefault() + qty, entity!.Recqty);
        }
    }

    [Theory]
    [InlineData(5)]
    public async Task AddMultipleItemsWhZStockTestAsync(int numberOfItems)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var whid = await this.GetFirstWarehouseIdAsync(true, ct);
        Assert.NotNull(whid);

        using var context = this.service.CreateContext();

        var list = new List<(int itemId, int zoneId, decimal qty, OlcWhzstock? entity)>();

        var random = new Random();
        for (var i = 0; i < numberOfItems; i++)
        {
            var itemId = await this.GetRandomItemIdAsync(ct);
            Assert.NotNull(itemId);

            var whZoneId = await this.GetRandomWhZoneIdAsync(whid!, cancellationToken: ct);
            Assert.NotNull(whZoneId);

            var request = new WhZStockDto
            {
                Itemid = itemId.Value,
                Whid = whid!,
                Whzoneid = whZoneId,
                Qty = Convert.ToDecimal(Math.Round(random.NextDouble() * random.NextDouble() * 1000, 2, MidpointRounding.AwayFromZero))
            };

            var entity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(i => i.Itemid == itemId && i.Whid == whid && i.Whzoneid == whZoneId, ct);
            list.Add((itemId.Value, whZoneId.Value, request.Qty.Value, entity));

            await this.AddReceivingAsync(context, request, ct);
        }

        await this.StoreAsync(context, ct);

        foreach (var (itemId, whZoneId, qty, stock) in list)
        {
            var entity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(i => i.Itemid == itemId && i.Whid == whid && i.Whzoneid == whZoneId, ct);
            Assert.NotNull(entity);
            Assert.Equal((stock?.Recqty).GetValueOrDefault() + qty, entity!.Recqty);
        }
    }

    [Theory]
    [InlineData(5)]
    public async Task CommitMultipleItemsWhStockTestAsync(int numberOfItems)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
        Assert.NotNull(whid);

        using var context = this.service.CreateContext();

        var list = new List<(OlcWhzstock stock, decimal? qty)>();

        var random = new Random();
        for (var i = 0; i < numberOfItems; i++)
        {
            var tries = 15;
            OlcWhzstock? stock;
            do
            {
                Assert.True(--tries >= 0, "Usable stock not found");

                stock = await this.GetRandomStockAsync(whid!, false, cancellationToken: ct);
                Assert.NotNull(stock);
                Assert.Null(stock!.Whzoneid);
                Assert.NotEqual(0M, stock.Recqty);
            } while (list.Any(l => OlcWhzstock.Comparer.Equals(l.stock, stock)));

            var request = new WhZStockDto
            {
                Itemid = stock.Itemid,
                Whid = stock.Whid,
                Whzoneid = stock.Whzoneid,
                Qty = Convert.ToDecimal(Math.Round(random.NextDouble() * (double)stock.Recqty, 2, MidpointRounding.AwayFromZero))
            };

            list.Add((stock, request.Qty));

            await this.CommitReceivingAsync(context, request, ct);
        }

        await this.StoreAsync(context, ct);

        var groupped = list
            .GroupBy(i => new { i.stock.Itemid, i.stock.Whid, i.stock.Whzoneid })
            .Select(i => (i.First().stock, i.Sum(j => j.qty.GetValueOrDefault())));
        foreach (var (stock, qty) in groupped)
        {
            var entity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(i => i.Itemid == stock!.Itemid && i.Whid == stock.Whid && i.Whzoneid == stock.Whzoneid, ct);
            Assert.NotNull(entity);
            Assert.Equal((stock?.Recqty).GetValueOrDefault() - qty, entity!.Recqty);
            Assert.Equal((stock?.Actqty).GetValueOrDefault() + qty, entity!.Actqty);
        }
    }

    [Theory]
    [InlineData(5)]
    public async Task CommitMultipleItemsWhZStockTestAsync(int numberOfItems)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var whid = await this.GetFirstWarehouseIdAsync(true, ct);
        Assert.NotNull(whid);

        using var context = this.service.CreateContext();

        var list = new List<(OlcWhzstock stock, decimal? qty)>();

        var random = new Random();
        for (var i = 0; i < numberOfItems; i++)
        {
            var tries = 15;
            OlcWhzstock? stock;
            do
            {
                Assert.True(--tries >= 0, "Usable stock not found");

                stock = await this.GetRandomStockAsync(whid!, true, cancellationToken: ct);
                Assert.NotNull(stock);
                Assert.NotNull(stock!.Whzoneid);
                Assert.NotEqual(0M, stock.Recqty);
            } while (list.Any(l => OlcWhzstock.Comparer.Equals(l.stock, stock)));

            var request = new WhZStockDto
            {
                Itemid = stock.Itemid,
                Whid = stock.Whid,
                Whzoneid = stock.Whzoneid,
                Qty = Convert.ToDecimal(Math.Round(random.NextDouble() * (double)stock.Recqty, 2, MidpointRounding.AwayFromZero))
            };

            list.Add((stock, request.Qty));

            await this.CommitReceivingAsync(context, request, ct);
        }

        await this.StoreAsync(context, ct);

        var groupped = list
            .GroupBy(i => new { i.stock.Itemid, i.stock.Whid, i.stock.Whzoneid })
            .Select(i => (i.First().stock, i.Sum(j => j.qty.GetValueOrDefault())));
        foreach (var (stock, qty) in groupped)
        {
            var entity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(i => i.Itemid == stock!.Itemid && i.Whid == stock.Whid && i.Whzoneid == stock.Whzoneid, ct);
            Assert.NotNull(entity);
            Assert.Equal((stock?.Recqty).GetValueOrDefault() - qty, entity!.Recqty);
            Assert.Equal((stock?.Actqty).GetValueOrDefault() + qty, entity!.Actqty);
        }
    }
}
