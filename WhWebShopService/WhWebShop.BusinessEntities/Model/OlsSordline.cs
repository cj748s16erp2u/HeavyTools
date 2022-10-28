using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("ols_sordline")]
    [Index("Itemid", Name = "idx_ols_sordline_itemid")]
    [Index("Resid", Name = "idx_ols_sordline_resid")]
    [Index("Sordid", Name = "idx_ols_sordline_sordid")]
    [Index("Ucdid", Name = "idx_ols_sordline_ucdid")]
    public partial class OlsSordline : Entity
    {
        public OlsSordline()
        {
            OlcSordlinePreordersordline = new HashSet<OlcSordline>();
            OlcSordlineResPreordersordline = new HashSet<OlcSordlineRes>();
            OlcSordlineResSordline = new HashSet<OlcSordlineRes>();
            OlsTmpSordst = new HashSet<OlsTmpSordst>();
        }

        [Key]
        [Column("sordlineid")]
        public int Sordlineid { get; set; }
        [Column("sordid")]
        public int Sordid { get; set; }
        [Column("linenum")]
        public int Linenum { get; set; }
        [Column("def")]
        public int Def { get; set; }
        [Column("itemid")]
        public int Itemid { get; set; }
        [Column("reqdate", TypeName = "datetime")]
        public DateTime Reqdate { get; set; }
        [Column("ref2")]
        [StringLength(30)]
        [Unicode(false)]
        public string? Ref2 { get; set; }
        [Column("ordqty", TypeName = "numeric(19, 6)")]
        public decimal Ordqty { get; set; }
        [Column("movqty", TypeName = "numeric(19, 6)")]
        public decimal Movqty { get; set; }
        [Column("selprc", TypeName = "numeric(19, 6)")]
        public decimal Selprc { get; set; }
        [Column("seltotprc", TypeName = "numeric(19, 6)")]
        public decimal? Seltotprc { get; set; }
        [Column("selprctype")]
        public int? Selprctype { get; set; }
        [Column("selprcprcid")]
        public int? Selprcprcid { get; set; }
        [Column("discpercnt", TypeName = "numeric(9, 4)")]
        public decimal Discpercnt { get; set; }
        [Column("discpercntprcid")]
        public int? Discpercntprcid { get; set; }
        [Column("discval", TypeName = "numeric(19, 6)")]
        public decimal Discval { get; set; }
        [Column("disctotval", TypeName = "numeric(19, 6)")]
        public decimal? Disctotval { get; set; }
        [Column("taxid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Taxid { get; set; } = null!;
        [Column("sordlinestat")]
        public int Sordlinestat { get; set; }
        [Column("note")]
        [StringLength(200)]
        [Unicode(false)]
        public string? Note { get; set; }
        [Column("resid")]
        public int? Resid { get; set; }
        [Column("ucdid")]
        public int? Ucdid { get; set; }
        [Column("pjpid")]
        public int? Pjpid { get; set; }
        [Column("gen")]
        public int Gen { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlsSordline")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [ForeignKey("Itemid")]
        [InverseProperty("OlsSordline")]
        public virtual OlsItem Item { get; set; } = null!;
        [ForeignKey("Resid")]
        [InverseProperty("OlsSordline")]
        public virtual OlsReserve? Res { get; set; }
        [ForeignKey("Sordid")]
        [InverseProperty("OlsSordline")]
        public virtual OlsSordhead Sord { get; set; } = null!;
        [ForeignKey("Taxid")]
        [InverseProperty("OlsSordline")]
        public virtual OlsTax Tax { get; set; } = null!;
        [InverseProperty("Sordline")]
        public virtual OlcSordline OlcSordlineSordline { get; set; } = null!;
        [InverseProperty("Preordersordline")]
        public virtual ICollection<OlcSordline> OlcSordlinePreordersordline { get; set; }
        [InverseProperty("Preordersordline")]
        public virtual ICollection<OlcSordlineRes> OlcSordlineResPreordersordline { get; set; }
        [InverseProperty("Sordline")]
        public virtual ICollection<OlcSordlineRes> OlcSordlineResSordline { get; set; }
        [InverseProperty("Sordline")]
        public virtual ICollection<OlsTmpSordst> OlsTmpSordst { get; set; }
    }
}