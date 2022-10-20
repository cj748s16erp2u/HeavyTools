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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace eLog.HeavyTools.Services.WhZone.Test.Validators;

public class OlcWhztranheadValidatorTest : TestBase<OlcWhztranhead, IWhZTranService>
{
    public OlcWhztranheadValidatorTest(
        ITestOutputHelper testOutputHelper,
        TestFixture fixture) : base(testOutputHelper, fixture)
    {
    }

    #region Add...

    [Fact]
    public async Task AddWrongTranHeadTest()
    {
        var entity = new OlcWhztranhead
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
    public Task AddWrongTranHeadCmpid01Test()
    {
        return this.AddWrongTranHeadFieldNotEmptyTest(nameof(OlcWhztranhead.Cmpid));
    }

    [Fact]
    public Task AddWrongTranHeadWhzttypeTest()
    {
        return this.AddWrongTranHeadFieldNotEmptyTest(nameof(OlcWhztranhead.Whzttype));
    }

    [Fact]
    public Task AddWrongTranHeadWhztdateTest()
    {
        return this.AddWrongTranHeadFieldNotEmptyTest(nameof(OlcWhztranhead.Whztdate));
    }

    [Fact]
    public Task AddWrongTranHeadWhztstatTest()
    {
        return this.AddWrongTranHeadFieldNotEmptyTest(nameof(OlcWhztranhead.Whztstat));
    }

    [Fact]
    public Task AddWrongTranHeadGenTest()
    {
        return this.AddWrongTranHeadFieldNotEmptyTest(nameof(OlcWhztranhead.Gen));
    }

    [Fact]
    public Task AddWrongTranHeadFromwhzid01Test()
    {
        return this.AddWrongTranHeadFieldEmptyTest(nameof(OlcWhztranhead.Fromwhzid), (int)WhZTranHead_Whzttype.Receiving, e => e.Fromwhzid = -1);
    }

    [Fact]
    public Task AddWrongTranHeadFromwhzid02Test()
    {
        return this.AddWrongTranHeadFieldNotEmptyTest(nameof(OlcWhztranhead.Fromwhzid), (int)WhZTranHead_Whzttype.Issuing);
    }

    [Fact]
    public Task AddWrongTranHeadFromwhzid03Test()
    {
        return this.AddWrongTranHeadFieldNotEmptyTest(nameof(OlcWhztranhead.Fromwhzid), (int)WhZTranHead_Whzttype.Transfering);
    }

    [Fact]
    public Task AddWrongTranHeadTowhzid01Test()
    {
        return this.AddWrongTranHeadFieldNotEmptyTest(nameof(OlcWhztranhead.Towhzid), (int)WhZTranHead_Whzttype.Receiving);
    }

    [Fact]
    public Task AddWrongTranHeadTowhzid02Test()
    {
        return this.AddWrongTranHeadFieldEmptyTest(nameof(OlcWhztranhead.Towhzid), (int)WhZTranHead_Whzttype.Issuing, e => e.Towhzid = -1);
    }

    [Fact]
    public Task AddWrongTranHeadTowhzid03Test()
    {
        return this.AddWrongTranHeadFieldNotEmptyTest(nameof(OlcWhztranhead.Towhzid), (int)WhZTranHead_Whzttype.Transfering);
    }

    [Fact]
    public Task AddWrongTranHeadStid01Test()
    {
        return this.AddWrongTranHeadFieldNotEmptyTest(nameof(OlcWhztranhead.Stid), (int)WhZTranHead_Whzttype.Receiving);
    }

    [Fact]
    public Task AddWrongTranHeadStid02Test()
    {
        return this.AddWrongTranHeadFieldEmptyTest(nameof(OlcWhztranhead.Stid), (int)WhZTranHead_Whzttype.Issuing, e => e.Stid = -1);
    }

    private async Task AddWrongTranHeadFieldNotEmptyTest(string fieldName, int whztType = 0)
    {
        var entity = new OlcWhztranhead
        {
            Whzttype = whztType
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

    private async Task AddWrongTranHeadFieldEmptyTest(string fieldName, int whztType = 0, Action<OlcWhztranhead> func = null!)
    {
        var entity = new OlcWhztranhead
        {
            Whzttype = whztType
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
    public async Task AddWrongTranHeadCmpid02Test()
    {
        var entity = new OlcWhztranhead
        {
            Cmpid = -1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The company doesn't exists (company: {entity.Cmpid})";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddWrongTranHeadCmpid03Test()
    {
        var stHead = await this.dbContext.OlsStheads
            .Where(st => st.Stid == 1)
            .FirstOrDefaultAsync();

        var entity = new OlcWhztranhead
        {
            Cmpid = 1,
            Whzttype = (int)WhZTranHead_Whzttype.Receiving,
            Towhzid = 1,
            Stid = stHead?.Stid,
            Whztdate = (stHead?.Stdate).GetValueOrDefault(DateTime.Today),
            Whztstat = (int)WhZTranHead_Whztstat.Created,
            Gen = 1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The company must be same as stock tran company (stock tran: {stHead?.Cmpid}, transaction: {entity.Cmpid})";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddWrongTranHeadFromwhzid04Test()
    {
        var entity = new OlcWhztranhead
        {
            Whzttype = (int)WhZTranHead_Whzttype.Issuing,
            Fromwhzid = -1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The source zone doesn't exists (zone: {entity.Fromwhzid})";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddWrongTranHeadFromwhzid05Test()
    {
        var entity = new OlcWhztranhead
        {
            Whzttype = (int)WhZTranHead_Whzttype.Transfering,
            Fromwhzid = -1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The source zone doesn't exists (zone: {entity.Fromwhzid})";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddWrongTranHeadTowhzid04Test()
    {
        var entity = new OlcWhztranhead
        {
            Whzttype = (int)WhZTranHead_Whzttype.Receiving,
            Towhzid = -1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The destination zone doesn't exists (zone: {entity.Towhzid})";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddWrongTranHeadTowhzid05Test()
    {
        var entity = new OlcWhztranhead
        {
            Whzttype = (int)WhZTranHead_Whzttype.Transfering,
            Towhzid = -1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The destination zone doesn't exists (zone: {entity.Towhzid})";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddWrongTranHeadTowhzid06Test()
    {
        var stHead = await this.dbContext.OlsStheads
            .Where(st => st.Stid == 1)
            .FirstOrDefaultAsync();

        var zone = await this.dbContext.OlcWhzones
            .Where(z => z.Whzoneid == 3)
            .FirstOrDefaultAsync();

        var entity = new OlcWhztranhead
        {
            Cmpid = 3,
            Whzttype = (int)WhZTranHead_Whzttype.Receiving,
            Towhzid = zone?.Whzoneid,
            Stid = stHead?.Stid,
            Whztdate = (stHead?.Stdate).GetValueOrDefault(DateTime.Today),
            Whztstat = (int)WhZTranHead_Whztstat.Created,
            Gen = 1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The destination zone's warehouse must be same as stock tran destination warehouse (stock tran: {stHead?.Towhid}, zone: {zone?.Whid})";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddWrongTranHeadTowhzid07Test()
    {
        var stHead = await this.dbContext.OlsStheads
            .Where(st => st.Stid == 1)
            .FirstOrDefaultAsync();

        var zone = await this.dbContext.OlcWhzones
            .Where(z => z.Whzoneid == 3)
            .FirstOrDefaultAsync();

        var entity = new OlcWhztranhead
        {
            Cmpid = 1,
            Whzttype = (int)WhZTranHead_Whzttype.Receiving,
            Towhzid = zone?.Whzoneid,
            Stid = stHead?.Stid,
            Whztdate = (stHead?.Stdate).GetValueOrDefault(DateTime.Today),
            Whztstat = (int)WhZTranHead_Whztstat.Created,
            Gen = 1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The destination warehouse doesn't valid in the given company (zone: {entity.Towhzid}, company: {entity.Cmpid})";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddWrongTranHeadStid03Test()
    {
        var entity = new OlcWhztranhead
        {
            Whzttype = (int)WhZTranHead_Whzttype.Receiving,
            Stid = -1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The stock tran doesn't exists (stock tran: {entity.Stid})";
        Assert.Contains(message, ex?.Message);
    }

    [Fact]
    public async Task AddWrongTranHeadStid04Test()
    {
        var entity = new OlcWhztranhead
        {
            Whzttype = (int)WhZTranHead_Whzttype.Transfering,
            Stid = -1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets));
        var message = $"The stock tran doesn't exists (stock tran: {entity.Stid})";
        Assert.Contains(message, ex?.Message);
    }

#pragma warning disable xUnit1004 // Test methods should not be skipped
    [Fact(Skip = "StHead bejegyzes nem letezik WhZTranHead bejegyzes nelkul")]
#pragma warning restore xUnit1004 // Test methods should not be skipped
    public async Task AddTranHead01Test()
    {
        var stHead = await this.dbContext.OlsStheads
            .Where(st => st.Stid == 1)
            .FirstOrDefaultAsync();        

        var whZone = stHead is not null
            ? await this.dbContext.OlcWhzones
                .Where(z => z.Whid == stHead.Towhid)
                .FirstOrDefaultAsync()
            : null;

        var entity = new OlcWhztranhead
        {
            Cmpid = (stHead?.Cmpid).GetValueOrDefault(),
            Whzttype = (int)WhZTranHead_Whzttype.Receiving,
            Towhzid = whZone?.Whzoneid,
            Stid = stHead?.Stid,
            Whztdate = (stHead?.Stdate).GetValueOrDefault(DateTime.Today),
            Whztstat = (int)WhZTranHead_Whztstat.Created,
            Gen = 1
        };

        var ruleSets = new[]
        {
            "Default",
            "Add"
        };

        await this.service.ValidateAndThrowAsync(entity, ruleSets: ruleSets);
    }

    #endregion

    #region Update...

    [Fact]
    public Task UpdateWrongTranHeadCmpid01Test()
    {
        return this.UpdateWrongTranHeadFieldReadOnlyTest(nameof(OlcWhztranhead.Cmpid), e => e.Cmpid = 1, e => e.Cmpid = 2);
    }

    [Fact]
    public Task UpdateWrongTranHeadWhzttype01Test()
    {
        return this.UpdateWrongTranHeadFieldReadOnlyTest(nameof(OlcWhztranhead.Whzttype), e => e.Whzttype = 1, e => e.Whzttype = 2);
    }

    [Fact]
    public Task UpdateWrongTranHeadWhztdate01Test()
    {
        return this.UpdateWrongTranHeadFieldReadOnlyTest(
            nameof(OlcWhztranhead.Whztdate),
            e =>
            {
                e.Whztdate = DateTime.Today.AddDays(-1);
                e.Whztstat = (int)WhZTranHead_Whztstat.Closed;
            },
            e => e.Whztdate = DateTime.Today);
    }

    [Fact]
    public Task UpdateWrongTranHeadWhztdate02Test()
    {
        return this.UpdateWrongTranHeadFieldReadOnlyTest(
            nameof(OlcWhztranhead.Whztdate),
            e =>
            {
                e.Whztdate = DateTime.Today.AddDays(-1);
                e.Whztstat = (int)WhZTranHead_Whztstat.Created;
            },
            e =>
            {
                e.Whztdate = DateTime.Today;
                e.Whztstat = (int)WhZTranHead_Whztstat.Closed;
            });
    }

    [Fact]
    public Task UpdateWrongTranHeadFromwhzid01Test()
    {
        return this.UpdateWrongTranHeadFieldReadOnlyTest(nameof(OlcWhztranhead.Fromwhzid), e => e.Fromwhzid = 1, e => e.Fromwhzid = 2);
    }

    //[Fact]
    //public Task UpdateWrongTranHeadTowhzid01Test()
    //{
    //    return this.UpdateWrongTranHeadFieldReadOnlyTest(nameof(OlcWhztranhead.Towhzid), e => e.Towhzid = 1, e => e.Towhzid = 2);
    //}

    [Fact]
    public Task UpdateWrongTranHeadStid01Test()
    {
        return this.UpdateWrongTranHeadFieldReadOnlyTest(nameof(OlcWhztranhead.Stid), e => e.Stid = 1, e => e.Stid = 2);
    }

    private async Task UpdateWrongTranHeadFieldReadOnlyTest(string fieldName, Action<OlcWhztranhead> func, Action<OlcWhztranhead> modifyFunc)
    {
        var originalEntity = new OlcWhztranhead
        {
        };

        func?.Invoke(originalEntity);

        var entity = originalEntity.Clone<OlcWhztranhead>();

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

    #region Delete

    [Fact]
    public async Task DeleteTranHeadTest()
    {
        var originalEntity = new OlcWhztranhead
        {
            Whztstat = (int)WhZTranHead_Whztstat.Closed
        };

        var entity = originalEntity.Clone<OlcWhztranhead>();

        var ruleSets = new[]
        {
            "Default",
            "Delete"
        };

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => this.service.ValidateAndThrowAsync(entity, originalEntity, ruleSets: ruleSets));
        var message = $"Unable to delete a closed transaction (transaction: {entity.Whztid})";
        Assert.Contains(message, ex?.Message);
    }

    #endregion
}
