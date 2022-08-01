using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.DataAccess.Context;
using eLog.HeavyTools.Services.WhZone.DataAccess.Repositories.Interfaces;

namespace eLog.HeavyTools.Services.WhZone.DataAccess.Repositories.Base
{
    /// <summary>
    /// Unit of work class.
    /// </summary>
    /// <seealso cref="IUnitOfWork"/>
    //[System.Diagnostics.DebuggerStepThrough]
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        /// <summary>
        /// Gets the database context
        /// </summary>
        private readonly WhZoneDbContext dbContext;

        /// <summary>
        /// Gets the currently used transaction
        /// </summary>
        private readonly Stack<WhZoneDbContext.Transaction> transactions = new Stack<WhZoneDbContext.Transaction>();

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <exception cref="ArgumentNullException">dbContext</exception>
        public UnitOfWork(WhZoneDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <returns>The number of state entities written to the database.</returns>
        public int SaveChanges()
        {
            return this.dbContext.SaveChanges();
        }

        /// <summary>
        /// Asynchronously saves all changes made in this context to the database.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return this.dbContext.SaveChangesAsync(cancellationToken);
        }

        ///// <summary>
        ///// Starts a new transaction.
        ///// </summary>
        //public void BeginTransaction()
        //{
        //    this.transactions.Push(this.dbContext.BeginTransaction());
        //}

        /// <summary>
        /// Asyncronously starts a new transaction
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous start operation.</returns>
        public async Task<WhZoneDbContext.Transaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            var transaction = await this.dbContext.BeginTransactionAsync(cancellationToken).ConfigureAwait(true);
            this.transactions.Push(transaction);
            return transaction;
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws when no transaction exists.</exception>
        protected void Commit()
        {
            if (this.transactions.TryPop(out var transaction))
            {
                transaction.Commit();
                transaction.Dispose();
            }
            else
            {
                throw new InvalidOperationException("No transaction exists.");
            }
        }

        /// <summary>
        /// Rolls back the transaction.
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws when no transaction exists.</exception>
        protected void Rollback()
        {
            if (this.transactions.TryPop(out var transaction))
            {
                transaction.Dispose();
            }
            else
            {
                throw new InvalidOperationException("No transaction exists.");
            }
        }

        /// <summary>
        /// Gets a value indicating whether a transaction is exists.
        /// </summary>
        /// <returns></returns>
        protected bool HasTransaction()
        {
            return this.transactions.Count > 0;
        }

        public void Dispose()
        {
            if (this.HasTransaction())
            {
                this.Rollback();
            }

            GC.SuppressFinalize(this);
        }
    }
}
