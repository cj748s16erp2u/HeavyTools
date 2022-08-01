using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
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

    public virtual async Task ValidateAndThrowAsync(TEntity entity, TEntity? originalEntity = null, params string[] ruleSets)
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

            var context = this.CreateValidationContext(entity, originalEntity, ruleSets);

            var validationResult = await this.Validator.ValidateAsync(context);
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

    protected virtual ValidationContext<TEntity> CreateValidationContext(TEntity entity, TEntity? originalEntity, string[] ruleSets)
    {
        var context = new ValidationContext<TEntity>(entity, new FluentValidation.Internal.PropertyChain(), new FluentValidation.Internal.RulesetValidatorSelector(ruleSets));
        if (originalEntity != null)
        {
            context.RootContextData[Validators.Base.EntityValidator<TEntity>.OriginalEntityKey] = originalEntity;
        }

        return context;
    }
}
