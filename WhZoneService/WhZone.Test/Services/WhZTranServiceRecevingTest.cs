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

public class WhZTranServiceRecevingTest : Base.WhZTranServiceTestBase
{
    private readonly int cmpId;
    private readonly OlsSthead stHead;
    private readonly int whZoneId;

    public WhZTranServiceRecevingTest(
        ITestOutputHelper testOutputHelper,
        TestFixture fixture) : base(testOutputHelper, fixture)
    {
        this.cmpId = this.GetCmpIdAsync("HUPS").GetAwaiter().GetResult() ?? throw new InvalidOperationException();

        this.stHead = this.GetFirstStHeadAsync(this.cmpId).GetAwaiter().GetResult() ?? throw new InvalidOperationException();
        this.whZoneId = this.GetFirstWhZoneIdAsync(this.stHead.Towhid!, false).GetAwaiter().GetResult() ?? throw new InvalidOperationException();
    }

    protected async Task<OlsSthead?> GetFirstStHeadAsync(int cmpId, CancellationToken cancellationToken = default)
    {
        var stHead = await this.dbContext.OlsStheads
            .Where(sth => sth.Cmpid == cmpId)
            .OrderBy(i => i.Stid)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (stHead is not null)
        {
            var entry = this.dbContext.Entry(stHead);
            entry.State = EntityState.Detached;
        }

        return stHead;
    }

    protected async Task<OlcWhztranhead?> GetCurrentEntryAsync(int whztid, CancellationToken cancellationToken = default)
    {
        return await this.dbContext.OlcWhztranheads
            .Where(h => h.Whztid == whztid)
            .FirstOrDefaultAsync(cancellationToken);
    }

    [Fact]
    public async Task<int> AddTranHeadTestAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var tran = await this.unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var request = new BusinessEntities.Dto.WhZReceivingTranHeadDto
            {
                Cmpid = this.cmpId,
                Whztdate = this.stHead.Stdate,
                Stid = this.stHead.Stid,
                Towhzid = this.whZoneId,
            };

            var tranHead = await this.service.AddReceivingAsync(request, ct);
            Assert.NotNull(tranHead?.Whztid);
            Assert.NotEqual(0, tranHead!.Whztid);
            Assert.Equal(this.cmpId, tranHead.Cmpid);
            Assert.Equal(this.stHead.Stdate, tranHead.Whztdate);
            Assert.Equal(this.stHead.Stid, tranHead.Stid);
            Assert.Equal(this.whZoneId, tranHead.Towhzid);

            var currentTranHead = await this.GetCurrentEntryAsync(tranHead.Whztid.Value, ct);
            Assert.NotNull(currentTranHead);

            return currentTranHead.Whztid;
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Fact]
    public async Task UpdateTranHeadTestAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var tran = await this.unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var whztid = await this.AddTranHeadTestAsync();

            var stHead = this.stHead.Clone<OlsSthead>();
            stHead.Stdate = DateTime.Today.AddMonths(3);
            var entry = this.dbContext.OlsStheads.Update(stHead);
            await this.unitOfWork.SaveChangesAsync(ct);

            entry.State = EntityState.Detached;

            var request = new BusinessEntities.Dto.WhZReceivingTranHeadDto
            {
                Whztid = whztid,
                Cmpid = this.cmpId,
                Whztdate = stHead.Stdate,
                Stid = stHead.Stid,
                Towhzid = this.whZoneId,
            };

            var tranHead = await this.service.UpdateReceivingAsync(request, ct);
            Assert.NotNull(tranHead?.Whztid);
            Assert.NotEqual(0, tranHead!.Whztid);
            Assert.Equal(this.cmpId, tranHead.Cmpid);
            Assert.Equal(stHead.Stdate, tranHead.Whztdate);
            Assert.Equal(stHead.Stid, tranHead.Stid);
            Assert.Equal(this.whZoneId, tranHead.Towhzid);

            var currentTranHead = await this.GetCurrentEntryAsync(tranHead.Whztid.Value, ct);
            Assert.NotNull(currentTranHead);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Fact]
    public async Task QueryTranHeadListTestAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var tran = await this.unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var whztid = await this.AddTranHeadTestAsync();

            var list = await this.service.QueryReceivingAsync(cancellationToken: ct);
            Assert.NotNull(list);
            Assert.NotEmpty(list);
        }
        finally
        {
            tran.Rollback();
        }
    }
}
