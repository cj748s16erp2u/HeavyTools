using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Enums;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhZone.DataAccess.Repositories.Interfaces;
using eLog.HeavyTools.Services.WhZone.Test.Base;
using eLog.HeavyTools.Services.WhZone.Test.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace eLog.HeavyTools.Services.WhZone.Test.Validators;

public class OlcWhztranlineValidatorTest : TestBase<OlcWhztranline, IWhZTranLineService>
{
    public OlcWhztranlineValidatorTest(
        ITestOutputHelper testOutputHelper,
        TestFixture fixture) : base(testOutputHelper, fixture)
    {
    }

    protected async Task<OlcWhztranhead?> GetFirstTranHeadAsync(WhZTranHead_Whzttype whzttype, CancellationToken cancellationToken = default)
    {
        return await this.dbContext.OlcWhztranheads
            .Where(h => h.Whzttype == (int)whzttype)
            .Where(h => h.St!.OlsStlines.Any())
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    #region Add...

    [Fact]
    public async Task AddWrongTranLineTest()
    {
        var entity = new OlcWhztranline
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
    public Task AddWrongTranLineWhztid01Test()
    {
        return this.AddWrongTranLineFieldNotEmptyTest(nameof(OlcWhztranline.Whztid));
    }

    [Fact]
    public Task AddWrongTranLineLinenum01Test()
    {
        return this.AddWrongTranLineFieldNotEmptyTest(nameof(OlcWhztranline.Linenum));
    }

    [Fact]
    public Task AddWrongTranLineItemid01Test()
    {
        return this.AddWrongTranLineFieldNotEmptyTest(nameof(OlcWhztranline.Itemid));
    }

    //[Fact]
    //public Task AddWrongTranLineOrdqty01Test()
    //{
    //    return this.AddWrongTranLineFieldNotEmptyTest(nameof(OlcWhztranline.Ordqty));
    //}

    //[Fact]
    //public Task AddWrongTranLineDispqty01Test()
    //{
    //    return this.AddWrongTranLineFieldNotEmptyTest(nameof(OlcWhztranline.Dispqty));
    //}

    //[Fact]
    //public Task AddWrongTranLineMovqty01Test()
    //{
    //    return this.AddWrongTranLineFieldNotEmptyTest(nameof(OlcWhztranline.Movqty));
    //}

    //[Fact]
    //public Task AddWrongTranLineInqty01Test()
    //{
    //    return this.AddWrongTranLineFieldNotEmptyTest(nameof(OlcWhztranline.Inqty));
    //}

    //[Fact]
    //public Task AddWrongTranLineOutqty01Test()
    //{
    //    return this.AddWrongTranLineFieldNotEmptyTest(nameof(OlcWhztranline.Outqty));
    //}

    [Fact]
    public Task AddWrongTranLineUnitid201Test()
    {
        return this.AddWrongTranLineFieldNotEmptyTest(nameof(OlcWhztranline.Unitid2));
    }

    //[Fact]
    //public Task AddWrongTranLineOrdqty201Test()
    //{
    //    return this.AddWrongTranLineFieldNotEmptyTest(nameof(OlcWhztranline.Ordqty2));
    //}

    //[Fact]
    //public Task AddWrongTranLineDispqty201Test()
    //{
    //    return this.AddWrongTranLineFieldNotEmptyTest(nameof(OlcWhztranline.Dispqty2));
    //}

    //[Fact]
    //public Task AddWrongTranLineMovqty201Test()
    //{
    //    return this.AddWrongTranLineFieldNotEmptyTest(nameof(OlcWhztranline.Movqty2));
    //}

    [Fact]
    public Task AddWrongTranLineGen01Test()
    {
        return this.AddWrongTranLineFieldNotEmptyTest(nameof(OlcWhztranline.Gen));
    }

    [Fact]
    public async Task AddWrongTranLineStlineid01Test()
    {
        var tranHead = await this.GetFirstTranHeadAsync(WhZTranHead_Whzttype.Receiving);
        Assert.NotNull(tranHead);

        await this.AddWrongTranLineFieldNotEmptyTest(nameof(OlcWhztranline.Stlineid), tranHead!.Whztid);
    }

    //[Fact]
    //public async Task AddWrongTranLineStlineid02Test()
    //{
    //    var tranHead = await this.GetFirstTranHeadAsync(WhZTranHead_Whzttype.Issuing);
    //    Assert.NotNull(tranHead);

    //    await this.AddWrongTranLineFieldEmptyTest(nameof(OlcWhztranline.Stlineid), tranHead!.Whztid, e => e.Stlineid = 1);
    //}

    private async Task AddWrongTranLineFieldNotEmptyTest(string fieldName, int whztid = 0)
    {
        var entity = new OlcWhztranline
        {
            Whztid = whztid
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

    private async Task AddWrongTranLineFieldEmptyTest(string fieldName, int whztid = 0, Action<OlcWhztranline> func = null!)
    {
        var entity = new OlcWhztranline
        {
            Whztid = whztid
        };

        func?.Invoke(entity);

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"'{fieldName}' must be empty.";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddWrongTranLineStlineid03Test()
    {
        var tranHead = await this.GetFirstTranHeadAsync(WhZTranHead_Whzttype.Receiving);
        Assert.NotNull(tranHead);

        var entity = new OlcWhztranline
        {
            Whztid = tranHead.Whztid,
            Stlineid = -1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The stock tran line doesn't exists (stock tran line: {entity.Stlineid})";
        Assert.Contains(message, ex?.Message);
    }

    //[Fact]
    //public async Task AddWrongTranLineStlineid04Test()
    //{
    //    var tranHead = await this.GetFirstTranHeadAsync(WhZTranHead_Whzttype.Transfering);
    //    Assert.NotNull(tranHead);

    //    var entity = new OlcWhztranline
    //    {
    //        Whztid = tranHead.Whztid,
    //        Stlineid = -1
    //    };

    //    var ruleSets = new[]
    //    {
    //        "Default",
    //        "Add"
    //    };

    //    var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
    //    var message = $"The stock tran line doesn't exists (stock tran line: {entity.Stlineid})";
    //    Assert.Contains(message, ex?.Message);
    //}

    [Fact]
    public async Task AddTranWrongLineLinenum01Test()
    {
        var tranHead = await this.GetFirstTranHeadAsync(WhZTranHead_Whzttype.Receiving);
        Assert.NotNull(tranHead);

        var stLine = await this.dbContext.OlsStlines
            .Where(sl => sl.Stid == tranHead.Stid)
            .FirstOrDefaultAsync();
        Assert.NotNull(stLine);

        var entity = new OlcWhztranline
        {
            Whztid = tranHead.Whztid,
            Linenum = -1,
            Itemid = stLine.Itemid,
            Stlineid = stLine.Stlineid,
            Ordqty = -1,
            Dispqty = -1,
            Movqty = -1,
            Inqty = -1,
            Outqty = -1,
            Unitid2 = nameof(OlcWhztranline.Unitid2),
            Ordqty2 = -1,
            Dispqty2 = -1,
            Movqty2 = -1,
            Gen = 1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The transaction line's linenum must be the same as stock tran line's linenum (stock tran: {stLine.Linenum}, transaction: {entity.Linenum})";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddTranWrongLineItemid01Test()
    {
        var tranHead = await this.GetFirstTranHeadAsync(WhZTranHead_Whzttype.Receiving);
        Assert.NotNull(tranHead);

        var stLine = await this.dbContext.OlsStlines
            .Where(sl => sl.Stid == tranHead.Stid)
            .FirstOrDefaultAsync();
        Assert.NotNull(stLine);

        var entity = new OlcWhztranline
        {
            Whztid = tranHead.Whztid,
            Linenum = 1,
            Itemid = -1,
            Stlineid = stLine.Stlineid,
            Ordqty = -1,
            Dispqty = -1,
            Movqty = -1,
            Inqty = -1,
            Outqty = -1,
            Unitid2 = nameof(OlcWhztranline.Unitid2),
            Ordqty2 = -1,
            Dispqty2 = -1,
            Movqty2 = -1,
            Gen = 1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The transaction line's itemid must be the same as stock tran line's itemid (stock tran: {stLine.Itemid}, transaction: {entity.Itemid})";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddTranWrongLineItemid02Test()
    {
        var tranHead = await this.GetFirstTranHeadAsync(WhZTranHead_Whzttype.Receiving);
        Assert.NotNull(tranHead);

        var stLine = await this.dbContext.OlsStlines
            .Where(sl => sl.Stid == tranHead.Stid)
            .FirstOrDefaultAsync();
        Assert.NotNull(stLine);

        var entity = new OlcWhztranline
        {
            Whztid = tranHead.Whztid,
            Linenum = 1,
            Itemid = -1,
            Stlineid = stLine.Stlineid,
            Ordqty = -1,
            Dispqty = -1,
            Movqty = -1,
            Inqty = -1,
            Outqty = -1,
            Unitid2 = nameof(OlcWhztranline.Unitid2),
            Ordqty2 = -1,
            Dispqty2 = -1,
            Movqty2 = -1,
            Gen = 1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The item doesn't exists (item: {entity.Itemid})";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddTranWrongLineOrdqty01Test()
    {
        var tranHead = await this.GetFirstTranHeadAsync(WhZTranHead_Whzttype.Receiving);
        Assert.NotNull(tranHead);

        var stLine = await this.dbContext.OlsStlines
            .Where(sl => sl.Stid == tranHead.Stid)
            .FirstOrDefaultAsync();
        Assert.NotNull(stLine);

        var entity = new OlcWhztranline
        {
            Whztid = tranHead.Whztid,
            Linenum = 1,
            Itemid = stLine.Itemid,
            Stlineid = stLine.Stlineid,
            Ordqty = -1,
            Dispqty = -1,
            Movqty = -1,
            Inqty = -1,
            Outqty = -1,
            Unitid2 = nameof(OlcWhztranline.Unitid2),
            Ordqty2 = -1,
            Dispqty2 = -1,
            Movqty2 = -1,
            Gen = 1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The transaction line's ordqty must be the same as stock tran line's ordqty (stock tran: {stLine.Ordqty}, transaction: {entity.Ordqty})";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddTranWrongLineDispqty01Test()
    {
        var tranHead = await this.GetFirstTranHeadAsync(WhZTranHead_Whzttype.Receiving);
        Assert.NotNull(tranHead);

        var stLine = await this.dbContext.OlsStlines
            .Where(sl => sl.Stid == tranHead.Stid)
            .FirstOrDefaultAsync();
        Assert.NotNull(stLine);

        var entity = new OlcWhztranline
        {
            Whztid = tranHead.Whztid,
            Linenum = 1,
            Itemid = stLine.Itemid,
            Stlineid = stLine.Stlineid,
            Ordqty = -1,
            Dispqty = -1,
            Movqty = -1,
            Inqty = -1,
            Outqty = -1,
            Unitid2 = nameof(OlcWhztranline.Unitid2),
            Ordqty2 = -1,
            Dispqty2 = -1,
            Movqty2 = -1,
            Gen = 1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The transaction line's dispqty must be the same as stock tran line's dispqty (stock tran: {stLine.Dispqty}, transaction: {entity.Dispqty})";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddTranWrongLineMovqty01Test()
    {
        var tranHead = await this.GetFirstTranHeadAsync(WhZTranHead_Whzttype.Receiving);
        Assert.NotNull(tranHead);

        var stLine = await this.dbContext.OlsStlines
            .Where(sl => sl.Stid == tranHead.Stid)
            .FirstOrDefaultAsync();
        Assert.NotNull(stLine);

        var entity = new OlcWhztranline
        {
            Whztid = tranHead.Whztid,
            Linenum = 1,
            Itemid = stLine.Itemid,
            Stlineid = stLine.Stlineid,
            Ordqty = -1,
            Dispqty = -1,
            Movqty = -1,
            Inqty = -1,
            Outqty = -1,
            Unitid2 = nameof(OlcWhztranline.Unitid2),
            Ordqty2 = -1,
            Dispqty2 = -1,
            Movqty2 = -1,
            Gen = 1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The transaction line's movqty must be the same as stock tran line's movqty (stock tran: {stLine.Movqty}, transaction: {entity.Movqty})";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddTranWrongLineInqty01Test()
    {
        var tranHead = await this.GetFirstTranHeadAsync(WhZTranHead_Whzttype.Receiving);
        Assert.NotNull(tranHead);

        var stLine = await this.dbContext.OlsStlines
            .Where(sl => sl.Stid == tranHead.Stid)
            .FirstOrDefaultAsync();
        Assert.NotNull(stLine);

        var entity = new OlcWhztranline
        {
            Whztid = tranHead.Whztid,
            Linenum = 1,
            Itemid = stLine.Itemid,
            Stlineid = stLine.Stlineid,
            Ordqty = -1,
            Dispqty = -1,
            Movqty = -1,
            Inqty = -1,
            Outqty = -1,
            Unitid2 = nameof(OlcWhztranline.Unitid2),
            Ordqty2 = -1,
            Dispqty2 = -1,
            Movqty2 = -1,
            Gen = 1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The transaction line's inqty must be the same as stock tran line's inqty (stock tran: {stLine.Inqty}, transaction: {entity.Inqty})";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddTranWrongLineOutqty01Test()
    {
        var tranHead = await this.GetFirstTranHeadAsync(WhZTranHead_Whzttype.Receiving);
        Assert.NotNull(tranHead);

        var stLine = await this.dbContext.OlsStlines
            .Where(sl => sl.Stid == tranHead.Stid)
            .FirstOrDefaultAsync();
        Assert.NotNull(stLine);

        var entity = new OlcWhztranline
        {
            Whztid = tranHead.Whztid,
            Linenum = 1,
            Itemid = stLine.Itemid,
            Stlineid = stLine.Stlineid,
            Ordqty = -1,
            Dispqty = -1,
            Movqty = -1,
            Inqty = -1,
            Outqty = -1,
            Unitid2 = nameof(OlcWhztranline.Unitid2),
            Ordqty2 = -1,
            Dispqty2 = -1,
            Movqty2 = -1,
            Gen = 1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The transaction line's outqty must be the same as stock tran line's outqty (stock tran: {stLine.Outqty}, transaction: {entity.Outqty})";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddTranWrongLineUnitid201Test()
    {
        var tranHead = await this.GetFirstTranHeadAsync(WhZTranHead_Whzttype.Receiving);
        Assert.NotNull(tranHead);

        var stLine = await this.dbContext.OlsStlines
            .Where(sl => sl.Stid == tranHead.Stid)
            .FirstOrDefaultAsync();
        Assert.NotNull(stLine);

        var entity = new OlcWhztranline
        {
            Whztid = tranHead.Whztid,
            Linenum = 1,
            Itemid = stLine.Itemid,
            Stlineid = stLine.Stlineid,
            Ordqty = -1,
            Dispqty = -1,
            Movqty = -1,
            Inqty = -1,
            Outqty = -1,
            Unitid2 = nameof(OlcWhztranline.Unitid2),
            Ordqty2 = -1,
            Dispqty2 = -1,
            Movqty2 = -1,
            Gen = 1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The transaction line's unitid2 must be the same as stock tran line's unitid2 (stock tran: {stLine.Unitid2}, transaction: {entity.Unitid2})";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddTranWrongLineOrdqty201Test()
    {
        var tranHead = await this.GetFirstTranHeadAsync(WhZTranHead_Whzttype.Receiving);
        Assert.NotNull(tranHead);

        var stLine = await this.dbContext.OlsStlines
            .Where(sl => sl.Stid == tranHead.Stid)
            .FirstOrDefaultAsync();
        Assert.NotNull(stLine);

        var entity = new OlcWhztranline
        {
            Whztid = tranHead.Whztid,
            Linenum = 1,
            Itemid = stLine.Itemid,
            Stlineid = stLine.Stlineid,
            Ordqty = -1,
            Dispqty = -1,
            Movqty = -1,
            Inqty = -1,
            Outqty = -1,
            Unitid2 = nameof(OlcWhztranline.Unitid2),
            Ordqty2 = -1,
            Dispqty2 = -1,
            Movqty2 = -1,
            Gen = 1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The transaction line's ordqty2 must be the same as stock tran line's ordqty2 (stock tran: {stLine.Ordqty2}, transaction: {entity.Ordqty2})";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddTranWrongLineDispqty201Test()
    {
        var tranHead = await this.GetFirstTranHeadAsync(WhZTranHead_Whzttype.Receiving);
        Assert.NotNull(tranHead);

        var stLine = await this.dbContext.OlsStlines
            .Where(sl => sl.Stid == tranHead.Stid)
            .FirstOrDefaultAsync();
        Assert.NotNull(stLine);

        var entity = new OlcWhztranline
        {
            Whztid = tranHead.Whztid,
            Linenum = 1,
            Itemid = stLine.Itemid,
            Stlineid = stLine.Stlineid,
            Ordqty = -1,
            Dispqty = -1,
            Movqty = -1,
            Inqty = -1,
            Outqty = -1,
            Unitid2 = nameof(OlcWhztranline.Unitid2),
            Ordqty2 = -1,
            Dispqty2 = -1,
            Movqty2 = -1,
            Gen = 1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The transaction line's dispqty2 must be the same as stock tran line's dispqty2 (stock tran: {stLine.Dispqty2}, transaction: {entity.Dispqty2})";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddTranWrongLineMovqty201Test()
    {
        var tranHead = await this.GetFirstTranHeadAsync(WhZTranHead_Whzttype.Receiving);
        Assert.NotNull(tranHead);

        var stLine = await this.dbContext.OlsStlines
            .Where(sl => sl.Stid == tranHead.Stid)
            .FirstOrDefaultAsync();
        Assert.NotNull(stLine);

        var entity = new OlcWhztranline
        {
            Whztid = tranHead.Whztid,
            Linenum = 1,
            Itemid = stLine.Itemid,
            Stlineid = stLine.Stlineid,
            Ordqty = -1,
            Dispqty = -1,
            Movqty = -1,
            Inqty = -1,
            Outqty = -1,
            Unitid2 = nameof(OlcWhztranline.Unitid2),
            Ordqty2 = -1,
            Dispqty2 = -1,
            Movqty2 = -1,
            Gen = 1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The transaction line's movqty2 must be the same as stock tran line's movqty2 (stock tran: {stLine.Movqty2}, transaction: {entity.Movqty2})";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddTranLine01Test()
    {
        var tranHead = await this.GetFirstTranHeadAsync(WhZTranHead_Whzttype.Receiving);
        Assert.NotNull(tranHead);

        var stLine = await this.dbContext.OlsStlines
            .Where(sl => sl.Stid == tranHead.Stid)
            .FirstOrDefaultAsync();
        Assert.NotNull(stLine);

        using var tran = await this.unitOfWork.BeginTransactionAsync();
        try
        {
            var line = await this.service.GetAsync(l => l.Stlineid == stLine.Stlineid);
            if (line is not null)
            {
                this.dbContext.OlcWhztranlines.Remove(line);
                await this.unitOfWork.SaveChangesAsync();

                var entry = this.dbContext.Entry(line);
                if (entry is not null)
                {
                    entry.State = EntityState.Detached;
                }
            }

            var entity = new OlcWhztranline
            {
                Whztid = tranHead.Whztid,
                Linenum = stLine.Linenum,
                Itemid = stLine.Itemid,
                Stlineid = stLine.Stlineid,
                Ordqty = stLine.Ordqty,
                Dispqty = stLine.Dispqty,
                Movqty = stLine.Movqty,
                Inqty = stLine.Inqty,
                Outqty = stLine.Outqty,
                Unitid2 = stLine.Unitid2,
                Change = stLine.Change,
                Ordqty2 = stLine.Ordqty2,
                Dispqty2 = stLine.Dispqty2,
                Movqty2 = stLine.Movqty2,
                Gen = 1
            };

            var ruleSets = new[]
            {
                "Default",
                "Add"
            };

            await this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets);
        }
        finally
        {
            tran.Rollback();
        }
    }

    #endregion

    #region Update...

    [Fact]
    public Task UpdateWrongTranLineWhztid01Test()
    {
        return this.UpdateWrongTranLineFieldReadOnlyTest(nameof(OlcWhztranline.Whztid), e => e.Whztid = 1, e => e.Whztid = 2);
    }

    [Fact]
    public Task UpdateWrongTranLineItemid01Test()
    {
        return this.UpdateWrongTranLineFieldReadOnlyTest(nameof(OlcWhztranline.Itemid), e => e.Itemid = 1, e => e.Itemid = 2);
    }

    [Fact]
    public Task UpdateWrongTranLineStlineid01Test()
    {
        return this.UpdateWrongTranLineFieldReadOnlyTest(nameof(OlcWhztranline.Stlineid), e => e.Stlineid = 1, e => e.Stlineid = 2);
    }

    private async Task UpdateWrongTranLineFieldReadOnlyTest(string fieldName, Action<OlcWhztranline> func, Action<OlcWhztranline> modifyFunc)
    {
        var originalEntity = new OlcWhztranline
        {
        };

        func?.Invoke(originalEntity);

        var entity = originalEntity.Clone<OlcWhztranline>();

        modifyFunc?.Invoke(entity);

        var ruleSets = new[]
        {
            "Default",
            "Update"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, originalEntity, ruleSets: ruleSets));
        var message = $"'{fieldName}' is read-only.";
        Assert.Contains(message, ex?.Message);
    }

    #endregion

    //#region Delete

    //[Fact]
    //public async Task DeleteTranHeadTest()
    //{
    //    var originalEntity = new OlcWhztranhead
    //    {
    //        Whztstat = (int)WhZTranHead_Whztstat.Closed
    //    };

    //    var entity = originalEntity.Clone<OlcWhztranhead>();

    //    var ruleSets = new[]
    //    {
    //        "Default",
    //        "Delete"
    //    };

    //    var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, originalEntity, ruleSets: ruleSets));
    //    var message = $"Unable to delete a closed transaction (transaction: {entity.Whztid})";
    //    Assert.Contains(message, ex?.Message);
    //}

    //#endregion
}
