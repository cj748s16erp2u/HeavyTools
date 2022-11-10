using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("ols_sinvhead")]
    [Index("Cmpid", Name = "idx_ols_sinvhead_cmpid")]
    [Index("Corrsinvid", Name = "idx_ols_sinvhead_corrsinvid")]
    [Index("Docdate", Name = "idx_ols_sinvhead_docdate")]
    [Index("Docnum", Name = "idx_ols_sinvhead_docnum")]
    [Index("Origsinvid", Name = "idx_ols_sinvhead_origsinvid")]
    [Index("Partnid", Name = "idx_ols_sinvhead_partnid")]
    [Index("Sinvdate", Name = "idx_ols_sinvhead_sinvdate")]
    [Index("Sinvdocid", Name = "idx_ols_sinvhead_sinvdocid")]
    [Index("Whid", Name = "idx_ols_sinvhead_whid")]
    public partial class OlsSinvhead : Entity
    {
        public OlsSinvhead()
        {
            InverseCorrsinv = new HashSet<OlsSinvhead>();
            InverseOrigsinv = new HashSet<OlsSinvhead>();
            OlcGiftcardlog = new HashSet<OlcGiftcardlog>();
        }

        [Key]
        [Column("sinvid")]
        public int Sinvid { get; set; }
        [Column("cmpid")]
        public int Cmpid { get; set; }
        [Column("sinvdocid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Sinvdocid { get; set; } = null!;
        [Column("docnum")]
        [StringLength(30)]
        [Unicode(false)]
        public string Docnum { get; set; } = null!;
        [Column("sinvtype")]
        public int Sinvtype { get; set; }
        [Column("sinvdate", TypeName = "datetime")]
        public DateTime Sinvdate { get; set; }
        [Column("whid")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Whid { get; set; }
        [Column("partnid")]
        public int Partnid { get; set; }
        [Column("partnname")]
        [StringLength(100)]
        [Unicode(false)]
        public string Partnname { get; set; } = null!;
        [Column("addrid")]
        public int Addrid { get; set; }
        [Column("delpartnid")]
        public int Delpartnid { get; set; }
        [Column("delpartnname")]
        [StringLength(100)]
        [Unicode(false)]
        public string Delpartnname { get; set; } = null!;
        [Column("deladdrid")]
        public int Deladdrid { get; set; }
        [Column("curid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Curid { get; set; } = null!;
        [Column("docdate", TypeName = "datetime")]
        public DateTime Docdate { get; set; }
        [Column("paymid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Paymid { get; set; } = null!;
        [Column("paycid")]
        public int? Paycid { get; set; }
        [Column("duedate", TypeName = "datetime")]
        public DateTime Duedate { get; set; }
        [Column("taxdatetype")]
        public int Taxdatetype { get; set; }
        [Column("taxdate", TypeName = "datetime")]
        public DateTime Taxdate { get; set; }
        [Column("docrate", TypeName = "numeric(19, 6)")]
        public decimal Docrate { get; set; }
        [Column("dualrate", TypeName = "numeric(19, 6)")]
        public decimal? Dualrate { get; set; }
        [Column("selval", TypeName = "numeric(19, 6)")]
        public decimal Selval { get; set; }
        [Column("discval", TypeName = "numeric(19, 6)")]
        public decimal Discval { get; set; }
        [Column("netval", TypeName = "numeric(19, 6)")]
        public decimal Netval { get; set; }
        [Column("taxval", TypeName = "numeric(19, 6)")]
        public decimal Taxval { get; set; }
        [Column("totval", TypeName = "numeric(19, 6)")]
        public decimal Totval { get; set; }
        [Column("payval", TypeName = "numeric(19, 6)")]
        public decimal Payval { get; set; }
        [Column("selvalhome", TypeName = "numeric(19, 6)")]
        public decimal Selvalhome { get; set; }
        [Column("discvalhome", TypeName = "numeric(19, 6)")]
        public decimal Discvalhome { get; set; }
        [Column("netvalhome", TypeName = "numeric(19, 6)")]
        public decimal Netvalhome { get; set; }
        [Column("taxvalhome", TypeName = "numeric(19, 6)")]
        public decimal Taxvalhome { get; set; }
        [Column("totvalhome", TypeName = "numeric(19, 6)")]
        public decimal Totvalhome { get; set; }
        [Column("payvalhome", TypeName = "numeric(19, 6)")]
        public decimal Payvalhome { get; set; }
        [Column("selvaldual", TypeName = "numeric(19, 6)")]
        public decimal? Selvaldual { get; set; }
        [Column("discvaldual", TypeName = "numeric(19, 6)")]
        public decimal? Discvaldual { get; set; }
        [Column("netvaldual", TypeName = "numeric(19, 6)")]
        public decimal? Netvaldual { get; set; }
        [Column("taxvaldual", TypeName = "numeric(19, 6)")]
        public decimal? Taxvaldual { get; set; }
        [Column("totvaldual", TypeName = "numeric(19, 6)")]
        public decimal? Totvaldual { get; set; }
        [Column("payvaldual", TypeName = "numeric(19, 6)")]
        public decimal? Payvaldual { get; set; }
        [Column("compval", TypeName = "numeric(19, 6)")]
        public decimal? Compval { get; set; }
        [Column("closeusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Closeusrid { get; set; }
        [Column("closedate", TypeName = "datetime")]
        public DateTime? Closedate { get; set; }
        [Column("sinvstat")]
        public int Sinvstat { get; set; }
        [Column("nofprinted", TypeName = "numeric(9, 3)")]
        public decimal Nofprinted { get; set; }
        [Column("note")]
        [StringLength(200)]
        [Unicode(false)]
        public string? Note { get; set; }
        [Column("footernote")]
        [StringLength(2000)]
        [Unicode(false)]
        public string? Footernote { get; set; }
        [Column("internalnote")]
        [StringLength(1000)]
        [Unicode(false)]
        public string? Internalnote { get; set; }
        [Column("corrtype")]
        public int Corrtype { get; set; }
        [Column("origsinvid")]
        public int? Origsinvid { get; set; }
        [Column("corrsinvid")]
        public int? Corrsinvid { get; set; }
        [Column("netweight", TypeName = "numeric(19, 6)")]
        public decimal? Netweight { get; set; }
        [Column("grossweight", TypeName = "numeric(19, 6)")]
        public decimal? Grossweight { get; set; }
        [Column("shipparitycode")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Shipparitycode { get; set; }
        [Column("shipparityplace")]
        [StringLength(30)]
        [Unicode(false)]
        public string? Shipparityplace { get; set; }
        [Column("cootype")]
        public int? Cootype { get; set; }
        [Column("colli")]
        [StringLength(40)]
        [Unicode(false)]
        public string? Colli { get; set; }
        [Column("projid")]
        public int? Projid { get; set; }
        [Column("bustypeid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Bustypeid { get; set; } = null!;
        [Column("supbankid")]
        public int? Supbankid { get; set; }
        [Column("ictype")]
        public int? Ictype { get; set; }
        [Column("docstartdate", TypeName = "datetime")]
        public DateTime? Docstartdate { get; set; }
        [Column("docenddate", TypeName = "datetime")]
        public DateTime? Docenddate { get; set; }
        [Column("gen")]
        public int Gen { get; set; }
        [Column("langid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Langid { get; set; } = null!;
        [Column("lastlinenum")]
        public int Lastlinenum { get; set; }
        [Column("xmldata", TypeName = "xml")]
        public string? Xmldata { get; set; }
        [Column("navstat")]
        public int? Navstat { get; set; }
        [Column("navtranid")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Navtranid { get; set; }
        [Column("navlogid")]
        public int? Navlogid { get; set; }
        [Column("ptvattypid")]
        [StringLength(30)]
        [Unicode(false)]
        public string? Ptvattypid { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }

        [ForeignKey("Addrid")]
        [InverseProperty("OlsSinvheadAddr")]
        public virtual OlsPartnaddr Addr { get; set; } = null!;
        [ForeignKey("Addusrid")]
        [InverseProperty("OlsSinvheadAddusr")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [ForeignKey("Closeusrid")]
        [InverseProperty("OlsSinvheadCloseusr")]
        public virtual CfwUser? Closeusr { get; set; }
        [ForeignKey("Cmpid")]
        [InverseProperty("OlsSinvhead")]
        public virtual OlsCompany Cmp { get; set; } = null!;
        [ForeignKey("Corrsinvid")]
        [InverseProperty("InverseCorrsinv")]
        public virtual OlsSinvhead? Corrsinv { get; set; }
        [ForeignKey("Curid")]
        [InverseProperty("OlsSinvhead")]
        public virtual OlsCurrency Cur { get; set; } = null!;
        [ForeignKey("Deladdrid")]
        [InverseProperty("OlsSinvheadDeladdr")]
        public virtual OlsPartnaddr Deladdr { get; set; } = null!;
        [ForeignKey("Delpartnid")]
        [InverseProperty("OlsSinvheadDelpartn")]
        public virtual OlsPartner Delpartn { get; set; } = null!;
        [ForeignKey("Origsinvid")]
        [InverseProperty("InverseOrigsinv")]
        public virtual OlsSinvhead? Origsinv { get; set; }
        [ForeignKey("Partnid")]
        [InverseProperty("OlsSinvheadPartn")]
        public virtual OlsPartner Partn { get; set; } = null!;
        [ForeignKey("Ptvattypid")]
        [InverseProperty("OlsSinvhead")]
        public virtual OlsPartnvattyp? Ptvattyp { get; set; }
        [InverseProperty("Corrsinv")]
        public virtual ICollection<OlsSinvhead> InverseCorrsinv { get; set; }
        [InverseProperty("Origsinv")]
        public virtual ICollection<OlsSinvhead> InverseOrigsinv { get; set; }
        [InverseProperty("Sinv")]
        public virtual ICollection<OlcGiftcardlog> OlcGiftcardlog { get; set; }
    }
}