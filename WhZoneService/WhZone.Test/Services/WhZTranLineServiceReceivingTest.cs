using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.Test.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace eLog.HeavyTools.Services.WhZone.Test.Services;

public class WhZTranLineServiceReceivingTest : Base.WhZTranLineServiceTestBase
{
    public WhZTranLineServiceReceivingTest(
        ITestOutputHelper testOutputHelper,
        TestFixture fixture) : base(testOutputHelper, fixture)
    {
    }

    protected async Task<OlcWhztranhead?> GetFirstWhZTranHeadAsync(int cmpId, bool hasStockTran = true, CancellationToken cancellationToken = default)
    {
        var query = this.dbContext.OlcWhztranheads
            .Where(h => h.Cmpid == cmpId);

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

    protected async Task<OlsSthead?> GetStHeadAsync(int stId, CancellationToken cancellationToken = default)
    {
        var stHead = await this.dbContext.OlsStheads
            .Where(h => h.Stid == stId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (stHead is not null)
        {
            var entry = this.dbContext.Entry(stHead);
            entry.State = EntityState.Deleted;
        }

        return stHead;
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

    protected async Task<OlcWhztranline?> GetCurrentEntryAsync(int whztlineid, CancellationToken cancellationToken = default)
    {
        var entity = await this.dbContext.OlcWhztranlines
            .Where(l => l.Whztlineid == whztlineid)
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
                this.dbContext.OlcWhztranlines.Remove(l!);
            }
        }

        await this.unitOfWork.SaveChangesAsync(cancellationToken);

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

    protected async Task<(OlsStline stLine, BusinessEntities.Dto.WhZReceivingTranLineDto tranLine)> AddTranLineAsync(int cmpId, bool hasStockTran, CancellationToken cancellationToken = default)
    {
        var tranHead = await this.GetFirstWhZTranHeadAsync(cmpId, hasStockTran, cancellationToken);
        Assert.NotNull(tranHead);
        Assert.NotNull(tranHead.Stid);

        return await this.AddTranLineAsync(tranHead, cancellationToken);
    }

    public async Task<(OlsStline stLine, BusinessEntities.Dto.WhZReceivingTranLineDto tranLine)> AddTranLineAsync(OlcWhztranhead tranHead, CancellationToken cancellationToken = default)
    {
        if (tranHead is null)
        {
            throw new ArgumentNullException(nameof(tranHead));
        }

        await this.RemoveExistingTranLinesAsync(tranHead.Whztid, cancellationToken);

        var stLine = await this.GetFirstStLineAsync(tranHead.Stid!.Value, cancellationToken);
        Assert.NotNull(stLine);

        var request = new BusinessEntities.Dto.WhZReceivingTranLineDto
        {
            Whztid = tranHead!.Whztid,
            Stlineid = stLine.Stlineid,
        };

        var tranLine = await this.service.AddReceivingAsync(request, cancellationToken);
        return (stLine, tranLine);
    }

    [Fact]
    public async Task<int> AddTranLineTestAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var tran = await this.unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var (stLine, tranLine) = await this.AddTranLineAsync(3, true, ct);
            Assert.NotNull(tranLine?.Whztlineid);
            Assert.NotEqual(0, tranLine!.Whztlineid);
            Assert.Equal(stLine.Linenum, tranLine.Linenum);
            Assert.Equal(stLine.Itemid, tranLine.Itemid);
            Assert.Equal(stLine.Unitid2, tranLine.Unitid2);
            Assert.Equal(stLine.Change, tranLine.Change);
            Assert.Equal(stLine.Dispqty2, tranLine.Dispqty2);
            Assert.Equal(stLine.Movqty2, tranLine.Movqty2);

            var currentTranLine = await this.GetCurrentEntryAsync(tranLine.Whztlineid.Value, ct);
            Assert.NotNull(currentTranLine);
            Assert.Equal(stLine.Ordqty, currentTranLine.Ordqty);
            Assert.Equal(stLine.Dispqty, currentTranLine.Dispqty);
            Assert.Equal(stLine.Movqty, currentTranLine.Movqty);
            Assert.Equal(stLine.Inqty, currentTranLine.Inqty);
            Assert.Equal(stLine.Outqty, currentTranLine.Outqty);

            return currentTranLine.Whztlineid;
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Fact]
    public async Task UpdateTranLineTestAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var tran = await this.unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var tranHead = await this.GetFirstWhZTranHeadAsync(3, true, ct);
            Assert.NotNull(tranHead);
            Assert.NotNull(tranHead.Stid);

            await this.RemoveExistingTranLinesAsync(tranHead.Whztid, ct);

            var whztlineid = await this.AddTranLineTestAsync();
            var whzTranLine = await this.service.GetByIdAsync(new object[] { whztlineid }, ct);
            Assert.NotNull(whzTranLine);

            var stLine = await this.dbContext.OlsStlines
                .Where(l => l.Stlineid == whzTranLine.Stlineid)
                .FirstOrDefaultAsync(ct);
            Assert.NotNull(stLine);
            stLine.Ordqty += 3;
            stLine.Ordqty2 += 3;
            var entry = this.dbContext.OlsStlines.Update(stLine);
            await this.unitOfWork.SaveChangesAsync(ct);

            entry.State = EntityState.Detached;

            var request = new BusinessEntities.Dto.WhZReceivingTranLineDto
            {
                Whztlineid = whztlineid,
                Whztid = tranHead!.Whztid,
                Stlineid = stLine.Stlineid,
            };

            var tranLine = await this.service.UpdateReceivingAsync(request, ct);
            Assert.NotNull(tranLine?.Whztlineid);
            Assert.NotEqual(0, tranLine!.Whztlineid);
            Assert.Equal(stLine.Linenum, tranLine.Linenum);
            Assert.Equal(stLine.Itemid, tranLine.Itemid);
            Assert.Equal(stLine.Unitid2, tranLine.Unitid2);
            Assert.Equal(stLine.Change, tranLine.Change);
            Assert.Equal(stLine.Dispqty2, tranLine.Dispqty2);
            Assert.Equal(stLine.Movqty2, tranLine.Movqty2);

            var currentTranLine = await this.GetCurrentEntryAsync(tranLine.Whztlineid.Value, ct);
            Assert.NotNull(currentTranLine);
            Assert.Equal(stLine.Ordqty, currentTranLine.Ordqty);
            Assert.Equal(stLine.Dispqty, currentTranLine.Dispqty);
            Assert.Equal(stLine.Movqty, currentTranLine.Movqty);
            Assert.Equal(stLine.Inqty, currentTranLine.Inqty);
            Assert.Equal(stLine.Outqty, currentTranLine.Outqty);
        }
        finally
        {
            tran.Rollback();
        }
    }
}
