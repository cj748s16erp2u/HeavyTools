using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model.Base;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhZone.DataAccess.Repositories.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Base;

[RegisterDI(Interface = typeof(ILogicService<>))]
public class LogicServiceBase<TEntity> : ILogicService<TEntity>
    where TEntity : class, IEntity
{
    protected static readonly string[] DefaultRuleSets = new[]
    {
        Validators.Base.EntityValidator<TEntity>.DefaultRuleSet
    };

    protected static readonly string[] AddRuleSets = new[]
    {
        Validators.Base.EntityValidator<TEntity>.AddRuleSet
    };

    protected static readonly string[] UpdateRuleSets = new[]
    {
        Validators.Base.EntityValidator<TEntity>.UpdateRuleSet
    };

    protected static readonly string[] DeleteRuleSets = new[]
    {
        Validators.Base.EntityValidator<TEntity>.DeleteRuleSet
    };

    protected IValidator<TEntity> Validator { get; }
    protected IRepository<TEntity> Repository { get; }
    protected IUnitOfWork UnitOfWork { get; }
    protected IEnvironmentService EnvironmentService { get; }

    public LogicServiceBase(
        IValidator<TEntity> validator,
        IRepository<TEntity> repository,
        IUnitOfWork unitOfWork,
        IEnvironmentService environmentService)
    {
        this.Validator = validator ?? throw new ArgumentNullException(nameof(validator));
        this.Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        this.UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.EnvironmentService = environmentService ?? throw new ArgumentNullException(nameof(environmentService));
    }

    public virtual ValueTask<TEntity?> GetByIdAsync(object keyValue, CancellationToken cancellationToken = default)
    {
        return this.GetByIdAsync(new[] { keyValue }, cancellationToken);
    }

    public virtual ValueTask<TEntity?> GetByIdAsync(object[] keyValues, CancellationToken cancellationToken = default)
    {
        if (keyValues is null)
        {
            throw new ArgumentNullException(nameof(keyValues));
        }

        if (keyValues.Length == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(keyValues));
        }

        return this.Repository.FindAsync(keyValues, cancellationToken);
    }

    /// <summary>
    /// A megadott feltétel alapján 1 bejegyzés betöltése.
    /// Több találat esetén kivétel kerül kiváltásra.
    /// </summary>
    /// <param name="predicate">A feltétel</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A betöltött bejegyzés vagy null</returns>
    /// <exception cref="InvalidOperationException"><paramref name="source" /> contains more than one element.</exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public virtual async ValueTask<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var result = this.Query(predicate);

        return await result.SingleOrDefaultAsync(cancellationToken);
    }

    public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate = null!)
    {
        var result = this.Repository.Entities.AsNoTracking();

        if (predicate is not null)
        {
            result = result.Where(predicate);
        }

        return result;
    }

    public virtual async Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> predicate = null!, CancellationToken cancellationToken = default)
    {
        var result = this.Query(predicate);

        return await result.ToListAsync(cancellationToken);
    }

    public virtual Task<TEntity?> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return this.AddIntlAsync(entity, cancellationToken);
    }

    protected virtual void ValidateAddOrUpdateIntlParameters(TEntity entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        if (string.IsNullOrWhiteSpace(this.EnvironmentService.CurrentUserId))
        {
            throw new InvalidOperationException("Invalid authentication!User identifier is not found.");
        }
    }

    protected virtual async Task<TEntity?> AddIntlAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        this.ValidateAddOrUpdateIntlParameters(entity);

        var entityToSave = entity.Clone<TEntity>();

        await this.FillSystemFieldsOnAddAsync(entityToSave, cancellationToken);

        await this.ValidateAndThrowAsync(entityToSave, ruleSets: LogicServiceBase<TEntity>.AddRuleSets);

        var entry = await this.Repository.AddAsync(entityToSave, cancellationToken);
        await this.UnitOfWork.SaveChangesAsync(cancellationToken);

        if (entry is not null)
        {
            entry.State = EntityState.Detached;
        }

        return entry?.Entity;
    }

    protected virtual async Task FillSystemFieldsOnAddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await this.PrepareEntityAsync(entity, cancellationToken);

        if (entity is IBusinessEntity businessEntity)
        {
            businessEntity.Addusrid = this.EnvironmentService.CurrentUserId!;
            businessEntity.Adddate = DateTime.Now;
        }
    }

    public virtual Task<TEntity?> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return this.UpdateIntlAsync(entity, cancellationToken);
    }

    protected virtual async Task<TEntity?> UpdateIntlAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        this.ValidateAddOrUpdateIntlParameters(entity);

        var primaryKey = this.Repository.GetPrimaryKey(entity);
        TEntity? knownEntity = null;
        if (primaryKey is not null)
        {
            knownEntity = await this.GetByIdAsync(primaryKey.Values.ToArray(), cancellationToken);
        }

        if (knownEntity is null)
        {
            this.ThrowKeyNotFoundException(primaryKey);
        }

        this.Repository.Detach(knownEntity!);

        var entityToSave = this.MergeOriginal(entity, knownEntity!);

        await this.FillSystemFieldsOnUpdateAsync(entityToSave, cancellationToken);

        await this.ValidateAndThrowAsync(entityToSave, knownEntity, UpdateRuleSets);

        var entry = this.Repository.Update(entityToSave);
        await this.UnitOfWork.SaveChangesAsync(cancellationToken);

        if (entry is not null)
        {
            entry.State = EntityState.Detached;
        }

        return entry?.Entity;
    }

    protected virtual async Task FillSystemFieldsOnUpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await this.PrepareEntityAsync(entity, cancellationToken);
    }

    protected virtual void ValidateDeleteIntlParameters(TEntity entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
    }

    public virtual Task<TEntity?> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return this.DeleteIntlAsync(entity, cancellationToken);
    }

    public virtual async Task ValidateAndThrowAsync(TEntity entity, TEntity? originalEntity = null, string[]? ruleSets = null, CancellationToken cancellationToken = default)
    {
        if (this.Validator is not null)
        {
            if (ruleSets?.Any() != true)
            {
                ruleSets = DefaultRuleSets;
            }
            else
            {
                ruleSets = DefaultRuleSets.Concat(ruleSets).Distinct().ToArray();
            }

            var context = await this.CreateValidationContextAsync(entity, originalEntity, ruleSets, cancellationToken);

            var validationResult = await this.Validator.ValidateAsync(context, cancellationToken);
            if (!validationResult.IsValid)
            {
                this.ThrowException(validationResult.Errors, entity, originalEntity);
            }
        }
    }

    protected virtual async Task<TEntity?> DeleteIntlAsync(TEntity entity, CancellationToken cancellationToken)
    {
        this.ValidateDeleteIntlParameters(entity);

        await this.ValidateAndThrowAsync(entity, entity, DeleteRuleSets);

        EntityEntry<TEntity> entry;
        try
        {
            entry = this.Repository.Remove(entity);
            await this.UnitOfWork.SaveChangesAsync(cancellationToken);

            if (entry is not null)
            {
                entry.State = EntityState.Deleted;
            }
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Unable to delete a referenced entity!", ex);
        }

        return entry?.Entity;
    }

    protected TEntity MergeOriginal(TEntity entity, TEntity knownEntity)
    {
        var newEntity = knownEntity.Clone<TEntity>();

        var entityType = this.Repository.FindEntityType();
        var navigationProperties = entityType?.GetNavigations().Select(n => n.Name);

        var iEntityProps = typeof(IEntity).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.FlattenHierarchy);
        var entityProps = typeof(TEntity).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.FlattenHierarchy)
            .Where(ep => !iEntityProps.Any(iep => string.Equals(iep.Name, ep.Name, StringComparison.InvariantCulture)));

        foreach (var propInfo in entityProps)
        {
            if (navigationProperties?.Any(n => string.Equals(n, propInfo.Name, StringComparison.InvariantCulture)) == true)
            {
                propInfo.SetValue(newEntity, null);
            }
            else
            {
                var value = propInfo.GetValue(entity);
                propInfo.SetValue(newEntity, value);
            }
        }

        return newEntity;
    }

    protected virtual Task PrepareEntityAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    protected void ThrowKeyNotFoundException(params IReadOnlyDictionary<string, object>?[] primaryKeyDictArray)
    {
        if (primaryKeyDictArray.Any(pkd => pkd is not null))
        {
            throw new KeyNotFoundException($"{typeof(TEntity).Name} is not found for ({string.Join(", ", primaryKeyDictArray.Where(pkd => pkd is not null).Select(primaryKeyDict => $"[{string.Join(", ", primaryKeyDict!.Select(pkd => $"\"{pkd.Key}\": {pkd.Value}"))}]"))})");
        }
        else
        {
            throw new KeyNotFoundException($"{typeof(TEntity).Name} is not found");
        }
    }

    protected virtual ValueTask<ValidationContext<TEntity>> CreateValidationContextAsync(TEntity entity, TEntity? originalEntity, string[] ruleSets, CancellationToken cancellationToken = default)
    {
        var context = new ValidationContext<TEntity>(entity, new FluentValidation.Internal.PropertyChain(), new FluentValidation.Internal.RulesetValidatorSelector(ruleSets));
        if (originalEntity != null)
        {
            context.RootContextData[Validators.Base.EntityValidator<TEntity>.OriginalEntityKey] = originalEntity;
        }

        return ValueTask.FromResult(context);
    }

    /// <summary>
    /// A két szűrési feltétel összekötése && logikai művelettel
    /// </summary>
    protected Expression CombineExpressions(Expression left, Expression right)
    {
        if (right is LambdaExpression lr)
        {
            right = lr.Body;
        }

        if (left is null)
        {
            return right;
        }
        else
        {
            if (left is LambdaExpression ll)
            {
                left = ll.Body;
            }
        }

        if (right is null)
        {
            return left;
        }

        return Expression.AndAlso(left, right);
    }

    /// <summary>
    /// A kapott kifejezésben kicseréli a Parameter expression-t a megadott paraméterre
    /// Támogatott szűrési műveletek:
    /// - <see cref="ExpressionType.Equal"/>
    /// - <see cref="ExpressionType.NotEqual"/>
    /// - <see cref="ExpressionType.GreaterThan"/>
    /// - <see cref="ExpressionType.GreaterThanOrEqual"/>
    /// - <see cref="ExpressionType.LessThanOrEqual"/>
    /// - <see cref="ExpressionType.LessThanOrEqual"/>
    /// - egyéb esetben, a <paramref name="func"/> kerül meghívásra
    /// </summary>
    /// <param name="expr">Kifejezés</param>
    /// <param name="parm">Új paraméter</param>
    /// <param name="func">A használt logikai művelet</param>
    /// <returns>A módosított kifejezés</returns>
    protected Expression ReplaceParameter(ParameterExpression parm, Expression expr, Func<Expression, Expression, Expression>? func = null!)
    {
        if (expr is not LambdaExpression expr1)
        {
            return expr;
        }

        if (expr1.Body is not BinaryExpression expr2)
        {
            return expr;
        }

        if (expr2.Left is not MemberExpression leftExpr)
        {
            return expr;
        }

        leftExpr = Expression.MakeMemberAccess(parm, leftExpr.Member);

        return expr2.NodeType switch
        {
            ExpressionType.Equal => Expression.Equal(leftExpr, expr2.Right),
            ExpressionType.NotEqual => Expression.NotEqual(leftExpr, expr2.Left),
            ExpressionType.GreaterThan => Expression.GreaterThan(leftExpr, expr2.Right),
            ExpressionType.GreaterThanOrEqual => Expression.GreaterThanOrEqual(leftExpr, expr2.Left),
            ExpressionType.LessThan => Expression.LessThan(leftExpr, expr2.Right),
            ExpressionType.LessThanOrEqual => Expression.LessThanOrEqual(leftExpr, expr2.Left),
            _ => func?.Invoke(leftExpr, expr2.Right)!
        };
    }

    /// <summary>
    /// Szűrési feltétel létrehozása
    /// Támogatott szűrési műveletek:
    /// - <see cref="ExpressionType.Equal"/>
    /// - <see cref="ExpressionType.NotEqual"/>
    /// - <see cref="ExpressionType.GreaterThan"/>
    /// - <see cref="ExpressionType.GreaterThanOrEqual"/>
    /// - <see cref="ExpressionType.LessThanOrEqual"/>
    /// - <see cref="ExpressionType.LessThanOrEqual"/>
    /// - egyéb esetben, a <paramref name="func"/> kerül meghívásra
    /// </summary>
    /// <param name="parm">Tábla paraméter kifejezés</param>
    /// <param name="predicate">Szűrési feltétel</param>
    /// <param name="func">Egyedi szűrési művelet létrehozása</param>
    /// <returns>Létrehozott szűrési feltétel</returns>
    protected Expression CreateQueryPredicate(ParameterExpression parm, Expression<Func<TEntity, bool>> predicate, Func<Expression, Expression, Expression>? func = null!)
    {
        return this.ReplaceParameter(parm, predicate, func);
    }

    /// <summary>
    /// Entity mező elérés kifejezés létrehozása
    /// (ha a <paramref name="propertyType"/> nem egyezik meg a mező tíusával, akkor <see cref="UnaryExpression"/> kerül létrehozása)
    /// </summary>
    /// <param name="parm">Entity kifejezés</param>
    /// <param name="name">Mező neve</param>
    /// <param name="propertyType">Mező értékének konvertálási típusa</param>
    /// <returns>Mező elérés kifejezés (<see cref="MemberExpression"/>)</returns>
    private Expression CreateFieldAccessExpression(ParameterExpression parm, string name, Type propertyType)
    {
        var entityType = parm.Type;
        var prop = entityType.GetProperty(name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty);
        if (prop is not null)
        {
            Expression expr = Expression.MakeMemberAccess(parm, prop);
            if (prop.PropertyType != propertyType)
            {
                expr = Expression.Convert(expr, propertyType);
            }

            return expr;
        }

        return null!;
    }

    /// <summary>
    /// Konstans érték kifejezés létrehozása
    /// </summary>
    /// <param name="value">Érték</param>
    /// <param name="propertyType">Kovertálás típusa</param>
    /// <returns>Konstans kifejezés (<see cref="UnaryExpression"/>)</returns>
    private Expression CreateConstantExpression(object? value, Type propertyType)
    {
        return Expression.Convert(Expression.Constant(value), propertyType);
    }

    /// <summary>
    /// Logikai kifejezés létrehozása
    /// </summary>
    /// <param name="parm">Entity kifejezés</param>
    /// <param name="prop">Mező információk</param>
    /// <param name="value">Szűrendő érték</param>
    /// <param name="binaryType">Logikai művelet</param>
    /// <param name="fieldName">Entity mező neve</param>
    /// <returns>Egyenlő kifejezés (<see cref="BinaryExpression"/>)</returns>
    private Expression CreateQueryBinaryExpr(ParameterExpression parm, System.Reflection.PropertyInfo prop, object? value, ExpressionType binaryType, string? fieldName)
    {
        fieldName = fieldName ?? prop.Name;
        var left = this.CreateFieldAccessExpression(parm, fieldName, prop.PropertyType);
        if (left != null)
        {
            var right = this.CreateConstantExpression(value, prop.PropertyType);
            return Expression.MakeBinary(binaryType, left, right);
        }

        return null!;
    }

    /// <summary>
    /// Szűrési kifejezés létrehozása
    /// </summary>
    /// <typeparam name="TQuery">Szűrési információkat tartalmazó konténer osztály típusa</typeparam>
    /// <param name="parm">Entity kifejezés</param>
    /// <param name="expr">Eddigi szűrési kifejezések</param>
    /// <param name="prop">Mező információk</param>
    /// <param name="query">Szűrési információk</param>
    /// <param name="attr">Logikai mávelet információk</param>
    /// <returns>Létrehozott szűrési művelet (<see cref="BinaryExpression"/>)</returns>
    /// <exception cref="ArgumentOutOfRangeException">Nem támogatott szűrési művelet esetén</exception>
    private Expression CreateQueryExpression<TQuery>(ParameterExpression parm, Expression expr, System.Reflection.PropertyInfo prop, TQuery query, BusinessEntities.Helpers.QueryOperationAttribute? attr = null)
    {
        var binaryType = attr?.ExpressionType;
        if (binaryType is null)
        {
            binaryType = ExpressionType.Equal;
        }

        var value = prop.GetValue(query);
        if (value is not null)
        {
            var expr2 = this.CreateQueryBinaryExpr(parm, prop, value, binaryType.Value, attr?.FieldName);
            expr = this.CombineExpressions(expr, expr2);
        }

        return expr;
    }

    /// <summary>
    /// Szűrési kifejezés létrehozása a <typeparamref name="TQuery"/> konténer össze adata alapján
    /// </summary>
    /// <typeparam name="TQuery">Szűrési információkat tartalmazó konténer osztály típusa</typeparam>
    /// <param name="parm">Entity kifejezés</param>
    /// <param name="query">Szűrési információk</param>
    /// <returns>Létrehozott szűrési műveletek (<see cref="BinaryExpression"/>)</returns>
    private Expression CreateQueryExpression<TQuery>(ParameterExpression parm, TQuery query)
    {
        Expression expr = null!;

        var queryOperationAttrType = typeof(BusinessEntities.Helpers.QueryOperationAttribute);
        var props = typeof(TQuery).GetProperties()
            .Select(p => new
            {
                prop = p,
                attr = p.GetCustomAttributes(queryOperationAttrType, true).FirstOrDefault() as BusinessEntities.Helpers.QueryOperationAttribute
            });
        foreach (var prop in props)
        {
            expr = this.CreateQueryExpression(parm, expr, prop.prop, query, prop.attr);
        }

        return expr;
    }

    /// <summary>
    /// Szűrési kifejezés létrehozása a <typeparamref name="TQuery"/> konténer össze adata alapján
    /// </summary>
    /// <typeparam name="TQuery">Szűrési információkat tartalmazó konténer osztály típusa</typeparam>
    /// <param name="query">Szűrési információk</param>
    /// <returns>Létrehozott szűrési műveletek (<see cref="Expression{Func{TEntity, bool}}"/>)</returns>
    protected Expression<Func<TEntity, bool>> CreatePredicate<TQuery>(TQuery query)
    {
        if (query is not null)
        {
            var parm = Expression.Parameter(typeof(TEntity), "entity");

            var expr = this.CreateQueryExpression(parm, query);
            if (expr is not null)
            {
                var lambda = Expression.Lambda<Func<TEntity, bool>>(expr, parm);
                lambda.Reduce();
                return lambda;
            }
        }

        return null!;
    }
}
