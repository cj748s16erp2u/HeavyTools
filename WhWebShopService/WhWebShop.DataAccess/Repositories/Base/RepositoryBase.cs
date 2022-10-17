using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Context;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Base;

/// <summary>
///     Generic base repository.
/// </summary>
/// <typeparam name="TEntity">Type of the entity</typeparam>
/// <seealso cref="IRepository{TEntity}"/>
public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    /// <summary>
    /// Get the database context
    /// </summary>
    private readonly WhWebShopDbContext dbContext;
     
    /// <summary>
    /// Gets the database set
    /// </summary>
    protected readonly DbSet<TEntity> dbSet;

    /// <summary>
    /// Gets the queryable entities with the default filter
    /// </summary>
    public virtual IQueryable<TEntity> Entities { get; protected set; }

    /// <summary>
    /// Initializing a new instance of the <see cref="RepositoryBase{TEntity}"/> class.
    /// </summary>
    /// <param name="dbContext">The database context</param>
    /// <exception cref="ArgumentNullException">dbContext</exception>
    public RepositoryBase(WhWebShopDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        this.dbSet = this.dbContext.Set<TEntity>();
        this.Entities = this.dbSet.AsQueryable();
    }



    /// <summary>
    ///     Finds an entity with the given primary key values. If an entity with the given primary key values
    ///     is being tracked by the context, then it is returned immediately without making a request to the
    ///     database. Otherwise, a query is made to the database for an entity with the given primary key values
    ///     and this entity, if found, is attached to the context and returned. If no entity is found, then
    ///     null is returned.
    /// </summary>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <returns>The entity found, or <see langword="null" />.</returns>
    public virtual async ValueTask<TEntity?> FindAsync(params object[] keyValues)
    {
        var entity = await this.dbSet.FindAsync(keyValues);
        if (entity is not null)
        {
            this.Detach(entity);
        }

        return entity;
    }

    /// <summary>
    ///     Finds an entity with the given primary key values. If an entity with the given primary key values
    ///     is being tracked by the context, then it is returned immediately without making a request to the
    ///     database. Otherwise, a query is made to the database for an entity with the given primary key values
    ///     and this entity, if found, is attached to the context and returned. If no entity is found, then
    ///     null is returned.
    /// </summary>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>The entity found, or <see langword="null" />.</returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public virtual async ValueTask<TEntity?> FindAsync(object[] keyValues, CancellationToken cancellationToken = default)
    {
        var entity = await this.dbSet.FindAsync(keyValues, cancellationToken);
        if (entity is not null)
        {
            this.Detach(entity);
        }

        return entity;
    }

    /// <summary>
    ///     <para>
    ///         Begins tracking the given entity, and any other reachable entities that are
    ///         not already being tracked, in the <see cref="EntityState.Added" /> state such that they will
    ///         be inserted into the database when <see cref="DbContext.SaveChanges()" /> is called.
    ///     </para>
    ///     <para>
    ///         This method is async only to allow special value generators, such as the one used by
    ///         'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo',
    ///         to access the database asynchronously. For all other cases the non async method should be used.
    ///     </para>
    ///     <para>
    ///         Use <see cref="EntityEntry.State" /> to set the state of only a single entity.
    ///     </para>
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A task that represents the asynchronous Add operation. The task result contains the
    ///     <see cref="EntityEntry{TEntity}" /> for the entity. The entry provides access to change tracking
    ///     information and operations for the entity.
    /// </returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public virtual ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return this.dbSet.AddAsync(entity, cancellationToken);
    }

    /// <summary>
    ///     <para>
    ///         Begins tracking the given entities, and any other reachable entities that are
    ///         not already being tracked, in the <see cref="EntityState.Added" /> state such that they will
    ///         be inserted into the database when <see cref="DbContext.SaveChanges()" /> is called.
    ///     </para>
    ///     <para>
    ///         This method is async only to allow special value generators, such as the one used by
    ///         'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo',
    ///         to access the database asynchronously. For all other cases the non async method should be used.
    ///     </para>
    /// </summary>
    /// <param name="entities">The entities to add.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public virtual async ValueTask<IEnumerable<EntityEntry<TEntity>>> AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default, bool returnEntries = true)
    {
        await this.dbSet.AddRangeAsync(entities, cancellationToken);

        return returnEntries ? entities.Select(e => this.dbContext.Entry(e)).ToList() : null!;
    }

    /// <summary>
    ///     <para>
    ///         Begins tracking the given entity and entries reachable from the given entity using
    ///         the <see cref="EntityState.Modified" /> state by default, but see below for cases
    ///         when a different state will be used.
    ///     </para>
    ///     <para>
    ///         Generally, no database interaction will be performed until <see cref="DbContext.SaveChanges()" /> is called.
    ///     </para>
    ///     <para>
    ///         A recursive search of the navigation properties will be performed to find reachable entities
    ///         that are not already being tracked by the context. All entities found will be tracked
    ///         by the context.
    ///     </para>
    ///     <para>
    ///         For entity types with generated keys if an entity has its primary key value set
    ///         then it will be tracked in the <see cref="EntityState.Modified" /> state. If the primary key
    ///         value is not set then it will be tracked in the <see cref="EntityState.Added" /> state.
    ///         This helps ensure new entities will be inserted, while existing entities will be updated.
    ///         An entity is considered to have its primary key value set if the primary key property is set
    ///         to anything other than the CLR default for the property type.
    ///     </para>
    ///     <para>
    ///         For entity types without generated keys, the state set is always <see cref="EntityState.Modified" />.
    ///     </para>
    ///     <para>
    ///         Use <see cref="EntityEntry.State" /> to set the state of only a single entity.
    ///     </para>
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>
    ///     The <see cref="EntityEntry" /> for the entity. The entry provides
    ///     access to change tracking information and operations for the entity.
    /// </returns>
    public virtual EntityEntry<TEntity> Update(TEntity entity)
    {
        return this.dbSet.Update(entity);
    }

    /// <summary>
    ///     Begins tracking the given entity in the <see cref="EntityState.Deleted" /> state such that it will
    ///     be removed from the database when <see cref="DbContext.SaveChanges()" /> is called.
    /// </summary>
    /// <param name="entity">The entity to remove.</param>
    /// <returns>
    ///     The <see cref="EntityEntry{TEntity}" /> for the entity. The entry provides
    ///     access to change tracking information and operations for the entity.
    /// </returns>
    public virtual EntityEntry<TEntity> Remove(TEntity entity)
    {
        return this.dbSet.Remove(entity);
    }

    /// <summary>
    /// Marks the given entity as <see cref="EntityState.Detached"/>.
    /// </summary>
    /// <param name="entity">Entity which will be marked as <see cref="EntityState.Detached"/></param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="entity"/> is not found in the database.</exception>
    public virtual void Detach(TEntity entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        var entry = this.dbContext.Entry(entity);
        if (entry is null)
        {
            throw new ArgumentOutOfRangeException(nameof(entity));
        }

        entry.State = EntityState.Detached;
    }

    /// <summary>
    /// Marks all tracked entities as <see cref="EntityState.Detached"/>.
    /// </summary>
    public virtual void DetachAllEntities()
    {
        var changedEntriesCopy = this.dbContext.ChangeTracker.Entries<TEntity>()
            .Where(e => e.State != EntityState.Detached)
            .ToList();

        foreach (var entry in changedEntriesCopy)
        {
            entry.State = EntityState.Detached;
        }
    }

    /// <summary>
    ///     Gets the entity that maps the given entity class. Returns <see langword="null" /> if no entity type with
    ///     the given CLR type is found or the given CLR type is being used by shared type entity type
    ///     or the entity type has a defining navigation.
    /// </summary>
    /// <param name="type">The type to find the corresponding entity type for.</param>
    /// <returns>The entity type, or <see langword="null" /> if none is found.</returns>
    public virtual IEntityType? FindEntityType()
    {
        return this.dbContext.Model.FindEntityType(typeof(TEntity));
    }

    /// <summary>
    ///     Gets primary key field names for this entity type. Returns <see langword="null" /> if no primary key is defined.
    /// </summary>
    /// <returns>The primary key field names, or <see langword="null" /> if none is defined.</returns>
    public virtual IEnumerable<string> GetPrimaryKeyFields()
    {
        return this.dbContext.GetPrimaryKeyFields<TEntity>();
    }

    /// <summary>
    ///     Gets primary key values for the given entity. Returns <see langword="null" /> if no primary key is defined.
    /// </summary>
    /// <returns>The primary key values, or <see langword="null" /> if none is defined.</returns>
    public virtual IEnumerable<object> GetPrimaryKeyValues(TEntity entity)
    {
        return this.dbContext.GetPrimaryKeyValues(entity);
    }

    /// <summary>
    ///     Gets primary key for the given entity. Returns <see langword="null" /> if no primary key is defined.
    /// </summary>
    /// <returns>The primary key, or <see langword="null" /> if none is defined.</returns>
    public virtual IReadOnlyDictionary<string, object> GetPrimaryKey(TEntity entity)
    {
        return this.dbContext.GetPrimaryKey(entity);
    }

    /// <summary>
    ///     Returns the name of the table to which the entity type is mapped
    ///     or <see langword="null" /> if not mapped to a table.
    /// </summary>
    /// <param name="includeSchema">Indicates that the result should contain the table's schema.</param>
    /// <returns>The name of the table to which the entity type is mapped.</returns>
    public virtual string GetTableName(bool includeSchema = true)
    {
        var entityType = this.dbContext.Model.FindEntityType(typeof(TEntity));
        if (entityType is not null)
        {
            var tableName = $"[{entityType.GetTableName()}]";
            if (includeSchema && !string.IsNullOrWhiteSpace(entityType.GetSchema()))
            {
                tableName = $"[{entityType.GetSchema()}].{tableName}";
            }

            return tableName;
        }

        return null!;
    }

    /// <summary>
    ///     <para>
    ///         Executes the given SQL against the database and returns the number of rows affected.
    ///     </para>
    ///     <para>
    ///         Note that this method does not start a transaction. To use this method with
    ///         a transaction, first call <see cref="BeginTransaction" /> or <see cref="O:UseTransaction" />.
    ///     </para>
    ///     <para>
    ///         Note that the current <see cref="ExecutionStrategy" /> is not used by this method
    ///         since the SQL may not be idempotent and does not run in a transaction. An <see cref="ExecutionStrategy" />
    ///         can be used explicitly, making sure to also use a transaction if the SQL is not
    ///         idempotent.
    ///     </para>
    ///     <para>
    ///         As with any API that accepts SQL it is important to parameterize any user input to protect against a SQL injection
    ///         attack. You can include parameter place holders in the SQL query string and then supply parameter values as additional
    ///         arguments. Any parameter values you supply will automatically be converted to a DbParameter:
    ///     </para>
    ///     <code>
    ///          var userSuppliedSearchTerm = ".NET";
    ///          context.Database.ExecuteSqlInterpolatedAsync($"UPDATE Blogs SET Rank = 50 WHERE Name = {userSuppliedSearchTerm})");
    ///      </code>
    /// </summary>
    /// </remarks>
    /// <param name="sql">The interpolated string representing a SQL query with parameters.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result is the number of rows affected.
    /// </returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public virtual Task<int> ExecuteSqlCommandAsync(FormattableString sql, CancellationToken cancellationToken = default)
        => this.dbContext.Database.ExecuteSqlInterpolatedAsync(sql, cancellationToken);

    /// <summary>
    ///     <para>
    ///         Creates a LINQ query based on an interpolated string representing a SQL query.
    ///     </para>
    ///     <para>
    ///         If the database provider supports composing on the supplied SQL, you can compose on top of the raw SQL query using
    ///         LINQ operators:
    ///     </para>
    ///     <code>context.Blogs.FromSqlInterpolated($"SELECT * FROM Blogs").OrderBy(b => b.Name)</code>
    ///     <para>
    ///         As with any API that accepts SQL it is important to parameterize any user input to protect against a SQL injection
    ///         attack. You can include interpolated parameter place holders in the SQL query string. Any interpolated parameter values
    ///         you supply will automatically be converted to a <see cref="DbParameter" />:
    ///     </para>
    ///     <code>context.Blogs.FromSqlInterpolated($"SELECT * FROM Blogs WHERE Name = {userSuppliedSearchTerm}")</code>
    /// </summary>
    /// <typeparam name="TQuery">The type of the elements of <paramref name="source" />.</typeparam>
    /// <param name="source">
    ///     An <see cref="IQueryable{T}" /> to use as the base of the interpolated string SQL query (typically a <see cref="DbSet{TEntity}" />).
    /// </param>
    /// <param name="sql">The interpolated string representing a SQL query with parameters.</param>
    /// <returns>An <see cref="IQueryable{T}" /> representing the interpolated string SQL query.</returns>
    public virtual IQueryable<TQuery> ExecuteSql<TQuery>(FormattableString sql)
        where TQuery : class
        => this.dbContext.Set<TQuery>().FromSqlInterpolated(sql);

    public IQueryable<TEntity>? FromSql(string sql)
    {
        return this.dbSet.FromSqlRaw(sql, new object[0]);
    }

    public Task<int> ExecuteStoredProcedure(string name, Dictionary<string, object> parms)
    {
        var spl = new List<SqlParameter>();
        foreach (var parameter in parms)
        {
            spl.Add(new SqlParameter(parameter.Key, parameter.Value));
        }
        return ExecuteStoredProcedure(name, spl);
    }

    public Task<int> ExecuteStoredProcedure(string name, List<SqlParameter> spl)
    { 
        var sql = name + " " + String.Join(", ", spl.Select(x =>
         $"@{x.ParameterName} = @{x.ParameterName}" +
         (x.Direction == ParameterDirection.Output ? " OUT" : "")
         ));
        return dbContext.Database.ExecuteSqlRawAsync(sql, spl.ToArray()); 
    }

    public Task<int> ExecuteSql(string sql, CancellationToken cancellationToken)
    {
        return dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken);
    }
}
