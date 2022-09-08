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

	
	        public virtual DbSet<CfwGroup> CfwGroup { get; set; } = null!;
        public virtual DbSet<CfwUser> CfwUser { get; set; } = null!;
        public virtual DbSet<CfwUsergroup> CfwUsergroup { get; set; } = null!;
        public virtual DbSet<OlcAction> OlcAction { get; set; } = null!;
        public virtual DbSet<OlcActioncountry> OlcActioncountry { get; set; } = null!;
        public virtual DbSet<OlcActioncouponnumber> OlcActioncouponnumber { get; set; } = null!;
        public virtual DbSet<OlcActionext> OlcActionext { get; set; } = null!;
        public virtual DbSet<OlcActionretail> OlcActionretail { get; set; } = null!;
        public virtual DbSet<OlcActionwebhop> OlcActionwebhop { get; set; } = null!;
        public virtual DbSet<OlcApilogger> OlcApilogger { get; set; } = null!;
        public virtual DbSet<OlcGiftcard> OlcGiftcard { get; set; } = null!;
        public virtual DbSet<OlcGiftcardlog> OlcGiftcardlog { get; set; } = null!;
        public virtual DbSet<OlcItem> OlcItem { get; set; } = null!;
        public virtual DbSet<OlcPrctable> OlcPrctable { get; set; } = null!;
        public virtual DbSet<OlcSordhead> OlcSordhead { get; set; } = null!;
        public virtual DbSet<OlcSordline> OlcSordline { get; set; } = null!;
        public virtual DbSet<OlcTaxtransext> OlcTaxtransext { get; set; } = null!;
        public virtual DbSet<OlsCompany> OlsCompany { get; set; } = null!;
        public virtual DbSet<OlsCountry> OlsCountry { get; set; } = null!;
        public virtual DbSet<OlsItem> OlsItem { get; set; } = null!;
        public virtual DbSet<OlsPartnaddr> OlsPartnaddr { get; set; } = null!;
        public virtual DbSet<OlsPartner> OlsPartner { get; set; } = null!;
        public virtual DbSet<OlsRecid> OlsRecid { get; set; } = null!;
        public virtual DbSet<OlsSordhead> OlsSordhead { get; set; } = null!;
        public virtual DbSet<OlsSordline> OlsSordline { get; set; } = null!;
        public virtual DbSet<OlsSysval> OlsSysval { get; set; } = null!;
        public virtual DbSet<OlsTaxtrans> OlsTaxtrans { get; set; } = null!;

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
                    .WithMany(p => p.CfwUsergroup)
                    .HasForeignKey(d => d.Grpid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_cfw_usergroup_grpid");

                entity.HasOne(d => d.Usr)
                    .WithMany(p => p.CfwUsergroup)
                    .HasForeignKey(d => d.Usrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_cfw_usergroup_usrid");
            });

            modelBuilder.Entity<OlcAction>(entity =>
            {
                entity.HasKey(e => e.Aid)
                    .HasName("pk_olc_action");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlcAction)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_action_addusrid");

                entity.HasOne(d => d.Discounta)
                    .WithMany(p => p.InverseDiscounta)
                    .HasForeignKey(d => d.Discountaid)
                    .HasConstraintName("fk_olc_action_discountaid");
            });

            modelBuilder.Entity<OlcActioncountry>(entity =>
            {
                entity.HasKey(e => e.Acid)
                    .HasName("pk_olc_actioncountry");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlcActioncountry)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_actioncountry_addusrid");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.OlcActioncountry)
                    .HasForeignKey(d => d.Countryid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_actioncountry_countryid");
            });

            modelBuilder.Entity<OlcActioncouponnumber>(entity =>
            {
                entity.HasKey(e => e.Acnid)
                    .HasName("pk_olc_actioncouponnumber");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlcActioncouponnumber)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_actioncouponnumber_addusrid");
            });

            modelBuilder.Entity<OlcActionext>(entity =>
            {
                entity.HasKey(e => e.Axid)
                    .HasName("pk_olc_actionext");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlcActionext)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_actionext_addusrid");
            });

            modelBuilder.Entity<OlcActionretail>(entity =>
            {
                entity.HasKey(e => e.Arid)
                    .HasName("pk_olc_actionretail");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlcActionretail)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_actionretail_addusrid");
            });

            modelBuilder.Entity<OlcActionwebhop>(entity =>
            {
                entity.HasKey(e => e.Awid)
                    .HasName("pk_olc_actionwebhop");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlcActionwebhop)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_actionwebhop_addusrid");
            });

            modelBuilder.Entity<OlcApilogger>(entity =>
            {
                entity.HasKey(e => e.Apiid)
                    .HasName("pk_olc_apilogger");
            });

            modelBuilder.Entity<OlcGiftcard>(entity =>
            {
                entity.HasKey(e => e.Gcid)
                    .HasName("pk_olc_giftcard");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlcGiftcard)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_giftcard_addusrid");
            });

            modelBuilder.Entity<OlcGiftcardlog>(entity =>
            {
                entity.HasKey(e => e.Gclid)
                    .HasName("pk_olc_giftcardlog");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlcGiftcardlog)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_giftcardlog_addusrid");

                entity.HasOne(d => d.Gc)
                    .WithMany(p => p.OlcGiftcardlog)
                    .HasForeignKey(d => d.Gcid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_giftcardlog_gcid");
            });

            modelBuilder.Entity<OlcItem>(entity =>
            {
                entity.HasKey(e => e.Itemid)
                    .HasName("pk_olc_item");

                entity.Property(e => e.Itemid).ValueGeneratedNever();

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlcItem)
                    .HasForeignKey(d => d.Addusrid)
                    .HasConstraintName("fk_olc_item_addusrid");
            });

            modelBuilder.Entity<OlcPrctable>(entity =>
            {
                entity.HasKey(e => e.Prcid)
                    .HasName("pk_olc_prctable");

                entity.HasOne(d => d.Addr)
                    .WithMany(p => p.OlcPrctable)
                    .HasForeignKey(d => d.Addrid)
                    .HasConstraintName("fk_olc_prctable_addrid");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlcPrctable)
                    .HasForeignKey(d => d.Addusrid)
                    .HasConstraintName("fk_olc_prctable_addusrid");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.OlcPrctable)
                    .HasForeignKey(d => d.Itemid)
                    .HasConstraintName("fk_olc_prctable_itemid");

                entity.HasOne(d => d.Partn)
                    .WithMany(p => p.OlcPrctable)
                    .HasForeignKey(d => d.Partnid)
                    .HasConstraintName("fk_olc_prctable_partnid");
            });

            modelBuilder.Entity<OlcSordhead>(entity =>
            {
                entity.HasKey(e => e.Sordid)
                    .HasName("pk_olc_sordhead");

                entity.Property(e => e.Sordid).ValueGeneratedNever();

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlcSordhead)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_sordhead_addusrid");

                entity.HasOne(d => d.Sord)
                    .WithOne(p => p.OlcSordhead)
                    .HasForeignKey<OlcSordhead>(d => d.Sordid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_sordhead_sordid");
            });

            modelBuilder.Entity<OlcSordline>(entity =>
            {
                entity.HasKey(e => e.Sordlineid)
                    .HasName("pk_olc_sordline");

                entity.Property(e => e.Sordlineid).ValueGeneratedNever();

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlcSordline)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_sordline_addusrid");

                entity.HasOne(d => d.Sordline)
                    .WithOne(p => p.OlcSordline)
                    .HasForeignKey<OlcSordline>(d => d.Sordlineid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_sordline_sordlineid");
            });

            modelBuilder.Entity<OlcTaxtransext>(entity =>
            {
                entity.HasKey(e => e.Tteid)
                    .HasName("pk_olc_taxtransext");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlcTaxtransext)
                    .HasForeignKey(d => d.Addusrid)
                    .HasConstraintName("fk_olc_taxtransext_addusrid");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.OlcTaxtransext)
                    .HasForeignKey(d => d.Countryid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_taxtransext_countryid");

                entity.HasOne(d => d.Tt)
                    .WithMany(p => p.OlcTaxtransext)
                    .HasForeignKey(d => d.Ttid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_taxtransext_ttid");
            });

            modelBuilder.Entity<OlsCompany>(entity =>
            {
                entity.HasKey(e => e.Cmpid)
                    .HasName("pk_ols_company");

                entity.Property(e => e.Cmpid).ValueGeneratedNever();

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsCompany)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_company_addusrid");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.OlsCompany)
                    .HasForeignKey(d => d.Countryid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_company_countryid");

                entity.HasOne(d => d.Partn)
                    .WithMany(p => p.OlsCompany)
                    .HasForeignKey(d => d.Partnid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_company_partnid");
            });

            modelBuilder.Entity<OlsCountry>(entity =>
            {
                entity.HasKey(e => e.Countryid)
                    .HasName("pk_ols_country");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsCountry)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_country_addusrid");
            });

            modelBuilder.Entity<OlsItem>(entity =>
            {
                entity.HasKey(e => e.Itemid)
                    .HasName("pk_ols_item");

                entity.Property(e => e.Itemid).ValueGeneratedNever();

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsItem)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_item_addusrid");

                entity.HasOne(d => d.Rootitem)
                    .WithMany(p => p.InverseRootitem)
                    .HasForeignKey(d => d.Rootitemid)
                    .HasConstraintName("fk_ols_item_rootitemid");
            });

            modelBuilder.Entity<OlsPartnaddr>(entity =>
            {
                entity.HasKey(e => e.Addrid)
                    .HasName("pk_ols_partnaddr");

                entity.Property(e => e.Addrid).ValueGeneratedNever();

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsPartnaddr)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_partnaddr_addusrid");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.OlsPartnaddr)
                    .HasForeignKey(d => d.Countryid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_partnaddr_countryid");

                entity.HasOne(d => d.Partn)
                    .WithMany(p => p.OlsPartnaddr)
                    .HasForeignKey(d => d.Partnid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_partnaddr_partnid");
            });

            modelBuilder.Entity<OlsPartner>(entity =>
            {
                entity.HasKey(e => e.Partnid)
                    .HasName("pk_ols_partner");

                entity.Property(e => e.Partnid).ValueGeneratedNever();

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsPartner)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_partner_addusrid");
            });

            modelBuilder.Entity<OlsRecid>(entity =>
            {
                entity.HasKey(e => e.Riid)
                    .HasName("pk_ols_recid");
            });

            modelBuilder.Entity<OlsSordhead>(entity =>
            {
                entity.HasKey(e => e.Sordid)
                    .HasName("pk_ols_sordhead");

                entity.Property(e => e.Sordid).ValueGeneratedNever();

                entity.HasOne(d => d.Addr)
                    .WithMany(p => p.OlsSordhead)
                    .HasForeignKey(d => d.Addrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sordhead_addrid");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsSordhead)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sordhead_addusrid");

                entity.HasOne(d => d.Cmp)
                    .WithMany(p => p.OlsSordhead)
                    .HasForeignKey(d => d.Cmpid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sordhead_cmpid");

                entity.HasOne(d => d.Partn)
                    .WithMany(p => p.OlsSordhead)
                    .HasForeignKey(d => d.Partnid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sordhead_partnid");
            });

            modelBuilder.Entity<OlsSordline>(entity =>
            {
                entity.HasKey(e => e.Sordlineid)
                    .HasName("pk_ols_sordline");

                entity.Property(e => e.Sordlineid).ValueGeneratedNever();

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsSordline)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sordline_addusrid");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.OlsSordline)
                    .HasForeignKey(d => d.Itemid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sordline_itemid");

                entity.HasOne(d => d.Sord)
                    .WithMany(p => p.OlsSordline)
                    .HasForeignKey(d => d.Sordid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sordline_sordid");
            });

            modelBuilder.Entity<OlsSysval>(entity =>
            {
                entity.HasKey(e => e.Sysvalid)
                    .HasName("pk_ols_sysval");
            });

            modelBuilder.Entity<OlsTaxtrans>(entity =>
            {
                entity.HasKey(e => e.Ttid)
                    .HasName("pk_ols_taxtrans");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsTaxtrans)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_taxtrans_addusrid");
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