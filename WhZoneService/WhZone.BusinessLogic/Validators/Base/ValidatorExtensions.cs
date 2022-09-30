using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Validators.Base;
using eLog.HeavyTools.Services.WhZone.DataAccess.Context;

namespace FluentValidation;

[System.Diagnostics.DebuggerStepThrough]
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

    public static bool TryAddEntity<TEntity, T>(this ValidationContext<TEntity> context, T entity)
        where TEntity : class, IEntity
        where T : class, IEntity
    {
        var key = typeof(T).Name;
        return TryAddEntity(context, entity, key);
    }

    public static bool TryAddEntity<TEntity, T>(this ValidationContext<TEntity> context, T entity, string key)
        where TEntity : class, IEntity
        where T : class, IEntity
    {
        return context.RootContextData.TryAdd(key, entity);
    }

    public static T? TryGetEntity<TEntity, T>(this ValidationContext<TEntity> context)
        where TEntity : class, IEntity
        where T : class, IEntity
    {
        var key = typeof(T).Name;
        return TryGetEntity<TEntity, T>(context, key);
    }

    public static T? TryGetEntity<TEntity, T>(this ValidationContext<TEntity> context, string key)
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
        return TryGetEntity<TEntity, TEntity>(context, EntityValidator<TEntity>.OriginalEntityKey);
    }

    public static IRuleBuilderOptions<T, TProperty> ReadOnly<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, Expression<Func<T, TProperty>> expression)
        where T : class, IEntity
        => ruleBuilder.SetValidator(new ReadOnlyValidator<T, TProperty>(expression))
            .WithMessage("'{PropertyName}' is read-only.");
}
