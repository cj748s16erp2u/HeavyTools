using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model.Interfaces;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;

public interface ILogicService<TEntity>
    where TEntity : class, IEntity
{
    ValueTask<TEntity?> GetByIdAsync(object keyValue, CancellationToken cancellationToken = default);
    ValueTask<TEntity?> GetByIdAsync(object[] keyValues, CancellationToken cancellationToken = default);
    /// <summary>
    /// A megadott feltétel alapján 1 bejegyzés betöltése.
    /// Több találat esetén kivétel kerül kiváltásra.
    /// </summary>
    /// <param name="predicate">A feltétel</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A betöltött bejegyzés vagy null</returns>
    /// <exception cref="InvalidOperationException"><paramref name="source" /> contains more than one element.</exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    ValueTask<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate = null!);
    Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> predicate = null!, CancellationToken cancellationToken = default);
    Task<TEntity?> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity?> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity?> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task ValidateAndThrowAsync(TEntity entity, TEntity? originalEntity = null, string[]? ruleSets = null, CancellationToken cancellationToken = default);
}
