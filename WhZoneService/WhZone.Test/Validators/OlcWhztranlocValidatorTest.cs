using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Enums;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhZone.Test.Base;
using eLog.HeavyTools.Services.WhZone.Test.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace eLog.HeavyTools.Services.WhZone.Test.Validators;

public class OlcWhztranlocValidatorTest : TestBase<OlcWhztranloc, IWhZTranLocService>
{
    public OlcWhztranlocValidatorTest(
        ITestOutputHelper testOutputHelper,
        TestFixture fixture) : base(testOutputHelper, fixture)
    {
    }

    protected async Task<OlcWhztranhead> GetFirstTranHeadAsync(WhZTranHead_Whzttype whzttype, bool checkStHeadExists = false, CancellationToken cancellationToken = default)
    {
        var query = this.dbContext.OlcWhztranheads
            .Where(h => h.Whzttype == (int)whzttype)
            .Where(h => h.St!.OlsStlines.Any());

        if (checkStHeadExists)
        {
            query = query.Where(h => h.Stid != null);
        }

        return await query
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    protected async Task<OlcWhztranline> GetFirstTranLineAsync(int whztid, CancellationToken cancellationToken = default)
    {
        return await this.dbContext.OlcWhztranlines
            .Where(h => h.Whztid == whztid)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    protected async Task<OlcWhztranline> GetOtherTranLineAsync(int whztid, CancellationToken cancellationToken = default)
    {
        return await this.dbContext.OlcWhztranlines
            .Where(h => h.Whztid != whztid)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    protected async Task<OlsSthead?> GetStHeadAsync(int stid, CancellationToken cancellationToken = default)
    {
        return await this.dbContext.OlsStheads
            .Where(h => h.Stid == stid)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    protected async Task<OlcWhlocation?> GetFirstLocationAsync(string whid, int whzoneid, CancellationToken cancellationToken = default)
    {
        return await this.dbContext.OlcWhlocations
            .Where(h => h.Whid == whid && h.Whzoneid == whzoneid)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    protected async Task<OlcWhlocation?> GetFirstLocationNotForAsync(string whid, int whzoneid, CancellationToken cancellationToken = default)
    {
        return await this.dbContext.OlcWhlocations
            .Where(h => h.Whid != whid || h.Whzoneid != whzoneid)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    #region Add...

    [Fact]
    public async Task AddWrongTranLocTest()
    {
        var entity = new OlcWhztranloc
        {
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        Assert.NotNull(ex?.Message);
        Assert.NotEqual(ex?.Message, string.Empty);
    }

    [Fact]
    public Task AddWrongTranLocWhztid01Test()
    {
        return this.AddWrongTranLocFieldNotEmptyTest(nameof(OlcWhztranloc.Whztid));
    }

    [Fact]
    public Task AddWrongTranLocWhztlineid01Test()
    {
        return this.AddWrongTranLocFieldNotEmptyTest(nameof(OlcWhztranloc.Whztlineid));
    }

    [Fact]
    public Task AddWrongTranLocWhid01Test()
    {
        return this.AddWrongTranLocFieldNotEmptyTest(nameof(OlcWhztranloc.Whid));
    }

    [Fact]
    public Task AddWrongTranLocWhzoneid01Test()
    {
        return this.AddWrongTranLocFieldNotEmptyTest(nameof(OlcWhztranloc.Whzoneid));
    }

    [Fact]
    public Task AddWrongTranLocWhlocid01Test()
    {
        return this.AddWrongTranLocFieldNotEmptyTest(nameof(OlcWhztranloc.Whlocid));
    }

    [Fact]
    public Task AddWrongTranLocWhztltype01Test()
    {
        return this.AddWrongTranLocFieldNotEmptyTest(nameof(OlcWhztranloc.Whztltype));
    }

    private async Task AddWrongTranLocFieldNotEmptyTest(string fieldName, int whztid = 0, int whztlineid = 0)
    {
        var entity = new OlcWhztranloc
        {
            Whztid = whztid,
            Whztlineid = whztlineid
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"'{fieldName}' must not be empty.";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddWrongTranLocWhztid02Test()
    {
        var entity = new OlcWhztranloc
        {
            Whztid = -1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The transaction doesn't exists (transaction: {entity.Whztid})";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddWrongTranLocWhztlineid02Test()
    {
        var tranHead = await this.GetFirstTranHeadAsync(WhZTranHead_Whzttype.Receiving);
        Assert.NotNull(tranHead);

        var entity = new OlcWhztranloc
        {
            Whztid = tranHead.Whztid,
            Whztlineid = -1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The transaction line doesn't exists (transaction line: {entity.Whztlineid})";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddWrongTranLocWhztlineid03Test()
    {
        var tranHead = await this.GetFirstTranHeadAsync(WhZTranHead_Whzttype.Receiving);
        Assert.NotNull(tranHead);

        var otherTranLine = await this.GetOtherTranLineAsync(tranHead.Whztid);
        Assert.NotNull(otherTranLine);

        var entity = new OlcWhztranloc
        {
            Whztid = tranHead.Whztid,
            Whztlineid = otherTranLine.Whztlineid
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The transaction line doesn't belongs to the transaction (transaction line: {entity.Whztlineid}, transaction: {tranHead?.Whztid})";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddWrongTranLocWhlocid02Test()
    {
        var entity = new OlcWhztranloc
        {
            Whlocid = -1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The location doesn't exists (location: {entity.Whlocid})";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddWrongTranLocWhlocid03Test()
    {
        var tranHead = await this.GetFirstTranHeadAsync(WhZTranHead_Whzttype.Receiving, true);
        Assert.NotNull(tranHead);
        Assert.NotNull(tranHead.Stid);
        Assert.NotNull(tranHead.Towhzid);

        var tranLine = await this.GetFirstTranLineAsync(tranHead.Whztid);
        Assert.NotNull(tranLine);

        var stHead = await this.GetStHeadAsync(tranHead.Stid.Value);
        Assert.NotNull(stHead);
        Assert.NotNull(stHead.Towhid);

        var loc = await this.GetFirstLocationNotForAsync(stHead.Towhid, tranHead.Towhzid.Value);
        Assert.NotNull(loc);

        var entity = new OlcWhztranloc
        {
            Whztid = tranHead.Whztid,
            Whztlineid = tranLine.Whztlineid,
            Whlocid = loc.Whlocid
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The location doesn't belongs to the transaction destination zone (destination zone: {tranHead.Towhzid}, location: {entity.Whlocid})";
        Assert.Contains(message, ex?.Message);
    }

    //[Fact]
    //public async Task AddWrongTranLocWhlocid04Test()
    //{
    //    var tranHead = await this.GetFirstTranHeadAsync(WhZTranHead_Whzttype.Receiving, true);
    //    Assert.NotNull(tranHead);
    //    Assert.NotNull(tranHead.Stid);
    //    Assert.NotNull(tranHead.Towhzid);

    //    var stHead = await this.GetStHeadAsync(tranHead.Stid.Value);
    //    Assert.NotNull(stHead);
    //    Assert.NotNull(stHead.Towhid);

    //    var loc = await this.GetFirstLocationAsync(stHead.Towhid, tranHead.Towhzid.Value);
    //    Assert.NotNull(loc);

    //    var entity = new OlcWhztranloc
    //    {
    //        Whztlocid = loc.Whlocid
    //    };

    //    var ruleSets = new[]
    //    {
    //        "Default",
    //        "Add"
    //    };

    //    var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
    //    var message = $"The location doesn't belongs to the zone (location: {entity.Whztlocid}, zone: {tranHead.Towhzid})";
    //    Assert.Contains(message, ex?.Message);
    //}

    #endregion
}
