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
using eLog.HeavyTools.Services.WhZone.Test.Base;
using eLog.HeavyTools.Services.WhZone.Test.Fixtures;
using Xunit;

namespace eLog.HeavyTools.Services.WhZone.Test.Services.Base;

public abstract class WhZStockMapServiceTestBase : TestBase<OlcWhzstockmap, IWhZStockMapService>
{
    internal const int Precision = 6;

    protected WhZStockMapServiceTestBase(
        ITestOutputHelper testOutputHelper,
        TestFixture fixture) : base(testOutputHelper, fixture)
    {
    }

    protected async Task StoreAsync(IWhZStockMapContext context, CancellationToken cancellationToken = default)
    {
        await this.service.StoreAsync(context, cancellationToken);
    }

    protected async Task<IWhZStockMapData?> AddReceivingAsync(IWhZStockMapContext context, WhZStockMapDto request, CancellationToken cancellationToken = default)
    {
        var receivingData = await this.service.AddReceivingAsync(context, request, cancellationToken);
        Assert.NotNull(receivingData);
        Assert.Equal(WhZStockMovement.AddReceving, receivingData.Movement);
        Assert.Equal(request.Qty.GetValueOrDefault(), (receivingData?.Qty).GetValueOrDefault(), Precision);

        return receivingData;
    }

    protected async Task<IWhZStockMapData?> AddReservedAsync(IWhZStockMapContext context, WhZStockMapDto request, CancellationToken cancellationToken = default)
    {
        var reservedData = await this.service.AddReservedAsync(context, request, cancellationToken);
        Assert.NotNull(reservedData);
        Assert.Equal(WhZStockMovement.AddReserved, reservedData.Movement);
        Assert.Equal(request.Qty.GetValueOrDefault(), (reservedData?.Qty).GetValueOrDefault(), Precision);

        return reservedData;
    }

    protected void Delete(IWhZStockMapContext context, IWhZStockMapData request)
    {
        this.service.Delete(context, request);
    }

    protected async Task<IWhZStockMapData?> CommitReceivingAsync(IWhZStockMapContext context, WhZStockMapDto request, CancellationToken cancellationToken = default)
    {
        var commitData = await this.service.CommitReceivingAsync(context, request, cancellationToken);
        Assert.NotNull(commitData);
        Assert.Equal(WhZStockMovement.CommitReceving, commitData.Movement);
        Assert.Equal(request.Qty.GetValueOrDefault(), (commitData?.Qty).GetValueOrDefault(), Precision);

        return commitData;
    }

    protected async Task<IWhZStockMapData?> CommitReservedAsync(IWhZStockMapContext context, WhZStockMapDto request, CancellationToken cancellationToken = default)
    {
        var commitData = await this.service.CommitReservedAsync(context, request, cancellationToken);
        Assert.NotNull(commitData);
        Assert.Equal(WhZStockMovement.CommitReserved, commitData.Movement);
        Assert.Equal(request.Qty.GetValueOrDefault(), (commitData?.Qty).GetValueOrDefault(), Precision);

        return commitData;
    }

    protected async Task<IWhZStockMapData?> RemoveReceivingAsync(IWhZStockMapContext context, WhZStockMapDto request, CancellationToken cancellationToken = default)
    {
        var reservedData = await this.service.RemoveReceivingAsync(context, request, cancellationToken);
        Assert.NotNull(reservedData);
        Assert.Equal(WhZStockMovement.RemoveReceiving, reservedData.Movement);
        Assert.Equal(request.Qty.GetValueOrDefault(), (reservedData?.Qty).GetValueOrDefault(), Precision);

        return reservedData;
    }

    protected async Task<IWhZStockMapData?> RemoveReservedAsync(IWhZStockMapContext context, WhZStockMapDto request, CancellationToken cancellationToken = default)
    {
        var reservedData = await this.service.RemoveReservedAsync(context, request, cancellationToken);
        Assert.NotNull(reservedData);
        Assert.Equal(WhZStockMovement.RemoveReserved, reservedData.Movement);
        Assert.Equal(request.Qty.GetValueOrDefault(), (reservedData?.Qty).GetValueOrDefault(), Precision);

        return reservedData;
    }
}
