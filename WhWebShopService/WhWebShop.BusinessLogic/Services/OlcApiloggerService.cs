using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Base;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;

[RegisterDI(Interface = typeof(IOlcApiloggerService))]
public class OlcApiloggerService : LogicServiceBase<OlcApilogger>, IOlcApiloggerService
{
    public OlcApiloggerService(IValidator<OlcApilogger> validator, IRepository<OlcApilogger> repository, IUnitOfWork unitOfWork, IEnvironmentService environmentService) : base(validator, repository, unitOfWork, environmentService)
    {
    }

    private IRepository<OlcApilogger> scopedRepository;
    private IUnitOfWork scopedUnitOfWork;
    private IServiceScopeFactory serviceScopeFactory;
    private IServiceScope scope;

    public OlcApiloggerService(
        IValidator<OlcApilogger> validator,
        IRepository<OlcApilogger> repository,
        IUnitOfWork unitOfWork,
        IEnvironmentService environmentService, IServiceScopeFactory serviceScopeFactory) : base(validator, repository, unitOfWork, environmentService)
    {
        this.serviceScopeFactory = serviceScopeFactory;
        this.scope = this.serviceScopeFactory.CreateScope();
        this.scopedRepository = scope.ServiceProvider.GetRequiredService<IRepository<OlcApilogger>>();
        this.scopedUnitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    }

    protected override IRepository<OlcApilogger> Repository => this.scopedRepository ?? base.Repository;

    protected override  IUnitOfWork UnitOfWork => this.scopedUnitOfWork ?? base.UnitOfWork;


    public void Dispose()
    {
        this.scopedUnitOfWork = null!;
        this.scopedRepository = null!;
        this.scope?.Dispose();
        this.scope = null!;
    }
}
