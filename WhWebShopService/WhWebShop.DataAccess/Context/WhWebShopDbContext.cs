using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace eLog.HeavyTools.Services.WhWebShop.DataAccess.Context;

public partial class WhWebShopDbContext : DbContext
{
    /// <summary>
    /// The current depth of the transaction
    /// </summary>
    private int transactionDepth;

    /// <summary>
    /// The used transaction
    /// </summary>
    private IDbContextTransaction? transaction;
    //private readonly IConfiguration configuration = null!;

    //public WhWebShopDbContext(IConfiguration configuration)
    //{
    //    this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    //}

    public WhWebShopDbContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<CfwGroup> CfwGroups { get; set; } = null!;
    public virtual DbSet<CfwUser> CfwUsers { get; set; } = null!;
    public virtual DbSet<CfwUsergroup> CfwUsergroups { get; set; } = null!;
    public virtual DbSet<OlsCompany> OlsCompanies { get; set; } = null!;
    public virtual DbSet<OlsSysval> OlsSysvals { get; set; } = null!;

    public virtual DbSet<OlcPriceCalcResult> OlcPriceCalcResults { get; set; } = null!;

    //protected override void OnConfiguring(DbContextOptionsBuilder options)
    //{
    //    if (this.configuration is not null)
    //    {
    //        options.UseSqlServer(this.configuration.GetConnectionString(nameof(WhWebShopDbContext)));
    //    }
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Hungarian_CP1250_CI_AS");

        modelBuilder.Entity<CfwGroup>(entity =>
        {
            entity.HasKey(e => e.Grpid)
                .HasName("pk_cfw_group");

            entity.Property(e => e.Grpid).ValueGeneratedNever();
        });

        modelBuilder.Entity<CfwUser>(entity =>
        {
            entity.HasKey(e => e.Usrid)
                .HasName("pk_cfw_user");
        });

        modelBuilder.Entity<CfwUsergroup>(entity =>
        {
            entity.HasKey(e => new { e.Usrid, e.Grpid })
                .HasName("pk_cfw_usergroup");

            entity.HasOne(d => d.Grp)
                .WithMany(p => p.CfwUsergroups)
                .HasForeignKey(d => d.Grpid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_cfw_usergroup_grpid");

            entity.HasOne(d => d.Usr)
                .WithMany(p => p.CfwUsergroups)
                .HasForeignKey(d => d.Usrid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_cfw_usergroup_usrid");
        });

        modelBuilder.Entity<OlsCompany>(entity =>
        {
            entity.HasKey(e => e.Cmpid)
                .HasName("pk_ols_company");

            entity.Property(e => e.Cmpid).ValueGeneratedNever();

            entity.HasOne(d => d.Addusr)
                .WithMany(p => p.OlsCompanies)
                .HasForeignKey(d => d.Addusrid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ols_company_addusrid");
        });

        modelBuilder.Entity<OlsSysval>(entity =>
        {
            entity.HasKey(e => e.Sysvalid)
                .HasName("pk_ols_sysval");
        });

        modelBuilder.Entity<OlcPriceCalcResult>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("pk_olc_pricecalcresult");
        });

        this.OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

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
    public async Task<Transaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (this.transactionDepth == 0)
        {
            this.transaction = await this.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(true);
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
        private readonly WhWebShopDbContext dbContext;

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
        public Transaction(WhWebShopDbContext dbContext, int transactionCount)
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
