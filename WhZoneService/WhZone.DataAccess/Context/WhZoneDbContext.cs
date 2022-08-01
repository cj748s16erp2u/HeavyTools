using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace eLog.HeavyTools.Services.WhZone.DataAccess.Context
{
    //[System.Diagnostics.DebuggerStepThrough]
    public partial class WhZoneDbContext : DbContext
    {
        /// <summary>
        /// The current depth of the transaction
        /// </summary>
        private int transactionDepth;

        /// <summary>
        /// The used transaction
        /// </summary>
        private IDbContextTransaction? transaction;
        private readonly IConfiguration configuration = null!;

        public WhZoneDbContext(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public WhZoneDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<CfwUser> CfwUsers { get; set; } = null!;
        public virtual DbSet<CfwGroup> CfwGroups { get; set; } = null!;
        public virtual DbSet<CfwUsergroup> CfwUsergroups { get; set; } = null!;
        public virtual DbSet<OlsSysval> OlsSysvals { get; set; } = null!;
        public virtual DbSet<OlcWhlocation> OlcWhlocations { get; set; } = null!;
        public virtual DbSet<OlcWhzloc> OlcWhzlocs { get; set; } = null!;
        public virtual DbSet<OlcWhzone> OlcWhzones { get; set; } = null!;
        public virtual DbSet<OlcWhzstock> OlcWhzstocks { get; set; } = null!;
        public virtual DbSet<OlcWhzstockmap> OlcWhzstockmaps { get; set; } = null!;
        public virtual DbSet<OlcWhztranhead> OlcWhztranheads { get; set; } = null!;
        public virtual DbSet<OlcWhztranline> OlcWhztranlines { get; set; } = null!;
        public virtual DbSet<OlcWhztst> OlcWhztsts { get; set; } = null!;
        public virtual DbSet<OlcWhztstdfstock> OlcWhztstdfstocks { get; set; } = null!;
        public virtual DbSet<OlsCompany> OlsCompanies { get; set; } = null!;
        public virtual DbSet<OlsItem> OlsItems { get; set; } = null!;
        public virtual DbSet<OlsPordhead> OlsPordheads { get; set; } = null!;
        public virtual DbSet<OlsPordline> OlsPordlines { get; set; } = null!;
        public virtual DbSet<OlsSordhead> OlsSordheads { get; set; } = null!;
        public virtual DbSet<OlsSordline> OlsSordlines { get; set; } = null!;
        public virtual DbSet<OlsSthead> OlsStheads { get; set; } = null!;
        public virtual DbSet<OlsStline> OlsStlines { get; set; } = null!;
        public virtual DbSet<OlsUnit> OlsUnits { get; set; } = null!;
        public virtual DbSet<OlsWarehouse> OlsWarehouses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (this.configuration is not null)
            {
                options.UseSqlServer(this.configuration.GetConnectionString(nameof(WhZoneDbContext)));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("SQL_Hungarian_CP1250_CI_AS");

            modelBuilder.Entity<CfwUser>(entity =>
            {
                entity.HasKey(e => e.Usrid)
                    .HasName("pk_cfw_user");

                entity.ToTable("cfw_user");

                entity.Property(e => e.Usrid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("usrid");

                entity.Property(e => e.Cs)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cs");

                entity.Property(e => e.Deflangid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("deflangid");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Ldapid)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ldapid");

                entity.Property(e => e.Name)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Note)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("note");

                entity.Property(e => e.Options).HasColumnName("options");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Pwdate)
                    .HasColumnType("datetime")
                    .HasColumnName("pwdate");

                entity.Property(e => e.Xmldata)
                    .HasColumnType("xml")
                    .HasColumnName("xmldata");
            });

            modelBuilder.Entity<CfwGroup>(entity =>
            {
                entity.HasKey(e => e.Grpid)
                    .HasName("pk_cfw_group");

                entity.ToTable("cfw_group");

                entity.Property(e => e.Cs)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cs");

                entity.Property(e => e.Grpid).HasColumnName("grpid");

                entity.Property(e => e.Lev).HasColumnName("lev");

                entity.Property(e => e.Name)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Note)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("note");

                entity.Property(e => e.Options).HasColumnName("options");

                entity.Property(e => e.Xmldata)
                    .HasColumnType("xml")
                    .HasColumnName("xmldata");
            });

            modelBuilder.Entity<CfwUsergroup>(entity =>
            {
                entity.HasKey(e => new { e.Usrid, e.Grpid })
                    .HasName("pk_cfw_usergroup");

                entity.ToTable("cfw_usergroup");

                entity.Property(e => e.Usrid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("usrid");

                entity.Property(e => e.Grpid).HasColumnName("grpid");

                entity.Property(e => e.Cs)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cs");

                entity.HasOne(d => d.CfwUser)
                    .WithMany(p => p.CfwUsergroups)
                    .HasForeignKey(d => d.Usrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_cfw_usergroup_usrid");

                entity.HasOne(d => d.CfwGroup)
                    .WithMany(p => p.CfwUsergroups)
                    .HasForeignKey(d => d.Grpid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_cfw_usergroup_grpid");
            });

            modelBuilder.Entity<OlsSysval>(entity =>
            {
                entity.HasKey(e => e.Sysvalid)
                    .HasName("pk_ols_sysval");

                entity.ToTable("ols_sysval");

                entity.Property(e => e.Sysvalid)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("sysvalid");

                entity.Property(e => e.Valueint).HasColumnName("valueint");

                entity.Property(e => e.Valuenum)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("valuenum");

                entity.Property(e => e.Valuestr)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("valuestr");

                entity.Property(e => e.Valuedate)
                    .HasColumnType("datetime")
                    .HasColumnName("valuedate");

                entity.Property(e => e.Valuevar)
                    .HasColumnType("sql_variant")
                    .HasColumnName("valuevar");
            });

            modelBuilder.Entity<OlcWhlocation>(entity =>
            {
                entity.HasKey(e => e.Whlocid)
                    .HasName("pk_olc_whlocation");

                entity.ToTable("olc_whlocation");

                entity.HasIndex(e => new { e.Whid, e.Whzoneid, e.Loctype }, "idx_olc_whlocation_loctype");

                entity.HasIndex(e => e.Whid, "idx_olc_whlocation_whid");

                entity.HasIndex(e => new { e.Whid, e.Whzoneid }, "idx_olc_whlocation_whzoneid");

                entity.HasIndex(e => new { e.Whid, e.Whzoneid, e.Whloccode }, "uq_olc_whlocation_whloccode")
                    .IsUnique();

                entity.Property(e => e.Whlocid).HasColumnName("whlocid");

                entity.Property(e => e.Adddate)
                    .HasColumnType("datetime")
                    .HasColumnName("adddate");

                entity.Property(e => e.Addusrid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("addusrid");

                entity.Property(e => e.Capacity)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("capacity");

                entity.Property(e => e.Capunitid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("capunitid");

                entity.Property(e => e.Crawlorder).HasColumnName("crawlorder");

                entity.Property(e => e.Delstat).HasColumnName("delstat");

                entity.Property(e => e.Ismulti).HasColumnName("ismulti");

                entity.Property(e => e.Loctype).HasColumnName("loctype");

                entity.Property(e => e.Movloctype).HasColumnName("movloctype");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Note)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("note");

                entity.Property(e => e.Overfillthreshold)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("overfillthreshold");

                entity.Property(e => e.Volume)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("volume");

                entity.Property(e => e.Whid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("whid");

                entity.Property(e => e.Whloccode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("whloccode");

                entity.Property(e => e.Whzoneid).HasColumnName("whzoneid");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlcWhlocations)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whlocation_addusrid");

                entity.HasOne(d => d.Capunit)
                    .WithMany(p => p.OlcWhlocations)
                    .HasForeignKey(d => d.Capunitid)
                    .HasConstraintName("fk_olc_whlocation_capunitid");

                entity.HasOne(d => d.Wh)
                    .WithMany(p => p.OlcWhlocations)
                    .HasForeignKey(d => d.Whid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whlocation_whid");

                entity.HasOne(d => d.Whzone)
                    .WithMany(p => p.OlcWhlocations)
                    .HasForeignKey(d => d.Whzoneid)
                    .HasConstraintName("fk_olc_whlocation_whzoneid");
            });

            modelBuilder.Entity<OlcWhzloc>(entity =>
            {
                entity.HasKey(e => e.Whztlocid)
                    .HasName("pk_olc_whzloc");

                entity.ToTable("olc_whzloc");

                entity.Property(e => e.Whztlocid).HasColumnName("whztlocid");

                entity.Property(e => e.Adddate)
                    .HasColumnType("datetime")
                    .HasColumnName("adddate");

                entity.Property(e => e.Addusrid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("addusrid");

                entity.Property(e => e.Dispqty)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("dispqty");

                entity.Property(e => e.Movqty)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("movqty");

                entity.Property(e => e.Whid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("whid");

                entity.Property(e => e.Whlocid).HasColumnName("whlocid");

                entity.Property(e => e.Whzoneid).HasColumnName("whzoneid");

                entity.Property(e => e.Whztid).HasColumnName("whztid");

                entity.Property(e => e.Whztlineid).HasColumnName("whztlineid");

                entity.Property(e => e.Whztltype).HasColumnName("whztltype");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlcWhzlocs)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whzloc_addusrid");

                entity.HasOne(d => d.Wh)
                    .WithMany(p => p.OlcWhzlocs)
                    .HasForeignKey(d => d.Whid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whzloc_whid");

                entity.HasOne(d => d.Whloc)
                    .WithMany(p => p.OlcWhzlocs)
                    .HasForeignKey(d => d.Whlocid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whzloc_whlocid");

                entity.HasOne(d => d.Whzone)
                    .WithMany(p => p.OlcWhzlocs)
                    .HasForeignKey(d => d.Whzoneid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whzloc_whzoneid");

                entity.HasOne(d => d.Whzt)
                    .WithMany(p => p.OlcWhzlocs)
                    .HasForeignKey(d => d.Whztid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whzloc_whztid");

                entity.HasOne(d => d.Whztline)
                    .WithMany(p => p.OlcWhzlocs)
                    .HasForeignKey(d => d.Whztlineid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whzloc_whztlineid");
            });

            modelBuilder.Entity<OlcWhzone>(entity =>
            {
                entity.HasKey(e => e.Whzoneid)
                    .HasName("pk_olc_whzone");

                entity.ToTable("olc_whzone");

                entity.HasIndex(e => e.Whid, "idx_olc_whzone_whid");

                entity.HasIndex(e => new { e.Whid, e.Whzonecode }, "idx_olc_whzone_whzonecode");

                entity.Property(e => e.Whzoneid).HasColumnName("whzoneid");

                entity.Property(e => e.Adddate)
                    .HasColumnType("datetime")
                    .HasColumnName("adddate");

                entity.Property(e => e.Addusrid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("addusrid");

                entity.Property(e => e.Delstat).HasColumnName("delstat");

                entity.Property(e => e.Isbackground).HasColumnName("isbackground");

                entity.Property(e => e.Ispuffer).HasColumnName("ispuffer");

                entity.Property(e => e.Locdefismulti).HasColumnName("locdefismulti");

                entity.Property(e => e.Locdefoverfillthreshold)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("locdefoverfillthreshold");

                entity.Property(e => e.Locdefvolume)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("locdefvolume");

                entity.Property(e => e.Name)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Note)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("note");

                entity.Property(e => e.Pickingcartaccessible).HasColumnName("pickingcartaccessible");

                entity.Property(e => e.Pickingtype).HasColumnName("pickingtype");

                entity.Property(e => e.Whid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("whid");

                entity.Property(e => e.Whzonecode)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("whzonecode");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlcWhzones)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whzone_addusrid");

                entity.HasOne(d => d.Wh)
                    .WithMany(p => p.OlcWhzones)
                    .HasForeignKey(d => d.Whid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whzone_whid");
            });

            modelBuilder.Entity<OlcWhzstock>(entity =>
            {
                entity.HasKey(e => e.Whzstockid)
                    .HasName("pk_olc_whzstock");

                entity.ToTable("olc_whzstock");

                entity.HasIndex(e => new { e.Itemid, e.Whid, e.Whzoneid }, "uq_olc_whzstock")
                    .IsUnique();

                entity.Property(e => e.Whzstockid).HasColumnName("whzstockid");

                entity.Property(e => e.Actqty)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("actqty");

                entity.Property(e => e.Itemid).HasColumnName("itemid");

                entity.Property(e => e.Provqty)
                    .HasColumnType("numeric(21, 6)")
                    .HasColumnName("provqty")
                    .HasComputedColumnSql("(([actqty]+[recqty])-[resqty])", true);

                entity.Property(e => e.Recqty)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("recqty");

                entity.Property(e => e.Resqty)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("resqty");

                entity.Property(e => e.Whid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("whid");

                entity.Property(e => e.Whzoneid).HasColumnName("whzoneid");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.OlcWhzstocks)
                    .HasForeignKey(d => d.Itemid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whzstock_itemid");

                entity.HasOne(d => d.Wh)
                    .WithMany(p => p.OlcWhzstocks)
                    .HasForeignKey(d => d.Whid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whzstock_whid");

                entity.HasOne(d => d.Whzone)
                    .WithMany(p => p.OlcWhzstocks)
                    .HasForeignKey(d => d.Whzoneid)
                    .HasConstraintName("fk_olc_whzstock_whzoneid");
            });

            modelBuilder.Entity<OlcWhzstockmap>(entity =>
            {
                entity.HasKey(e => e.Whzstockmapid)
                    .HasName("pk_olc_whzstockmap");

                entity.ToTable("olc_whzstockmap");

                entity.HasIndex(e => new { e.Itemid, e.Whid, e.Whzoneid, e.Whlocid }, "uq_olc_whzstockmap")
                    .IsUnique();

                entity.Property(e => e.Whzstockmapid).HasColumnName("whzstockmapid");

                entity.Property(e => e.Actqty)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("actqty");

                entity.Property(e => e.Itemid).HasColumnName("itemid");

                entity.Property(e => e.Provqty)
                    .HasColumnType("numeric(21, 6)")
                    .HasColumnName("provqty")
                    .HasComputedColumnSql("(([actqty]+[recqty])-[resqty])", true);

                entity.Property(e => e.Recqty)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("recqty");

                entity.Property(e => e.Resqty)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("resqty");

                entity.Property(e => e.Whid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("whid");

                entity.Property(e => e.Whlocid).HasColumnName("whlocid");

                entity.Property(e => e.Whzoneid).HasColumnName("whzoneid");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.OlcWhzstockmaps)
                    .HasForeignKey(d => d.Itemid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whzstockmap_itemid");

                entity.HasOne(d => d.Wh)
                    .WithMany(p => p.OlcWhzstockmaps)
                    .HasForeignKey(d => d.Whid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whzstockmap_whid");

                entity.HasOne(d => d.Whloc)
                    .WithMany(p => p.OlcWhzstockmaps)
                    .HasForeignKey(d => d.Whlocid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whzstockmap_whlocid");

                entity.HasOne(d => d.Whzone)
                    .WithMany(p => p.OlcWhzstockmaps)
                    .HasForeignKey(d => d.Whzoneid)
                    .HasConstraintName("fk_olc_whzstockmap_whzoneid");
            });

            modelBuilder.Entity<OlcWhztranhead>(entity =>
            {
                entity.HasKey(e => e.Whztid)
                    .HasName("pk_olc_whztranhead");

                entity.ToTable("olc_whztranhead");

                entity.Property(e => e.Whztid).HasColumnName("whztid");

                entity.Property(e => e.Adddate)
                    .HasColumnType("datetime")
                    .HasColumnName("adddate");

                entity.Property(e => e.Addusrid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("addusrid");

                entity.Property(e => e.Closedate)
                    .HasColumnType("datetime")
                    .HasColumnName("closedate");

                entity.Property(e => e.Closeusrid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("closeusrid");

                entity.Property(e => e.Cmpid).HasColumnName("cmpid");

                entity.Property(e => e.Fromwhzid).HasColumnName("fromwhzid");

                entity.Property(e => e.Gen).HasColumnName("gen");

                entity.Property(e => e.Note)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("note");

                entity.Property(e => e.Pordid).HasColumnName("pordid");

                entity.Property(e => e.Sordid).HasColumnName("sordid");

                entity.Property(e => e.Stid).HasColumnName("stid");

                entity.Property(e => e.Taskid).HasColumnName("taskid");

                entity.Property(e => e.Towhzid).HasColumnName("towhzid");

                entity.Property(e => e.Whztdate)
                    .HasColumnType("datetime")
                    .HasColumnName("whztdate");

                entity.Property(e => e.Whztstat).HasColumnName("whztstat");

                entity.Property(e => e.Whzttype).HasColumnName("whzttype");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlcWhztranheadAddusrs)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whztranhead_addusrid");

                entity.HasOne(d => d.Closeusr)
                    .WithMany(p => p.OlcWhztranheadCloseusrs)
                    .HasForeignKey(d => d.Closeusrid)
                    .HasConstraintName("fk_olc_whztranhead_closeusrid");

                entity.HasOne(d => d.Cmp)
                    .WithMany(p => p.OlcWhztranheads)
                    .HasForeignKey(d => d.Cmpid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whztranhead_cmpid");

                entity.HasOne(d => d.Fromwhz)
                    .WithMany(p => p.OlcWhztranheadFromwhzs)
                    .HasForeignKey(d => d.Fromwhzid)
                    .HasConstraintName("fk_olc_whztranhead_fromwhzid");

                entity.HasOne(d => d.Pord)
                    .WithMany(p => p.OlcWhztranheads)
                    .HasForeignKey(d => d.Pordid)
                    .HasConstraintName("fk_olc_whztranhead_pordid");

                entity.HasOne(d => d.Sord)
                    .WithMany(p => p.OlcWhztranheads)
                    .HasForeignKey(d => d.Sordid)
                    .HasConstraintName("fk_olc_whztranhead_sordid");

                entity.HasOne(d => d.St)
                    .WithMany(p => p.OlcWhztranheads)
                    .HasForeignKey(d => d.Stid)
                    .HasConstraintName("fk_olc_whztranhead_stid");

                entity.HasOne(d => d.Towhz)
                    .WithMany(p => p.OlcWhztranheadTowhzs)
                    .HasForeignKey(d => d.Towhzid)
                    .HasConstraintName("fk_olc_whztranhead_towhzid");
            });

            modelBuilder.Entity<OlcWhztranline>(entity =>
            {
                entity.HasKey(e => e.Whztlineid)
                    .HasName("pk_olc_whztranline");

                entity.ToTable("olc_whztranline");

                entity.Property(e => e.Whztlineid).HasColumnName("whztlineid");

                entity.Property(e => e.Adddate)
                    .HasColumnType("datetime")
                    .HasColumnName("adddate");

                entity.Property(e => e.Addusrid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("addusrid");

                entity.Property(e => e.Dispqty)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("dispqty");

                entity.Property(e => e.Dispqty2)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("dispqty2");

                entity.Property(e => e.Gen).HasColumnName("gen");

                entity.Property(e => e.Inqty)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("inqty");

                entity.Property(e => e.Itemid).HasColumnName("itemid");

                entity.Property(e => e.Linenum).HasColumnName("linenum");

                entity.Property(e => e.Movqty)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("movqty");

                entity.Property(e => e.Movqty2)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("movqty2");

                entity.Property(e => e.Note)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("note");

                entity.Property(e => e.Outqty)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("outqty");

                entity.Property(e => e.Pordlineid).HasColumnName("pordlineid");

                entity.Property(e => e.Sordlineid).HasColumnName("sordlineid");

                entity.Property(e => e.Stlineid).HasColumnName("stlineid");

                entity.Property(e => e.Taskitemid).HasColumnName("taskitemid");

                entity.Property(e => e.Unitid2)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("unitid2");

                entity.Property(e => e.Whztid).HasColumnName("whztid");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlcWhztranlines)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whztranline_addusrid");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.OlcWhztranlines)
                    .HasForeignKey(d => d.Itemid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whztranline_itemid");

                entity.HasOne(d => d.Pordline)
                    .WithMany(p => p.OlcWhztranlines)
                    .HasForeignKey(d => d.Pordlineid)
                    .HasConstraintName("fk_olc_whztranline_pordlineid");

                entity.HasOne(d => d.Sordline)
                    .WithMany(p => p.OlcWhztranlines)
                    .HasForeignKey(d => d.Sordlineid)
                    .HasConstraintName("fk_olc_whztranline_sordlineid");

                entity.HasOne(d => d.Stline)
                    .WithMany(p => p.OlcWhztranlines)
                    .HasForeignKey(d => d.Stlineid)
                    .HasConstraintName("fk_olc_whztranline_stlineid");

                entity.HasOne(d => d.Unitid2Navigation)
                    .WithMany(p => p.OlcWhztranlines)
                    .HasForeignKey(d => d.Unitid2)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whztranline_unitid2");

                entity.HasOne(d => d.Whzt)
                    .WithMany(p => p.OlcWhztranlines)
                    .HasForeignKey(d => d.Whztid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whztranline_whztid");
            });

            modelBuilder.Entity<OlcWhztst>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("olc_whztst");

                entity.HasIndex(e => new { e.Whztid, e.Whztlineid, e.Stid, e.Stlineid }, "uq_olc_whztst")
                    .IsUnique();

                entity.Property(e => e.Stid).HasColumnName("stid");

                entity.Property(e => e.Stlineid).HasColumnName("stlineid");

                entity.Property(e => e.Whztid).HasColumnName("whztid");

                entity.Property(e => e.Whztlineid).HasColumnName("whztlineid");

                entity.HasOne(d => d.St)
                    .WithMany()
                    .HasForeignKey(d => d.Stid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whztst_stid");

                entity.HasOne(d => d.Stline)
                    .WithMany()
                    .HasForeignKey(d => d.Stlineid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whztst_stlineid");

                entity.HasOne(d => d.Whzt)
                    .WithMany()
                    .HasForeignKey(d => d.Whztid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whztst_whztid");

                entity.HasOne(d => d.Whztline)
                    .WithMany()
                    .HasForeignKey(d => d.Whztlineid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whztst_whztlineid");
            });

            modelBuilder.Entity<OlcWhztstdfstock>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("olc_whztstdfstock");

                entity.HasIndex(e => new { e.Whztid, e.Whztlineid, e.Stid, e.Stlineid }, "uq_olc_whztstdfstock")
                    .IsUnique();

                entity.Property(e => e.Stid).HasColumnName("stid");

                entity.Property(e => e.Stlineid).HasColumnName("stlineid");

                entity.Property(e => e.Whztid).HasColumnName("whztid");

                entity.Property(e => e.Whztlineid).HasColumnName("whztlineid");

                entity.HasOne(d => d.St)
                    .WithMany()
                    .HasForeignKey(d => d.Stid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whztstdfstock_stid");

                entity.HasOne(d => d.Stline)
                    .WithMany()
                    .HasForeignKey(d => d.Stlineid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whztstdfstock_stlineid");

                entity.HasOne(d => d.Whzt)
                    .WithMany()
                    .HasForeignKey(d => d.Whztid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whztstdfstock_whztid");

                entity.HasOne(d => d.Whztline)
                    .WithMany()
                    .HasForeignKey(d => d.Whztlineid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whztstdfstock_whztlineid");
            });

            modelBuilder.Entity<OlsCompany>(entity =>
            {
                entity.HasKey(e => e.Cmpid)
                    .HasName("pk_ols_company");

                entity.ToTable("ols_company");

                entity.HasIndex(e => e.Cmpcode, "idx_ols_company_cmpcode")
                    .IsUnique();

                entity.Property(e => e.Cmpid)
                    .ValueGeneratedNever()
                    .HasColumnName("cmpid");

                entity.Property(e => e.Abbr)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("abbr");

                entity.Property(e => e.Adddate)
                    .HasColumnType("datetime")
                    .HasColumnName("adddate");

                entity.Property(e => e.Addusrid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("addusrid");

                entity.Property(e => e.Cmpcode)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("cmpcode");

                entity.Property(e => e.Codacode)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("codacode");

                entity.Property(e => e.Codatempcode)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("codatempcode");

                entity.Property(e => e.Countryid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("countryid");

                entity.Property(e => e.Def).HasColumnName("def");

                entity.Property(e => e.Delstat).HasColumnName("delstat");

                entity.Property(e => e.Dualcurid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("dualcurid");

                entity.Property(e => e.Grp)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("grp");

                entity.Property(e => e.Homecurid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("homecurid");

                entity.Property(e => e.Itembarcodemandtype).HasColumnName("itembarcodemandtype");

                entity.Property(e => e.Note)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("note");

                entity.Property(e => e.Partnid).HasColumnName("partnid");

                entity.Property(e => e.Ratesrctype).HasColumnName("ratesrctype");

                entity.Property(e => e.Selprccodetype).HasColumnName("selprccodetype");

                entity.Property(e => e.Sinvdocnumpartnpropgrpid).HasColumnName("sinvdocnumpartnpropgrpid");

                entity.Property(e => e.Sinvmodtype).HasColumnName("sinvmodtype");

                entity.Property(e => e.Sinvusecorrtype).HasColumnName("sinvusecorrtype");

                entity.Property(e => e.Sname)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("sname");

                entity.Property(e => e.Sordselprcsrctype).HasColumnName("sordselprcsrctype");

                entity.Property(e => e.Sordstaccesstype).HasColumnName("sordstaccesstype");

                entity.Property(e => e.Sordstselprcsrctype).HasColumnName("sordstselprcsrctype");

                entity.Property(e => e.Stcommoncmpcodes)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("stcommoncmpcodes");

                entity.Property(e => e.Xmldata)
                    .HasColumnType("xml")
                    .HasColumnName("xmldata");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsCompanies)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_company_addusrid");
            });

            modelBuilder.Entity<OlsItem>(entity =>
            {
                entity.HasKey(e => e.Itemid)
                    .HasName("pk_ols_item");

                entity.ToTable("ols_item");

                entity.HasIndex(e => e.Itemcode, "idx_ols_item_itemcode")
                    .IsUnique();

                entity.HasIndex(e => e.Name01, "idx_ols_item_name01");

                entity.Property(e => e.Itemid)
                    .ValueGeneratedNever()
                    .HasColumnName("itemid");

                entity.Property(e => e.Adddate)
                    .HasColumnType("datetime")
                    .HasColumnName("adddate");

                entity.Property(e => e.Addusrid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("addusrid");

                entity.Property(e => e.Cmpcodes)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("cmpcodes");

                entity.Property(e => e.Cmpid).HasColumnName("cmpid");

                entity.Property(e => e.Custtarid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("custtarid");

                entity.Property(e => e.Delstat).HasColumnName("delstat");

                entity.Property(e => e.Itemcode)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("itemcode");

                entity.Property(e => e.Itemgrpid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("itemgrpid");

                entity.Property(e => e.Name01)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("name01");

                entity.Property(e => e.Name02)
                    .HasMaxLength(100)
                    .HasColumnName("name02");

                entity.Property(e => e.Name03)
                    .HasMaxLength(100)
                    .HasColumnName("name03");

                entity.Property(e => e.Netweight)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("netweight");

                entity.Property(e => e.Note)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("note");

                entity.Property(e => e.Releasedate)
                    .HasColumnType("datetime")
                    .HasColumnName("releasedate");

                entity.Property(e => e.Rootitemid).HasColumnName("rootitemid");

                entity.Property(e => e.Sname)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("sname");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.Unitid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("unitid");

                entity.Property(e => e.Uqcardtype).HasColumnName("uqcardtype");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsItems)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_item_addusrid");

                entity.HasOne(d => d.Rootitem)
                    .WithMany(p => p.InverseRootitem)
                    .HasForeignKey(d => d.Rootitemid)
                    .HasConstraintName("fk_ols_item_rootitemid");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.OlsItems)
                    .HasForeignKey(d => d.Unitid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_item_unitid");
            });

            modelBuilder.Entity<OlsPordhead>(entity =>
            {
                entity.HasKey(e => e.Pordid)
                    .HasName("pk_ols_pordhead");

                entity.ToTable("ols_pordhead");

                entity.HasIndex(e => e.Cmpid, "idx_ols_pordhead_cmpid");

                entity.HasIndex(e => e.Docnum, "idx_ols_pordhead_docnum");

                entity.HasIndex(e => e.Partnid, "idx_ols_pordhead_partnid");

                entity.HasIndex(e => e.Porddate, "idx_ols_pordhead_porddate");

                entity.HasIndex(e => e.Porddocid, "idx_ols_pordhead_porddocid");

                entity.HasIndex(e => e.Whid, "idx_ols_pordhead_whid");

                entity.Property(e => e.Pordid)
                    .ValueGeneratedNever()
                    .HasColumnName("pordid");

                entity.Property(e => e.Adddate)
                    .HasColumnType("datetime")
                    .HasColumnName("adddate");

                entity.Property(e => e.Addrid).HasColumnName("addrid");

                entity.Property(e => e.Addusrid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("addusrid");

                entity.Property(e => e.Approvalstat).HasColumnName("approvalstat");

                entity.Property(e => e.Cmpid).HasColumnName("cmpid");

                entity.Property(e => e.Curid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("curid");

                entity.Property(e => e.Docnum)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("docnum");

                entity.Property(e => e.Gen).HasColumnName("gen");

                entity.Property(e => e.Langid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("langid");

                entity.Property(e => e.Lastlinenum).HasColumnName("lastlinenum");

                entity.Property(e => e.Note)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("note");

                entity.Property(e => e.Paritycode)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("paritycode");

                entity.Property(e => e.Parityplace)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("parityplace");

                entity.Property(e => e.Partnid).HasColumnName("partnid");

                entity.Property(e => e.Porddate)
                    .HasColumnType("datetime")
                    .HasColumnName("porddate");

                entity.Property(e => e.Porddocid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("porddocid");

                entity.Property(e => e.Pordstat).HasColumnName("pordstat");

                entity.Property(e => e.Pordtype).HasColumnName("pordtype");

                entity.Property(e => e.Projid).HasColumnName("projid");

                entity.Property(e => e.Ref1)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ref1");

                entity.Property(e => e.Whid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("whid");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsPordheads)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_pordhead_addusrid");

                entity.HasOne(d => d.Cmp)
                    .WithMany(p => p.OlsPordheads)
                    .HasForeignKey(d => d.Cmpid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_pordhead_cmpid");

                entity.HasOne(d => d.Wh)
                    .WithMany(p => p.OlsPordheads)
                    .HasForeignKey(d => d.Whid)
                    .HasConstraintName("fk_ols_pordhead_whid");
            });

            modelBuilder.Entity<OlsPordline>(entity =>
            {
                entity.HasKey(e => e.Pordlineid)
                    .HasName("pk_ols_pordline");

                entity.ToTable("ols_pordline");

                entity.HasIndex(e => e.Itemid, "idx_ols_pordline_itemid");

                entity.HasIndex(e => e.Pordid, "idx_ols_pordline_pordid");

                entity.Property(e => e.Pordlineid)
                    .ValueGeneratedNever()
                    .HasColumnName("pordlineid");

                entity.Property(e => e.Adddate)
                    .HasColumnType("datetime")
                    .HasColumnName("adddate");

                entity.Property(e => e.Addusrid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("addusrid");

                entity.Property(e => e.Change)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("change");

                entity.Property(e => e.Confqty)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("confqty");

                entity.Property(e => e.Confqty2)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("confqty2");

                entity.Property(e => e.Confreqdate)
                    .HasColumnType("datetime")
                    .HasColumnName("confreqdate");

                entity.Property(e => e.Def).HasColumnName("def");

                entity.Property(e => e.Gen).HasColumnName("gen");

                entity.Property(e => e.Itemid).HasColumnName("itemid");

                entity.Property(e => e.Linenum).HasColumnName("linenum");

                entity.Property(e => e.Movqty)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("movqty");

                entity.Property(e => e.Movqty2)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("movqty2");

                entity.Property(e => e.Note)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("note");

                entity.Property(e => e.Ordqty)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ordqty");

                entity.Property(e => e.Ordqty2)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ordqty2");

                entity.Property(e => e.Parentpordlineid).HasColumnName("parentpordlineid");

                entity.Property(e => e.Pglineid).HasColumnName("pglineid");

                entity.Property(e => e.Pordid).HasColumnName("pordid");

                entity.Property(e => e.Pordlinestat).HasColumnName("pordlinestat");

                entity.Property(e => e.Purchprc)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("purchprc");

                entity.Property(e => e.Purchprc2)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("purchprc2");

                entity.Property(e => e.Ref2)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ref2");

                entity.Property(e => e.Reqdate)
                    .HasColumnType("datetime")
                    .HasColumnName("reqdate");

                entity.Property(e => e.Unitid2)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("unitid2");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsPordlines)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_pordline_addusrid");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.OlsPordlines)
                    .HasForeignKey(d => d.Itemid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_pordline_itemid");

                entity.HasOne(d => d.Parentpordline)
                    .WithMany(p => p.InverseParentpordline)
                    .HasForeignKey(d => d.Parentpordlineid)
                    .HasConstraintName("fk_ols_pordline_parentpordlineid");

                entity.HasOne(d => d.Pord)
                    .WithMany(p => p.OlsPordlines)
                    .HasForeignKey(d => d.Pordid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_pordline_pordid");

                entity.HasOne(d => d.Unitid2Navigation)
                    .WithMany(p => p.OlsPordlines)
                    .HasForeignKey(d => d.Unitid2)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_pordline_unitid2");
            });

            modelBuilder.Entity<OlsSordhead>(entity =>
            {
                entity.HasKey(e => e.Sordid)
                    .HasName("pk_ols_sordhead");

                entity.ToTable("ols_sordhead");

                entity.HasIndex(e => e.Cmpid, "idx_ols_sordhead_cmpid");

                entity.HasIndex(e => e.Docnum, "idx_ols_sordhead_docnum");

                entity.HasIndex(e => e.Partnid, "idx_ols_sordhead_partnid");

                entity.HasIndex(e => e.Sorddate, "idx_ols_sordhead_sorddate");

                entity.HasIndex(e => e.Sorddocid, "idx_ols_sordhead_sorddocid");

                entity.HasIndex(e => e.Whid, "idx_ols_sordhead_whid");

                entity.Property(e => e.Sordid)
                    .ValueGeneratedNever()
                    .HasColumnName("sordid");

                entity.Property(e => e.Adddate)
                    .HasColumnType("datetime")
                    .HasColumnName("adddate");

                entity.Property(e => e.Addrid).HasColumnName("addrid");

                entity.Property(e => e.Addusrid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("addusrid");

                entity.Property(e => e.Cmpid).HasColumnName("cmpid");

                entity.Property(e => e.Curid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("curid");

                entity.Property(e => e.Docnum)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("docnum");

                entity.Property(e => e.Gen).HasColumnName("gen");

                entity.Property(e => e.Langid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("langid");

                entity.Property(e => e.Lastlinenum).HasColumnName("lastlinenum");

                entity.Property(e => e.Note)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("note");

                entity.Property(e => e.Paritycode)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("paritycode");

                entity.Property(e => e.Parityplace)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("parityplace");

                entity.Property(e => e.Partnid).HasColumnName("partnid");

                entity.Property(e => e.Paycid).HasColumnName("paycid");

                entity.Property(e => e.Paymid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("paymid");

                entity.Property(e => e.Projid).HasColumnName("projid");

                entity.Property(e => e.Ref1)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ref1");

                entity.Property(e => e.Sorddate)
                    .HasColumnType("datetime")
                    .HasColumnName("sorddate");

                entity.Property(e => e.Sorddocid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("sorddocid");

                entity.Property(e => e.Sordstat).HasColumnName("sordstat");

                entity.Property(e => e.Sordtype).HasColumnName("sordtype");

                entity.Property(e => e.Whid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("whid");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsSordheads)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sordhead_addusrid");

                entity.HasOne(d => d.Cmp)
                    .WithMany(p => p.OlsSordheads)
                    .HasForeignKey(d => d.Cmpid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sordhead_cmpid");

                entity.HasOne(d => d.Wh)
                    .WithMany(p => p.OlsSordheads)
                    .HasForeignKey(d => d.Whid)
                    .HasConstraintName("fk_ols_sordhead_whid");
            });

            modelBuilder.Entity<OlsSordline>(entity =>
            {
                entity.HasKey(e => e.Sordlineid)
                    .HasName("pk_ols_sordline");

                entity.ToTable("ols_sordline");

                entity.HasIndex(e => e.Itemid, "idx_ols_sordline_itemid");

                entity.HasIndex(e => e.Resid, "idx_ols_sordline_resid");

                entity.HasIndex(e => e.Sordid, "idx_ols_sordline_sordid");

                entity.HasIndex(e => e.Ucdid, "idx_ols_sordline_ucdid");

                entity.Property(e => e.Sordlineid)
                    .ValueGeneratedNever()
                    .HasColumnName("sordlineid");

                entity.Property(e => e.Adddate)
                    .HasColumnType("datetime")
                    .HasColumnName("adddate");

                entity.Property(e => e.Addusrid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("addusrid");

                entity.Property(e => e.Def).HasColumnName("def");

                entity.Property(e => e.Discpercnt)
                    .HasColumnType("numeric(9, 4)")
                    .HasColumnName("discpercnt");

                entity.Property(e => e.Discpercntprcid).HasColumnName("discpercntprcid");

                entity.Property(e => e.Disctotval)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("disctotval");

                entity.Property(e => e.Discval)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("discval");

                entity.Property(e => e.Gen).HasColumnName("gen");

                entity.Property(e => e.Itemid).HasColumnName("itemid");

                entity.Property(e => e.Linenum).HasColumnName("linenum");

                entity.Property(e => e.Movqty)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("movqty");

                entity.Property(e => e.Note)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("note");

                entity.Property(e => e.Ordqty)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ordqty");

                entity.Property(e => e.Pjpid).HasColumnName("pjpid");

                entity.Property(e => e.Ref2)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ref2");

                entity.Property(e => e.Reqdate)
                    .HasColumnType("datetime")
                    .HasColumnName("reqdate");

                entity.Property(e => e.Resid).HasColumnName("resid");

                entity.Property(e => e.Selprc)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("selprc");

                entity.Property(e => e.Selprcprcid).HasColumnName("selprcprcid");

                entity.Property(e => e.Selprctype).HasColumnName("selprctype");

                entity.Property(e => e.Seltotprc)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("seltotprc");

                entity.Property(e => e.Sordid).HasColumnName("sordid");

                entity.Property(e => e.Sordlinestat).HasColumnName("sordlinestat");

                entity.Property(e => e.Taxid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("taxid");

                entity.Property(e => e.Ucdid).HasColumnName("ucdid");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsSordlines)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sordline_addusrid");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.OlsSordlines)
                    .HasForeignKey(d => d.Itemid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sordline_itemid");

                entity.HasOne(d => d.Sord)
                    .WithMany(p => p.OlsSordlines)
                    .HasForeignKey(d => d.Sordid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sordline_sordid");
            });

            modelBuilder.Entity<OlsSthead>(entity =>
            {
                entity.HasKey(e => e.Stid)
                    .HasName("pk_ols_sthead");

                entity.ToTable("ols_sthead");

                entity.HasIndex(e => e.Cmpid, "idx_ols_sthead_cmpid");

                entity.HasIndex(e => e.Dispstid, "idx_ols_sthead_dispstid");

                entity.HasIndex(e => e.Docnum, "idx_ols_sthead_docnum");

                entity.HasIndex(e => e.Frompartnid, "idx_ols_sthead_frompartnid");

                entity.HasIndex(e => e.Fromwhid, "idx_ols_sthead_fromwhid");

                entity.HasIndex(e => e.Retorigstid, "idx_ols_sthead_retorigstid");

                entity.HasIndex(e => e.Sinvid, "idx_ols_sthead_sinvid");

                entity.HasIndex(e => e.Stdate, "idx_ols_sthead_stdate");

                entity.HasIndex(e => e.Stdocid, "idx_ols_sthead_stdocid");

                entity.HasIndex(e => e.Topartnid, "idx_ols_sthead_topartnid");

                entity.HasIndex(e => e.Towhid, "idx_ols_sthead_towhid");

                entity.Property(e => e.Stid)
                    .ValueGeneratedNever()
                    .HasColumnName("stid");

                entity.Property(e => e.Adddate)
                    .HasColumnType("datetime")
                    .HasColumnName("adddate");

                entity.Property(e => e.Addusrid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("addusrid");

                entity.Property(e => e.Bustypeid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("bustypeid");

                entity.Property(e => e.Closedate)
                    .HasColumnType("datetime")
                    .HasColumnName("closedate");

                entity.Property(e => e.Closeusrid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("closeusrid");

                entity.Property(e => e.Cmpid).HasColumnName("cmpid");

                entity.Property(e => e.Colli)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("colli");

                entity.Property(e => e.Cootype).HasColumnName("cootype");

                entity.Property(e => e.Corrstid).HasColumnName("corrstid");

                entity.Property(e => e.Corrtype).HasColumnName("corrtype");

                entity.Property(e => e.Curid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("curid");

                entity.Property(e => e.Dispstid).HasColumnName("dispstid");

                entity.Property(e => e.Docnum)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("docnum");

                entity.Property(e => e.Fromaddrid).HasColumnName("fromaddrid");

                entity.Property(e => e.Frompartnid).HasColumnName("frompartnid");

                entity.Property(e => e.Fromwhid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("fromwhid");

                entity.Property(e => e.Gen).HasColumnName("gen");

                entity.Property(e => e.Grossweight)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("grossweight");

                entity.Property(e => e.Intransittowhid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("intransittowhid");

                entity.Property(e => e.Intransitwhid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("intransitwhid");

                entity.Property(e => e.Langid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("langid");

                entity.Property(e => e.Lastlinenum).HasColumnName("lastlinenum");

                entity.Property(e => e.Manufusestat).HasColumnName("manufusestat");

                entity.Property(e => e.Netweight)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("netweight");

                entity.Property(e => e.Note)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("note");

                entity.Property(e => e.Origstid).HasColumnName("origstid");

                entity.Property(e => e.Paycid).HasColumnName("paycid");

                entity.Property(e => e.Paymid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("paymid");

                entity.Property(e => e.Pinvid).HasColumnName("pinvid");

                entity.Property(e => e.Prodrecstid).HasColumnName("prodrecstid");

                entity.Property(e => e.Projid).HasColumnName("projid");

                entity.Property(e => e.Ref1)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ref1");

                entity.Property(e => e.Ref2)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ref2");

                entity.Property(e => e.Reldatemax)
                    .HasColumnType("datetime")
                    .HasColumnName("reldatemax");

                entity.Property(e => e.Retorigstid).HasColumnName("retorigstid");

                entity.Property(e => e.Shipparitycode)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("shipparitycode");

                entity.Property(e => e.Shipparityplace)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("shipparityplace");

                entity.Property(e => e.Sinvid).HasColumnName("sinvid");

                entity.Property(e => e.Stdate)
                    .HasColumnType("datetime")
                    .HasColumnName("stdate");

                entity.Property(e => e.Stdocid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("stdocid");

                entity.Property(e => e.Ststat).HasColumnName("ststat");

                entity.Property(e => e.Sttype).HasColumnName("sttype");

                entity.Property(e => e.Taxdatetype).HasColumnName("taxdatetype");

                entity.Property(e => e.Toaddrid).HasColumnName("toaddrid");

                entity.Property(e => e.Topartnid).HasColumnName("topartnid");

                entity.Property(e => e.Towhid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("towhid");

                entity.Property(e => e.Transpid).HasColumnName("transpid");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsStheadAddusrs)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sthead_addusrid");

                entity.HasOne(d => d.Closeusr)
                    .WithMany(p => p.OlsStheadCloseusrs)
                    .HasForeignKey(d => d.Closeusrid)
                    .HasConstraintName("fk_ols_sthead_closeusrid");

                entity.HasOne(d => d.Cmp)
                    .WithMany(p => p.OlsStheads)
                    .HasForeignKey(d => d.Cmpid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_sthead_cmpid");

                entity.HasOne(d => d.Corrst)
                    .WithMany(p => p.InverseCorrst)
                    .HasForeignKey(d => d.Corrstid)
                    .HasConstraintName("fk_ols_sthead_corrstid");

                entity.HasOne(d => d.Dispst)
                    .WithMany(p => p.InverseDispst)
                    .HasForeignKey(d => d.Dispstid)
                    .HasConstraintName("fk_ols_sthead_dispstid");

                entity.HasOne(d => d.Fromwh)
                    .WithMany(p => p.OlsStheadFromwhs)
                    .HasForeignKey(d => d.Fromwhid)
                    .HasConstraintName("fk_ols_sthead_fromwhid");

                entity.HasOne(d => d.Intransittowh)
                    .WithMany(p => p.OlsStheadIntransittowhs)
                    .HasForeignKey(d => d.Intransittowhid)
                    .HasConstraintName("fk_ols_sthead_intransittowhid");

                entity.HasOne(d => d.Intransitwh)
                    .WithMany(p => p.OlsStheadIntransitwhs)
                    .HasForeignKey(d => d.Intransitwhid)
                    .HasConstraintName("fk_ols_sthead_intransitwhid");

                entity.HasOne(d => d.Origst)
                    .WithMany(p => p.InverseOrigst)
                    .HasForeignKey(d => d.Origstid)
                    .HasConstraintName("fk_ols_sthead_origstid");

                entity.HasOne(d => d.Prodrecst)
                    .WithMany(p => p.InverseProdrecst)
                    .HasForeignKey(d => d.Prodrecstid)
                    .HasConstraintName("fk_ols_sthead_prodrecstid");

                entity.HasOne(d => d.Retorigst)
                    .WithMany(p => p.InverseRetorigst)
                    .HasForeignKey(d => d.Retorigstid)
                    .HasConstraintName("fk_ols_sthead_retorigstid");

                entity.HasOne(d => d.Towh)
                    .WithMany(p => p.OlsStheadTowhs)
                    .HasForeignKey(d => d.Towhid)
                    .HasConstraintName("fk_ols_sthead_towhid");
            });

            modelBuilder.Entity<OlsStline>(entity =>
            {
                entity.HasKey(e => e.Stlineid)
                    .HasName("pk_ols_stline");

                entity.ToTable("ols_stline");

                entity.HasIndex(e => e.Delstlineid, "idx_ols_stline_delstlineid");

                entity.HasIndex(e => e.Icstlineid, "idx_ols_stline_icstlineid");

                entity.HasIndex(e => e.Intransitstlineid, "idx_ols_stline_intransitstlineid");

                entity.HasIndex(e => e.Itemid, "idx_ols_stline_itemid");

                entity.HasIndex(e => e.Origstlineid, "idx_ols_stline_origstlineid");

                entity.HasIndex(e => e.Pordlineid, "idx_ols_stline_pordlineid");

                entity.HasIndex(e => e.Reccorstlineid, "idx_ols_stline_reccorstlineid");

                entity.HasIndex(e => e.Reqid, "idx_ols_stline_reqid");

                entity.HasIndex(e => e.Retorigstlineid, "idx_ols_stline_retorigstlineid");

                entity.HasIndex(e => e.Sordlineid, "idx_ols_stline_sordlineid");

                entity.HasIndex(e => e.Stid, "idx_ols_stline_stid");

                entity.HasIndex(e => new { e.Stid, e.Linenum }, "uq_ols_stline")
                    .IsUnique();

                entity.Property(e => e.Stlineid)
                    .ValueGeneratedNever()
                    .HasColumnName("stlineid");

                entity.Property(e => e.Adddate)
                    .HasColumnType("datetime")
                    .HasColumnName("adddate");

                entity.Property(e => e.Addusrid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("addusrid");

                entity.Property(e => e.Bsordlineid).HasColumnName("bsordlineid");

                entity.Property(e => e.Change)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("change");

                entity.Property(e => e.Corrtype).HasColumnName("corrtype");

                entity.Property(e => e.Costcalcfix).HasColumnName("costcalcfix");

                entity.Property(e => e.Costprc)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("costprc");

                entity.Property(e => e.Costprcdual)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("costprcdual");

                entity.Property(e => e.Costprchome)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("costprchome");

                entity.Property(e => e.Costval)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("costval");

                entity.Property(e => e.Costvaldual)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("costvaldual");

                entity.Property(e => e.Costvalhome)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("costvalhome");

                entity.Property(e => e.Delstlineid).HasColumnName("delstlineid");

                entity.Property(e => e.Dglineid).HasColumnName("dglineid");

                entity.Property(e => e.Discpercnt)
                    .HasColumnType("numeric(9, 4)")
                    .HasColumnName("discpercnt");

                entity.Property(e => e.Discpercntprcid).HasColumnName("discpercntprcid");

                entity.Property(e => e.Disctotval)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("disctotval");

                entity.Property(e => e.Discval)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("discval");

                entity.Property(e => e.Dispqty)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("dispqty");

                entity.Property(e => e.Dispqty2)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("dispqty2");

                entity.Property(e => e.Gen).HasColumnName("gen");

                entity.Property(e => e.Icstlineid).HasColumnName("icstlineid");

                entity.Property(e => e.Inqty)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("inqty");

                entity.Property(e => e.Intransitstlineid).HasColumnName("intransitstlineid");

                entity.Property(e => e.Invdiscpercnt)
                    .HasColumnType("numeric(9, 4)")
                    .HasColumnName("invdiscpercnt");

                entity.Property(e => e.Invdisctotval)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("invdisctotval");

                entity.Property(e => e.Invdiscval)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("invdiscval");

                entity.Property(e => e.Invnetval)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("invnetval");

                entity.Property(e => e.Invselprc)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("invselprc");

                entity.Property(e => e.Invseltotprc)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("invseltotprc");

                entity.Property(e => e.Invseltotval)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("invseltotval");

                entity.Property(e => e.Invselval)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("invselval");

                entity.Property(e => e.Invtaxval)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("invtaxval");

                entity.Property(e => e.Invtotval)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("invtotval");

                entity.Property(e => e.Itemid).HasColumnName("itemid");

                entity.Property(e => e.Linenum).HasColumnName("linenum");

                entity.Property(e => e.Mediatedservices).HasColumnName("mediatedservices");

                entity.Property(e => e.Mordlineid).HasColumnName("mordlineid");

                entity.Property(e => e.Movqty)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("movqty");

                entity.Property(e => e.Movqty2)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("movqty2");

                entity.Property(e => e.Netval)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("netval");

                entity.Property(e => e.Note)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("note");

                entity.Property(e => e.Ordqty)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ordqty");

                entity.Property(e => e.Ordqty2)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ordqty2");

                entity.Property(e => e.Origid)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("origid");

                entity.Property(e => e.Origstlineid).HasColumnName("origstlineid");

                entity.Property(e => e.Outqty)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("outqty");

                entity.Property(e => e.Philineid).HasColumnName("philineid");

                entity.Property(e => e.Pjpid).HasColumnName("pjpid");

                entity.Property(e => e.Pordlineid).HasColumnName("pordlineid");

                entity.Property(e => e.Pplid).HasColumnName("pplid");

                entity.Property(e => e.Prodrepid).HasColumnName("prodrepid");

                entity.Property(e => e.Reccorstlineid).HasColumnName("reccorstlineid");

                entity.Property(e => e.Reqid).HasColumnName("reqid");

                entity.Property(e => e.Retlineid).HasColumnName("retlineid");

                entity.Property(e => e.Retorigstlineid).HasColumnName("retorigstlineid");

                entity.Property(e => e.Selprc)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("selprc");

                entity.Property(e => e.Selprcprcid).HasColumnName("selprcprcid");

                entity.Property(e => e.Selprctype).HasColumnName("selprctype");

                entity.Property(e => e.Seltotprc)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("seltotprc");

                entity.Property(e => e.Seltotval)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("seltotval");

                entity.Property(e => e.Selval)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("selval");

                entity.Property(e => e.Sordlineid).HasColumnName("sordlineid");

                entity.Property(e => e.Stid).HasColumnName("stid");

                entity.Property(e => e.Svcwsid).HasColumnName("svcwsid");

                entity.Property(e => e.Taxid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("taxid");

                entity.Property(e => e.Unitid2)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("unitid2");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsStlines)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_stline_addusrid");

                entity.HasOne(d => d.Bsordline)
                    .WithMany(p => p.OlsStlineBsordlines)
                    .HasForeignKey(d => d.Bsordlineid)
                    .HasConstraintName("fk_ols_stline_bsordlineid");

                entity.HasOne(d => d.Delstline)
                    .WithMany(p => p.InverseDelstline)
                    .HasForeignKey(d => d.Delstlineid)
                    .HasConstraintName("fk_ols_stline_delstlineid");

                entity.HasOne(d => d.Icstline)
                    .WithMany(p => p.InverseIcstline)
                    .HasForeignKey(d => d.Icstlineid)
                    .HasConstraintName("fk_ols_stline_icstlineid");

                entity.HasOne(d => d.Intransitstline)
                    .WithMany(p => p.InverseIntransitstline)
                    .HasForeignKey(d => d.Intransitstlineid)
                    .HasConstraintName("fk_ols_stline_intransitstlineid");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.OlsStlines)
                    .HasForeignKey(d => d.Itemid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_stline_itemid");

                entity.HasOne(d => d.Origstline)
                    .WithMany(p => p.InverseOrigstline)
                    .HasForeignKey(d => d.Origstlineid)
                    .HasConstraintName("fk_ols_stline_origstlineid");

                entity.HasOne(d => d.Pordline)
                    .WithMany(p => p.OlsStlines)
                    .HasForeignKey(d => d.Pordlineid)
                    .HasConstraintName("fk_ols_stline_pordlineid");

                entity.HasOne(d => d.Reccorstline)
                    .WithMany(p => p.InverseReccorstline)
                    .HasForeignKey(d => d.Reccorstlineid)
                    .HasConstraintName("fk_ols_stline_reccorstlineid");

                entity.HasOne(d => d.Retorigstline)
                    .WithMany(p => p.InverseRetorigstline)
                    .HasForeignKey(d => d.Retorigstlineid)
                    .HasConstraintName("fk_ols_stline_retorigstlineid");

                entity.HasOne(d => d.Sordline)
                    .WithMany(p => p.OlsStlineSordlines)
                    .HasForeignKey(d => d.Sordlineid)
                    .HasConstraintName("fk_ols_stline_sordlineid");

                entity.HasOne(d => d.St)
                    .WithMany(p => p.OlsStlines)
                    .HasForeignKey(d => d.Stid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_stline_stid");

                entity.HasOne(d => d.Unitid2Navigation)
                    .WithMany(p => p.OlsStlines)
                    .HasForeignKey(d => d.Unitid2)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_stline_unitid2");
            });

            modelBuilder.Entity<OlsUnit>(entity =>
            {
                entity.HasKey(e => e.Unitid)
                    .HasName("pk_ols_unit");

                entity.ToTable("ols_unit");

                entity.Property(e => e.Unitid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("unitid");

                entity.Property(e => e.Adddate)
                    .HasColumnType("datetime")
                    .HasColumnName("adddate");

                entity.Property(e => e.Addusrid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("addusrid");

                entity.Property(e => e.Code2)
                    .HasMaxLength(12)
                    .HasColumnName("code2");

                entity.Property(e => e.Delstat).HasColumnName("delstat");

                entity.Property(e => e.Name)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Navcode)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("navcode");

                entity.Property(e => e.Note)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("note");

                entity.Property(e => e.Unittype).HasColumnName("unittype");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsUnits)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_unit_addusrid");
            });

            modelBuilder.Entity<OlsWarehouse>(entity =>
            {
                entity.HasKey(e => e.Whid)
                    .HasName("pk_ols_warehouse");

                entity.ToTable("ols_warehouse");

                entity.Property(e => e.Whid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("whid");

                entity.Property(e => e.Adddate)
                    .HasColumnType("datetime")
                    .HasColumnName("adddate");

                entity.Property(e => e.Addrid).HasColumnName("addrid");

                entity.Property(e => e.Addusrid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("addusrid");

                entity.Property(e => e.Backordertype).HasColumnName("backordertype");

                entity.Property(e => e.Cmpcodes)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("cmpcodes");

                entity.Property(e => e.Cmpid).HasColumnName("cmpid");

                entity.Property(e => e.Codacode)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("codacode");

                entity.Property(e => e.Delstat).HasColumnName("delstat");

                entity.Property(e => e.Loctype).HasColumnName("loctype");

                entity.Property(e => e.Name)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Note)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("note");

                entity.Property(e => e.Partnid).HasColumnName("partnid");

                entity.Property(e => e.Pickmtype).HasColumnName("pickmtype");

                entity.Property(e => e.Projid).HasColumnName("projid");

                entity.Property(e => e.Storemtype).HasColumnName("storemtype");

                entity.Property(e => e.Whtype).HasColumnName("whtype");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsWarehouses)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_warehouse_addusrid");
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
}
