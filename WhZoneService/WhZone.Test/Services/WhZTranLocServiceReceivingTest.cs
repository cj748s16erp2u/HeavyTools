using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Enums;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhZone.DataAccess.Repositories.Interfaces;
using eLog.HeavyTools.Services.WhZone.Test.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Xunit;

namespace eLog.HeavyTools.Services.WhZone.Test.Services;

public class WhZTranLocServiceReceivingTest : Base.WhZTranLocServiceTestBase
{
    private readonly IWhZStockMapService whZStockMapService;
    private readonly IWhZTranLineService tranLineService;
    private readonly IRepository<OlsStline> stLineRepository;

    public WhZTranLocServiceReceivingTest(
        ITestOutputHelper testOutputHelper,
        TestFixture fixture) : base(testOutputHelper, fixture)
    {
        this.whZStockMapService = this._fixture.GetService<IWhZStockMapService>(this._testOutputHelper) ?? throw new InvalidOperationException($"{typeof(IWhZStockMapService).Name} is not found.");
        this.tranLineService = this._fixture.GetService<IWhZTranLineService>(this._testOutputHelper) ?? throw new InvalidOperationException($"{typeof(IWhZTranLineService).Name} is not found.");
        this.stLineRepository = this._fixture.GetService<IRepository<OlsStline>>(this._testOutputHelper) ?? throw new InvalidOperationException($"{typeof(IRepository<OlsStline>).Name} is not found.");
    }

    protected async Task<OlcWhztranhead?> GetFirstWhZTranHeadAsync(int cmpId, bool hasStockTran = true, CancellationToken cancellationToken = default)
    {
        var query = this.dbContext.OlcWhztranheads
            .Where(h => h.Cmpid == cmpId && h.Whzttype == (int)WhZTranHead_Whzttype.Receiving && h.Whztstat < (int)WhZTranHead_Whztstat.Closed);

        if (hasStockTran)
        {
            query = query.Where(h => h.Stid != null);
        }
        else
        {
            query = query.Where(h => h.Stid == null);
        }

        var whzTranHead = await query
            .OrderBy(h => h.Whztid)
            .FirstOrDefaultAsync(cancellationToken);

        if (whzTranHead is not null)
        {
            var entry = this.dbContext.Entry(whzTranHead);
            entry.State = EntityState.Detached;
        }

        return whzTranHead;
    }

    protected async Task<OlsStline?> GetFirstStLineAsync(int stId, CancellationToken cancellationToken = default)
    {
        var stLine = await this.dbContext.OlsStlines
            .Where(l => l.Stid == stId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (stLine is not null)
        {
            var entry = this.dbContext.Entry(stLine);
            entry.State = EntityState.Deleted;
        }

        return stLine;
    }

    protected async Task<OlcWhztranloc?> GetCurrentEntryAsync(int whztlocid, CancellationToken cancellationToken = default)
    {
        var entity = await this.dbContext.OlcWhztranlocs
            .Where(l => l.Whztlocid == whztlocid)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity is not null)
        {
            var entry = this.dbContext.Entry(entity);
            if (entry is not null)
            {
                entry.State = EntityState.Detached;
            }
        }

        return entity;
    }

    protected async Task RemoveExistingTranLinesAsync(int whztid, CancellationToken cancellationToken = default)
    {
        // a duplikált rögzítés elleni védelem miatt, törölni szükséges a meglévő bejegyzéseket
        var existingWhztlines = await this.dbContext.OlcWhztranlines
            .Where(l => l.Whztid == whztid)
            .ToListAsync(cancellationToken);
        foreach (var l in existingWhztlines)
        {
            if (l is not null)
            {
                var request = new WhZTranLineDeleteDto
                {
                    Whztlineid = l.Whztlineid,
                    DeleteLoc = true,
                };
                await this.tranLineService.DeleteAsync(request, cancellationToken);
            }
        }

        foreach (var l in existingWhztlines)
        {
            if (l is not null)
            {
                var entry = this.dbContext.Entry(l);
                if (entry is not null)
                {
                    entry.State = EntityState.Detached;
                }
            }
        }
    }

