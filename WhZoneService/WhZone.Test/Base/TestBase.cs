using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhZone.DataAccess.Context;
using eLog.HeavyTools.Services.WhZone.Test.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhZone.Test.Base;

public abstract class TestBase<TEntity, TService> : TestBed<TestFixture>
    where TEntity : class, IEntity
    where TService : ILogicService<TEntity>
{
    protected readonly WhZoneDbContext dbContext;
    protected readonly TService service;

    protected TestBase(
        ITestOutputHelper testOutputHelper,
        TestFixture fixture) : base(testOutputHelper, fixture)
    {
        this.dbContext = this._fixture.GetService<WhZoneDbContext>(this._testOutputHelper) ?? throw new InvalidOperationException($"{nameof(WhZoneDbContext)} is not found.");
        this.service = this._fixture.GetService<TService>(this._testOutputHelper) ?? throw new InvalidOperationException($"{typeof(TService).Name} is not found.");
    }

    protected async Task<int?> GetFirstItemIdAsync(CancellationToken cancellationToken = default)
    {
        return await this.dbContext.OlsItems
            .OrderBy(i => i.Itemid)
            .Select(i => i.Itemid)
            .FirstOrDefaultAsync(cancellationToken);
    }

    protected async Task<string?> GetFirstWarehouseIdAsync(bool needWhZone = false, CancellationToken cancellationToken = default)
    {
        var query = this.dbContext.OlsWarehouses
            .AsQueryable();
        if (needWhZone)
        {
            query = query.Where(i => i.OlcWhzones.Any());
        }

        return await query
            .OrderBy(i => i.Whid)
            .Select(i => i.Whid)
            .FirstOrDefaultAsync(cancellationToken);
    }

    protected async Task<int?> GetFirstWhZoneIdAsync(string whid, bool needWhLocation = false, CancellationToken cancellationToken = default)
    {
        var query = this.dbContext.OlcWhzones
            .AsQueryable()
            .Where(i => i.Whid == whid);
        if (needWhLocation)
        {
            query = query.Where(i => i.OlcWhlocations.Any());
        }

        return await query
            .OrderBy(i => i.Whzoneid)
            .Select(i => i.Whzoneid)
            .FirstOrDefaultAsync(cancellationToken);
    }

    protected async Task<int?> GetFirstWhLocIdAsync(string whid, int? whZoneId, CancellationToken cancellationToken = default)
    {
        return await this.dbContext.OlcWhlocations
            .Where(i => i.Whid == whid)
            .Where(i => i.Whzoneid == whZoneId)
            .Select(i => i.Whlocid)
            .FirstOrDefaultAsync(cancellationToken);
    }

    protected async Task<int?> GetRandomItemIdAsync(CancellationToken cancellationToken = default)
    {
        var itemCount = await this.dbContext.OlsItems.CountAsync(cancellationToken: cancellationToken);
        var skipCount = new Random().Next(itemCount - 1);
        return await this.dbContext.OlsItems
            .Skip(skipCount)
            .Select(i => i.Itemid)
            .FirstOrDefaultAsync(cancellationToken);
    }

    protected async Task<int?> GetRandomWhZoneIdAsync(string whid, bool needWhLocation = false, CancellationToken cancellationToken = default)
    {
        var query = this.dbContext.OlcWhzones
            .AsQueryable()
            .Where(i => i.Whid == whid);
        if (needWhLocation)
        {
            query = query.Where(i => i.OlcWhlocations.Any());
        }

        var zoneCount = await query.CountAsync(cancellationToken: cancellationToken);
        var skipCount = new Random().Next(zoneCount - 1);
        return await query
            .Skip(skipCount)
            .Select(i => i.Whzoneid)
            .FirstOrDefaultAsync(cancellationToken);
    }

    protected async Task<int?> GetRandomWhLocIdAsync(string whid, int? whZoneId, CancellationToken cancellationToken = default)
    {
        var query = this.dbContext.OlcWhlocations
            .AsQueryable()
            .Where(i => i.Whid == whid)
            .Where(i => i.Whzoneid == whZoneId);

        var locCount = await query.CountAsync(cancellationToken: cancellationToken);
        var skipCount = new Random().Next(locCount - 1);
        return await query
            .Skip(skipCount)
            .Select(i => i.Whlocid)
            .FirstOrDefaultAsync(cancellationToken);
    }

    protected async Task<OlcWhzstock?> GetRandomStockAsync(string whid, bool hasZone, bool hasResQty = false, CancellationToken cancellationToken = default)
    {
        var query = this.dbContext.OlcWhzstocks
            .AsQueryable()
            .Where(i => i.Whid == whid);
        if (hasZone)
        {
            query = query.Where(i => i.Whzoneid != null);
        }
        else
        {
            query = query.Where(i => i.Whzoneid == null);
        }

        if (hasResQty)
        {
            query = query.Where(i => i.Resqty > 0M && i.Actqty > 0M);
        }

        var stockCount = await query.CountAsync(cancellationToken: cancellationToken);
        var skipCount = new Random().Next(stockCount - 1);
        return await query
            .Skip(skipCount)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
