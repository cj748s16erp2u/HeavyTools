using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Enums;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.DataAccess.Repositories.Interfaces;
using eLog.HeavyTools.Services.WhZone.Test.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace eLog.HeavyTools.Services.WhZone.Test.Services;

public class WhZTranServiceRecevingTest : Base.WhZTranServiceTestBase
{
    private readonly IRepository<OlsSthead> stHeadRepository;
    private readonly int cmpId;
    private readonly int whZoneId;

    private OlsSthead stHead;

    public WhZTranServiceRecevingTest(
        ITestOutputHelper testOutputHelper,
        TestFixture fixture) : base(testOutputHelper, fixture)
    {
        this.stHeadRepository =this._fixture.GetService<IRepository<OlsSthead>>(this._testOutputHelper) ?? throw new InvalidOperationException($"{typeof(IRepository<OlsSthead>).Name} is not found.");

        this.cmpId = this.GetCmpIdAsync("HUPS").GetAwaiter().GetResult() ?? throw new InvalidOperationException();

        this.stHead = this.GetFirstStHeadAsync(this.cmpId).GetAwaiter().GetResult() ?? throw new InvalidOperationException();
        this.whZoneId = this.GetFirstWhZoneIdAsync(this.stHead.Towhid!, false).GetAwaiter().GetResult() ?? throw new InvalidOperationException();
    }

    protected async Task<OlsSthead?> GetFirstStHeadAsync(int cmpId, CancellationToken cancellationToken = default)
    {
        var stHead = await this.dbContext.OlsStheads
            .Where(sth => sth.Sttype == 1 && sth.Cmpid == cmpId && sth.Ststat == 10)
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
        var tranHead = await this.dbContext.OlcWhztranheads
            .Where(h => h.Whztid == whztid)
            .FirstOrDefaultAsync(cancellationToken);

        if (tranHead is not null)
        {
            var entry = this.dbContext.Entry(tranHead);
            entry.State = EntityState.Detached;
        }

        return tranHead;
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
                AuthUser = "dev",
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

#pragma warning disable CS8601 // Possible null reference assignment.
            this.stHead = await this.stHeadRepository.ReloadAsync(this.stHead, ct);
#pragma warning restore CS8601 // Possible null reference assignment.
            Assert.NotNull(this.stHead);

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
            stHead.ClearNavigationProperties();
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
                AuthUser = "dev",
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

    protected async Task<int> PrepareToStatChangeAsync(CancellationToken cancellationToken = default)
    {
        var tranLineTest = new WhZTranLineServiceReceivingTest(this._testOutputHelper, this._fixture);

        using var tran = await this.unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var whztid = await this.AddTranHeadTestAsync();
            var tranHead = await this.GetCurrentEntryAsync(whztid, cancellationToken);
            Assert.NotNull(tranHead);

            await tranLineTest.AddTranLineAsync(tranHead, cancellationToken);

            return whztid;
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Fact]
    public async Task<int> StatChange_Creating2CreatedAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var tran = await this.unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var whztid = await this.PrepareToStatChangeAsync(ct);
            var request = new BusinessEntities.Dto.WhZTranHeadStatChangeDto
            {
                Whztid = whztid,
                NewStat = WhZTranHead_Whztstat.Created,
                AuthUser = "dev",
            };
            var result = await this.service.StatChangeAsync(request, ct);
            Assert.Equal(0, result.Result);
            Assert.Null(result.Message);

            var tranHead = await this.GetCurrentEntryAsync(whztid, ct);
            Assert.NotNull(tranHead);
            Assert.Equal((int)WhZTranHead_Whztstat.Created, tranHead.Whztstat);

            return whztid;
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Fact]
    public async Task StatChange_Created2CreatingAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var tran = await this.unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var whztid = await this.StatChange_Creating2CreatedAsync();
            var request = new BusinessEntities.Dto.WhZTranHeadStatChangeDto
            {
                Whztid = whztid,
                NewStat = WhZTranHead_Whztstat.Creating,
                AuthUser = "dev",
            };
            var result = await this.service.StatChangeAsync(request, ct);
            Assert.Equal(0, result.Result);
            Assert.Null(result.Message);

            var tranHead = await this.GetCurrentEntryAsync(whztid, ct);
            Assert.NotNull(tranHead);
            Assert.Equal((int)WhZTranHead_Whztstat.Creating, tranHead.Whztstat);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Fact]
    public async Task StatChange_CloseFailed01Async()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var tran = await this.unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var whztid = await this.PrepareToStatChangeAsync(ct);
            var request = new BusinessEntities.Dto.WhZTranHeadStatChangeDto
            {
                Whztid = whztid,
                NewStat = WhZTranHead_Whztstat.Closed,
                AuthUser = "dev",
            };
            var result = await this.service.StatChangeAsync(request, ct);
            Assert.Equal(-1, result.Result);
            Assert.Contains("Unable to change status from 'F' to 'L'", result.Message);

            var tranHead = await this.GetCurrentEntryAsync(whztid, ct);
            Assert.NotNull(tranHead);
            Assert.Equal((int)WhZTranHead_Whztstat.Creating, tranHead.Whztstat);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Fact]
    public async Task StatChange_CloseFailed02Async()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var tran = await this.unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var whztid = await this.PrepareToStatChangeAsync(ct);
            var request = new BusinessEntities.Dto.WhZTranHeadCloseDto
            {
                Whztid = whztid,
                AuthUser = "dev",
            };
            var result = await this.service.CloseAsync(request, ct);
            Assert.Equal(-1, result.Result);
            Assert.Contains("Unable to close", result.Message);

            var tranHead = await this.GetCurrentEntryAsync(whztid, ct);
            Assert.NotNull(tranHead);
            Assert.Equal((int)WhZTranHead_Whztstat.Creating, tranHead.Whztstat);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Fact]
    public async Task StatChange_Close01Async()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var tran = await this.unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var whztid = await this.StatChange_Creating2CreatedAsync();
            var request = new BusinessEntities.Dto.WhZTranHeadStatChangeDto
            {
                Whztid = whztid,
                NewStat = WhZTranHead_Whztstat.Closed,
                AuthUser = "dev",
            };
            var result = await this.service.StatChangeAsync(request, ct);
            Assert.Equal(0, result.Result);
            Assert.Null(result.Message);

            var tranHead = await this.GetCurrentEntryAsync(whztid, ct);
            Assert.NotNull(tranHead);
            Assert.Equal((int)WhZTranHead_Whztstat.Closed, tranHead.Whztstat);
        }
        finally
        {
            tran.Rollback();
        }
    }

    [Fact]
    public async Task StatChange_Close02Async()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TestFixture.TimeoutSeconds);
        var ct = cts.Token;

        using var tran = await this.unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var whztid = await this.StatChange_Creating2CreatedAsync();
            var request = new BusinessEntities.Dto.WhZTranHeadCloseDto
            {
                Whztid = whztid,
                AuthUser = "dev",
            };
            var result = await this.service.CloseAsync(request, ct);
            Assert.Equal(0, result.Result);
            Assert.Null(result.Message);

            var tranHead = await this.GetCurrentEntryAsync(whztid, ct);
            Assert.NotNull(tranHead);
            Assert.Equal((int)WhZTranHead_Whztstat.Closed, tranHead.Whztstat);
        }
        finally
        {
            tran.Rollback();
        }
    }
}
