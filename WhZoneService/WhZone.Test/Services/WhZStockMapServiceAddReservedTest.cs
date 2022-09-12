using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Enums;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services;
using eLog.HeavyTools.Services.WhZone.Test.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace eLog.HeavyTools.Services.WhZone.Test.Services;

public class WhZStockMapServiceAddReservedTest : Base.WhZStockMapServiceTestBase
{
    public WhZStockMapServiceAddReservedTest(ITestOutputHelper testOutputHelper, TestFixture fixture) : base(testOutputHelper, fixture)
    {
    }

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public async Task AddWhStockTestAsync(double? qty)
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TestFixture.TimeoutSeconds);
    //    var ct = cts.Token;

    //    var itemId = await this.GetFirstItemIdAsync(ct);
    //    Assert.NotNull(itemId);

    //    var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
    //    Assert.NotNull(whid);

    //    var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
    //    Assert.NotNull(whLocId);

    //    await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, null, whLocId.Value, (decimal?)qty, ct);

    //    using var context = this.service.CreateContext();

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whlocid = whLocId.Value,
    //        Qty = (decimal?)qty
    //    };

    //    await this.AddReservedAsync(context, request, ct);
    //}

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public async Task AddWhZStockTestAsync(double? qty)
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TestFixture.TimeoutSeconds);
    //    var ct = cts.Token;

    //    var itemId = await this.GetFirstItemIdAsync(ct);
    //    Assert.NotNull(itemId);

    //    var whid = await this.GetFirstWarehouseIdAsync(true, ct);
    //    Assert.NotNull(whid);

    //    var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
    //    Assert.NotNull(whZoneId);

    //    var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
    //    Assert.NotNull(whLocId);

    //    await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, whZoneId, whLocId.Value, (decimal?)qty, ct);

    //    using var context = this.service.CreateContext();

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whzoneid = whZoneId,
    //        Whlocid = whLocId.Value,
    //        Qty = (decimal?)qty
    //    };

    //    await this.AddReservedAsync(context, request, ct);
    //}

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public async Task AddAfterReceivingWhStockTestAsync(double? qty)
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TestFixture.TimeoutSeconds);
    //    var ct = cts.Token;

    //    var itemId = await this.GetFirstItemIdAsync(ct);
    //    Assert.NotNull(itemId);

    //    var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
    //    Assert.NotNull(whid);

    //    var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
    //    Assert.NotNull(whLocId);

    //    using var context = this.service.CreateContext();

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whlocid = whLocId.Value,
    //        Qty = (decimal?)qty * (new Random().Next(5) + 1)
    //    };

    //    await this.AddReceivingAsync(context, request, ct);

    //    request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whlocid = whLocId.Value,
    //        Qty = (decimal?)qty
    //    };

    //    await this.AddReservedAsync(context, request, ct);
    //}

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public async Task AddAfterReceivingWhZStockTestAsync(double? qty)
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TestFixture.TimeoutSeconds);
    //    var ct = cts.Token;

    //    var itemId = await this.GetFirstItemIdAsync(ct);
    //    Assert.NotNull(itemId);

    //    var whid = await this.GetFirstWarehouseIdAsync(true, ct);
    //    Assert.NotNull(whid);

    //    var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
    //    Assert.NotNull(whZoneId);

    //    var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
    //    Assert.NotNull(whLocId);

    //    using var context = this.service.CreateContext();

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whzoneid = whZoneId,
    //        Whlocid = whLocId.Value,
    //        Qty = (decimal?)qty * (new Random().Next(5) + 1)
    //    };

    //    await this.AddReceivingAsync(context, request, ct);

    //    request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whzoneid = whZoneId,
    //        Whlocid = whLocId.Value,
    //        Qty = (decimal?)qty
    //    };

    //    await this.AddReservedAsync(context, request, ct);
    //}

    //private async Task AddReceivingAndCommitWhStockTestAsync(int itemId, string whid, int? whzoneid, int whlocid, decimal? qty, CancellationToken cancellationToken = default)
    //{
    //    using var context = this.service.CreateContext();

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId,
    //        Whid = whid!,
    //        Whzoneid = whzoneid,
    //        Whlocid = whlocid,
    //        Qty = qty
    //    };

    //    await this.AddReceivingAsync(context, request, cancellationToken);

    //    await this.CommitReceivingAsync(context, request, cancellationToken);

    //    await this.StoreAsync(context, cancellationToken);
    //}

    //[Fact]
    //public async Task AddWhStockNullFailedTestAsync()
    //{
    //    var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.AddWhStockTestAsync(null));
    //    Assert.Equal(WhZStockExceptionType.InvalidRequestQty, ex.Type);
    //    Assert.Equal("The add qty is not set", ex.Message);
    //}

    //[Fact]
    //public async Task AddWhZStockNullFailedTestAsync()
    //{
    //    var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.AddWhZStockTestAsync(null));
    //    Assert.Equal(WhZStockExceptionType.InvalidRequestQty, ex.Type);
    //    Assert.Equal("The add qty is not set", ex.Message);
    //}

    //[Theory]
    //[InlineData(-1.0)]
    //[InlineData(-15.0)]
    //[InlineData(-0.25)]
    //[InlineData(-5.25)]
    //public async Task AddWhStockFailedTestAsync(double? qty)
    //{
    //    var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.AddWhStockTestAsync(qty));
    //    Assert.Equal(WhZStockExceptionType.InvalidRequestQty, ex.Type);
    //    Assert.Equal("The add qty cannot be less or equal to 0", ex.Message);
    //}

    //[Theory]
    //[InlineData(-1.0)]
    //[InlineData(-15.0)]
    //[InlineData(-0.25)]
    //[InlineData(-5.25)]
    //public async Task AddWhZStockFailedTestAsync(double? qty)
    //{
    //    var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.AddWhZStockTestAsync(qty));
    //    Assert.Equal(WhZStockExceptionType.InvalidRequestQty, ex.Type);
    //    Assert.Equal("The add qty cannot be less or equal to 0", ex.Message);
    //}

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public async Task AddAndStoreWhStockTestAsync(double? qty)
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TestFixture.TimeoutSeconds);
    //    var ct = cts.Token;

    //    var itemId = await this.GetFirstItemIdAsync(ct);
    //    Assert.NotNull(itemId);

    //    var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
    //    Assert.NotNull(whid);

    //    var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
    //    Assert.NotNull(whLocId);

    //    await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, null, whLocId.Value, (decimal?)qty, ct);

    //    var originalEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);

    //    using var context = this.service.CreateContext();

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whlocid = whLocId.Value,
    //        Qty = (decimal?)qty
    //    };

    //    await this.AddReservedAsync(context, request, ct);

    //    await this.StoreAsync(context, ct);

    //    var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
    //    Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
    //    Assert.Equal((originalEntity?.Resqty).GetValueOrDefault() + request.Qty, currentEntity!.Resqty);
    //}

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public async Task AddAndStoreWhZStockTestAsync(double? qty)
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TestFixture.TimeoutSeconds);
    //    var ct = cts.Token;

    //    var itemId = await this.GetFirstItemIdAsync(ct);
    //    Assert.NotNull(itemId);

    //    var whid = await this.GetFirstWarehouseIdAsync(true, ct);
    //    Assert.NotNull(whid);

    //    var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
    //    Assert.NotNull(whZoneId);

    //    var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
    //    Assert.NotNull(whLocId);

    //    await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, whZoneId, whLocId.Value, (decimal?)qty, ct);

    //    var originalEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whlocid == whLocId, ct);

    //    using var context = this.service.CreateContext();

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whzoneid = whZoneId,
    //        Whlocid = whLocId.Value,
    //        Qty = (decimal?)qty
    //    };

    //    await this.AddReservedAsync(context, request, ct);

    //    await this.StoreAsync(context, ct);

    //    var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whlocid == whLocId, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
    //    Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
    //    Assert.Equal((originalEntity?.Resqty).GetValueOrDefault() + request.Qty, currentEntity!.Resqty);
    //}

    //[Fact]
    //public async Task DeleteWhStock01TestAsync()
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TestFixture.TimeoutSeconds);
    //    var ct = cts.Token;

    //    var itemId = await this.GetFirstItemIdAsync(ct);
    //    Assert.NotNull(itemId);

    //    var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
    //    Assert.NotNull(whid);

    //    var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
    //    Assert.NotNull(whLocId);

    //    using var context = this.service.CreateContext();

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whlocid = whLocId.Value,
    //        Qty = 10
    //    };

    //    await this.AddReceivingAsync(context, request, ct);
    //    var data = await this.AddReservedAsync(context, request, ct);
    //    Assert.Contains(context.MovementList, l => l == data);
    //    this.Delete(context, data!);
    //    Assert.DoesNotContain(context.MovementList, l => l == data);
    //}

    //[Fact]
    //public async Task DeleteWhStock02TestAsync()
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TestFixture.TimeoutSeconds);
    //    var ct = cts.Token;

    //    var itemId = await this.GetFirstItemIdAsync(ct);
    //    Assert.NotNull(itemId);

    //    var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
    //    Assert.NotNull(whid);

    //    var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
    //    Assert.NotNull(whLocId);

    //    using var context = this.service.CreateContext();

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whlocid = whLocId.Value,
    //        Qty = 10
    //    };

    //    await this.AddReceivingAsync(context, request, ct);
    //    await this.AddReservedAsync(context, request, ct);
    //    var data = await this.CommitReservedAsync(context, request, ct);
    //    Assert.Contains(context.MovementList, l => l == data);
    //    this.Delete(context, data!);
    //    Assert.DoesNotContain(context.MovementList, l => l == data);
    //}

    //[Fact]
    //public async Task DeleteWhZStock01TestAsync()
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TestFixture.TimeoutSeconds);
    //    var ct = cts.Token;

    //    var itemId = await this.GetFirstItemIdAsync(ct);
    //    Assert.NotNull(itemId);

    //    var whid = await this.GetFirstWarehouseIdAsync(true, ct);
    //    Assert.NotNull(whid);

    //    var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
    //    Assert.NotNull(whZoneId);

    //    var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
    //    Assert.NotNull(whLocId);

    //    using var context = this.service.CreateContext();

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whzoneid = whZoneId,
    //        Whlocid = whLocId.Value,
    //        Qty = 10
    //    };

    //    await this.AddReceivingAsync(context, request, ct);
    //    var data = await this.AddReservedAsync(context, request, ct);
    //    Assert.Contains(context.MovementList, l => l == data);
    //    this.Delete(context, data!);
    //    Assert.DoesNotContain(context.MovementList, l => l == data);
    //}

    //[Fact]
    //public async Task DeleteWhZStock02TestAsync()
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TestFixture.TimeoutSeconds);
    //    var ct = cts.Token;

    //    var itemId = await this.GetFirstItemIdAsync(ct);
    //    Assert.NotNull(itemId);

    //    var whid = await this.GetFirstWarehouseIdAsync(true, ct);
    //    Assert.NotNull(whid);

    //    var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
    //    Assert.NotNull(whZoneId);

    //    var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
    //    Assert.NotNull(whLocId);

    //    using var context = this.service.CreateContext();

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whzoneid = whZoneId,
    //        Whlocid = whLocId.Value,
    //        Qty = 10
    //    };

    //    await this.AddReceivingAsync(context, request, ct);
    //    await this.AddReservedAsync(context, request, ct);
    //    var data = await this.CommitReservedAsync(context, request, ct);
    //    Assert.Contains(context.MovementList, l => l == data);
    //    this.Delete(context, data!);
    //    Assert.DoesNotContain(context.MovementList, l => l == data);
    //}

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public async Task AddAndRemoveAndStoreWhStockTest01Async(double? qty)
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TestFixture.TimeoutSeconds);
    //    var ct = cts.Token;

    //    var itemId = await this.GetFirstItemIdAsync(ct);
    //    Assert.NotNull(itemId);

    //    var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
    //    Assert.NotNull(whid);

    //    var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
    //    Assert.NotNull(whLocId);

    //    await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, null, whLocId.Value, (decimal?)qty, ct);

    //    var originalEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId.Value, ct);

    //    using var context = this.service.CreateContext();

    //    var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId.Value, ct);
    //    Assert.NotNull(currentEntity);

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whlocid = whLocId.Value,
    //        Qty = (decimal?)qty
    //    };

    //    await this.AddReservedAsync(context, request, ct);

    //    var removeRequest = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whlocid = whLocId.Value,
    //        Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)(currentEntity!.Resqty + request.Qty.GetValueOrDefault()), Precision, MidpointRounding.AwayFromZero))
    //    };

    //    await this.RemoveReservedAsync(context, removeRequest, ct);

    //    await this.StoreAsync(context, ct);

    //    currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId.Value, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
    //    Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
    //    Assert.Equal((originalEntity?.Resqty).GetValueOrDefault() + request.Qty - removeRequest.Qty, currentEntity!.Resqty);
    //}

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public async Task AddAndRemoveAndStoreWhZStockTest01Async(double? qty)
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TestFixture.TimeoutSeconds);
    //    var ct = cts.Token;

    //    var itemId = await this.GetFirstItemIdAsync(ct);
    //    Assert.NotNull(itemId);

    //    var whid = await this.GetFirstWarehouseIdAsync(true, ct);
    //    Assert.NotNull(whid);

    //    var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
    //    Assert.NotNull(whZoneId);

    //    var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
    //    Assert.NotNull(whLocId);

    //    await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, whZoneId, whLocId.Value, (decimal?)qty, ct);

    //    var originalEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whzoneid == whLocId.Value, ct);

    //    using var context = this.service.CreateContext();

    //    var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whzoneid == whLocId.Value, ct);
    //    Assert.NotNull(currentEntity);

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whzoneid = whZoneId,
    //        Whlocid = whLocId.Value,
    //        Qty = (decimal?)qty
    //    };

    //    await this.AddReservedAsync(context, request, ct);

    //    var removeRequest = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whzoneid = whZoneId,
    //        Whlocid = whLocId.Value,
    //        Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)(currentEntity!.Resqty + request.Qty.GetValueOrDefault()), Precision, MidpointRounding.AwayFromZero))
    //    };

    //    await this.RemoveReservedAsync(context, removeRequest, ct);

    //    await this.StoreAsync(context, ct);

    //    currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whzoneid == whLocId.Value, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
    //    Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
    //    Assert.Equal((originalEntity?.Resqty).GetValueOrDefault() + request.Qty - removeRequest.Qty, currentEntity!.Resqty);
    //}

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public async Task AddAndRemoveAndStoreWhStockTest02Async(double? qty)
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TestFixture.TimeoutSeconds);
    //    var ct = cts.Token;

    //    var itemId = await this.GetFirstItemIdAsync(ct);
    //    Assert.NotNull(itemId);

    //    var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
    //    Assert.NotNull(whid);

    //    var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
    //    Assert.NotNull(whLocId);

    //    await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, null, whLocId.Value, (decimal?)qty, ct);

    //    var originalEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);

    //    using var context = this.service.CreateContext();

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whlocid = whLocId.Value,
    //        Qty = (decimal?)qty
    //    };

    //    await this.AddReservedAsync(context, request, ct);

    //    await this.StoreAsync(context, ct);

    //    var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.NotEqual(0M, currentEntity!.Resqty);

    //    var removeRequest = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whlocid = whLocId.Value,
    //        Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)currentEntity.Resqty, Precision, MidpointRounding.AwayFromZero))
    //    };

    //    await this.RemoveReservedAsync(context, removeRequest, ct);

    //    await this.StoreAsync(context, ct);

    //    currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
    //    Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
    //    Assert.Equal((originalEntity?.Resqty).GetValueOrDefault() + request.Qty - removeRequest.Qty, currentEntity!.Resqty);
    //}

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public async Task AddAndRemoveAndStoreWhZStockTest02Async(double? qty)
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TestFixture.TimeoutSeconds);
    //    var ct = cts.Token;

    //    var itemId = await this.GetFirstItemIdAsync(ct);
    //    Assert.NotNull(itemId);

    //    var whid = await this.GetFirstWarehouseIdAsync(true, ct);
    //    Assert.NotNull(whid);

    //    var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
    //    Assert.NotNull(whZoneId);

    //    var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
    //    Assert.NotNull(whLocId);

    //    await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, whZoneId, whLocId.Value, (decimal?)qty, ct);

    //    var originalEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whzoneid == whLocId.Value, ct);

    //    using var context = this.service.CreateContext();

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whzoneid = whZoneId,
    //        Whlocid = whLocId.Value,
    //        Qty = (decimal?)qty
    //    };

    //    await this.AddReservedAsync(context, request, ct);

    //    await this.StoreAsync(context, ct);

    //    var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whzoneid == whLocId.Value, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.NotEqual(0M, currentEntity!.Resqty);

    //    var removeRequest = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whzoneid = whZoneId,
    //        Whlocid = whLocId.Value,
    //        Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)currentEntity.Resqty, Precision, MidpointRounding.AwayFromZero))
    //    };

    //    await this.RemoveReservedAsync(context, removeRequest, ct);

    //    await this.StoreAsync(context, ct);

    //    currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whzoneid == whLocId.Value, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
    //    Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
    //    Assert.Equal((originalEntity?.Resqty).GetValueOrDefault() + request.Qty - removeRequest.Qty, currentEntity!.Resqty);
    //}

    //[Fact]
    //public async Task AddAndRemoveAndStoreWhStockTo0TestAsync()
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TestFixture.TimeoutSeconds);
    //    var ct = cts.Token;

    //    var itemId = await this.GetFirstItemIdAsync(ct);
    //    Assert.NotNull(itemId);

    //    var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
    //    Assert.NotNull(whid);

    //    var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
    //    Assert.NotNull(whLocId);

    //    await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, null, whLocId.Value, 15.75M, ct);

    //    var originalEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);

    //    using var context = this.service.CreateContext();

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whlocid = whLocId.Value,
    //        Qty = 15.75M
    //    };

    //    await this.AddReservedAsync(context, request, ct);

    //    await this.StoreAsync(context, ct);

    //    var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.NotEqual(0M, currentEntity!.Resqty);

    //    var removeRequest = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whlocid = whLocId.Value,
    //        Qty = currentEntity.Resqty
    //    };

    //    await this.RemoveReservedAsync(context, removeRequest, ct);

    //    await this.StoreAsync(context, ct);

    //    currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
    //    Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
    //    Assert.Equal(0M, currentEntity!.Resqty);
    //}

    //[Fact]
    //public async Task AddAndRemoveAndStoreWhZStockTo0TestAsync()
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TestFixture.TimeoutSeconds);
    //    var ct = cts.Token;

    //    var itemId = await this.GetFirstItemIdAsync(ct);
    //    Assert.NotNull(itemId);

    //    var whid = await this.GetFirstWarehouseIdAsync(true, ct);
    //    Assert.NotNull(whid);

    //    var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
    //    Assert.NotNull(whZoneId);

    //    var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
    //    Assert.NotNull(whLocId);

    //    await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, whZoneId, whLocId.Value, 15.75M, ct);

    //    var originalEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whzoneid == whLocId.Value, ct);

    //    using var context = this.service.CreateContext();

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whzoneid = whZoneId,
    //        Whlocid = whLocId.Value,
    //        Qty = 15.75M
    //    };

    //    await this.AddReservedAsync(context, request, ct);

    //    await this.StoreAsync(context, ct);

    //    var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whzoneid == whLocId.Value, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.NotEqual(0M, currentEntity!.Resqty);

    //    var removeRequest = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whzoneid = whZoneId,
    //        Whlocid = whLocId.Value,
    //        Qty = currentEntity.Resqty
    //    };

    //    await this.RemoveReservedAsync(context, removeRequest, ct);

    //    await this.StoreAsync(context, ct);

    //    currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whzoneid == whLocId.Value, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
    //    Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
    //    Assert.Equal(0M, currentEntity!.Resqty);
    //}

    //[Fact]
    //public async Task AddAndRemoveAndStoreWhStockFailedTestAsync()
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TestFixture.TimeoutSeconds);
    //    var ct = cts.Token;

    //    var itemId = await this.GetFirstItemIdAsync(ct);
    //    Assert.NotNull(itemId);

    //    var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
    //    Assert.NotNull(whid);

    //    var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
    //    Assert.NotNull(whLocId);

    //    await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, null, whLocId.Value, 15.75M, ct);

    //    using var context = this.service.CreateContext();

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whlocid = whLocId.Value,
    //        Qty = 15.75M
    //    };

    //    await this.AddReservedAsync(context, request, ct);

    //    await this.StoreAsync(context, ct);

    //    var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.NotEqual(0M, currentEntity!.Resqty);

    //    var removeRequest = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whlocid = whLocId.Value,
    //        Qty = currentEntity.Resqty * 1.75M
    //    };

    //    var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.RemoveReservedAsync(context, removeRequest, ct));
    //    Assert.Equal(WhZStockExceptionType.RemoveReservedQty, ex.Type);
    //    Assert.Equal("Not enough reserved quantity to fulfill the remove request", ex.Message);
    //}

    //[Fact]
    //public async Task AddAndRemoveAndStoreWhZStockFailedTestAsync()
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TestFixture.TimeoutSeconds);
    //    var ct = cts.Token;

    //    var itemId = await this.GetFirstItemIdAsync(ct);
    //    Assert.NotNull(itemId);

    //    var whid = await this.GetFirstWarehouseIdAsync(true, ct);
    //    Assert.NotNull(whid);

    //    var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
    //    Assert.NotNull(whZoneId);

    //    var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
    //    Assert.NotNull(whLocId);

    //    await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, whZoneId, whLocId.Value, 15.75M, ct);

    //    using var context = this.service.CreateContext();

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whzoneid = whZoneId,
    //        Whlocid = whLocId.Value,
    //        Qty = 15.75M
    //    };

    //    await this.AddReservedAsync(context, request, ct);

    //    await this.StoreAsync(context, ct);

    //    var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId && s.Whzoneid == whLocId.Value, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.NotEqual(0M, currentEntity!.Resqty);

    //    var removeRequest = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whzoneid = whZoneId,
    //        Whlocid = whLocId.Value,
    //        Qty = currentEntity.Resqty * 1.75M
    //    };

    //    var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.RemoveReservedAsync(context, removeRequest, ct));
    //    Assert.Equal(WhZStockExceptionType.RemoveReservedQty, ex.Type);
    //    Assert.Equal("Not enough reserved quantity to fulfill the remove request", ex.Message);
    //}

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public async Task AddAndCommitAndStoreWhStockTest01Async(double? qty)
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TestFixture.TimeoutSeconds);
    //    var ct = cts.Token;

    //    var itemId = await this.GetFirstItemIdAsync(ct);
    //    Assert.NotNull(itemId);

    //    var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
    //    Assert.NotNull(whid);

    //    var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
    //    Assert.NotNull(whLocId);

    //    await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, null, whLocId.Value, (decimal?)qty, ct);

    //    var originalEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);

    //    using var context = this.service.CreateContext();

    //    var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.NotEqual(0M, currentEntity!.Actqty);

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whlocid = whLocId.Value,
    //        Qty = (decimal?)qty
    //    };

    //    await this.AddReservedAsync(context, request, ct);

    //    var commitRequest = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whlocid = whLocId.Value,
    //        Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)Math.Min(currentEntity!.Resqty + request.Qty.GetValueOrDefault(), currentEntity.Actqty), Precision, MidpointRounding.AwayFromZero))
    //    };

    //    await this.CommitReservedAsync(context, commitRequest, ct);

    //    await this.StoreAsync(context, ct);

    //    currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
    //    Assert.Equal((originalEntity?.Actqty).GetValueOrDefault() - commitRequest.Qty, currentEntity!.Actqty);
    //    Assert.Equal((originalEntity?.Resqty).GetValueOrDefault() + request.Qty - commitRequest.Qty, currentEntity!.Resqty);
    //}

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public async Task AddAndCommitAndStoreWhZStockTest01Async(double? qty)
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TestFixture.TimeoutSeconds);
    //    var ct = cts.Token;

    //    var itemId = await this.GetFirstItemIdAsync(ct);
    //    Assert.NotNull(itemId);

    //    var whid = await this.GetFirstWarehouseIdAsync(true, ct);
    //    Assert.NotNull(whid);

    //    var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
    //    Assert.NotNull(whZoneId);

    //    var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
    //    Assert.NotNull(whLocId);

    //    await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, whZoneId, whLocId.Value, (decimal?)qty, ct);

    //    var originalEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);

    //    using var context = this.service.CreateContext();

    //    var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.NotEqual(0M, currentEntity!.Actqty);

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whzoneid = whZoneId,
    //        Whlocid = whLocId.Value,
    //        Qty = (decimal?)qty
    //    };

    //    await this.AddReservedAsync(context, request, ct);

    //    var commitRequest = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whzoneid = whZoneId,
    //        Whlocid = whLocId.Value,
    //        Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)Math.Min(currentEntity!.Resqty + request.Qty.GetValueOrDefault(), currentEntity.Actqty), Precision, MidpointRounding.AwayFromZero))
    //    };

    //    await this.CommitReservedAsync(context, commitRequest, ct);

    //    await this.StoreAsync(context, ct);

    //    currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
    //    Assert.Equal((originalEntity?.Actqty).GetValueOrDefault() - commitRequest.Qty, currentEntity!.Actqty);
    //    Assert.Equal((originalEntity?.Resqty).GetValueOrDefault() + request.Qty - commitRequest.Qty, currentEntity!.Resqty);
    //}

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public async Task AddAndCommitAndStoreWhStockTest02Async(double? qty)
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TestFixture.TimeoutSeconds);
    //    var ct = cts.Token;

    //    var itemId = await this.GetFirstItemIdAsync(ct);
    //    Assert.NotNull(itemId);

    //    var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
    //    Assert.NotNull(whid);

    //    var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
    //    Assert.NotNull(whLocId);

    //    await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, null, whLocId.Value, (decimal?)qty, ct);

    //    var originalEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);

    //    using var context = this.service.CreateContext();

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whlocid = whLocId.Value,
    //        Qty = (decimal?)qty
    //    };

    //    await this.AddReservedAsync(context, request, ct);

    //    await this.StoreAsync(context, ct);

    //    var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.NotEqual(0M, currentEntity!.Resqty);
    //    Assert.NotEqual(0M, currentEntity!.Actqty);

    //    var commitRequest = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whlocid = whLocId.Value,
    //        Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)Math.Min(currentEntity.Resqty, currentEntity.Actqty), Precision, MidpointRounding.AwayFromZero))
    //    };

    //    await this.CommitReservedAsync(context, commitRequest, ct);

    //    await this.StoreAsync(context, ct);

    //    currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
    //    Assert.Equal((originalEntity?.Actqty).GetValueOrDefault() - commitRequest.Qty, currentEntity!.Actqty);
    //    Assert.Equal((originalEntity?.Resqty).GetValueOrDefault() + request.Qty - commitRequest.Qty, currentEntity!.Resqty);
    //}

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public async Task AddAndCommitAndStoreWhZStockTest02Async(double? qty)
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TestFixture.TimeoutSeconds);
    //    var ct = cts.Token;

    //    var itemId = await this.GetFirstItemIdAsync(ct);
    //    Assert.NotNull(itemId);

    //    var whid = await this.GetFirstWarehouseIdAsync(true, ct);
    //    Assert.NotNull(whid);

    //    var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
    //    Assert.NotNull(whZoneId);

    //    var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
    //    Assert.NotNull(whLocId);

    //    await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, whZoneId, whLocId.Value, (decimal?)qty, ct);

    //    var originalEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);

    //    using var context = this.service.CreateContext();

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whzoneid = whZoneId,
    //        Whlocid = whLocId.Value,
    //        Qty = (decimal?)qty
    //    };

    //    await this.AddReservedAsync(context, request, ct);

    //    await this.StoreAsync(context, ct);

    //    var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.NotEqual(0M, currentEntity!.Resqty);
    //    Assert.NotEqual(0M, currentEntity!.Actqty);

    //    var commitRequest = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whzoneid = whZoneId,
    //        Whlocid = whLocId.Value,
    //        Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)Math.Min(currentEntity.Resqty, currentEntity.Actqty), Precision, MidpointRounding.AwayFromZero))
    //    };

    //    await this.CommitReservedAsync(context, commitRequest, ct);

    //    await this.StoreAsync(context, ct);

    //    currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
    //    Assert.Equal((originalEntity?.Actqty).GetValueOrDefault() - commitRequest.Qty, currentEntity!.Actqty);
    //    Assert.Equal((originalEntity?.Resqty).GetValueOrDefault() + request.Qty - commitRequest.Qty, currentEntity!.Resqty);
    //}

    //[Fact]
    //public async Task AddAndCommitAndStoreWhStockTo0TestAsync()
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TestFixture.TimeoutSeconds);
    //    var ct = cts.Token;

    //    var itemId = await this.GetFirstItemIdAsync(ct);
    //    Assert.NotNull(itemId);

    //    var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
    //    Assert.NotNull(whid);

    //    var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
    //    Assert.NotNull(whLocId);

    //    await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, null, whLocId.Value, 15.75M, ct);

    //    var originalEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);

    //    using var context = this.service.CreateContext();

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whlocid = whLocId.Value,
    //        Qty = 15.75M
    //    };

    //    await this.AddReservedAsync(context, request, ct);

    //    await this.StoreAsync(context, ct);

    //    var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.NotEqual(0M, currentEntity!.Resqty);

    //    var commitRequest = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whlocid = whLocId.Value,
    //        Qty = currentEntity.Resqty
    //    };

    //    await this.CommitReservedAsync(context, commitRequest, ct);

    //    await this.StoreAsync(context, ct);

    //    currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
    //    Assert.Equal((originalEntity?.Actqty).GetValueOrDefault() - commitRequest.Qty, currentEntity!.Actqty);
    //    Assert.Equal(0M, currentEntity!.Resqty);
    //}

    //[Fact]
    //public async Task AddAndCommitAndStoreWhZStockTo0TestAsync()
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TestFixture.TimeoutSeconds);
    //    var ct = cts.Token;

    //    var itemId = await this.GetFirstItemIdAsync(ct);
    //    Assert.NotNull(itemId);

    //    var whid = await this.GetFirstWarehouseIdAsync(true, ct);
    //    Assert.NotNull(whid);

    //    var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
    //    Assert.NotNull(whZoneId);

    //    var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
    //    Assert.NotNull(whLocId);

    //    await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, whZoneId, whLocId.Value, 15.75M, ct);

    //    var originalEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);

    //    using var context = this.service.CreateContext();

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whzoneid = whZoneId,
    //        Whlocid = whLocId.Value,
    //        Qty = 15.75M
    //    };

    //    await this.AddReservedAsync(context, request, ct);

    //    await this.StoreAsync(context, ct);

    //    var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.NotEqual(0M, currentEntity!.Resqty);

    //    var commitRequest = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whzoneid = whZoneId,
    //        Whlocid = whLocId.Value,
    //        Qty = currentEntity.Resqty
    //    };

    //    await this.CommitReservedAsync(context, commitRequest, ct);

    //    await this.StoreAsync(context, ct);

    //    currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
    //    Assert.Equal((originalEntity?.Actqty).GetValueOrDefault() - commitRequest.Qty, currentEntity!.Actqty);
    //    Assert.Equal(0M, currentEntity!.Resqty);
    //}

    //[Fact]
    //public async Task AddAndCommitAndStoreWhStockFailedTestAsync()
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TestFixture.TimeoutSeconds);
    //    var ct = cts.Token;

    //    var itemId = await this.GetFirstItemIdAsync(ct);
    //    Assert.NotNull(itemId);

    //    var whid = await this.GetFirstWarehouseIdAsync(cancellationToken: ct);
    //    Assert.NotNull(whid);

    //    var whLocId = await this.GetFirstWhLocIdAsync(whid!, null, ct);
    //    Assert.NotNull(whLocId);

    //    await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, null, whLocId.Value, 15.75M, ct);

    //    using var context = this.service.CreateContext();

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whlocid = whLocId.Value,
    //        Qty = 15.75M
    //    };

    //    await this.AddReservedAsync(context, request, ct);

    //    await this.StoreAsync(context, ct);

    //    var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == null && s.Whlocid == whLocId, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.NotEqual(0M, currentEntity!.Resqty);

    //    var commitRequest = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whlocid = whLocId.Value,
    //        Qty = currentEntity.Resqty * 1.75M
    //    };

    //    var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.CommitReservedAsync(context, commitRequest, ct));
    //    Assert.Equal(WhZStockExceptionType.CommitReservedQty, ex.Type);
    //    Assert.Equal("Not enough reserved quantity to fulfill the commit request", ex.Message);
    //}

    //[Fact]
    //public async Task AddAndCommitAndStoreWhZStockFailedTestAsync()
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TestFixture.TimeoutSeconds);
    //    var ct = cts.Token;

    //    var itemId = await this.GetFirstItemIdAsync(ct);
    //    Assert.NotNull(itemId);

    //    var whid = await this.GetFirstWarehouseIdAsync(true, ct);
    //    Assert.NotNull(whid);

    //    var whZoneId = await this.GetFirstWhZoneIdAsync(whid!, true, ct);
    //    Assert.NotNull(whZoneId);

    //    var whLocId = await this.GetFirstWhLocIdAsync(whid!, whZoneId, ct);
    //    Assert.NotNull(whLocId);

    //    await this.AddReceivingAndCommitWhStockTestAsync(itemId.Value, whid!, whZoneId, whLocId.Value, 15.75M, ct);

    //    using var context = this.service.CreateContext();

    //    var request = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whzoneid = whZoneId,
    //        Whlocid = whLocId.Value,
    //        Qty = 15.75M
    //    };

    //    await this.AddReservedAsync(context, request, ct);

    //    await this.StoreAsync(context, ct);

    //    var currentEntity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(s => s.Itemid == itemId && s.Whid == whid && s.Whzoneid == whZoneId, ct);
    //    Assert.NotNull(currentEntity);
    //    Assert.NotEqual(0M, currentEntity!.Resqty);

    //    var commitRequest = new WhZStockMapDto
    //    {
    //        Itemid = itemId.Value,
    //        Whid = whid!,
    //        Whzoneid = whZoneId,
    //        Whlocid = whLocId.Value,
    //        Qty = currentEntity.Resqty * 1.75M
    //    };

    //    var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.CommitReservedAsync(context, commitRequest, ct));
    //    Assert.Equal(WhZStockExceptionType.CommitReservedQty, ex.Type);
    //    Assert.Equal("Not enough reserved quantity to fulfill the commit request", ex.Message);
    //}

    //// AddMultipleItemsWhStock01TestAsync
}
