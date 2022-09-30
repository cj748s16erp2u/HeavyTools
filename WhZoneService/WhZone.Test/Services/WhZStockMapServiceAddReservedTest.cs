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

    private async Task AddReceivingTest01Async(string whId, int? whZoneId, int? whLocId, decimal? qty, BusinessLogic.Containers.Interfaces.IWhZStockMapContext context, CancellationToken cancellationToken = default)
    {
        var request = new WhZStockMapDto
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Qty = qty
        };

        using var tran = await this.unitOfWork.BeginTransactionAsync();
        try
        {
            await this.AddReceivingAsync(context, request, cancellationToken);

            await this.StoreAsync(context, cancellationToken);
        }
        finally
        {
            tran.Rollback();
        }
    }

    private async Task AddAndCommitReceivingTest01Async(string whId, int? whZoneId, int? whLocId, decimal? qty, BusinessLogic.Containers.Interfaces.IWhZStockMapContext? context = null!, CancellationToken cancellationToken = default)
    {
        var request = new WhZStockMapDto
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Qty = qty
        };

        using var tran = await this.unitOfWork.BeginTransactionAsync();
        try
        {
            if (context is null)
            {
                context = this.service.CreateContext();
            }

            await this.AddReceivingAsync(context, request, cancellationToken);

            await this.CommitReceivingAsync(context, request, cancellationToken);

            await this.StoreAsync(context, cancellationToken);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddReserve01TestAsync(double qty)
    {
        return this.AddReserveTest01Async(this.whIdNoZoneNoLoc, null, null, (decimal)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddReserve02TestAsync(double qty)
    {
        return this.AddReserveTest01Async(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddReserve03TestAsync(double qty)
    {
        return this.AddReserveTest01Async(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddReserve04TestAsync(double qty)
    {
        return this.AddReserveTest01Async(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal)qty);
    }

    private async Task AddReserveTest01Async(string whId, int? whZoneId, int? whLocId, decimal? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var tran = await this.unitOfWork.BeginTransactionAsync();
        try
        {
            using var context = this.service.CreateContext();

            await this.AddAndCommitReceivingTest01Async(whId, whZoneId, whLocId, qty, context, ct);

            var request = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = qty
            };

            await this.AddReservedAsync(context, request, ct);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAfterReceivingWhStock01TestAsync(double? qty)
    {
        return this.AddAfterReceivingTest01Async(this.whIdNoZoneNoLoc, null, null, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAfterReceivingWhStock02TestAsync(double? qty)
    {
        return this.AddAfterReceivingTest01Async(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAfterReceivingWhStock03TestAsync(double? qty)
    {
        return this.AddAfterReceivingTest01Async(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAfterReceivingWhStock04TestAsync(double? qty)
    {
        return this.AddAfterReceivingTest01Async(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal?)qty);
    }

    private async Task AddAfterReceivingTest01Async(string whId, int? whZoneId, int? whLocId, decimal? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var tran = await this.unitOfWork.BeginTransactionAsync();
        try
        {
            using var context = this.service.CreateContext();

            await this.AddReceivingTest01Async(whId, whZoneId, whLocId, qty, context, ct);

            var request = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = qty
            };

            await this.AddReservedAsync(context, request, ct);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Fact]
    public Task AddWhStockNullFailed01TestAsync()
    {
        return this.AddWhStockNullFailedTest01Async(this.whIdNoZoneNoLoc, null, null);
    }

    [Fact]
    public Task AddWhStockNullFailed02TestAsync()
    {
        return this.AddWhStockNullFailedTest01Async(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null);
    }

    [Fact]
    public Task AddWhStockNullFailed03TestAsync()
    {
        return this.AddWhStockNullFailedTest01Async(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone);
    }

    [Fact]
    public Task AddWhStockNullFailed04TestAsync()
    {
        return this.AddWhStockNullFailedTest01Async(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId);
    }

    private async Task AddWhStockNullFailedTest01Async(string whId, int? whZoneId, int? whLocId)
    {
        var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.AddReserveTest01Async(whId, whZoneId, whLocId, null));
        Assert.Equal(WhZStockExceptionType.InvalidRequestQty, ex.Type);
        Assert.Equal("The add qty is not set", ex.Message);
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-15.0)]
    [InlineData(-0.25)]
    [InlineData(-5.25)]
    public Task AddWhStockFailed01TestAsync(double? qty)
    {
        return this.AddWhStockFailedTest01Async(this.whIdNoZoneNoLoc, null, null, (decimal?)qty);
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-15.0)]
    [InlineData(-0.25)]
    [InlineData(-5.25)]
    public Task AddWhStockFailed02TestAsync(double? qty)
    {
        return this.AddWhStockFailedTest01Async(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal?)qty);
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-15.0)]
    [InlineData(-0.25)]
    [InlineData(-5.25)]
    public Task AddWhStockFailed03TestAsync(double? qty)
    {
        return this.AddWhStockFailedTest01Async(this.whIdNoZoneWithLoc, null, this.whLocId, (decimal?)qty);
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-15.0)]
    [InlineData(-0.25)]
    [InlineData(-5.25)]
    public Task AddWhStockFailed04TestAsync(double? qty)
    {
        return this.AddWhStockFailedTest01Async(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal?)qty);
    }

    private async Task AddWhStockFailedTest01Async(string whId, int? whZoneId, int? whLocId, decimal? qty)
    {
        var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.AddReserveTest01Async(whId, whZoneId, whLocId, qty));
        Assert.Equal(WhZStockExceptionType.InvalidRequestQty, ex.Type);
        Assert.Equal("The add qty cannot be less or equal to 0", ex.Message);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndStoreWhStock01TestAsync(double? qty)
    {
        return this.AddAndStoreWhStockTest01Async(this.whIdNoZoneNoLoc, null, null, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndStoreWhStock02TestAsync(double? qty)
    {
        return this.AddAndStoreWhStockTest01Async(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndStoreWhStock03TestAsync(double? qty)
    {
        return this.AddAndStoreWhStockTest01Async(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndStoreWhStock04TestAsync(double? qty)
    {
        return this.AddAndStoreWhStockTest01Async(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal?)qty);
    }

    private async Task AddAndStoreWhStockTest01Async(string whId, int? whZoneId, int? whLocId, decimal? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var tran = await this.unitOfWork.BeginTransactionAsync(ct);
        try
        {
            await this.AddAndCommitReceivingTest01Async(whId, whZoneId, whLocId, qty, cancellationToken: ct);

            var originalEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);

            using var context = this.service.CreateContext();

            var request = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = qty
            };

            await this.AddReservedAsync(context, request, ct);

            await this.StoreAsync(context, ct);

            var currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
            Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
            Assert.Equal((originalEntity?.Resqty).GetValueOrDefault() + request.Qty, currentEntity!.Resqty);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Fact]
    public Task DeleteWhStock01TestAsync()
    {
        return this.DeleteWhStockTest01Async(this.whIdNoZoneNoLoc, null, null);
    }

    [Fact]
    public Task DeleteWhStock02TestAsync()
    {
        return this.DeleteWhStockTest01Async(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null);
    }

    [Fact]
    public Task DeleteWhStock03TestAsync()
    {
        return this.DeleteWhStockTest01Async(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone);
    }

    [Fact]
    public Task DeleteWhStock04TestAsync()
    {
        return this.DeleteWhStockTest01Async(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId);
    }

    private async Task DeleteWhStockTest01Async(string whId, int? whZoneId, int? whLocId)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var tran = await this.unitOfWork.BeginTransactionAsync(ct);
        try
        {
            using var context = this.service.CreateContext();

            var request = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = 10
            };

            await this.AddReceivingAsync(context, request, ct);
            var data = await this.AddReservedAsync(context, request, ct);
            Assert.Contains(context.MovementList, l => l == data);

            this.Delete(context, data!);
            Assert.DoesNotContain(context.MovementList, l => l == data);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Fact]
    public Task DeleteWhStock05TestAsync()
    {
        return this.DeleteWhStockTest02Async(this.whIdNoZoneNoLoc, null, null);
    }

    [Fact]
    public Task DeleteWhStock06TestAsync()
    {
        return this.DeleteWhStockTest02Async(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null);
    }

    [Fact]
    public Task DeleteWhStock07TestAsync()
    {
        return this.DeleteWhStockTest02Async(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone);
    }

    [Fact]
    public Task DeleteWhStock08TestAsync()
    {
        return this.DeleteWhStockTest02Async(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocIdNoZone);
    }

    private async Task DeleteWhStockTest02Async(string whId, int? whZoneId, int? whLocId)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var tran = await this.unitOfWork.BeginTransactionAsync(ct);
        try
        {
            using var context = this.service.CreateContext();

            var request = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = 10
            };

            await this.AddReceivingAsync(context, request, ct);
            await this.AddReservedAsync(context, request, ct);
            var data = await this.CommitReservedAsync(context, request, ct);
            Assert.Contains(context.MovementList, l => l == data);

            this.Delete(context, data!);
            Assert.DoesNotContain(context.MovementList, l => l == data);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndRemoveAndStoreWhStock01TestAsync(double? qty)
    {
        return this.AddAndRemoveAndStoreWhStockTest01Async(this.whIdNoZoneNoLoc, null, null, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndRemoveAndStoreWhStock02TestAsync(double? qty)
    {
        return this.AddAndRemoveAndStoreWhStockTest01Async(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndRemoveAndStoreWhStock03TestAsync(double? qty)
    {
        return this.AddAndRemoveAndStoreWhStockTest01Async(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndRemoveAndStoreWhStock04TestAsync(double? qty)
    {
        return this.AddAndRemoveAndStoreWhStockTest01Async(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal?)qty);
    }

    private async Task AddAndRemoveAndStoreWhStockTest01Async(string whId, int? whZoneId, int? whLocId, decimal? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var tran = await this.unitOfWork.BeginTransactionAsync(ct);
        try
        {
            await this.AddAndCommitReceivingTest01Async(whId, whZoneId, whLocId, qty, cancellationToken: ct);

            var originalEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);

            using var context = this.service.CreateContext();

            var currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);

            var request = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = qty
            };

            await this.AddReservedAsync(context, request, ct);

            var removeRequest = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)(currentEntity!.Resqty + request.Qty.GetValueOrDefault()), Precision, MidpointRounding.AwayFromZero))
            };

            await this.RemoveReservedAsync(context, removeRequest, ct);

            await this.StoreAsync(context, ct);

            currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
            Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
            Assert.Equal((originalEntity?.Resqty).GetValueOrDefault() + request.Qty - removeRequest.Qty, currentEntity!.Resqty);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndRemoveAndStoreWhStock05TestAsync(double? qty)
    {
        return this.AddAndRemoveAndStoreWhStockTest02Async(this.whIdNoZoneNoLoc, null, null, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndRemoveAndStoreWhStock06TestAsync(double? qty)
    {
        return this.AddAndRemoveAndStoreWhStockTest02Async(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndRemoveAndStoreWhStock07TestAsync(double? qty)
    {
        return this.AddAndRemoveAndStoreWhStockTest02Async(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndRemoveAndStoreWhStock08TestAsync(double? qty)
    {
        return this.AddAndRemoveAndStoreWhStockTest02Async(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal?)qty);
    }

    private async Task AddAndRemoveAndStoreWhStockTest02Async(string whId, int? whZoneId, int? whLocId, decimal? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var tran = await this.unitOfWork.BeginTransactionAsync(ct);
        try
        {
            await this.AddAndCommitReceivingTest01Async(whId, whZoneId, whLocId, qty, cancellationToken: ct);

            var originalEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);

            using var context = this.service.CreateContext();

            var request = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = qty
            };

            await this.AddReservedAsync(context, request, ct);

            await this.StoreAsync(context, ct);

            var currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.NotEqual(0M, currentEntity!.Resqty);

            var removeRequest = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)currentEntity.Resqty, Precision, MidpointRounding.AwayFromZero))
            };

            await this.RemoveReservedAsync(context, removeRequest, ct);

            await this.StoreAsync(context, ct);

            currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
            Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
            Assert.Equal((originalEntity?.Resqty).GetValueOrDefault() + request.Qty - removeRequest.Qty, currentEntity!.Resqty);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Fact]
    public Task AddAndRemoveAndStoreWhStockTo001TestAsync()
    {
        return this.AddAndRemoveAndStoreWhStockTo0TestAsync(this.whIdNoZoneNoLoc, null, null);
    }

    [Fact]
    public Task AddAndRemoveAndStoreWhStockTo002TestAsync()
    {
        return this.AddAndRemoveAndStoreWhStockTo0TestAsync(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null);
    }

    [Fact]
    public Task AddAndRemoveAndStoreWhStockTo003TestAsync()
    {
        return this.AddAndRemoveAndStoreWhStockTo0TestAsync(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone);
    }

    [Fact]
    public Task AddAndRemoveAndStoreWhStockTo004TestAsync()
    {
        return this.AddAndRemoveAndStoreWhStockTo0TestAsync(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId);
    }

    private async Task AddAndRemoveAndStoreWhStockTo0TestAsync(string whId, int? whZoneId, int? whLocId)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var tran = await this.unitOfWork.BeginTransactionAsync(ct);
        try
        {
            await this.AddAndCommitReceivingTest01Async(whId!, whZoneId, whLocId, 15.75M, cancellationToken: ct);

            var originalEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);

            using var context = this.service.CreateContext();

            var request = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = 15.75M
            };

            await this.AddReservedAsync(context, request, ct);

            await this.StoreAsync(context, ct);

            var currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.NotEqual(0M, currentEntity!.Resqty);

            var removeRequest = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = currentEntity.Resqty
            };

            await this.RemoveReservedAsync(context, removeRequest, ct);

            await this.StoreAsync(context, ct);

            currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
            Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
            Assert.Equal(0M, currentEntity!.Resqty);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Fact]
    public Task AddAndRemoveAndStoreWhStockFailed01TestAsync()
    {
        return this.AddAndRemoveAndStoreWhStockFailedTestAsync(this.whIdNoZoneNoLoc, null, null);
    }

    [Fact]
    public Task AddAndRemoveAndStoreWhStockFailed02TestAsync()
    {
        return this.AddAndRemoveAndStoreWhStockFailedTestAsync(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null);
    }

    [Fact]
    public Task AddAndRemoveAndStoreWhStockFailed03TestAsync()
    {
        return this.AddAndRemoveAndStoreWhStockFailedTestAsync(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone);
    }

    [Fact]
    public Task AddAndRemoveAndStoreWhStockFailed04TestAsync()
    {
        return this.AddAndRemoveAndStoreWhStockFailedTestAsync(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId);
    }

    private async Task AddAndRemoveAndStoreWhStockFailedTestAsync(string whId, int? whZoneId, int? whLocId)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var tran = await this.unitOfWork.BeginTransactionAsync(ct);
        try
        {
            await this.AddAndCommitReceivingTest01Async(whId, whZoneId, whLocId, 15.75M, cancellationToken: ct);

            using var context = this.service.CreateContext();

            var request = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = 15.75M
            };

            await this.AddReservedAsync(context, request, ct);

            await this.StoreAsync(context, ct);

            var currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.NotEqual(0M, currentEntity!.Resqty);

            var removeRequest = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = currentEntity.Resqty * 1.75M
            };

            var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.RemoveReservedAsync(context, removeRequest, ct));
            Assert.Equal(WhZStockExceptionType.RemoveReservedQty, ex.Type);
            Assert.Equal("Not enough reserved quantity to fulfill the remove request", ex.Message);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndCommitAndStoreWhStock01TestAsync(double? qty)
    {
        return this.AddAndCommitAndStoreWhStockTest01Async(this.whIdNoZoneNoLoc, null, null, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndCommitAndStoreWhStock02TestAsync(double? qty)
    {
        return this.AddAndCommitAndStoreWhStockTest01Async(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndCommitAndStoreWhStock03TestAsync(double? qty)
    {
        return this.AddAndCommitAndStoreWhStockTest01Async(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndCommitAndStoreWhStock04TestAsync(double? qty)
    {
        return this.AddAndCommitAndStoreWhStockTest01Async(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal?)qty);
    }

    private async Task AddAndCommitAndStoreWhStockTest01Async(string whId, int? whZoneId, int? whLocId, decimal? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var tran = await this.unitOfWork.BeginTransactionAsync(ct);
        try
        {
            await this.AddAndCommitReceivingTest01Async(whId, whZoneId, whLocId, qty, cancellationToken: ct);

            var originalEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);

            using var context = this.service.CreateContext();

            var currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.NotEqual(0M, currentEntity!.Actqty);

            var request = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = qty
            };

            await this.AddReservedAsync(context, request, ct);

            var commitRequest = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)Math.Min(currentEntity!.Resqty + request.Qty.GetValueOrDefault(), currentEntity.Actqty), Precision, MidpointRounding.AwayFromZero))
            };

            await this.CommitReservedAsync(context, commitRequest, ct);

            await this.StoreAsync(context, ct);

            currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
            Assert.Equal((originalEntity?.Actqty).GetValueOrDefault() - commitRequest.Qty, currentEntity!.Actqty);
            Assert.Equal((originalEntity?.Resqty).GetValueOrDefault() + request.Qty - commitRequest.Qty, currentEntity!.Resqty);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndCommitAndStoreWhStock05TestAsync(double? qty)
    {
        return this.AddAndCommitAndStoreWhStockTest02Async(this.whIdNoZoneNoLoc, null, null, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndCommitAndStoreWhStock06TestAsync(double? qty)
    {
        return this.AddAndCommitAndStoreWhStockTest02Async(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndCommitAndStoreWhStock07TestAsync(double? qty)
    {
        return this.AddAndCommitAndStoreWhStockTest02Async(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndCommitAndStoreWhStock08TestAsync(double? qty)
    {
        return this.AddAndCommitAndStoreWhStockTest02Async(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal?)qty);
    }

    private async Task AddAndCommitAndStoreWhStockTest02Async(string whId, int? whZoneId, int? whLocId, decimal? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var tran = await this.unitOfWork.BeginTransactionAsync(ct);
        try
        {
            await this.AddAndCommitReceivingTest01Async(whId!, whZoneId, whLocId, qty, cancellationToken: ct);

            var originalEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);

            using var context = this.service.CreateContext();

            var request = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = qty
            };

            await this.AddReservedAsync(context, request, ct);

            await this.StoreAsync(context, ct);

            var currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.NotEqual(0M, currentEntity!.Resqty);
            Assert.NotEqual(0M, currentEntity!.Actqty);

            var commitRequest = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)Math.Min(currentEntity.Resqty, currentEntity.Actqty), Precision, MidpointRounding.AwayFromZero))
            };

            await this.CommitReservedAsync(context, commitRequest, ct);

            await this.StoreAsync(context, ct);

            currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
            Assert.Equal((originalEntity?.Actqty).GetValueOrDefault() - commitRequest.Qty, currentEntity!.Actqty);
            Assert.Equal((originalEntity?.Resqty).GetValueOrDefault() + request.Qty - commitRequest.Qty, currentEntity!.Resqty);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Fact]
    public Task AndAndCommitAndStoreWhStockTo001TestAsync()
    {
        return this.AddAndCommitAndStoreWhStockTo0TestAsync(this.whIdNoZoneNoLoc, null, null);
    }

    [Fact]
    public Task AndAndCommitAndStoreWhStockTo002TestAsync()
    {
        return this.AddAndCommitAndStoreWhStockTo0TestAsync(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null);
    }

    [Fact]
    public Task AndAndCommitAndStoreWhStockTo003TestAsync()
    {
        return this.AddAndCommitAndStoreWhStockTo0TestAsync(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone);
    }

    [Fact]
    public Task AndAndCommitAndStoreWhStockTo004TestAsync()
    {
        return this.AddAndCommitAndStoreWhStockTo0TestAsync(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId);
    }

    private async Task AddAndCommitAndStoreWhStockTo0TestAsync(string whId, int? whZoneId, int? whLocId)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var tran = await this.unitOfWork.BeginTransactionAsync(ct);
        try
        {
            await this.AddAndCommitReceivingTest01Async(whId, whZoneId, whLocId, 15.75M, cancellationToken: ct);

            var originalEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);

            using var context = this.service.CreateContext();

            var request = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = 15.75M
            };

            await this.AddReservedAsync(context, request, ct);

            await this.StoreAsync(context, ct);

            var currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.NotEqual(0M, currentEntity!.Resqty);

            var commitRequest = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = currentEntity.Resqty
            };

            await this.CommitReservedAsync(context, commitRequest, ct);

            await this.StoreAsync(context, ct);

            currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.Equal((originalEntity?.Recqty).GetValueOrDefault(), currentEntity!.Recqty);
            Assert.Equal((originalEntity?.Actqty).GetValueOrDefault() - commitRequest.Qty, currentEntity!.Actqty);
            Assert.Equal(0M, currentEntity!.Resqty);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Fact]
    public Task AddAndCommitAndStoreWhStockFailed01TestAsync()
    {
        return this.AddAndCommitAndStoreWhStockFailedTestAsync(this.whIdNoZoneNoLoc, null, null);
    }

    [Fact]
    public Task AddAndCommitAndStoreWhStockFailed02TestAsync()
    {
        return this.AddAndCommitAndStoreWhStockFailedTestAsync(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null);
    }

    [Fact]
    public Task AddAndCommitAndStoreWhStockFailed03TestAsync()
    {
        return this.AddAndCommitAndStoreWhStockFailedTestAsync(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone);
    }

    [Fact]
    public Task AddAndCommitAndStoreWhStockFailed04TestAsync()
    {
        return this.AddAndCommitAndStoreWhStockFailedTestAsync(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId);
    }

    private async Task AddAndCommitAndStoreWhStockFailedTestAsync(string whId, int? whZoneId, int? whLocId)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var tran = await this.unitOfWork.BeginTransactionAsync(ct);
        try
        {
            await this.AddAndCommitReceivingTest01Async(whId, whZoneId, whLocId, 15.75M, cancellationToken: ct);

            using var context = this.service.CreateContext();

            var request = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = 15.75M
            };

            await this.AddReservedAsync(context, request, ct);

            await this.StoreAsync(context, ct);

            var currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.NotEqual(0M, currentEntity!.Resqty);

            var commitRequest = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = currentEntity.Resqty * 1.75M
            };

            var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.CommitReservedAsync(context, commitRequest, ct));
            Assert.Equal(WhZStockExceptionType.CommitReservedQty, ex.Type);
            Assert.Equal("Not enough reserved quantity to fulfill the commit request", ex.Message);
        }
        finally
        {
            tran.Rollback();
        }
    }

    //// AddMultipleItemsWhStock01TestAsync
}
