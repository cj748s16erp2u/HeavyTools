using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Base;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;


[RegisterDI(Interface = typeof(IApiLoggerService))]
public class ApiLoggerService : LogicServiceBase<OlcApilogger>, IApiLoggerService, IDisposable
{ 
    private IRepository<OlcApilogger> scopedRepository;
    private IUnitOfWork scopedUnitOfWork;
    private IServiceScopeFactory serviceScopeFactory;
    private IServiceScope scope;

    public ApiLoggerService(
        IValidator<OlcApilogger> validator,
        IRepository<OlcApilogger> repository,
        IUnitOfWork unitOfWork,
        IEnvironmentService environmentService, IServiceScopeFactory serviceScopeFactory) : base(validator, repository, unitOfWork, environmentService)
    {
        this.serviceScopeFactory = serviceScopeFactory;
        this.scope = this.serviceScopeFactory.CreateScope(); 
        this.scopedRepository= scope.ServiceProvider.GetRequiredService<IRepository<OlcApilogger>>();
        this.scopedUnitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    }

    protected override IRepository<OlcApilogger> Repository => this.scopedRepository ?? base.Repository;
    override protected IUnitOfWork UnitOfWork => this.scopedUnitOfWork ?? base.UnitOfWork;
 

    public void Dispose()
    {
        this.scopedUnitOfWork = null!;
        this.scopedRepository = null!;
        this.scope?.Dispose();
        this.scope = null!;
    }
}