    protected async Task RemoveExistingTranLocsAsync(int whztlineid, CancellationToken cancellationToken = default)
    {
        // a duplikált rögzítés elleni védelem miatt, törölni szükséges a meglévő bejegyzéseket
        var existingWhztlocs = await this.dbContext.OlcWhztranlocs
            .Where(l => l.Whztlineid == whztlineid)
            .ToListAsync(cancellationToken);
        foreach (var l in existingWhztlocs)
        {
            if (l is not null)
            {
                this.dbContext.OlcWhztranlocs.Remove(l);
            }
        }

        await this.unitOfWork.SaveChangesAsync(cancellationToken);

        foreach (var l in existingWhztlocs)
        {
            if (l is not null)
            {
                var entry = this.dbContext.Entry(l);
                if (entry is not null)
                {
                    entry.State = EntityState.Detached;
                }
            }
        }
    }

    protected async Task<(OlcWhztranhead tranHead, WhZTranLineDto tranLine, WhZTranLocDto tranLoc)> AddTranLocAsync(int cmpId, bool hasStockTran, CancellationToken cancellationToken = default)
    {
        var context = this.whZStockMapService.CreateContext();

        var tranHead = await this.GetFirstWhZTranHeadAsync(cmpId, hasStockTran, cancellationToken);
        Assert.NotNull(tranHead);
        Assert.NotNull(tranHead.Stid);

        var tranLine = await this.AddTranLineAsync(tranHead, cancellationToken);
        Assert.NotNull(tranLine);

        var tranLoc = await this.AddTranLocAsync(tranHead, tranLine, context, cancellationToken);
        Assert.NotNull(tranLoc);

        await this.whZStockMapService.StoreAsync(context, cancellationToken);

        return (tranHead, tranLine, tranLoc);
    }

    protected async Task<WhZTranLineDto?> AddTranLineAsync(OlcWhztranhead tranHead, CancellationToken cancellationToken = default)
    {
        if (tranHead is null)
        {
            throw new ArgumentNullException(nameof(tranHead));
        }

        await this.RemoveExistingTranLinesAsync(tranHead.Whztid, cancellationToken);

        var stLine = await this.GetFirstStLineAsync(tranHead.Stid!.Value, cancellationToken);
        Assert.NotNull(stLine);

        var request = new WhZReceivingTranLineDto
        {
            Whztid = tranHead!.Whztid,
            Stlineid = stLine.Stlineid,
        };

        return await this.tranLineService.AddReceivingAsync(request, cancellationToken);
    }

    protected async Task<WhZTranLocDto?> AddTranLocAsync(OlcWhztranhead tranHead, WhZTranLineDto tranLine, BusinessLogic.Containers.Interfaces.IWhZStockMapContext context, CancellationToken cancellationToken = default)
    {
        if (tranHead is null)
        {
            throw new ArgumentNullException(nameof(tranHead));
        }

        if (tranLine is null)
        {
            throw new ArgumentNullException(nameof(tranLine));
        }

        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var olcTranLine = await this.dbContext.OlcWhztranlines.FirstOrDefaultAsync(l => l.Whztlineid == tranLine.Whztlineid, cancellationToken);
        Assert.NotNull(olcTranLine);

        return await this.service.AddReceivingDefaultIfNotExistsAsync(tranHead, olcTranLine, context, cancellationToken);
    }

    [Fact]
    public async Task<int> AddTranLocTestAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var tran = await this.unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var (tranHead, tranLine, tranLoc) = await this.AddTranLocAsync(3, true, ct);
            Assert.NotNull(tranHead);
            var stHead = await this.dbContext.OlsStheads.FirstOrDefaultAsync(h => h.Stid == tranHead.Stid, ct);
            Assert.NotNull(stHead);

