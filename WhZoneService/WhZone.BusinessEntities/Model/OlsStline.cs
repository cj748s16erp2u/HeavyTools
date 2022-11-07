using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFC = Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    [Table("ols_stline")]
    [EFC.Index("Delstlineid", Name = "idx_ols_stline_delstlineid")]
    [EFC.Index("Icstlineid", Name = "idx_ols_stline_icstlineid")]
    [EFC.Index("Intransitstlineid", Name = "idx_ols_stline_intransitstlineid")]
    [EFC.Index("Itemid", Name = "idx_ols_stline_itemid")]
    [EFC.Index("Origstlineid", Name = "idx_ols_stline_origstlineid")]
    [EFC.Index("Pordlineid", Name = "idx_ols_stline_pordlineid")]
    [EFC.Index("Reccorstlineid", Name = "idx_ols_stline_reccorstlineid")]
    [EFC.Index("Reqid", Name = "idx_ols_stline_reqid")]
    [EFC.Index("Retorigstlineid", Name = "idx_ols_stline_retorigstlineid")]
    [EFC.Index("Sordlineid", Name = "idx_ols_stline_sordlineid")]
    [EFC.Index("Stid", Name = "idx_ols_stline_stid")]
    [EFC.Index("Stid", "Linenum", Name = "uq_ols_stline", IsUnique = true)]
    public partial class OlsStline
    {
        public OlsStline()
        {
            InverseDelstline = new HashSet<OlsStline>();
            InverseIcstline = new HashSet<OlsStline>();
            InverseIntransitstline = new HashSet<OlsStline>();
            InverseOrigstline = new HashSet<OlsStline>();
            InverseReccorstline = new HashSet<OlsStline>();
            InverseRetorigstline = new HashSet<OlsStline>();
            OlcWhztranlines = new HashSet<OlcWhztranline>();
        }

        [Key]
        [Column("stlineid")]
        public int Stlineid { get; set; }
        [Column("stid")]
        public int Stid { get; set; }
        [Column("linenum")]
        public int Linenum { get; set; }
        [Column("itemid")]
        public int Itemid { get; set; }
        [Column("ordqty", TypeName = "numeric(19, 6)")]
        public decimal Ordqty { get; set; }
        [Column("dispqty", TypeName = "numeric(19, 6)")]
        public decimal Dispqty { get; set; }
        [Column("movqty", TypeName = "numeric(19, 6)")]
        public decimal Movqty { get; set; }
        [Column("inqty", TypeName = "numeric(19, 6)")]
        public decimal Inqty { get; set; }
        [Column("outqty", TypeName = "numeric(19, 6)")]
        public decimal Outqty { get; set; }
        [Column("unitid2")]
        [StringLength(12)]
        [EFC.Unicode(false)]
        public string Unitid2 { get; set; } = null!;
        [Column("change", TypeName = "numeric(19, 6)")]
        public decimal Change { get; set; }
        [Column("ordqty2", TypeName = "numeric(19, 6)")]
        public decimal Ordqty2 { get; set; }
        [Column("dispqty2", TypeName = "numeric(19, 6)")]
        public decimal Dispqty2 { get; set; }
        [Column("movqty2", TypeName = "numeric(19, 6)")]
        public decimal Movqty2 { get; set; }
        [Column("costprc", TypeName = "numeric(19, 6)")]
        public decimal Costprc { get; set; }
        [Column("costprchome", TypeName = "numeric(19, 6)")]
        public decimal Costprchome { get; set; }
        [Column("costprcdual", TypeName = "numeric(19, 6)")]
        public decimal? Costprcdual { get; set; }
        [Column("costval", TypeName = "numeric(19, 6)")]
        public decimal Costval { get; set; }
        [Column("costvalhome", TypeName = "numeric(19, 6)")]
        public decimal Costvalhome { get; set; }
        [Column("costvaldual", TypeName = "numeric(19, 6)")]
        public decimal? Costvaldual { get; set; }
        [Column("selprc", TypeName = "numeric(19, 6)")]
        public decimal? Selprc { get; set; }
        [Column("seltotprc", TypeName = "numeric(19, 6)")]
        public decimal? Seltotprc { get; set; }
        [Column("selprctype")]
        public int? Selprctype { get; set; }
        [Column("selprcprcid")]
        public int? Selprcprcid { get; set; }
        [Column("selval", TypeName = "numeric(19, 6)")]
        public decimal? Selval { get; set; }
        [Column("seltotval", TypeName = "numeric(19, 6)")]
        public decimal? Seltotval { get; set; }
        [Column("discpercnt", TypeName = "numeric(9, 4)")]
        public decimal? Discpercnt { get; set; }
        [Column("discpercntprcid")]
        public int? Discpercntprcid { get; set; }
        [Column("discval", TypeName = "numeric(19, 6)")]
        public decimal? Discval { get; set; }
        [Column("disctotval", TypeName = "numeric(19, 6)")]
        public decimal? Disctotval { get; set; }
        [Column("netval", TypeName = "numeric(19, 6)")]
        public decimal? Netval { get; set; }
        [Column("taxid")]
        [StringLength(12)]
        [EFC.Unicode(false)]
        public string Taxid { get; set; } = null!;
        [Column("invselprc", TypeName = "numeric(19, 6)")]
        public decimal? Invselprc { get; set; }
        [Column("invseltotprc", TypeName = "numeric(19, 6)")]
        public decimal? Invseltotprc { get; set; }
        [Column("invselval", TypeName = "numeric(19, 6)")]
        public decimal? Invselval { get; set; }
        [Column("invseltotval", TypeName = "numeric(19, 6)")]
        public decimal? Invseltotval { get; set; }
        [Column("invdiscpercnt", TypeName = "numeric(9, 4)")]
        public decimal? Invdiscpercnt { get; set; }
        [Column("invdiscval", TypeName = "numeric(19, 6)")]
        public decimal? Invdiscval { get; set; }
        [Column("invdisctotval", TypeName = "numeric(19, 6)")]
        public decimal? Invdisctotval { get; set; }
        [Column("invnetval", TypeName = "numeric(19, 6)")]
        public decimal? Invnetval { get; set; }
        [Column("invtaxval", TypeName = "numeric(19, 6)")]
        public decimal? Invtaxval { get; set; }
        [Column("invtotval", TypeName = "numeric(19, 6)")]
        public decimal? Invtotval { get; set; }
        [Column("note")]
        [StringLength(200)]
        [EFC.Unicode(false)]
        public string? Note { get; set; }
        [Column("origid")]
        [StringLength(6)]
        [EFC.Unicode(false)]
        public string? Origid { get; set; }
        [Column("corrtype")]
        public int Corrtype { get; set; }
        [Column("origstlineid")]
        public int? Origstlineid { get; set; }
        [Column("delstlineid")]
        public int? Delstlineid { get; set; }
        [Column("sordlineid")]
        public int? Sordlineid { get; set; }
        [Column("bsordlineid")]
        public int? Bsordlineid { get; set; }
        [Column("pordlineid")]
        public int? Pordlineid { get; set; }
        [Column("dglineid")]
        public int? Dglineid { get; set; }
        [Column("retlineid")]
        public int? Retlineid { get; set; }
        [Column("philineid")]
        public int? Philineid { get; set; }
        [Column("reqid")]
        public int? Reqid { get; set; }
        [Column("intransitstlineid")]
        public int? Intransitstlineid { get; set; }
        [Column("mordlineid")]
        public int? Mordlineid { get; set; }
        [Column("retorigstlineid")]
        public int? Retorigstlineid { get; set; }
        [Column("reccorstlineid")]
        public int? Reccorstlineid { get; set; }
        [Column("reccorrected")]
        public int? Reccorrected { get; set; }
        [Column("prodrepid")]
        public int? Prodrepid { get; set; }
        [Column("pplid")]
        public int? Pplid { get; set; }
        [Column("costcalcfix")]
        public int? Costcalcfix { get; set; }
        [Column("pjpid")]
        public int? Pjpid { get; set; }
        [Column("icstlineid")]
        public int? Icstlineid { get; set; }
        [Column("mediatedservices")]
        public int? Mediatedservices { get; set; }
        [Column("svcwsid")]
        public int? Svcwsid { get; set; }
        [Column("gen")]
        public int Gen { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlsStlines")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [ForeignKey("Delstlineid")]
        [InverseProperty("InverseDelstline")]
        public virtual OlsStline? Delstline { get; set; }
        [ForeignKey("Icstlineid")]
        [InverseProperty("InverseIcstline")]
        public virtual OlsStline? Icstline { get; set; }
        [ForeignKey("Intransitstlineid")]
        [InverseProperty("InverseIntransitstline")]
        public virtual OlsStline? Intransitstline { get; set; }
        [ForeignKey("Itemid")]
        [InverseProperty("OlsStlines")]
        public virtual OlsItem Item { get; set; } = null!;
        [ForeignKey("Origstlineid")]
        [InverseProperty("InverseOrigstline")]
        public virtual OlsStline? Origstline { get; set; }
        [ForeignKey("Reccorstlineid")]
        [InverseProperty("InverseReccorstline")]
        public virtual OlsStline? Reccorstline { get; set; }
        [ForeignKey("Retorigstlineid")]
        [InverseProperty("InverseRetorigstline")]
        public virtual OlsStline? Retorigstline { get; set; }
        [ForeignKey("Stid")]
        [InverseProperty("OlsStlines")]
        public virtual OlsSthead St { get; set; } = null!;
        [ForeignKey("Unitid2")]
        [InverseProperty("OlsStlines")]
        public virtual OlsUnit Unitid2Navigation { get; set; } = null!;
        [InverseProperty("Delstline")]
        public virtual ICollection<OlsStline> InverseDelstline { get; set; }
        [InverseProperty("Icstline")]
        public virtual ICollection<OlsStline> InverseIcstline { get; set; }
        [InverseProperty("Intransitstline")]
        public virtual ICollection<OlsStline> InverseIntransitstline { get; set; }
        [InverseProperty("Origstline")]
        public virtual ICollection<OlsStline> InverseOrigstline { get; set; }
        [InverseProperty("Reccorstline")]
        public virtual ICollection<OlsStline> InverseReccorstline { get; set; }
        [InverseProperty("Retorigstline")]
        public virtual ICollection<OlsStline> InverseRetorigstline { get; set; }
        [InverseProperty("Stline")]
        public virtual ICollection<OlcWhztranline> OlcWhztranlines { get; set; }
    }
}
