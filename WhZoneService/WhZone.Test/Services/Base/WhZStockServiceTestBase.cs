using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Containers.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Enums;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhZone.DataAccess.Context;
using eLog.HeavyTools.Services.WhZone.Test.Base;
using eLog.HeavyTools.Services.WhZone.Test.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace eLog.HeavyTools.Services.WhZone.Test.Services.Base;

public abstract class WhZStockServiceTestBase : TestBase<OlcWhzstock, IWhZStockService>
{
    internal const int Precision = 6;

    protected WhZStockServiceTestBase(
        ITestOutputHelper testOutputHelper,
        TestFixture fixture) : base(testOutputHelper, fixture)
    {
    }

    protected async Task StoreAsync(IWhZStockContext context, CancellationToken cancellationToken = default)
    {
        await this.service.StoreAsync(context, cancellationToken);
    }

    protected async Task<IWhZStockData?> AddReceivingAsync(IWhZStockContext context, WhZStockDto request, CancellationToken cancellationToken = default)
    {
        var receivingData = await this.service.AddReceivingAsync(context, request, cancellationToken);
        Assert.NotNull(receivingData);
        Assert.Equal(WhZStockMovement.AddReceving, receivingData.Movement);
        Assert.Equal(request.Qty.GetValueOrDefault(), (receivingData?.Qty).GetValueOrDefault(), Precision);

        return receivingData;
    }

    protected async Task<IWhZStockData?> AddReservedAsync(IWhZStockContext context, WhZStockDto request, CancellationToken cancellationToken = default)
    {
        var reservedData = await this.service.AddReservedAsync(context, request, cancellationToken);
        Assert.NotNull(reservedData);
        Assert.Equal(WhZStockMovement.AddReserved, reservedData.Movement);
        Assert.Equal(request.Qty.GetValueOrDefault(), (reservedData?.Qty).GetValueOrDefault(), Precision);

        return reservedData;
    }

    protected void Delete(IWhZStockContext context, IWhZStockData request)
    {
        this.service.Delete(context, request);
    }

    protected async Task<IWhZStockData?> CommitReceivingAsync(IWhZStockContext context, WhZStockDto request, CancellationToken cancellationToken = default)
    {
        var commitData = await this.service.CommitReceivingAsync(context, request, cancellationToken);
        Assert.NotNull(commitData);
        Assert.Equal(WhZStockMovement.CommitReceving, commitData.Movement);
        Assert.Equal(request.Qty.GetValueOrDefault(), (commitData?.Qty).GetValueOrDefault(), Precision);

        return commitData;
    }

    protected async Task<IWhZStockData?> CommitReservedAsync(IWhZStockContext context, WhZStockDto request, CancellationToken cancellationToken = default)
    {
        var commitData = await this.service.CommitReservedAsync(context, request, cancellationToken);
        Assert.NotNull(commitData);
        Assert.Equal(WhZStockMovement.CommitReserved, commitData.Movement);
        Assert.Equal(request.Qty.GetValueOrDefault(), (commitData?.Qty).GetValueOrDefault(), Precision);

        return commitData;
    }

    protected async Task<IWhZStockData?> RemoveReceivingAsync(IWhZStockContext context, WhZStockDto request, CancellationToken cancellationToken = default)
    {
        var reservedData = await this.service.RemoveReceivingAsync(context, request, cancellationToken);
        Assert.NotNull(reservedData);
        Assert.Equal(WhZStockMovement.RemoveReceiving, reservedData.Movement);
        Assert.Equal(request.Qty.GetValueOrDefault(), (reservedData?.Qty).GetValueOrDefault(), Precision);

        return reservedData;
    }

    protected async Task<IWhZStockData?> RemoveReservedAsync(IWhZStockContext context, WhZStockDto request, CancellationToken cancellationToken = default)
    {
        var reservedData = await this.service.RemoveReservedAsync(context, request, cancellationToken);
        Assert.NotNull(reservedData);
        Assert.Equal(WhZStockMovement.RemoveReserved, reservedData.Movement);
        Assert.Equal(request.Qty.GetValueOrDefault(), (reservedData?.Qty).GetValueOrDefault(), Precision);

        return reservedData;
    }
}
