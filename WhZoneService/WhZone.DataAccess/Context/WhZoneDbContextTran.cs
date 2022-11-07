using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace eLog.HeavyTools.Services.WhZone.DataAccess.Context;

[System.Diagnostics.DebuggerStepThrough]
public partial class WhZoneDbContext
{
    /// <summary>
    /// The current depth of the transaction
    /// </summary>
    private int transactionDepth;

    /// <summary>
    /// The used transaction
    /// </summary>
    private IDbContextTransaction? transaction;


    /// <summary>
    /// Starts a new transaction.
    /// </summary>
    /// <returns>Transaction wrapper.</returns>
    public Transaction BeginTransaction()
    {
        if (this.transactionDepth == 0)
        {
            this.transaction = this.Database.BeginTransaction();
        }

        this.transactionDepth++;

        return new Transaction(this, this.transactionDepth);
    }

    /// <summary>
    /// Asyncronously starts a new transaction.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the transaction wrapper.</returns>
    public async Task<Transaction> BeginTransactionAsync(System.Data.IsolationLevel isolationLevel = System.Data.IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
    {
        if (this.transactionDepth == 0)
        {
            this.transaction = await this.Database.BeginTransactionAsync(isolationLevel, cancellationToken).ConfigureAwait(true);
        }

        this.transactionDepth++;

        return new Transaction(this, this.transactionDepth);
    }

    public override void Dispose()
    {
        base.Dispose();

        if (this.transaction != null)
        {
            this.transaction.Dispose();
            this.transaction = null;
        }

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Commits the transaction when the depths count riches 0.
    /// </summary>
    /// <param name="depth">The calling transaction wrapper's depth</param>
    private void Commit(int depth)
    {
        if (this.transactionDepth >= depth)
        {
            this.transactionDepth = depth;

            if (--this.transactionDepth <= 0)
            {
                if (this.transaction != null)
                {
                    this.transaction.Commit();
                    this.transaction.Dispose();
                    this.transaction = null;
                }
                this.transactionDepth = 0;
            }
        }
    }

    /// <summary>
    /// Rolls back the transaction when the depths count riches 0.
    /// </summary>
    /// <param name="depth">The calling transaction wrapper's depth</param>
    private void Rollback(int depth)
    {
        if (this.transactionDepth >= depth)
        {
            this.transactionDepth = depth;

            if (--this.transactionDepth <= 0)
            {
                if (this.transaction != null)
                {
                    this.transaction.Rollback();
                    this.transaction.Dispose();
                    this.transaction = null;
                }
                this.transactionDepth = 0;
            }
        }
    }

    /// <summary>
    /// Transaction wrapper which allow the nested transactions.
    /// </summary>
    public sealed class Transaction : IDisposable
    {
        /// <summary>
        /// Gets the database context
        /// </summary>
        private readonly WhZoneDbContext dbContext;

        /// <summary>
        /// The current depth of the transaction
        /// </summary>
        private readonly int transactionCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="transactionCount">The current depth of the transaction.</param>
        /// <exception cref="ArgumentNullException">dbContext</exception>
        public Transaction(WhZoneDbContext dbContext, int transactionCount)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.transactionCount = transactionCount;
        }

        /// <summary>
        /// Calls the <see cref="DocuDepoDbContext.Commit(int)"/>.
        /// </summary>
        public void Commit()
        {
            this.dbContext.Commit(this.transactionCount);
        }

        /// <summary>
        /// Calls the <see cref="DocuDepoDbContext.Rollback(int)"/>.
        /// </summary>
        public void Rollback()
        {
            this.dbContext.Rollback(this.transactionCount);
        }

        /// <summary>
        /// Checks that a transaction exists.
        /// </summary>
        public bool HasTransaction()
        {
            return this.dbContext.transactionDepth >= this.transactionCount;
        }

        /// <summary>
        /// If transaction is still alive, rolling it back.
        /// </summary>
        public void Dispose()
        {
            this.Rollback();
            GC.SuppressFinalize(this);
        }
    }
}
