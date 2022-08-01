using System;
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
using eLog.HeavyTools.Services.WhZone.DataAccess.Context;
using eLog.HeavyTools.Services.WhZone.Test.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace eLog.HeavyTools.Services.WhZone.Test.Services;

public class WhZStockServiceAddReservedTest : Base.WhZStockServiceTestBase
{
    public WhZStockServiceAddReservedTest(ITestOutputHelper testOutputHelper, TestFixture fixture) : base(testOutputHelper, fixture)
    {
    }

    // TODO: nagyobb keszletmozgas random halmazt tomegesen futtatni (pl.: 5000 random bevet/kivet/raktarkozi), sorosan es parhumazosan

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public async Task AddWhStockTestAsync(double? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
        Assert.NotNull(whid);

        await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, null, (decimal?)qty, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = (decimal?)qty
        };

        await this.AddReservedAsync(context, request, ct);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public async Task AddWhZStockTestAsync(double? qty)
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

        await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, whZoneId, (decimal?)qty, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = (decimal?)qty
        };

        await this.AddReservedAsync(context, request, ct);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public async Task AddAfterReceivingWhStockTestAsync(double? qty)
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
            Qty = (decimal?)qty * (new Random().Next(5) + 1)
        };

        await this.AddReceivingAsync(context, request, ct);

        request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = (decimal?)qty
        };

        await this.AddReservedAsync(context, request, ct);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public async Task AddAfterReceivingWhZStockTestAsync(double? qty)
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
            Qty = (decimal?)qty * (new Random().Next(5) + 1)
        };

        await this.AddReceivingAsync(context, request, ct);

        request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = (decimal?)qty
        };

        await this.AddReservedAsync(context, request, ct);
    }

    [Fact]
    public async Task AddWhStockNullFailedTestAsync()
    {
        var ex = await Assert.ThrowsAsync<WhZStockServiceException>(() => this.AddWhStockTestAsync(null));
        Assert.Equal(WhZStockExceptionType.InvalidRequestQty, ex.Type);
        Assert.Equal("The add qty is not set", ex.Message);
    }

    [Fact]
    public async Task AddWhZStockNullFailedTestAsync()
    {
        var ex = await Assert.ThrowsAsync<WhZStockServiceException>(() => this.AddWhZStockTestAsync(null));
        Assert.Equal(WhZStockExceptionType.InvalidRequestQty, ex.Type);
        Assert.Equal("The add qty is not set", ex.Message);
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-15.0)]
    [InlineData(-0.25)]
    [InlineData(-5.25)]
    public async Task AddWhStockFailedTestAsync(double? qty)
    {
        var ex = await Assert.ThrowsAsync<WhZStockServiceException>(() => this.AddWhStockTestAsync(qty));
        Assert.Equal(WhZStockExceptionType.InvalidRequestQty, ex.Type);
        Assert.Equal("The add qty cannot be less or equal to 0", ex.Message);
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-15.0)]
    [InlineData(-0.25)]
    [InlineData(-5.25)]
    public async Task AddWhZStockFailedTestAsync(double? qty)
    {
        var ex = await Assert.ThrowsAsync<WhZStockServiceException>(() => this.AddWhZStockTestAsync(qty));
        Assert.Equal(WhZStockExceptionType.InvalidRequestQty, ex.Type);
        Assert.Equal("The add qty cannot be less or equal to 0", ex.Message);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public async Task AddAndStoreWhStockTestAsync(double? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
        Assert.NotNull(whid);

        await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, null, (decimal?)qty, ct);

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = (decimal?)qty
        };

        await this.AddReservedAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
        Assert.Equal((originalEntity?.Resqty).GetValueOrDefault() + request.Qty, currentEntity!.Resqty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public async Task AddAndStoreWhZStockTestAsync(double? qty)
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

        await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, whZoneId, (decimal?)qty, ct);

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = (decimal?)qty
        };

        await this.AddReservedAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
        Assert.Equal((originalEntity?.Resqty).GetValueOrDefault() + request.Qty, currentEntity!.Resqty);
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

        await this.AddReceivingAsync(context, request, ct);
        var data = await this.AddReservedAsync(context, request, ct);
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

        await this.AddReceivingAsync(context, request, ct);
        await this.AddReservedAsync(context, request, ct);
        var data = await this.CommitReservedAsync(context, request, ct);
        Assert.Contains(context.MovementList, l => l == data);
        this.Delete(context, data!);
        Assert.DoesNotContain(context.MovementList, l => l == data);
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

        await this.AddReceivingAsync(context, request, ct);
        var data = await this.AddReservedAsync(context, request, ct);
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

        await this.AddReceivingAsync(context, request, ct);
        await this.AddReservedAsync(context, request, ct);
        var data = await this.CommitReservedAsync(context, request, ct);
        Assert.Contains(context.MovementList, l => l == data);
        this.Delete(context, data!);
        Assert.DoesNotContain(context.MovementList, l => l == data);
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

        await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, null, (decimal?)qty, ct);

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

        await this.AddReservedAsync(context, request, ct);

        var removeRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)(currentEntity!.Resqty + request.Qty.GetValueOrDefault()), Precision, MidpointRounding.AwayFromZero))
        };

        await this.RemoveReservedAsync(context, removeRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
        Assert.Equal((originalEntity?.Resqty).GetValueOrDefault() + request.Qty - removeRequest.Qty, currentEntity!.Resqty);
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

        await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, whZoneId, (decimal?)qty, ct);

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

        await this.AddReservedAsync(context, request, ct);

        var removeRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)(currentEntity!.Resqty + request.Qty.GetValueOrDefault()), Precision, MidpointRounding.AwayFromZero))
        };

        await this.RemoveReservedAsync(context, removeRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
        Assert.Equal((originalEntity?.Resqty).GetValueOrDefault() + request.Qty - removeRequest.Qty, currentEntity!.Resqty);
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

        await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, null, (decimal?)qty, ct);

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = (decimal?)qty
        };

        await this.AddReservedAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Resqty);

        var removeRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)currentEntity.Resqty, Precision, MidpointRounding.AwayFromZero))
        };

        await this.RemoveReservedAsync(context, removeRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
        Assert.Equal((originalEntity?.Resqty).GetValueOrDefault() + request.Qty - removeRequest.Qty, currentEntity!.Resqty);
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

        await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, whZoneId, (decimal?)qty, ct);

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = (decimal?)qty
        };

        await this.AddReservedAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Resqty);

        var removeRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)currentEntity.Resqty, Precision, MidpointRounding.AwayFromZero))
        };

        await this.RemoveReservedAsync(context, removeRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
        Assert.Equal((originalEntity?.Resqty).GetValueOrDefault() + request.Qty - removeRequest.Qty, currentEntity!.Resqty);
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

        await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, null, 15.75M, ct);

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = 15.75M
        };

        await this.AddReservedAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Resqty);

        var removeRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = currentEntity.Resqty
        };

        await this.RemoveReservedAsync(context, removeRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
        Assert.Equal(0M, currentEntity!.Resqty);
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

        await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, whZoneId, 15.75M, ct);

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = 15.75M
        };

        await this.AddReservedAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Resqty);

        var removeRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = currentEntity.Resqty
        };

        await this.RemoveReservedAsync(context, removeRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
        Assert.Equal(0M, currentEntity!.Resqty);
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

        await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, null, 15.75M, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = 15.75M
        };

        await this.AddReservedAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Resqty);

        var removeRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = currentEntity.Resqty * 1.75M
        };

        var ex = await Assert.ThrowsAsync<WhZStockServiceException>(() => this.RemoveReservedAsync(context, removeRequest, ct));
        Assert.Equal(WhZStockExceptionType.RemoveReservedQty, ex.Type);
        Assert.Equal("Not enough reserved quantity to fulfill the remove request", ex.Message);
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

        await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, whZoneId, 15.75M, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = 15.75M
        };

        await this.AddReservedAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Resqty);

        var removeRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = currentEntity.Resqty * 1.75M
        };

        var ex = await Assert.ThrowsAsync<WhZStockServiceException>(() => this.RemoveReservedAsync(context, removeRequest, ct));
        Assert.Equal(WhZStockExceptionType.RemoveReservedQty, ex.Type);
        Assert.Equal("Not enough reserved quantity to fulfill the remove request", ex.Message);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public async Task AddAndCommitAndStoreWhStockTest01Async(double? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
        Assert.NotNull(whid);

        await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, null, (decimal?)qty, ct);

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);

        using var context = this.service.CreateContext();

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Actqty);

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = (decimal?)qty
        };

        await this.AddReservedAsync(context, request, ct);

        var commitRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)Math.Min(currentEntity!.Resqty + request.Qty.GetValueOrDefault(), currentEntity.Actqty), Precision, MidpointRounding.AwayFromZero))
        };

        await this.CommitReservedAsync(context, commitRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault() - commitRequest.Qty, currentEntity!.Actqty);
        Assert.Equal((originalEntity?.Resqty).GetValueOrDefault() + request.Qty - commitRequest.Qty, currentEntity!.Resqty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public async Task AddAndCommitAndStoreWhZStockTest01Async(double? qty)
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

        await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, whZoneId, (decimal?)qty, ct);

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);

        using var context = this.service.CreateContext();

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Actqty);

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = (decimal?)qty
        };

        await this.AddReservedAsync(context, request, ct);

        var commitRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)Math.Min(currentEntity!.Resqty + request.Qty.GetValueOrDefault(), currentEntity.Actqty), Precision, MidpointRounding.AwayFromZero))
        };

        await this.CommitReservedAsync(context, commitRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault() - commitRequest.Qty, currentEntity!.Actqty);
        Assert.Equal((originalEntity?.Resqty).GetValueOrDefault() + request.Qty - commitRequest.Qty, currentEntity!.Resqty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public async Task AddAndCommitAndStoreWhStockTest02Async(double? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
        Assert.NotNull(whid);

        await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, null, (decimal?)qty, ct);

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = (decimal?)qty
        };

        await this.AddReservedAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Resqty);
        Assert.NotEqual(0M, currentEntity!.Actqty);

        var commitRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)Math.Min(currentEntity.Resqty, currentEntity.Actqty), Precision, MidpointRounding.AwayFromZero))
        };

        await this.CommitReservedAsync(context, commitRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault() - commitRequest.Qty, currentEntity!.Actqty);
        Assert.Equal((originalEntity?.Resqty).GetValueOrDefault() + request.Qty - commitRequest.Qty, currentEntity!.Resqty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public async Task AddAndCommitAndStoreWhZStockTest02Async(double? qty)
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

        await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, whZoneId, (decimal?)qty, ct);

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = (decimal?)qty
        };

        await this.AddReservedAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Resqty);
        Assert.NotEqual(0M, currentEntity!.Actqty);

        var commitRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)Math.Min(currentEntity.Resqty, currentEntity.Actqty), Precision, MidpointRounding.AwayFromZero))
        };

        await this.CommitReservedAsync(context, commitRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault() - commitRequest.Qty, currentEntity!.Actqty);
        Assert.Equal((originalEntity?.Resqty).GetValueOrDefault() + request.Qty - commitRequest.Qty, currentEntity!.Resqty);
    }

    [Fact]
    public async Task AddAndCommitAndStoreWhStockTo0TestAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
        Assert.NotNull(whid);

        await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, null, 15.75M, ct);

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = 15.75M
        };

        await this.AddReservedAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Resqty);

        var commitRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = currentEntity.Resqty
        };

        await this.CommitReservedAsync(context, commitRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault() - commitRequest.Qty, currentEntity!.Actqty);
        Assert.Equal(0M, currentEntity!.Resqty);
    }

    [Fact]
    public async Task AddAndCommitAndStoreWhZStockTo0TestAsync()
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

        await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, whZoneId, 15.75M, ct);

        var originalEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = 15.75M
        };

        await this.AddReservedAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Resqty);

        var commitRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = currentEntity.Resqty
        };

        await this.CommitReservedAsync(context, commitRequest, ct);

        await this.StoreAsync(context, ct);

        currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);
        Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
        Assert.Equal((originalEntity?.Actqty).GetValueOrDefault() - commitRequest.Qty, currentEntity!.Actqty);
        Assert.Equal(0M, currentEntity!.Resqty);
    }

    [Fact]
    public async Task AddAndCommitAndStoreWhStockFailedTestAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var itemId = await this.GetFirstItemIdAsync(ct);
        Assert.NotNull(itemId);

        var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
        Assert.NotNull(whid);

        await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, null, 15.75M, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = 15.75M
        };

        await this.AddReservedAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Resqty);

        var commitRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Qty = currentEntity.Resqty * 1.75M
        };

        var ex = await Assert.ThrowsAsync<WhZStockServiceException>(() => this.CommitReservedAsync(context, commitRequest, ct));
        Assert.Equal(WhZStockExceptionType.CommitReservedQty, ex.Type);
        Assert.Equal("Not enough reserved quantity to fulfill the commit request", ex.Message);
    }

    [Fact]
    public async Task AddAndCommitAndStoreWhZStockFailedTestAsync()
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

        await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, whZoneId, 15.75M, ct);

        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = 15.75M
        };

        await this.AddReservedAsync(context, request, ct);

        await this.StoreAsync(context, ct);

        var currentEntity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
        Assert.NotNull(currentEntity);
        Assert.NotEqual(0M, currentEntity!.Resqty);

        var commitRequest = new WhZStockDto
        {
            Itemid = itemId.Value,
            Whid = whid!,
            Whzoneid = whZoneId,
            Qty = currentEntity.Resqty * 1.75M
        };

        var ex = await Assert.ThrowsAsync<WhZStockServiceException>(() => this.CommitReservedAsync(context, commitRequest, ct));
        Assert.Equal(WhZStockExceptionType.CommitReservedQty, ex.Type);
        Assert.Equal("Not enough reserved quantity to fulfill the commit request", ex.Message);
    }

    [Theory]
    [InlineData(5)]
    public async Task AddMultipleItemsWhStock01TestAsync(int numberOfItems)
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
            var tries = 15;
            int? itemId;
            do
            {
                Assert.True(--tries >= 0, "Usable item not found");

                itemId = await this.GetRandomItemIdAsync(ct);
                Assert.NotNull(itemId);
            } while (list.Any(l => l.itemId == itemId));

            var request = new WhZStockDto
            {
                Itemid = itemId.Value,
                Whid = whid!,
                Qty = Convert.ToDecimal(Math.Round(random.NextDouble() * random.NextDouble() * 1000, 2, MidpointRounding.AwayFromZero))
            };

            await this.AddReceivingAsync(context, request, ct);
            await this.CommitReceivingAsync(context, request, ct);

            request = new WhZStockDto
            {
                Itemid = request.Itemid,
                Whid = request.Whid,
                Qty = Convert.ToDecimal(Math.Round(random.NextDouble() * (double)request.Qty, 2, MidpointRounding.AwayFromZero))
            };

            var entity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(i => i.Itemid == itemId && i.Whid == whid && i.Whzoneid == null, ct);
            list.Add((itemId.Value, request.Qty.Value, entity));

            await this.AddReservedAsync(context, request, ct);
        }

        await this.StoreAsync(context, ct);

        foreach (var (itemId, qty, stock) in list)
        {
            var entity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(i => i.Itemid == itemId && i.Whid == whid && i.Whzoneid == null, ct);
            Assert.NotNull(entity);
            Assert.Equal((stock?.Resqty).GetValueOrDefault() + qty, entity!.Resqty);
        }
    }

    [Theory]
    [InlineData(5)]
    public async Task AddMultipleItemsWhZStock01TestAsync(int numberOfItems)
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
            var tries = 15;
            int? itemId;
            do
            {
                Assert.True(--tries >= 0, "Usable item not found");

                itemId = await this.GetRandomItemIdAsync(ct);
                Assert.NotNull(itemId);
            } while (list.Any(l => l.itemId == itemId));

            var whZoneId = await this.GetRandomWhZoneIdAsync(whid!, cancellationToken: ct);
            Assert.NotNull(whZoneId);

            var request = new WhZStockDto
            {
                Itemid = itemId.Value,
                Whid = whid!,
                Whzoneid = whZoneId,
                Qty = Convert.ToDecimal(Math.Round(random.NextDouble() * random.NextDouble() * 1000, 2, MidpointRounding.AwayFromZero))
            };

            await this.AddReceivingAsync(context, request, ct);
            await this.CommitReceivingAsync(context, request, ct);

            request = new WhZStockDto
            {
                Itemid = request.Itemid,
                Whid = request.Whid,
                Whzoneid = whZoneId,
                Qty = Convert.ToDecimal(Math.Round(random.NextDouble() * (double)request.Qty, 2, MidpointRounding.AwayFromZero))
            };

            var entity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(i => i.Itemid == itemId && i.Whid == whid && i.Whzoneid == null, ct);
            list.Add((itemId.Value, whZoneId.Value, request.Qty.Value, entity));

            await this.AddReservedAsync(context, request, ct);
        }

        await this.StoreAsync(context, ct);

        foreach (var (itemId, whZoneId, qty, stock) in list)
        {
            var entity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(i => i.Itemid == itemId && i.Whid == whid && i.Whzoneid == whZoneId, ct);
            Assert.NotNull(entity);
            Assert.Equal((stock?.Resqty).GetValueOrDefault() + qty, entity!.Resqty);
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

                stock = await this.GetRandomStockAsync(whid!, false, true, ct);
                Assert.NotNull(stock);
                Assert.Null(stock!.Whzoneid);
                Assert.NotEqual(0M, stock.Resqty);
            } while (list.Any(l => OlcWhzstock.Comparer.Equals(l.stock, stock)));

            var request = new WhZStockDto
            {
                Itemid = stock.Itemid,
                Whid = stock.Whid,
                Whzoneid = stock.Whzoneid,
                Qty = Convert.ToDecimal(Math.Round(random.NextDouble() * (double)Math.Min(stock.Resqty, stock.Actqty), 2, MidpointRounding.AwayFromZero))
            };

            list.Add((stock, request.Qty));

            await this.CommitReservedAsync(context, request, ct);
        }

        await this.StoreAsync(context, ct);

        var groupped = list
            .GroupBy(i => new { i.stock.Itemid, i.stock.Whid, i.stock.Whzoneid })
            .Select(i => (i.First().stock, i.Sum(j => j.qty.GetValueOrDefault())));
        foreach (var (stock, qty) in groupped)
        {
            var entity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(i => i.Itemid == stock!.Itemid && i.Whid == stock.Whid && i.Whzoneid == stock.Whzoneid, ct);
            Assert.NotNull(entity);
            Assert.Equal((stock?.Actqty).GetValueOrDefault() - qty, entity!.Actqty);
            Assert.Equal((stock?.Resqty).GetValueOrDefault() - qty, entity!.Resqty);
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

                stock = await this.GetRandomStockAsync(whid!, true, true, ct);
                Assert.NotNull(stock);
                Assert.NotNull(stock!.Whzoneid);
                Assert.NotEqual(0M, stock.Resqty);
            } while (list.Any(l => OlcWhzstock.Comparer.Equals(l.stock, stock)));

            var request = new WhZStockDto
            {
                Itemid = stock.Itemid,
                Whid = stock.Whid,
                Whzoneid = stock.Whzoneid,
                Qty = Convert.ToDecimal(Math.Round(random.NextDouble() * (double)Math.Min(stock.Resqty, stock.Actqty), 2, MidpointRounding.AwayFromZero))
            };

            list.Add((stock, request.Qty));

            await this.CommitReservedAsync(context, request, ct);
        }

        await this.StoreAsync(context, ct);

        var groupped = list
            .GroupBy(i => new { i.stock.Itemid, i.stock.Whid, i.stock.Whzoneid })
            .Select(i => (i.First().stock, i.Sum(j => j.qty.GetValueOrDefault())));
        foreach (var (stock, qty) in groupped)
        {
            var entity = await this.dbContext.OlcWhzstocks.FirstOrDefaultAsync(i => i.Itemid == stock!.Itemid && i.Whid == stock.Whid && i.Whzoneid == stock.Whzoneid, ct);
            Assert.NotNull(entity);
            Assert.Equal((stock?.Actqty).GetValueOrDefault() - qty, entity!.Actqty);
            Assert.Equal((stock?.Resqty).GetValueOrDefault() - qty, entity!.Resqty);
        }
    }

    private async Task AddReceivingAndCommitWhStockTestAsync(int itemId, string whid, int? whzoneid, decimal? qty, CancellationToken cancellationToken = default)
    {
        using var context = this.service.CreateContext();

        var request = new WhZStockDto
        {
            Itemid = itemId,
            Whid = whid!,
            Whzoneid = whzoneid,
            Qty = qty
        };

        await this.AddReceivingAsync(context, request, cancellationToken);

        await this.CommitReceivingAsync(context, request, cancellationToken);

        await this.StoreAsync(context, cancellationToken);
    }
}
