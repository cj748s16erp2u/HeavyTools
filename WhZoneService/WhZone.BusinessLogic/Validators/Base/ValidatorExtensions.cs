using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Validators.Base;
using eLog.HeavyTools.Services.WhZone.DataAccess.Context;

namespace FluentValidation;

public static class PrimaryKeyValidatorExtensions
{
    public static IRuleBuilder<TEntity, TEntity> PrimaryKeyMustHaveValue<TEntity>(this IRuleBuilder<TEntity, TEntity> ruleBuilder, WhZoneDbContext dbContext)
        where TEntity : class, IEntity
    {
        return ruleBuilder.SetValidator(new PrimaryKeyNotEmptyValidator<TEntity>(dbContext));
    }

    public static IRuleBuilder<TEntity, TEntity> PrimaryKeyMustSame<TEntity>(this IRuleBuilder<TEntity, TEntity> ruleBuilder, WhZoneDbContext dbContext)
        where TEntity : class, IEntity
    {
        return ruleBuilder.SetValidator(new PrimaryKeySameValidator<TEntity>(dbContext));
    }

    public static T? GetEntity<TEntity, T>(this ValidationContext<TEntity> context, string key)
        where TEntity : class, IEntity
        where T : class, IEntity
    {
        if (context.RootContextData.TryGetValue(key, out var o) && o is T t)
        {
            return t;
        }
        return null;
    }

    public static TEntity? GetOriginalEntity<TEntity>(this ValidationContext<TEntity> context)
        where TEntity : class, IEntity
    {
        return GetEntity<TEntity, TEntity>(context, EntityValidator<TEntity>.OriginalEntityKey);
    }
}
