using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Base;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Interfaces;
using FluentValidation;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;

[RegisterDI(Interface = typeof(ICfwUsergroupService))]
public class CfwUsergroupService : LogicServiceBase<CfwUsergroup>, ICfwUsergroupService
{
    public CfwUsergroupService(IValidator<CfwUsergroup> validator, IRepository<CfwUsergroup> repository, IUnitOfWork unitOfWork, IEnvironmentService environmentService) : base(validator, repository, unitOfWork, environmentService)
    {
    }
}
