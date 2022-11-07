using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhZone.DataAccess.Context;
using FluentValidation;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Validators.Base;

[RegisterDI(Interface = typeof(IValidator<>))]
public class EntityValidator<TEntity> : AbstractValidator<TEntity>
    where TEntity : class, IEntity
{
    public const string DefaultRuleSet = "Default";
    public const string AddRuleSet = "Add";
    public const string UpdateRuleSet = "Update";
    public const string DeleteRuleSet = "Delete";
    public const string OriginalEntityKey = "OriginalEntity";

    protected readonly WhZoneDbContext dbContext;

    public EntityValidator(WhZoneDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        this.AddRules();
    }

    private void AddRules()
    {
        this.BeforeAddRules();

        this.RuleSet(DefaultRuleSet, () => this.AddDefaultRules());
        this.RuleSet(AddRuleSet, () => this.AddAddRules());
        this.RuleSet(UpdateRuleSet, () => this.AddUpdateRules());
        this.RuleSet(DeleteRuleSet, () => this.AddDeleteRules());

        this.AfterAddRules();
    }

    protected virtual void BeforeAddRules()
    {
    }

    protected virtual void AfterAddRules()
    {
    }

    protected virtual void AddDefaultRules()
    {
    }

    protected virtual void AddAddRules()
    {
    }

    protected virtual void AddUpdateRules()
    {
    }

    protected virtual void AddDeleteRules()
    {
    }

    protected virtual string GetEntityKey<T>(string? key = null)
        where T : class, IEntity
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            key = typeof(T).Name;
        }

        return key;
    }

    protected virtual bool AddEntity<T>(ValidationContext<TEntity> context, T entity, string? key = null)
        where T : class, IEntity
    {
        key = this.GetEntityKey<T>(key);

        return context.RootContextData.TryAdd(key, entity);
    }

    protected virtual T? GetEntity<T>(ValidationContext<TEntity> context, string? key = null)
        where T : class, IEntity
    {
        key = this.GetEntityKey<T>(key);

        if (context.RootContextData.TryGetValue(key, out var o) && o is T entity)
        {
            return entity;
        }

        return null;
    }

    protected virtual bool RemoveEntity(ValidationContext<TEntity> context, string? key = null)
    {
        return context.RootContextData.Remove(key);
    }
}
