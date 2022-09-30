using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        return await this.dbContext.OlcWhztranlines
            .Where(l => l.Whztlineid == whztlineid)
            .FirstOrDefaultAsync(cancellationToken);
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
            var tranHead = await this.GetFirstWhZTranHeadAsync(3, true, ct);
            Assert.NotNull(tranHead);
            Assert.NotNull(tranHead.Stid);

            var stLine = await this.GetFirstStLineAsync(tranHead.Stid.Value, ct);
            Assert.NotNull(stLine);

            var request = new BusinessEntities.Dto.WhZReceivingTranLineDto
            {
                Whztid = tranHead!.Whztid,
                Stlineid = stLine.Stlineid,
            };

            var tranLine = await this.service.AddReceivingAsync(request, ct);
            Assert.NotNull(tranLine?.Whztlineid);
            Assert.NotEqual(0, tranLine!.Whztlineid);
            Assert.Equal(stLine.Linenum, tranLine.Linenum);
            Assert.Equal(stLine.Itemid, tranLine.Itemid);
            Assert.Equal(stLine.Dispqty, tranLine.Dispqty);
            Assert.Equal(stLine.Movqty, tranLine.Movqty);
            Assert.Equal(stLine.Inqty, tranLine.Inqty);
            Assert.Equal(stLine.Outqty, tranLine.Outqty);
            Assert.Equal(stLine.Unitid2, tranLine.Unitid2);
            Assert.Equal(stLine.Change, tranLine.Change);
            Assert.Equal(stLine.Dispqty2, tranLine.Dispqty2);
            Assert.Equal(stLine.Movqty2, tranLine.Movqty2);

            var currentTranLine = await this.GetCurrentEntryAsync(tranLine.Whztlineid.Value, ct);
            Assert.NotNull(currentTranLine);

            return currentTranLine.Whztlineid;
        }
        finally
        {
            tran.Rollback();
        }
    }
}
