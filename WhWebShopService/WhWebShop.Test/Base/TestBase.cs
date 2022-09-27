using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Context;
using eLog.HeavyTools.Services.WhWebShop.Test.Fixtures;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace eLog.HeavyTools.Services.WhWebShop.Test.Base;

public abstract class TestBase<TEntity, TService> : TestBed<TestFixture>
    where TEntity : class, IEntity
    where TService : ILogicService<TEntity>
{
    protected readonly WhWebShopDbContext dbContext;
    protected readonly TService service;

    protected TestBase(
        ITestOutputHelper testOutputHelper,
        TestFixture fixture) : base(testOutputHelper, fixture)
    {
        this.dbContext = this._fixture.GetService<WhWebShopDbContext>(this._testOutputHelper);
        this.service = this._fixture.GetService<TService>(this._testOutputHelper);
    }
}
