using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Base;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Interfaces;
using FluentValidation;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;

[RegisterDI(Interface = typeof(IOlcItemmodelseasonService))]
public class OlcItemmodelseasonService : LogicServiceBase<OlcItemmodelseason>, IOlcItemmodelseasonService
{
    public OlcItemmodelseasonService(IValidator<OlcItemmodelseason> validator, IRepository<OlcItemmodelseason> repository, IUnitOfWork unitOfWork, IEnvironmentService environmentService) : base(validator, repository, unitOfWork, environmentService)
    {
    }
}
