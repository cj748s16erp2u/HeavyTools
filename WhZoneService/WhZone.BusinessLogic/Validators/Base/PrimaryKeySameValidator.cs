using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model.Interfaces;
using eLog.HeavyTools.Services.WhZone.DataAccess.Context;
using FluentValidation;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Validators.Base;

public class PrimaryKeySameValidator<TEntity> : PrimaryKeyValidator<TEntity>
   where TEntity : class, IEntity
{
    public override string Name => "PrimaryKeySameValidator";

    public PrimaryKeySameValidator(WhZoneDbContext dbContext) : base(dbContext)
    {
    }

    public override string GetDefaultMessageTemplate(string errorCode) => this.Localized(errorCode, this.Name);

    public override bool IsValid(ValidationContext<TEntity> context, TEntity value)
    {
        var originalEntity = context.GetOriginalEntity();
        if (originalEntity is null)
        {
            return false;
        }

        var primaryKey = this.GetPrimaryKey(value);
        if (primaryKey is null)
        {
            return false;
        }

        var origPrimaryKey = this.GetPrimaryKey(originalEntity);
        if (origPrimaryKey.Count != primaryKey.Count)
        {
            return false;
        }

        return !origPrimaryKey
            .GroupJoin(primaryKey, opk => opk.Key, pk => pk.Key, (opk, pks) => new { orig = opk, pk = pks.FirstOrDefault() }, StringComparer.InvariantCultureIgnoreCase)
            .Any(x => x.orig.Value != x.pk.Value);
    }
}
