using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Enums;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Base;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhZone.DataAccess.Repositories.Interfaces;
using FluentValidation;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Services;

[RegisterDI(Interface = typeof(IWHZTranService))]
public class WhZTranService : LogicServiceBase<OlcWhztranhead>, IWHZTranService
{
    public WhZTranService(
        IValidator<OlcWhztranhead> validator,
        IRepository<OlcWhztranhead> repository,
        IUnitOfWork unitOfWork,
        IEnvironmentService environmentService) : base(validator, repository, unitOfWork, environmentService)
    {
    }

    public void AddReceivingAsync(WhZReceivingTranHeadDto request, CancellationToken cancellationToken = default)
    {
        this.ValidateAddRecevingParameters(request);
    }

    private void ValidateAddRecevingParameters(WhZReceivingTranHeadDto request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.Towhzid is null)
        {
            this.ThrowException(WhZTranExceptionType.InvalidTowhzid, "The destination zone identifier is not set", fieldName: nameof(WhZReceivingTranHeadDto.Towhzid));
        }
    }

    private void ThrowException(WhZTranExceptionType type, string message, OlcWhztranhead? entity = null, string? fieldName = null)
    {
        throw new WhZTranServiceException(type, message, entity, fieldName); 
    }
}
