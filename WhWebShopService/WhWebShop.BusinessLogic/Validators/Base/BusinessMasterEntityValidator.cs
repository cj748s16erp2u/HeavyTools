using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Context;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Validators.Base;

public class BusinessMasterEntityValidator<TEntity> : BusinessEntityValidator<TEntity>
    where TEntity : class, IBusinessMasterEntity
{
    public BusinessMasterEntityValidator(WhWebShopDbContext dbContext) : base(dbContext)
    {
    }
}
