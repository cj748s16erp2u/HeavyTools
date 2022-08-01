using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Context;
using FluentValidation;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Validators.Base;

public class BusinessEntityValidator<TEntity> : EntityValidator<TEntity>
    where TEntity : class, IBusinessEntity
{
    public BusinessEntityValidator(WhWebShopDbContext dbContext) : base(dbContext)
    {
    }

    protected override void AddAddRules()
    {
        base.AddAddRules();

        this.RuleFor(entity => entity).PrimaryKeyMustHaveValue(this.dbContext);
        this.RuleFor(entity => entity.Addusrid).NotEmpty();
        this.RuleFor(entity => entity.Adddate).NotEmpty();
    }

    protected override void AddUpdateRules()
    {
        base.AddUpdateRules();

        this.RuleFor(entity => entity).PrimaryKeyMustSame(this.dbContext);
    }
}
