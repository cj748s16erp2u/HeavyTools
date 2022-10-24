using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Base;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Interfaces;
using FluentValidation;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;

[RegisterDI(Interface = typeof(IOlcPrctableCurrentService))]
public class OlcPrctableCurrentService : LogicServiceBase<OlcPrctableCurrent>, IOlcPrctableCurrentService
{
    public OlcPrctableCurrentService(IValidator<OlcPrctableCurrent> validator, IRepository<OlcPrctableCurrent> repository, IUnitOfWork unitOfWork, IEnvironmentService environmentService) : base(validator, repository, unitOfWork, environmentService)
    {
    }
}
