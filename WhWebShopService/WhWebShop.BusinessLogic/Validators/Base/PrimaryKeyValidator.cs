using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Context;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Validators.Base;

public abstract class PrimaryKeyValidator<TEntity> : IPropertyValidator<TEntity, TEntity>
    where TEntity : class, IEntity
{
    private readonly WhWebShopDbContext dbContext;

    public virtual string Name => "PrimaryKeyValidator";

    protected PrimaryKeyValidator(WhWebShopDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public virtual string GetDefaultMessageTemplate(string errorCode) => "No default error message has been specified";

    protected IReadOnlyDictionary<string, object> GetPrimaryKey(TEntity entity) => this.dbContext.GetPrimaryKey(entity);

    public abstract bool IsValid(ValidationContext<TEntity> context, TEntity value);

    protected string Localized(string errorCode, string fallbackKey)
    {
        if (errorCode != null)
        {
            var result = ValidatorOptions.Global.LanguageManager.GetString(errorCode);

            if (!string.IsNullOrWhiteSpace(result))
            {
                return result;
            }
        }

        return ValidatorOptions.Global.LanguageManager.GetString(fallbackKey);
    }
}
