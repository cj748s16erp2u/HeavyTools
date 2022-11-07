using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFC = Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    [Table("ols_sthead")]
    [EFC.Index("Cmpid", Name = "idx_ols_sthead_cmpid")]
    [EFC.Index("Dispstid", Name = "idx_ols_sthead_dispstid")]
    [EFC.Index("Docnum", Name = "idx_ols_sthead_docnum")]
    [EFC.Index("Frompartnid", Name = "idx_ols_sthead_frompartnid")]
    [EFC.Index("Fromwhid", Name = "idx_ols_sthead_fromwhid")]
    [EFC.Index("Retorigstid", Name = "idx_ols_sthead_retorigstid")]
    [EFC.Index("Sinvid", Name = "idx_ols_sthead_sinvid")]
    [EFC.Index("Stdate", Name = "idx_ols_sthead_stdate")]
    [EFC.Index("Stdocid", Name = "idx_ols_sthead_stdocid")]
    [EFC.Index("Topartnid", Name = "idx_ols_sthead_topartnid")]
    [EFC.Index("Towhid", Name = "idx_ols_sthead_towhid")]
    public partial class OlsSthead
    {
        public OlsSthead()
        {
            InverseCorrst = new HashSet<OlsSthead>();
            InverseDispst = new HashSet<OlsSthead>();
            InverseOrigst = new HashSet<OlsSthead>();
            InverseProdrecst = new HashSet<OlsSthead>();
            InverseRetorigst = new HashSet<OlsSthead>();
            OlcWhztranheads = new HashSet<OlcWhztranhead>();
            OlsStlines = new HashSet<OlsStline>();
        }

        [Key]
        [Column("stid")]
        public int Stid { get; set; }
        [Column("cmpid")]
        public int Cmpid { get; set; }
        [Column("stdocid")]
        [StringLength(12)]
        [EFC.Unicode(false)]
        public string Stdocid { get; set; } = null!;
        [Column("docnum")]
        [StringLength(12)]
        [EFC.Unicode(false)]
        public string Docnum { get; set; } = null!;
        [Column("sttype")]
        public int Sttype { get; set; }
        [Column("stdate", TypeName = "datetime")]
        public DateTime Stdate { get; set; }
        [Column("taxdatetype")]
        public int? Taxdatetype { get; set; }
        [Column("reldatemax", TypeName = "datetime")]
        public DateTime Reldatemax { get; set; }
        [Column("frompartnid")]
        public int Frompartnid { get; set; }
        [Column("fromaddrid")]
        public int Fromaddrid { get; set; }
        [Column("fromwhid")]
        [StringLength(12)]
        [EFC.Unicode(false)]
        public string? Fromwhid { get; set; }
        [Column("topartnid")]
        public int Topartnid { get; set; }
        [Column("toaddrid")]
        public int Toaddrid { get; set; }
        [Column("towhid")]
        [StringLength(12)]
        [EFC.Unicode(false)]
        public string? Towhid { get; set; }
        [Column("curid")]
        [StringLength(12)]
        [EFC.Unicode(false)]
        public string Curid { get; set; } = null!;
        [Column("paymid")]
        [StringLength(12)]
        [EFC.Unicode(false)]
        public string? Paymid { get; set; }
        [Column("paycid")]
        public int? Paycid { get; set; }
        [Column("netweight", TypeName = "numeric(19, 6)")]
        public decimal? Netweight { get; set; }
        [Column("grossweight", TypeName = "numeric(19, 6)")]
        public decimal? Grossweight { get; set; }
        [Column("shipparitycode")]
        [StringLength(12)]
        [EFC.Unicode(false)]
        public string? Shipparitycode { get; set; }
        [Column("shipparityplace")]
        [StringLength(30)]
        [EFC.Unicode(false)]
        public string? Shipparityplace { get; set; }
        [Column("cootype")]
        public int? Cootype { get; set; }
        [Column("colli")]
        [StringLength(40)]
        [EFC.Unicode(false)]
        public string? Colli { get; set; }
        [Column("ref1")]
        [StringLength(30)]
        [EFC.Unicode(false)]
        public string? Ref1 { get; set; }
        [Column("ref2")]
        [StringLength(30)]
        [EFC.Unicode(false)]
        public string? Ref2 { get; set; }
        [Column("closeusrid")]
        [StringLength(12)]
        [EFC.Unicode(false)]
        public string? Closeusrid { get; set; }
        [Column("closedate", TypeName = "datetime")]
        public DateTime? Closedate { get; set; }
        [Column("ststat")]
        public int Ststat { get; set; }
        [Column("manufusestat")]
        public int? Manufusestat { get; set; }
        [Column("note")]
        [StringLength(200)]
        [EFC.Unicode(false)]
        public string? Note { get; set; }
        [Column("sinvid")]
        public int? Sinvid { get; set; }
        [Column("corrtype")]
        public int Corrtype { get; set; }
        [Column("origstid")]
        public int? Origstid { get; set; }
        [Column("corrstid")]
        public int? Corrstid { get; set; }
        [Column("transpid")]
        public int? Transpid { get; set; }
        [Column("intransitwhid")]
        [StringLength(12)]
        [EFC.Unicode(false)]
        public string? Intransitwhid { get; set; }
        [Column("intransittowhid")]
        [StringLength(12)]
        [EFC.Unicode(false)]
        public string? Intransittowhid { get; set; }
        [Column("pinvid")]
        public int? Pinvid { get; set; }
        [Column("prodrecstid")]
        public int? Prodrecstid { get; set; }
        [Column("projid")]
        public int? Projid { get; set; }
        [Column("retorigstid")]
        public int? Retorigstid { get; set; }
        [Column("bustypeid")]
        [StringLength(12)]
        [EFC.Unicode(false)]
        public string Bustypeid { get; set; } = null!;
        [Column("dispstid")]
        public int? Dispstid { get; set; }
        [Column("gen")]
        public int Gen { get; set; }
        [Column("langid")]
        [StringLength(12)]
        [EFC.Unicode(false)]
        public string Langid { get; set; } = null!;
        [Column("lastlinenum")]
        public int Lastlinenum { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlsStheadAddusrs")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [ForeignKey("Closeusrid")]
        [InverseProperty("OlsStheadCloseusrs")]
        public virtual CfwUser? Closeusr { get; set; }
        [ForeignKey("Cmpid")]
        [InverseProperty("OlsStheads")]
        public virtual OlsCompany Cmp { get; set; } = null!;
        [ForeignKey("Corrstid")]
        [InverseProperty("InverseCorrst")]
        public virtual OlsSthead? Corrst { get; set; }
        [ForeignKey("Dispstid")]
        [InverseProperty("InverseDispst")]
        public virtual OlsSthead? Dispst { get; set; }
        [ForeignKey("Fromwhid")]
        [InverseProperty("OlsStheadFromwhs")]
        public virtual OlsWarehouse? Fromwh { get; set; }
        [ForeignKey("Intransittowhid")]
        [InverseProperty("OlsStheadIntransittowhs")]
        public virtual OlsWarehouse? Intransittowh { get; set; }
        [ForeignKey("Intransitwhid")]
        [InverseProperty("OlsStheadIntransitwhs")]
        public virtual OlsWarehouse? Intransitwh { get; set; }
        [ForeignKey("Origstid")]
        [InverseProperty("InverseOrigst")]
        public virtual OlsSthead? Origst { get; set; }
        [ForeignKey("Prodrecstid")]
        [InverseProperty("InverseProdrecst")]
        public virtual OlsSthead? Prodrecst { get; set; }
        [ForeignKey("Retorigstid")]
        [InverseProperty("InverseRetorigst")]
        public virtual OlsSthead? Retorigst { get; set; }
        [ForeignKey("Towhid")]
        [InverseProperty("OlsStheadTowhs")]
        public virtual OlsWarehouse? Towh { get; set; }
        [InverseProperty("Corrst")]
        public virtual ICollection<OlsSthead> InverseCorrst { get; set; }
        [InverseProperty("Dispst")]
        public virtual ICollection<OlsSthead> InverseDispst { get; set; }
        [InverseProperty("Origst")]
        public virtual ICollection<OlsSthead> InverseOrigst { get; set; }
        [InverseProperty("Prodrecst")]
        public virtual ICollection<OlsSthead> InverseProdrecst { get; set; }
        [InverseProperty("Retorigst")]
        public virtual ICollection<OlsSthead> InverseRetorigst { get; set; }
        [InverseProperty("St")]
        public virtual ICollection<OlcWhztranhead> OlcWhztranheads { get; set; }
        [InverseProperty("St")]
        public virtual ICollection<OlsStline> OlsStlines { get; set; }
    }
}
