using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.DataAccess.Context;

namespace eLog.HeavyTools.Services.WhZone.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Interface of the unit of work class.
    /// </summary>
    public interface IUnitOfWork
    {
        ///// <summary>
        ///// Starts a new transaction.
        ///// </summary>
        //void BeginTransaction();

        /// <summary>
        /// Asyncronously starts a new transaction
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous start operation.</returns>
        Task<WhZoneDbContext.Transaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        ///// <summary>
        ///// Commits the transaction.
        ///// </summary>
        ///// <exception cref="InvalidOperationException">Throws when no transaction exists.</exception>
        //void Commit();

        ///// <summary>
        ///// Rolls back the transaction.
        ///// </summary>
        ///// <exception cref="InvalidOperationException">Throws when no transaction exists.</exception>
        //void Rollback();

        ///// <summary>
        ///// Gets a value indicating whether a transaction is exists.
        ///// </summary>
        ///// <returns></returns>
        //bool HasTransaction();

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <returns>The number of state entities written to the database.</returns>
        int SaveChanges();

        /// <summary>
        /// Asynchronously saves all changes made in this context to the database.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