            Assert.NotNull(tranLine);
            var olcTranLine = await this.dbContext.OlcWhztranlines.FirstOrDefaultAsync(l => l.Whztlineid == tranLine.Whztlineid, ct);
            Assert.NotNull(olcTranLine);

            Assert.NotNull(tranLoc?.Whztlocid);
            Assert.NotEqual(0, tranLoc!.Whztlocid);
            Assert.Equal(tranLine.Whztid, tranLoc.Whztid);
            Assert.Equal(tranLine.Whztlineid, tranLoc.Whztlineid);
            Assert.Equal(stHead.Towhid, tranLoc.Whid);
            Assert.Equal(tranHead.Towhzid, tranLoc.Whzoneid);
            Assert.NotEqual(0, tranLoc.Whlocid);
            Assert.Equal(tranHead.Whzttype, tranLoc.Whztltype);
            Assert.Equal(olcTranLine.Ordqty, tranLoc.Ordqty);
            Assert.Equal(olcTranLine.Dispqty, tranLoc.Dispqty);
            Assert.Equal(olcTranLine.Movqty, tranLoc.Movqty);

            var currentTranLoc = await this.GetCurrentEntryAsync(tranLoc.Whztlocid.Value, ct);
            Assert.NotNull(currentTranLoc);
            Assert.Equal(olcTranLine.Ordqty, currentTranLoc.Ordqty);
            Assert.Equal(olcTranLine.Dispqty, currentTranLoc.Dispqty);
            Assert.Equal(olcTranLine.Movqty, currentTranLoc.Movqty);

            return currentTranLoc.Whztlineid;
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Fact]
    public async Task UpdateTranLocTest01Async()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        OlsStline? stLine = null;

        using var tran = await this.unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var tranHead = await this.GetFirstWhZTranHeadAsync(3, true, ct);
            Assert.NotNull(tranHead);
            Assert.NotNull(tranHead.Stid);

            await this.RemoveExistingTranLinesAsync(tranHead.Whztid, ct);

            var whztlocid = await this.AddTranLocTestAsync();
            var whzTranLoc = await this.service.GetByIdAsync(new object[] { whztlocid }, ct);
            Assert.NotNull(whzTranLoc);

            var whzTranLine = await this.tranLineService.GetByIdAsync(new object[] { whzTranLoc.Whztlineid }, ct);
            Assert.NotNull(whzTranLine);

            stLine = await this.dbContext.OlsStlines
                .Where(l => l.Stlineid == whzTranLine.Stlineid)
                .FirstOrDefaultAsync(ct);
            Assert.NotNull(stLine);
            stLine.Ordqty += 3;
            stLine.Ordqty2 += 3;
            var entry = this.dbContext.OlsStlines.Update(stLine);
            await this.unitOfWork.SaveChangesAsync(ct);

            entry.State = EntityState.Detached;

            var request = new WhZReceivingTranLineDto
            {
                Whztlineid = whzTranLine.Whztlineid,
                Whztid = tranHead!.Whztid,
                Stlineid = stLine.Stlineid,
            };

            var tranLine = await this.tranLineService.UpdateReceivingAsync(request, ct);
            Assert.NotNull(tranLine?.Whztlineid);
            Assert.NotEqual(0, tranLine!.Whztlineid);

            var context = this.whZStockMapService.CreateContext();

            var tranLocs = await this.tranLineService.GenerateReceivingLocAsync(tranHead, context, ct);
            Assert.NotNull(tranLocs);
            Assert.NotEmpty(tranLocs);

            await this.whZStockMapService.StoreAsync(context, ct);

            ...
        }
        finally
        {
            tran.Rollback();

            if (stLine is not null)
            {
                await this.stLineRepository.ReloadAsync(stLine, ct);
            }
        }
    }
}
