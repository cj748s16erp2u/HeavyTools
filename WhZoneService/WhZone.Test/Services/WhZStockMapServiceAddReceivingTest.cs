using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Enums;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services;
using eLog.HeavyTools.Services.WhZone.DataAccess.Repositories.Interfaces;
using eLog.HeavyTools.Services.WhZone.Test.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace eLog.HeavyTools.Services.WhZone.Test.Services;

public class WhZStockMapServiceAddReceivingTest : Base.WhZStockMapServiceTestBase
{
    public WhZStockMapServiceAddReceivingTest(ITestOutputHelper testOutputHelper, TestFixture fixture) : base(testOutputHelper, fixture)
    {
    }

    #region AddReceivingWhStock

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddReceiving01TestAsync(double qty)
    {
        return this.AddReceivingTest01Async(this.whIdNoZoneNoLoc, null, null, (decimal)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddReceiving02TestAsync(double qty)
    {
        return this.AddReceivingTest01Async(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddReceiving03TestAsync(double qty)
    {
        return this.AddReceivingTest01Async(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddReceiving04TestAsync(double qty)
    {
        return this.AddReceivingTest01Async(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal)qty);
    }

    private async Task AddReceivingTest01Async(string whId, int? whZoneId, int? whLocId, decimal? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var context = this.service.CreateContext();

        var request = new WhZStockMapDto
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Qty = qty
        };

        await this.AddReceivingAsync(context, request, ct);
    }

    [Fact]
    public Task AddReceivingNullFailed01TestAsync()
    {
        return this.AddReceivingNullFailedTest01Async(this.whIdNoZoneNoLoc, null, null);
    }

    [Fact]
    public Task AddReceivingNullFailed02TestAsync()
    {
        return this.AddReceivingNullFailedTest01Async(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null);
    }

    [Fact]
    public Task AddReceivingNullFailed03TestAsync()
    {
        return this.AddReceivingNullFailedTest01Async(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone);
    }

    [Fact]
    public Task AddReceivingNullFailed04TestAsync()
    {
        return this.AddReceivingNullFailedTest01Async(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId);
    }

    private async Task AddReceivingNullFailedTest01Async(string whId, int? whZoneId, int? whLocId)
    {
        var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.AddReceivingTest01Async(whId, whZoneId, whLocId, null));
        Assert.Equal(WhZStockExceptionType.InvalidRequestQty, ex.Type);
        Assert.Equal("The add qty is not set", ex.Message);
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-15.0)]
    [InlineData(-0.25)]
    [InlineData(-5.25)]
    public Task AddReceivingFailed01TestAsync(double? qty)
    {
        return this.AddReceivingFailedTest01Async(this.whIdNoZoneNoLoc, null, null, (decimal?)qty);
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-15.0)]
    [InlineData(-0.25)]
    [InlineData(-5.25)]
    public Task AddReceivingFailed02TestAsync(double? qty)
    {
        return this.AddReceivingFailedTest01Async(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal?)qty);
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-15.0)]
    [InlineData(-0.25)]
    [InlineData(-5.25)]
    public Task AddReceivingFailed03TestAsync(double? qty)
    {
        return this.AddReceivingFailedTest01Async(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal?)qty);
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-15.0)]
    [InlineData(-0.25)]
    [InlineData(-5.25)]
    public Task AddReceivingFailed04TestAsync(double? qty)
    {
        return this.AddReceivingFailedTest01Async(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal?)qty);
    }

    private async Task AddReceivingFailedTest01Async(string whId, int? whZoneId, int? whLocId, decimal? qty)
    {
        var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.AddReceivingTest01Async(whId, whZoneId, whLocId, qty));
        Assert.Equal(WhZStockExceptionType.InvalidRequestQty, ex.Type);
        Assert.Equal("The add qty cannot be less or equal to 0", ex.Message);
    }

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public Task AddReceivingAndStore01TestAsync(double? qty)
    //{
    //    return this.AddReceivingAndStoreTest01Async(this.whIdNoZoneNoLoc, null, null, (decimal?)qty);
    //}

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public Task AddReceivingAndStore02TestAsync(double? qty)
    //{
    //    return this.AddReceivingAndStoreTest01Async(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal?)qty);
    //}

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddReceivingAndStore03TestAsync(double? qty)
    {
        return this.AddReceivingAndStoreTest01Async(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddReceivingAndStore04TestAsync(double? qty)
    {
        return this.AddReceivingAndStoreTest01Async(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal?)qty);
    }

    private async Task AddReceivingAndStoreTest01Async(string whId, int? whZoneId, int? whLocId, decimal? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

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

        using var tran = await this.unitOfWork.BeginTransactionAsync();
        try
        {
            await this.AddReceivingAsync(context, request, ct);

            await this.StoreAsync(context, ct);

            var currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.Equal((originalEntity?.Recqty).GetValueOrDefault() + request.Qty, currentEntity!.Recqty);
            Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
            Assert.Equal((originalEntity?.Resqty).GetValueOrDefault(), currentEntity!.Resqty);
        }
        finally
        {
            tran.Rollback();
        }
    }

    //[Theory]
    //[InlineData(1.0, 2.0)]
    //[InlineData(15.0, 2.0)]
    //[InlineData(0.25, 2.0)]
    //[InlineData(5.25, 2.0)]
    //public Task AddAndUpdateReceivingAndStore01TestAsync(double? qty1, double? qty2)
    //{
    //    return this.AddAndUpdateReceivingAndStoreTest01Async(this.whIdNoZoneNoLoc, null, null, (decimal?)qty1, (decimal?)qty2);
    //}

    //[Theory]
    //[InlineData(1.0, 2.0)]
    //[InlineData(15.0, 2.0)]
    //[InlineData(0.25, 2.0)]
    //[InlineData(5.25, 2.0)]
    //public Task AddAndUpdateReceivingAndStore02TestAsync(double? qty1, double? qty2)
    //{
    //    return this.AddAndUpdateReceivingAndStoreTest01Async(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal?)qty1, (decimal?)qty2);
    //}

    [Theory]
    [InlineData(1.0, 2.0)]
    [InlineData(15.0, 2.0)]
    [InlineData(0.25, 2.0)]
    [InlineData(5.25, 2.0)]
    public Task AddAndUpdateReceivingAndStore03TestAsync(double? qty1, double? qty2)
    {
        return this.AddAndUpdateReceivingAndStoreTest01Async(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal?)qty1, (decimal?)qty2);
    }

    [Theory]
    [InlineData(1.0, 2.0)]
    [InlineData(15.0, 2.0)]
    [InlineData(0.25, 2.0)]
    [InlineData(5.25, 2.0)]
    public Task AddAndUpdateReceivingAndStore04TestAsync(double? qty1, double? qty2)
    {
        return this.AddAndUpdateReceivingAndStoreTest01Async(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal?)qty1, (decimal?)qty2);
    }

    private async Task AddAndUpdateReceivingAndStoreTest01Async(string whId, int? whZoneId, int? whLocId, decimal? qty1, decimal? qty2)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var originalEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);

        var request1 = new WhZStockMapDto
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Qty = qty1
        };

        var request2 = new WhZStockMapDto
        {
            Itemid = this.itemId,
            Whid = whId,
            Whzoneid = whZoneId,
            Whlocid = whLocId,
            Qty = qty2
        };

        using var tran = await this.unitOfWork.BeginTransactionAsync();
        try
        {
            using (var context = this.service.CreateContext())
            {
                await this.AddReceivingAsync(context, request1, ct);

                await this.StoreAsync(context, ct);

                var currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
                Assert.NotNull(currentEntity);
                Assert.Equal((originalEntity?.Recqty).GetValueOrDefault() + request1.Qty, currentEntity!.Recqty);
                Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
                Assert.Equal((originalEntity?.Resqty).GetValueOrDefault(), currentEntity!.Resqty);

                originalEntity = currentEntity;
            }

            using (var context = this.service.CreateContext())
            {
                await this.AddReceivingAsync(context, request2, ct);

                await this.StoreAsync(context, ct);

                var currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
                Assert.NotNull(currentEntity);
                Assert.Equal((originalEntity?.Recqty).GetValueOrDefault() + request2.Qty, currentEntity!.Recqty);
                Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
                Assert.Equal((originalEntity?.Resqty).GetValueOrDefault(), currentEntity!.Resqty);
            }
        }
        finally
        {
            tran.Rollback();
        }
    }

    #endregion

    #region DeleteWhStock

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task DeleteWhStock01TestAsync(double qty)
    {
        return this.DeleteWhStockTest01Async(this.whIdNoZoneNoLoc, null, null, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task DeleteWhStock02TestAsync(double qty)
    {
        return this.DeleteWhStockTest01Async(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task DeleteWhStock03TestAsync(double qty)
    {
        return this.DeleteWhStockTest01Async(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task DeleteWhStock04TestAsync(double qty)
    {
        return this.DeleteWhStockTest01Async(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal?)qty);
    }

    private async Task DeleteWhStockTest01Async(string whId, int? whZoneId, int? whLocId, decimal? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

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
            using var context = this.service.CreateContext();

            var data = await this.AddReceivingAsync(context, request, ct);
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
    public Task DeleteWhStock05TestAsync(double qty)
    {
        return this.DeleteWhStockTest02Async(this.whIdNoZoneNoLoc, null, null, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task DeleteWhStock06TestAsync(double qty)
    {
        return this.DeleteWhStockTest02Async(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task DeleteWhStock07TestAsync(double qty)
    {
        return this.DeleteWhStockTest02Async(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task DeleteWhStock08TestAsync(double qty)
    {
        return this.DeleteWhStockTest02Async(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal?)qty);
    }

    private async Task DeleteWhStockTest02Async(string whId, int? whZoneId, int? whLocId, decimal? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

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
            using var context = this.service.CreateContext();

            var data = await this.AddReceivingAsync(context, request, ct);
            Assert.Contains(context.MovementList, l => l == data);
            await this.AddReceivingAsync(context, request, ct);
            await this.AddReservedAsync(context, request, ct);
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
    public Task DeleteWhStock09TestAsync(double qty)
    {
        return this.DeleteWhStockTest03Async(this.whIdNoZoneNoLoc, null, null, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task DeleteWhStock10TestAsync(double qty)
    {
        return this.DeleteWhStockTest03Async(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task DeleteWhStock11TestAsync(double qty)
    {
        return this.DeleteWhStockTest03Async(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task DeleteWhStock12TestAsync(double qty)
    {
        return this.DeleteWhStockTest03Async(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal?)qty);
    }

    private async Task DeleteWhStockTest03Async(string whId, int? whZoneId, int? whLocId, decimal? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

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
            using var context = this.service.CreateContext();

            await this.AddReceivingAsync(context, request, ct);
            var data = await this.CommitReceivingAsync(context, request, ct);
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
    public Task DeleteWhStockFailed01TestAsync(double qty)
    {
        return this.DeleteWhStockFailedTestAsync(this.whIdNoZoneNoLoc, null, null, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task DeleteWhStockFailed02TestAsync(double qty)
    {
        return this.DeleteWhStockFailedTestAsync(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task DeleteWhStockFailed03TestAsync(double qty)
    {
        return this.DeleteWhStockFailedTestAsync(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task DeleteWhStockFailed04TestAsync(double qty)
    {
        return this.DeleteWhStockFailedTestAsync(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal?)qty);
    }

    private async Task DeleteWhStockFailedTestAsync(string whId, int? whZoneId, int? whLocId, decimal? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

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
            using var context = this.service.CreateContext();

            var data = await this.AddReceivingAsync(context, request, ct);
            await this.AddReservedAsync(context, request, ct);
            var ex = Assert.Throws<WhZStockMapServiceException>(() => this.Delete(context, data!));
            Assert.Equal(WhZStockExceptionType.DeleteNotEnoughQty, ex.Type);
            Assert.Equal("Unable to remove this request, cause the further requests uses its quantity", ex.Message);
            Assert.Contains(context.MovementList, l => l == data);
        }
        finally
        {
            tran.Rollback();
        }
    }

    #endregion

    #region RemoveWhStockFailed

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-15.0)]
    [InlineData(-0.25)]
    [InlineData(-5.25)]
    public Task RemoveWhStockFailed01TestAsync(double qty)
    {
        return this.RemoveWhStockFailedTestAsync(this.whIdNoZoneNoLoc, null, null, (decimal?)qty);
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-15.0)]
    [InlineData(-0.25)]
    [InlineData(-5.25)]
    public Task RemoveWhStockFailed02TestAsync(double qty)
    {
        return this.RemoveWhStockFailedTestAsync(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal?)qty);
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-15.0)]
    [InlineData(-0.25)]
    [InlineData(-5.25)]
    public Task RemoveWhStockFailed03TestAsync(double qty)
    {
        return this.RemoveWhStockFailedTestAsync(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal?)qty);
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-15.0)]
    [InlineData(-0.25)]
    [InlineData(-5.25)]
    public Task RemoveWhStockFailed04TestAsync(double qty)
    {
        return this.RemoveWhStockFailedTestAsync(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal?)qty);
    }

    private async Task RemoveWhStockFailedTestAsync(string whId, int? whZoneId, int? whLocId, decimal? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

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
            using var context = this.service.CreateContext();

            var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.RemoveReceivingAsync(context, request, ct));
            Assert.Equal(WhZStockExceptionType.InvalidRequestQty, ex.Type);
            Assert.Equal("The remove qty cannot be less or equal to 0", ex.Message);
        }
        finally
        {
            tran.Rollback();
        }
    }

    #endregion

    #region AddAndRemoveAndStoreWhStock

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public Task AddAndRemoveAndStoreWhStock01TestAsync(double qty)
    //{
    //    return this.AddAndRemoveAndStoreWhStockTest01Async(this.whIdNoZoneNoLoc, null, null, (decimal?)qty);
    //}

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public Task AddAndRemoveAndStoreWhStock02TestAsync(double qty)
    //{
    //    return this.AddAndRemoveAndStoreWhStockTest01Async(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal?)qty);
    //}

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndRemoveAndStoreWhStock03TestAsync(double qty)
    {
        return this.AddAndRemoveAndStoreWhStockTest01Async(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndRemoveAndStoreWhStock04TestAsync(double qty)
    {
        return this.AddAndRemoveAndStoreWhStockTest01Async(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal?)qty);
    }

    private async Task AddAndRemoveAndStoreWhStockTest01Async(string whId, int? whZoneId, int? whLocId, decimal? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var originalEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);

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
            using var context = this.service.CreateContext();

            await this.AddReceivingAsync(context, request, ct);

            var currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            if (currentEntity == null)
            {
                currentEntity = new OlcWhzstockmap
                {
                    Recqty = 0M
                };
            }

            var removeRequest = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)(currentEntity!.Recqty + request.Qty.GetValueOrDefault()), Precision, MidpointRounding.AwayFromZero))
            };

            await this.RemoveReceivingAsync(context, removeRequest, ct);

            await this.StoreAsync(context, ct);

            currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.Equal((originalEntity?.Recqty).GetValueOrDefault() + request.Qty.GetValueOrDefault() - removeRequest.Qty.GetValueOrDefault(), currentEntity!.Recqty, Precision);
            Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty, Precision);
            Assert.Equal((originalEntity?.Resqty).GetValueOrDefault(), currentEntity!.Resqty, Precision);
        }
        finally
        {
            tran.Rollback();
        }
    }

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public Task AddAndRemoveAndStoreWhStock05TestAsync(double qty)
    //{
    //    return this.AddAndRemoveAndStoreWhStockTest02Async(this.whIdNoZoneNoLoc, null, null, (decimal?)qty);
    //}

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public Task AddAndRemoveAndStoreWhStock06TestAsync(double qty)
    //{
    //    return this.AddAndRemoveAndStoreWhStockTest02Async(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal?)qty);
    //}

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndRemoveAndStoreWhStock07TestAsync(double qty)
    {
        return this.AddAndRemoveAndStoreWhStockTest02Async(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndRemoveAndStoreWhStock08TestAsync(double qty)
    {
        return this.AddAndRemoveAndStoreWhStockTest02Async(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal?)qty);
    }

    private async Task AddAndRemoveAndStoreWhStockTest02Async(string whId, int? whZoneId, int? whLocId, decimal? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var originalEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);

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
            using var context = this.service.CreateContext();

            await this.AddReceivingAsync(context, request, ct);

            await this.StoreAsync(context, ct);

            var currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.NotEqual(0M, currentEntity!.Recqty);

            var removeRequest = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)currentEntity.Recqty, Precision, MidpointRounding.AwayFromZero))
            };

            await this.RemoveReceivingAsync(context, removeRequest, ct);

            await this.StoreAsync(context, ct);

            currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.Equal((originalEntity?.Recqty).GetValueOrDefault() + request.Qty - removeRequest.Qty, currentEntity!.Recqty);
            Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
            Assert.Equal((originalEntity?.Resqty).GetValueOrDefault(), currentEntity!.Resqty);
        }
        finally
        {
            tran.Rollback();
        }
    }

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public Task AddAndRemoveAndStoreWhStockTo001TestAsync(double qty)
    //{
    //    return this.AddAndRemoveAndStoreWhStockTo0TestAsync(this.whIdNoZoneNoLoc, null, null, (decimal?)qty);
    //}

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public Task AddAndRemoveAndStoreWhStockTo002TestAsync(double qty)
    //{
    //    return this.AddAndRemoveAndStoreWhStockTo0TestAsync(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal?)qty);
    //}

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndRemoveAndStoreWhStockTo003TestAsync(double qty)
    {
        return this.AddAndRemoveAndStoreWhStockTo0TestAsync(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndRemoveAndStoreWhStockTo004TestAsync(double qty)
    {
        return this.AddAndRemoveAndStoreWhStockTo0TestAsync(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal?)qty);
    }

    private async Task AddAndRemoveAndStoreWhStockTo0TestAsync(string whId, int? whZoneId, int? whLocId, decimal? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var originalEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);

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
            using var context = this.service.CreateContext();

            await this.AddReceivingAsync(context, request, ct);

            await this.StoreAsync(context, ct);

            var currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.NotEqual(0M, currentEntity!.Recqty);

            var removeRequest = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = currentEntity.Recqty
            };

            await this.RemoveReceivingAsync(context, removeRequest, ct);

            await this.StoreAsync(context, ct);

            currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.Equal(0M, currentEntity!.Recqty);
            Assert.Equal((originalEntity?.Actqty).GetValueOrDefault(), currentEntity!.Actqty);
            Assert.Equal((originalEntity?.Resqty).GetValueOrDefault(), currentEntity!.Resqty);
        }
        finally
        {
            tran.Rollback();
        }
    }

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public Task AddAndRemoveAndStoreWhStockFailed01TestAsync(double qty)
    //{
    //    return this.AddAndRemoveAndStoreWhStockFailedTestAsync(this.whIdNoZoneNoLoc, null, null, (decimal?)qty);
    //}

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public Task AddAndRemoveAndStoreWhStockFailed02TestAsync(double qty)
    //{
    //    return this.AddAndRemoveAndStoreWhStockFailedTestAsync(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal?)qty);
    //}

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndRemoveAndStoreWhStockFailed03TestAsync(double qty)
    {
        return this.AddAndRemoveAndStoreWhStockFailedTestAsync(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndRemoveAndStoreWhStockFailed04TestAsync(double qty)
    {
        return this.AddAndRemoveAndStoreWhStockFailedTestAsync(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal?)qty);
    }

    private async Task AddAndRemoveAndStoreWhStockFailedTestAsync(string whId, int? whZoneId, int? whLocId, decimal? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

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
            using var context = this.service.CreateContext();

            await this.AddReceivingAsync(context, request, ct);

            await this.StoreAsync(context, ct);

            var currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.NotEqual(0M, currentEntity!.Recqty);

            var removeRequest = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = currentEntity.Recqty * 1.75M
            };

            var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.RemoveReceivingAsync(context, removeRequest, ct));
            Assert.Equal(WhZStockExceptionType.RemoveReceivingQty, ex.Type);
            Assert.Equal("Not enough receiving quantity to fulfill the remove request", ex.Message);
        }
        finally
        {
            tran.Rollback();
        }
    }

    #endregion

    #region AddAndCommitWhStock

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public Task AddAndCommitWhStock01TestAsync(double? qty)
    //{
    //    return this.AddAndCommitWhStockTest01Async(this.whIdNoZoneNoLoc, null, null, (decimal?)qty);
    //}

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public Task AddAndCommitWhStock02TestAsync(double? qty)
    //{
    //    return this.AddAndCommitWhStockTest01Async(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal?)qty);
    //}

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndCommitWhStock03TestAsync(double? qty)
    {
        return this.AddAndCommitWhStockTest01Async(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndCommitWhStock04TestAsync(double? qty)
    {
        return this.AddAndCommitWhStockTest01Async(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal?)qty);
    }

    private async Task AddAndCommitWhStockTest01Async(string whId, int? whZoneId, int? whLocId, decimal? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var originalEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);

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
            using var context = this.service.CreateContext();

            await this.AddReceivingAsync(context, request, ct);
            var currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            if (currentEntity == null)
            {
                currentEntity = new OlcWhzstockmap
                {
                    Recqty = 0M
                };
            }

            var commitRequest = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)(currentEntity!.Recqty + request.Qty.GetValueOrDefault()), Precision, MidpointRounding.AwayFromZero))
            };

            await this.CommitReceivingAsync(context, commitRequest, ct);

            await this.StoreAsync(context, ct);

            currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.Equal((originalEntity?.Recqty).GetValueOrDefault() + request.Qty - commitRequest.Qty, currentEntity!.Recqty);
            Assert.Equal((originalEntity?.Actqty).GetValueOrDefault() + commitRequest.Qty, currentEntity!.Actqty);
            Assert.Equal((originalEntity?.Resqty).GetValueOrDefault(), currentEntity!.Resqty);
        }
        finally
        {
            tran.Rollback();
        }
    }

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public Task AddAndCommitWhStock05TestAsync(double? qty)
    //{
    //    return this.AddAndCommitWhStockTest02Async(this.whIdNoZoneNoLoc, null, null, (decimal?)qty);
    //}

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public Task AddAndCommitWhStock06TestAsync(double? qty)
    //{
    //    return this.AddAndCommitWhStockTest02Async(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal?)qty);
    //}

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndCommitWhStock07TestAsync(double? qty)
    {
        return this.AddAndCommitWhStockTest02Async(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndCommitWhStock08TestAsync(double? qty)
    {
        return this.AddAndCommitWhStockTest02Async(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal?)qty);
    }

    private async Task AddAndCommitWhStockTest02Async(string whId, int? whZoneId, int? whLocId, decimal? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var originalEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);

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
            using var context = this.service.CreateContext();

            await this.AddReceivingAsync(context, request, ct);

            await this.StoreAsync(context, ct);

            var currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.NotEqual(0M, currentEntity!.Recqty);

            var commitRequest = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = Convert.ToDecimal(Math.Round(new Random().NextDouble() * (double)currentEntity!.Recqty, Precision, MidpointRounding.AwayFromZero))
            };

            await this.CommitReceivingAsync(context, commitRequest, ct);

            await this.StoreAsync(context, ct);

            currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.Equal((originalEntity?.Recqty).GetValueOrDefault() + request.Qty - commitRequest.Qty, currentEntity!.Recqty);
            Assert.Equal((originalEntity?.Actqty).GetValueOrDefault() + commitRequest.Qty, currentEntity!.Actqty);
            Assert.Equal((originalEntity?.Resqty).GetValueOrDefault(), currentEntity!.Resqty);
        }
        finally
        {
            tran.Rollback();
        }
    }

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public Task AddAndCommitWhStockTo001TestAsync(double qty)
    //{
    //    return this.AddAndCommitWhStockTo0TestAsync(this.whIdNoZoneNoLoc, null, null, (decimal?)qty);
    //}

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public Task AddAndCommitWhStockTo002TestAsync(double qty)
    //{
    //    return this.AddAndCommitWhStockTo0TestAsync(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal?)qty);
    //}

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndCommitWhStockTo003TestAsync(double qty)
    {
        return this.AddAndCommitWhStockTo0TestAsync(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndCommitWhStockTo004TestAsync(double qty)
    {
        return this.AddAndCommitWhStockTo0TestAsync(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal?)qty);
    }

    private async Task AddAndCommitWhStockTo0TestAsync(string whId, int? whZoneId, int? whLocId, decimal? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        var originalEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);

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
            using var context = this.service.CreateContext();

            await this.AddReceivingAsync(context, request, ct);

            await this.StoreAsync(context, ct);

            var currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.NotEqual(0M, currentEntity!.Recqty);

            var commitRequest = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = currentEntity.Recqty
            };

            await this.CommitReceivingAsync(context, commitRequest, ct);

            await this.StoreAsync(context, ct);

            currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.Equal(0M, currentEntity!.Recqty);
            Assert.Equal((originalEntity?.Actqty).GetValueOrDefault() + commitRequest.Qty, currentEntity!.Actqty);
            Assert.Equal((originalEntity?.Resqty).GetValueOrDefault(), currentEntity!.Resqty);
        }
        finally
        {
            tran.Rollback();
        }
    }

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public Task AddAndCommitWhStockFailed01TestAsync(double qty)
    //{
    //    return this.AddAndCommitWhStockFailedTestAsync(this.whIdNoZoneNoLoc, null, null, (decimal?)qty);
    //}

    //[Theory]
    //[InlineData(1.0)]
    //[InlineData(15.0)]
    //[InlineData(0.25)]
    //[InlineData(5.25)]
    //public Task AddAndCommitWhStockFailed02TestAsync(double qty)
    //{
    //    return this.AddAndCommitWhStockFailedTestAsync(this.whIdWithZoneNoLoc, this.whZoneIdNoLoc, null, (decimal?)qty);
    //}

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndCommitWhStockFailed03TestAsync(double qty)
    {
        return this.AddAndCommitWhStockFailedTestAsync(this.whIdNoZoneWithLoc, null, this.whLocIdNoZone, (decimal?)qty);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(15.0)]
    [InlineData(0.25)]
    [InlineData(5.25)]
    public Task AddAndCommitWhStockFailed04TestAsync(double qty)
    {
        return this.AddAndCommitWhStockFailedTestAsync(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, this.whLocId, (decimal?)qty);
    }

    private async Task AddAndCommitWhStockFailedTestAsync(string whId, int? whZoneId, int? whLocId, decimal? qty)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

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
            using var context = this.service.CreateContext();

            await this.AddReceivingAsync(context, request, ct);

            await this.StoreAsync(context, ct);

            var currentEntity = await this.GetCurrentEntryAsync(whId, whZoneId, whLocId, ct);
            Assert.NotNull(currentEntity);
            Assert.NotEqual(0M, currentEntity!.Recqty);

            var commitRequest = new WhZStockMapDto
            {
                Itemid = this.itemId,
                Whid = whId,
                Whzoneid = whZoneId,
                Whlocid = whLocId,
                Qty = currentEntity.Recqty * 1.75M
            };

            var ex = await Assert.ThrowsAsync<WhZStockMapServiceException>(() => this.CommitReceivingAsync(context, commitRequest, ct));
            Assert.Equal(WhZStockExceptionType.CommitReceivingQty, ex.Type);
            Assert.Equal("Not enough receiving quantity to fulfill the commit request", ex.Message);
        }
        finally
        {
            tran.Rollback();
        }
    }

    #endregion

    [Theory]
    [InlineData(5, 7)]
    public Task AddMultipleItemsWhStock01TestAsync(int numberOfItems, int numberOfLocations)
    {
        return this.AddMultipleItemsWhStockTestAsync(this.whIdNoZoneWithLoc, null, numberOfItems, numberOfLocations);
    }

    [Theory]
    [InlineData(5, 7)]
    public Task AddMultipleItemsWhStock02TestAsync(int numberOfItems, int numberOfLocations)
    {
        return this.AddMultipleItemsWhStockTestAsync(this.whIdWithZoneWithLoc, this.whZoneIdWithLoc, numberOfItems, numberOfLocations);
    }

    private async Task AddMultipleItemsWhStockTestAsync(string whId, int? whZoneId, int numberOfItems, int numberOfLocations)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var tran = await this.unitOfWork.BeginTransactionAsync();
        try
        {
            using var context = this.service.CreateContext();

            var list = new List<(int itemId, int whLocId, decimal qty, OlcWhzstockmap? stockMap)>();

            var random = new Random();
            for (var i = 0; i < numberOfItems; i++)
            {
                var itemId = await this.GetRandomItemIdAsync(ct);
                Assert.NotNull(itemId);

                for (var j = 0; j < numberOfLocations; j++)
                {
                    var whLocId = await this.GetRandomWhLocIdAsync(whId, whZoneId, ct);
                    Assert.NotNull(whLocId);

                    var request = new WhZStockMapDto
                    {
                        Itemid = itemId.Value,
                        Whid = whId,
                        Whzoneid = whZoneId,
                        Whlocid = whLocId.Value,
                        Qty = Convert.ToDecimal(Math.Round(random.NextDouble() * random.NextDouble() * 1000, 2, MidpointRounding.AwayFromZero))
                    };

                    var entity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(i => i.Itemid == itemId && i.Whid == whId && i.Whzoneid == whZoneId && i.Whlocid == whLocId, ct);
                    list.Add((itemId.Value, whLocId.Value, request.Qty.Value, entity));

                    await this.AddReceivingAsync(context, request, ct);
                }
            }

            await this.StoreAsync(context, ct);

            foreach (var (itemId, whLocId, qty, stockMap) in list.GroupBy(l => new { l.itemId, l.whLocId }).Select(l => (l.Key.itemId, l.Key.whLocId, l.Sum(l1 => l1.qty), l.First().stockMap)))
            {
                var entity = await this.dbContext.OlcWhzstockmaps.FirstOrDefaultAsync(i => i.Itemid == itemId && i.Whid == whId && i.Whzoneid == whZoneId && i.Whlocid == whLocId, ct);
                Assert.NotNull(entity);
                Assert.Equal((stockMap?.Recqty).GetValueOrDefault() + qty, entity!.Recqty);
            }
        }
        finally
        {
            tran.Rollback();
        }
    }


    //// CommitMultipleItemsWhStockTestAsync
}
