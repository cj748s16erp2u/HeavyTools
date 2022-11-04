using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;

namespace eLog.HeavyTools.Services.WhZone.DataAccess.Context
{
    public partial class WhZoneDbContext : DbContext
    {
        public WhZoneDbContext()
        {
        }

        public WhZoneDbContext(DbContextOptions<WhZoneDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CfwGroup> CfwGroups { get; set; } = null!;
        public virtual DbSet<CfwUser> CfwUsers { get; set; } = null!;
        public virtual DbSet<CfwUsergroup> CfwUsergroups { get; set; } = null!;
        public virtual DbSet<OlcWhlocation> OlcWhlocations { get; set; } = null!;
        public virtual DbSet<OlcWhzone> OlcWhzones { get; set; } = null!;
        public virtual DbSet<OlcWhzstockmap> OlcWhzstockmaps { get; set; } = null!;
        public virtual DbSet<OlcWhztranhead> OlcWhztranheads { get; set; } = null!;
        public virtual DbSet<OlcWhztranline> OlcWhztranlines { get; set; } = null!;
        public virtual DbSet<OlcWhztranloc> OlcWhztranlocs { get; set; } = null!;
        public virtual DbSet<OlsCompany> OlsCompanies { get; set; } = null!;
        public virtual DbSet<OlsItem> OlsItems { get; set; } = null!;
        public virtual DbSet<OlsStathead> OlsStatheads { get; set; } = null!;
        public virtual DbSet<OlsStatline> OlsStatlines { get; set; } = null!;
        public virtual DbSet<OlsSthead> OlsStheads { get; set; } = null!;
        public virtual DbSet<OlsStline> OlsStlines { get; set; } = null!;
        public virtual DbSet<OlsSysval> OlsSysvals { get; set; } = null!;
        public virtual DbSet<OlsUnit> OlsUnits { get; set; } = null!;
        public virtual DbSet<OlsWarehouse> OlsWarehouses { get; set; } = null!;

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

            modelBuilder.Entity<OlcWhlocation>(entity =>
            {
                entity.HasKey(e => e.Whlocid)
                    .HasName("pk_olc_whlocation");

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

            modelBuilder.Entity<OlcWhzone>(entity =>
            {
                entity.HasKey(e => e.Whzoneid)
                    .HasName("pk_olc_whzone");

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

            modelBuilder.Entity<OlcWhzstockmap>(entity =>
            {
                entity.HasKey(e => e.Whzstockmapid)
                    .HasName("pk_olc_whzstockmap");

                entity.Property(e => e.Provqty).HasComputedColumnSql("(([actqty]+[recqty])-[resqty])", true);

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

            modelBuilder.Entity<OlcWhztranloc>(entity =>
            {
                entity.HasKey(e => e.Whztlocid)
                    .HasName("pk_olc_whztranloc");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlcWhztranlocs)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whztranloc_addusrid");

                entity.HasOne(d => d.Wh)
                    .WithMany(p => p.OlcWhztranlocs)
                    .HasForeignKey(d => d.Whid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whztranloc_whid");

                entity.HasOne(d => d.Whloc)
                    .WithMany(p => p.OlcWhztranlocs)
                    .HasForeignKey(d => d.Whlocid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whztranloc_whlocid");

                entity.HasOne(d => d.Whzone)
                    .WithMany(p => p.OlcWhztranlocs)
                    .HasForeignKey(d => d.Whzoneid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whztranloc_whzoneid");

                entity.HasOne(d => d.Whzt)
                    .WithMany(p => p.OlcWhztranlocs)
                    .HasForeignKey(d => d.Whztid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whztranloc_whztid");

                entity.HasOne(d => d.Whztline)
                    .WithMany(p => p.OlcWhztranlocs)
                    .HasForeignKey(d => d.Whztlineid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_olc_whztranloc_whztlineid");
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

            modelBuilder.Entity<OlsItem>(entity =>
            {
                entity.HasKey(e => e.Itemid)
                    .HasName("pk_ols_item");

                entity.Property(e => e.Itemid).ValueGeneratedNever();

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

            modelBuilder.Entity<OlsStathead>(entity =>
            {
                entity.HasKey(e => e.Statgrpid)
                    .HasName("pk_ols_stathead");

                entity.Property(e => e.Statgrpid).ValueGeneratedNever();

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsStatheads)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_stathead_addusrid");
            });

            modelBuilder.Entity<OlsStatline>(entity =>
            {
                entity.HasKey(e => new { e.Statgrpid, e.Value })
                    .HasName("pk_ols_statline");

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsStatlines)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_statline_addusrid");

                entity.HasOne(d => d.Statgrp)
                    .WithMany(p => p.OlsStatlines)
                    .HasForeignKey(d => d.Statgrpid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_statline_statgrpid");
            });

            modelBuilder.Entity<OlsSthead>(entity =>
            {
                entity.HasKey(e => e.Stid)
                    .HasName("pk_ols_sthead");

                entity.Property(e => e.Stid).ValueGeneratedNever();

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

                entity.Property(e => e.Stlineid).ValueGeneratedNever();

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsStlines)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_stline_addusrid");

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

                entity.HasOne(d => d.Reccorstline)
                    .WithMany(p => p.InverseReccorstline)
                    .HasForeignKey(d => d.Reccorstlineid)
                    .HasConstraintName("fk_ols_stline_reccorstlineid");

                entity.HasOne(d => d.Retorigstline)
                    .WithMany(p => p.InverseRetorigstline)
                    .HasForeignKey(d => d.Retorigstlineid)
                    .HasConstraintName("fk_ols_stline_retorigstlineid");

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

            modelBuilder.Entity<OlsSysval>(entity =>
            {
                entity.HasKey(e => e.Sysvalid)
                    .HasName("pk_ols_sysval");
            });

            modelBuilder.Entity<OlsUnit>(entity =>
            {
                entity.HasKey(e => e.Unitid)
                    .HasName("pk_ols_unit");

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

                entity.HasOne(d => d.Addusr)
                    .WithMany(p => p.OlsWarehouses)
                    .HasForeignKey(d => d.Addusrid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ols_warehouse_addusrid");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
