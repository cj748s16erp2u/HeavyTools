using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Enums;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Base;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhZone.DataAccess.Repositories.Interfaces;
using FluentValidation;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Services;

[RegisterDI(Interface = typeof(IWarehouseService))]
internal class WarehouseService : LogicServiceBase<OlsWarehouse>, IWarehouseService
{
    public WarehouseService(
        IValidator<OlsWarehouse> validator,
        IRepository<OlsWarehouse> repository,
        IUnitOfWork unitOfWork,
        IEnvironmentService environmentService) : base(validator, repository, unitOfWork, environmentService)
    {
    }

    public bool HasLocationHandling(OlsWarehouse warehouse)
    {
        return (OlsWarehouse_LocType)warehouse.Loctype switch
        {
            OlsWarehouse_LocType.Yes => true,
            OlsWarehouse_LocType.WhZone_Location => true,
            _ => false,
        };
    }
}
