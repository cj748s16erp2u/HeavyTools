using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model.Interfaces;
using eLog.HeavyTools.Services.WhZone.DataAccess.Context;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Validators.Base;

public class BusinessMasterEntityValidator<TEntity> : BusinessEntityValidator<TEntity>
    where TEntity : class, IBusinessMasterEntity
{
    public BusinessMasterEntityValidator(WhZoneDbContext dbContext) : base(dbContext)
    {
    }
}
