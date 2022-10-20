using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Base;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhZone.DataAccess.Repositories.Interfaces;
using FluentValidation;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Services;

[RegisterDI(Interface = typeof(IWhZoneService))]
internal class WhZoneService : LogicServiceBase<OlcWhzone>, IWhZoneService
{
    public WhZoneService(
        IValidator<OlcWhzone> validator,
        IRepository<OlcWhzone> repository,
        IUnitOfWork unitOfWork,
        IEnvironmentService environmentService) : base(validator, repository, unitOfWork, environmentService)
    {
    }
}
