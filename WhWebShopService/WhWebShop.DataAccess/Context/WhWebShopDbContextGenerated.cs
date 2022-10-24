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
        public virtual DbSet<OlcItemmodel> OlcItemmodel { get; set; } = null!;
        public virtual DbSet<OlcItemmodelseason> OlcItemmodelseason { get; set; } = null!;
        public virtual DbSet<OlcPartner> OlcPartner { get; set; } = null!;
        public virtual DbSet<OlcPrctable> OlcPrctable { get; set; } = null!;
        public virtual DbSet<OlcPrctableCurrent> OlcPrctableCurrent { get; set; } = null!;
        public virtual DbSet<OlcPrctype> OlcPrctype { get; set; } = null!;
        public virtual DbSet<OlcSordhead> OlcSordhead { get; set; } = null!;
        public virtual DbSet<OlcSordline> OlcSordline { get; set; } = null!;
        public virtual DbSet<OlcSordlineRes> OlcSordlineRes { get; set; } = null!;
        public virtual DbSet<OlcSpOlsReserveReservestock> OlcSpOlsReserveReservestock { get; set; } = null!;
        public virtual DbSet<OlcTaxtransext> OlcTaxtransext { get; set; } = null!;
        public virtual DbSet<OlcTmpSordsord> OlcTmpSordsord { get; set; } = null!;
        public virtual DbSet<OlsCompany> OlsCompany { get; set; } = null!;
        public virtual DbSet<OlsCountry> OlsCountry { get; set; } = null!;
        public virtual DbSet<OlsCurrency> OlsCurrency { get; set; } = null!;
        public virtual DbSet<OlsItem> OlsItem { get; set; } = null!;
        public virtual DbSet<OlsPartnaddr> OlsPartnaddr { get; set; } = null!;
        public virtual DbSet<OlsPartner> OlsPartner { get; set; } = null!;
        public virtual DbSet<OlsRecid> OlsRecid { get; set; } = null!;
        public virtual DbSet<OlsReserve> OlsReserve { get; set; } = null!;
        public virtual DbSet<OlsSinvhead> OlsSinvhead { get; set; } = null!;
        public virtual DbSet<OlsSorddoc> OlsSorddoc { get; set; } = null!;
        public virtual DbSet<OlsSordhead> OlsSordhead { get; set; } = null!;
        public virtual DbSet<OlsSordline> OlsSordline { get; set; } = null!;
        public virtual DbSet<OlsStock> OlsStock { get; set; } = null!;
        public virtual DbSet<OlsSysval> OlsSysval { get; set; } = null!;
        public virtual DbSet<OlsTax> OlsTax { get; set; } = null!;
        public virtual DbSet<OlsTaxtrans> OlsTaxtrans { get; set; } = null!;
        public virtual DbSet<OlsTmpSordst> OlsTmpSordst { get; set; } = null!;

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

                entity.HasOne(d => d.Cur)
                    .WithMany(p => p.OlcAction)
                    .HasForeignKey(d => d.Curid)
                    .HasConstraintName("fk_olc_action_curid");

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

                entity.HasOne(d => d.Sinv)
                    .WithMany(p => p.OlcGiftcardlog)
                    .HasForeignKey(d => d.Sinvid)
                    .HasConstraintName("fk_olc_giftcardlog_sinvidid");
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

                entity.HasOne(d => d.Ims)
                    .WithMany(p => p.OlcItem)
                    .HasForeignKey(d => d.Imsid)
                    .HasConstraintName("fk_olc_item_imsid");
            });

            modelBuilder.Entity<OlcItemmodel>(entity =>
            {
                entity.HasKey(e => e.Imid)
                    .HasName("pk_olc_itemmodel");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlcItemmodel)
                    .HasForeignKey(d => d.Addusrid)
                    .HasConstraintName("fk_olc_itemmodel_addusrid");
            });

            modelBuilder.Entity<OlcItemmodelseason>(entity =>
            {
                entity.HasKey(e => e.Imsid)
                    .HasName("pk_olc_itemmodelseason");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlcItemmodelseason)
                    .HasForeignKey(d => d.Addusrid)
                    .HasConstraintName("fk_olc_itemmodelseason_addusrid");

                entity.HasOne(d => d.Im)
                    .WithMany(p => p.OlcItemmodelseason)
                    .HasForeignKey(d => d.Imid)
                    .HasConstraintName("fk_olc_itemmodelseason_imid");
            });

            modelBuilder.Entity<OlcPartner>(entity =>
            {
                entity.HasKey(e => e.Partnid)
                    .HasName("pk_olc_partner");

                entity.Property(e => e.Partnid).ValueGeneratedNever();

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlcPartner)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_partner_addusrid");

                entity.HasOne(d => d.Partn)
                    .WithOne(p => p.OlcPartner)
                    .HasForeignKey<OlcPartner>(d => d.Partnid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_partner_partnid");

                entity.HasOne(d => d.Tax)
                    .WithMany(p => p.OlcPartner)
                    .HasForeignKey(d => d.Taxid)
                    .HasConstraintName("fk_olc_partner_taxid");
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

                entity.HasOne(d => d.Cur)
                    .WithMany(p => p.OlcPrctable)
                    .HasForeignKey(d => d.Curid)
                    .HasConstraintName("fk_olc_prctable_curid");

                entity.HasOne(d => d.Im)
                    .WithMany(p => p.OlcPrctable)
                    .HasForeignKey(d => d.Imid)
                    .HasConstraintName("fk_olc_prctable_imid");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.OlcPrctable)
                    .HasForeignKey(d => d.Itemid)
                    .HasConstraintName("fk_olc_prctable_itemid");

                entity.HasOne(d => d.Partn)
                    .WithMany(p => p.OlcPrctable)
                    .HasForeignKey(d => d.Partnid)
                    .HasConstraintName("fk_olc_prctable_partnid");

                entity.HasOne(d => d.Pt)
                    .WithMany(p => p.OlcPrctable)
                    .HasForeignKey(d => d.Ptid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_prctable_tpid");
            });

            modelBuilder.Entity<OlcPrctableCurrent>(entity =>
            {
                entity.HasKey(e => e.Prccid)
                    .HasName("pk_olc_prctable_current");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.OlcPrctableCurrent)
                    .HasForeignKey(d => d.Itemid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_prctable_current_itemid");
            });

            modelBuilder.Entity<OlcPrctype>(entity =>
            {
                entity.HasKey(e => e.Ptid)
                    .HasName("pk_olc_prctypen");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlcPrctype)
                    .HasForeignKey(d => d.Addusrid)
                    .HasConstraintName("fk_olc_prctype_addusrid");
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

                entity.HasOne(d => d.Preordersordline)
                    .WithMany(p => p.OlcSordlinePreordersordline)
                    .HasForeignKey(d => d.Preordersordlineid)
                    .HasConstraintName("fk_olc_sordline_preordersordlineid");

                entity.HasOne(d => d.Sordline)
                    .WithOne(p => p.OlcSordlineSordline)
                    .HasForeignKey<OlcSordline>(d => d.Sordlineid)
                    .HasConstraintName("fk_olc_sordline_sordlineid");
            });

            modelBuilder.Entity<OlcSordlineRes>(entity =>
            {
                entity.HasKey(e => e.Sordlineidres)
                    .HasName("pk_olc_sordline_res");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlcSordlineRes)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_sordline_res_addusrid");

                entity.HasOne(d => d.Preordersordline)
                    .WithMany(p => p.OlcSordlineResPreordersordline)
                    .HasForeignKey(d => d.Preordersordlineid)
                    .HasConstraintName("fk_olc_sordline_res_presordersordlineid");

                entity.HasOne(d => d.Res)
                    .WithMany(p => p.OlcSordlineRes)
                    .HasForeignKey(d => d.Resid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_sordline_res_resid");

                entity.HasOne(d => d.Sordline)
                    .WithMany(p => p.OlcSordlineResSordline)
                    .HasForeignKey(d => d.Sordlineid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_sordline_res_sordlineid");
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

                entity.HasOne(d => d.Tax)
                    .WithMany(p => p.OlcTaxtransext)
                    .HasForeignKey(d => d.Taxid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_taxtransext_taxid");

                entity.HasOne(d => d.Tt)
                    .WithMany(p => p.OlcTaxtransext)
                    .HasForeignKey(d => d.Ttid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_taxtransext_ttid");
            });

            modelBuilder.Entity<OlcTmpSordsord>(entity =>
            {
                entity.HasKey(e => e.Sordlineid)
                    .HasName("pk_tmp_sordsord_sordline");

                entity.Property(e => e.Sordlineid).ValueGeneratedNever();
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

                entity.HasOne(d => d.Dualcur)
                    .WithMany(p => p.OlsCompanyDualcur)
                    .HasForeignKey(d => d.Dualcurid)
                    .HasConstraintName("fk_ols_company_dualcurid");

                entity.HasOne(d => d.Homecur)
                    .WithMany(p => p.OlsCompanyHomecur)
                    .HasForeignKey(d => d.Homecurid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_company_homecurid");

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

            modelBuilder.Entity<OlsCurrency>(entity =>
            {
                entity.HasKey(e => e.Curid)
                    .HasName("pk_ols_currency");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsCurrency)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_currency_addusrid");
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

            modelBuilder.Entity<OlsReserve>(entity =>
            {
                entity.HasKey(e => e.Resid)
                    .HasName("pk_ols_reserve");

                entity.Property(e => e.Resid).ValueGeneratedNever();

                entity.HasOne(d => d.Addr)
                    .WithMany(p => p.OlsReserve)
                    .HasForeignKey(d => d.Addrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_reserve_addrid");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsReserve)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_reserve_addusrid");

                entity.HasOne(d => d.Cmp)
                    .WithMany(p => p.OlsReserve)
                    .HasForeignKey(d => d.Cmpid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_reserve_cmpid");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.OlsReserve)
                    .HasForeignKey(d => d.Itemid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_reserve_itemid");

                entity.HasOne(d => d.Partn)
                    .WithMany(p => p.OlsReserve)
                    .HasForeignKey(d => d.Partnid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_reserve_partnid");
            });

            modelBuilder.Entity<OlsSinvhead>(entity =>
            {
                entity.HasKey(e => e.Sinvid)
                    .HasName("pk_ols_sinvhead");

                entity.Property(e => e.Sinvid).ValueGeneratedNever();

                entity.HasOne(d => d.Addr)
                    .WithMany(p => p.OlsSinvheadAddr)
                    .HasForeignKey(d => d.Addrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sinvhead_addrid");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsSinvheadAddusr)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sinvhead_addusrid");

                entity.HasOne(d => d.Closeusr)
                    .WithMany(p => p.OlsSinvheadCloseusr)
                    .HasForeignKey(d => d.Closeusrid)
                    .HasConstraintName("fk_ols_sinvhead_closeusrid");

                entity.HasOne(d => d.Cmp)
                    .WithMany(p => p.OlsSinvhead)
                    .HasForeignKey(d => d.Cmpid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sinvhead_cmpid");

                entity.HasOne(d => d.Corrsinv)
                    .WithMany(p => p.InverseCorrsinv)
                    .HasForeignKey(d => d.Corrsinvid)
                    .HasConstraintName("fk_ols_sinvhead_corrsinvid");

                entity.HasOne(d => d.Cur)
                    .WithMany(p => p.OlsSinvhead)
                    .HasForeignKey(d => d.Curid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sinvhead_curid");

                entity.HasOne(d => d.Deladdr)
                    .WithMany(p => p.OlsSinvheadDeladdr)
                    .HasForeignKey(d => d.Deladdrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sinvhead_deladdrid");

                entity.HasOne(d => d.Delpartn)
                    .WithMany(p => p.OlsSinvheadDelpartn)
                    .HasForeignKey(d => d.Delpartnid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sinvhead_delpartnid");

                entity.HasOne(d => d.Origsinv)
                    .WithMany(p => p.InverseOrigsinv)
                    .HasForeignKey(d => d.Origsinvid)
                    .HasConstraintName("fk_ols_sinvhead_origsinvid");

                entity.HasOne(d => d.Partn)
                    .WithMany(p => p.OlsSinvheadPartn)
                    .HasForeignKey(d => d.Partnid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sinvhead_partnid");
            });

            modelBuilder.Entity<OlsSorddoc>(entity =>
            {
                entity.HasKey(e => e.Sorddocid)
                    .HasName("pk_ols_sorddoc");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsSorddoc)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sorddoc_addusrid");
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

                entity.HasOne(d => d.Cur)
                    .WithMany(p => p.OlsSordhead)
                    .HasForeignKey(d => d.Curid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sordhead_curid");

                entity.HasOne(d => d.Partn)
                    .WithMany(p => p.OlsSordhead)
                    .HasForeignKey(d => d.Partnid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sordhead_partnid");

                entity.HasOne(d => d.Sorddoc)
                    .WithMany(p => p.OlsSordhead)
                    .HasForeignKey(d => d.Sorddocid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sordhead_sorddocid");
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

                entity.HasOne(d => d.Res)
                    .WithMany(p => p.OlsSordline)
                    .HasForeignKey(d => d.Resid)
                    .HasConstraintName("fk_ols_sordline_resid");

                entity.HasOne(d => d.Sord)
                    .WithMany(p => p.OlsSordline)
                    .HasForeignKey(d => d.Sordid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sordline_sordid");

                entity.HasOne(d => d.Tax)
                    .WithMany(p => p.OlsSordline)
                    .HasForeignKey(d => d.Taxid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sordline_taxid");
            });

            modelBuilder.Entity<OlsStock>(entity =>
            {
                entity.HasKey(e => new { e.Itemid, e.Whid })
                    .HasName("pk_ols_stock");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.OlsStock)
                    .HasForeignKey(d => d.Itemid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_stock_itemid");
            });

            modelBuilder.Entity<OlsSysval>(entity =>
            {
                entity.HasKey(e => e.Sysvalid)
                    .HasName("pk_ols_sysval");
            });

            modelBuilder.Entity<OlsTax>(entity =>
            {
                entity.HasKey(e => e.Taxid)
                    .HasName("pk_ols_tax");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsTax)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_tax_addusrid");
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

                entity.HasOne(d => d.Realtax)
                    .WithMany(p => p.OlsTaxtransRealtax)
                    .HasForeignKey(d => d.Realtaxid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_taxtrans_realtaxid");

                entity.HasOne(d => d.Tax)
                    .WithMany(p => p.OlsTaxtransTax)
                    .HasForeignKey(d => d.Taxid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_taxtrans_taxid");
            });

            modelBuilder.Entity<OlsTmpSordst>(entity =>
            {
                entity.HasKey(e => new { e.Ssid, e.Sordlineid })
                    .HasName("pk_ols_tmp_sordst");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsTmpSordst)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_tmp_sordst_addusrid");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.OlsTmpSordst)
                    .HasForeignKey(d => d.Itemid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_tmp_sordst_itemid");

                entity.HasOne(d => d.Sord)
                    .WithMany(p => p.OlsTmpSordst)
                    .HasForeignKey(d => d.Sordid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_tmp_sordst_sordid");

                entity.HasOne(d => d.Sordline)
                    .WithMany(p => p.OlsTmpSordst)
                    .HasForeignKey(d => d.Sordlineid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_tmp_sordst_sordlineid");

                entity.HasOne(d => d.Tax)
                    .WithMany(p => p.OlsTmpSordst)
                    .HasForeignKey(d => d.Taxid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_tmp_sordst_taxid");
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