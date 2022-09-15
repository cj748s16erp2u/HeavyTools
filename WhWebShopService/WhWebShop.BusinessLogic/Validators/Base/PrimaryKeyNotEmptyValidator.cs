using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Context;
using FluentValidation;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Validators.Base;

public class PrimaryKeyNotEmptyValidator<TEntity> : PrimaryKeyValidator<TEntity>
    where TEntity : class, IEntity
{
    public override string Name => "PrimaryKeyNotEmptyValidator";

    public PrimaryKeyNotEmptyValidator(WhWebShopDbContext dbContext) : base(dbContext)
    {
    }

    public override string GetDefaultMessageTemplate(string errorCode) => this.Localized(errorCode, this.Name);

    public override bool IsValid(ValidationContext<TEntity> context, TEntity value)
    {
        if (value is null)
        {
            return true;
        }

        var primaryKey = this.GetPrimaryKey(value);
        if (primaryKey is null)
        {
            return true;
        }

        return primaryKey.Any(pk => pk.Value is not null);
    }
}
