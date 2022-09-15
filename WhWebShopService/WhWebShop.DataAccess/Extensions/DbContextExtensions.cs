using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.EntityFrameworkCore;

[System.Diagnostics.DebuggerStepThrough]
public static class DbContextExtensions
{
    /// <summary>
    ///     Gets primary key field names for this entity type. Returns <see langword="null" /> if no primary key is defined.
    /// </summary>
    /// <returns>The primary key field names, or <see langword="null" /> if none is defined.</returns>
    public static IEnumerable<string> GetPrimaryKeyFields<T>(this DbContext dbContext)
        where T : class, IEntity
    {
        if (dbContext is null)
        {
            throw new ArgumentNullException(nameof(dbContext));
        }

        var entityType = dbContext.Model.FindEntityType(typeof(T));
        if (entityType is null)
        {
            return null!;
        }

        var primaryKey = entityType.FindPrimaryKey();
        if (primaryKey is null)
        {
            return null!;
        }

        return primaryKey.Properties.Select(p => p.Name).ToList();
    }

    /// <summary>
    ///     Gets primary key values for the given entity. Returns <see langword="null" /> if no primary key is defined.
    /// </summary>
    /// <returns>The primary key values, or <see langword="null" /> if none is defined.</returns>
    public static IEnumerable<object> GetPrimaryKeyValues<T>(this DbContext dbContext, T entity, IEnumerable<string> primaryKeyFields = null!)
        where T : class, IEntity
    {
        if (dbContext is null)
        {
            throw new ArgumentNullException(nameof(dbContext));
        }

        if (entity is null)
        {
            return null!;
        }

        if (primaryKeyFields is null)
        {
            primaryKeyFields = dbContext.GetPrimaryKeyFields<T>();
        }

        if (primaryKeyFields is null)
        {
            return null!;
        }

        var type = typeof(T);
        var propertyInfos = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.FlattenHierarchy)
            .Where(p => primaryKeyFields.Any(pkf => string.Equals(pkf, p.Name, StringComparison.InvariantCultureIgnoreCase)));
        return propertyInfos.Select(pi => pi.GetValue(entity)!).ToList();
    }

    /// <summary>
    ///     Gets primary key for the given entity. Returns <see langword="null" /> if no primary key is defined.
    /// </summary>
    /// <returns>The primary key, or <see langword="null" /> if none is defined.</returns>
    public static IReadOnlyDictionary<string, object> GetPrimaryKey<T>(this DbContext dbContext, T entity, IEnumerable<string> primaryKeyFields = null!)
        where T : class, IEntity
    {
        if (dbContext is null)
        {
            throw new ArgumentNullException(nameof(dbContext));
        }

        if (entity is null)
        {
            return null!;
        }

        if (primaryKeyFields is null)
        {
            primaryKeyFields = dbContext.GetPrimaryKeyFields<T>();
        }

        if (primaryKeyFields is null)
        {
            return null!;
        }

        var type = typeof(T);
        var propertyInfos = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.FlattenHierarchy)
            .Where(p => primaryKeyFields.Any(pkf => string.Equals(pkf, p.Name, StringComparison.InvariantCultureIgnoreCase)));
        return propertyInfos.Select(pi => new { pi.Name, Value = pi.GetValue(entity) }).ToDictionary(x => x.Name, x => x.Value!);
    }
}
