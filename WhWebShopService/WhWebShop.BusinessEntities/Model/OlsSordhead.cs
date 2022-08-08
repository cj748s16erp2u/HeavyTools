using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("ols_sordhead")]
    [Index("Cmpid", Name = "idx_ols_sordhead_cmpid")]
    [Index("Docnum", Name = "idx_ols_sordhead_docnum")]
    [Index("Partnid", Name = "idx_ols_sordhead_partnid")]
    [Index("Sorddate", Name = "idx_ols_sordhead_sorddate")]
    [Index("Sorddocid", Name = "idx_ols_sordhead_sorddocid")]
    [Index("Whid", Name = "idx_ols_sordhead_whid")]
    public partial class OlsSordhead : Entity
    {
        public OlsSordhead()
        {
            OlsSordline = new HashSet<OlsSordline>();
        }

        [Key]
        [Column("sordid")]
        public int Sordid { get; set; }
        [Column("sorddocid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Sorddocid { get; set; } = null!;
        [Column("cmpid")]
        public int Cmpid { get; set; }
        [Column("docnum")]
        [StringLength(12)]
        [Unicode(false)]
        public string Docnum { get; set; } = null!;
        [Column("ref1")]
        [StringLength(30)]
        [Unicode(false)]
        public string? Ref1 { get; set; }
        [Column("sordtype")]
        public int Sordtype { get; set; }
        [Column("sorddate", TypeName = "datetime")]
        public DateTime Sorddate { get; set; }
        [Column("partnid")]
        public int Partnid { get; set; }
        [Column("addrid")]
        public int Addrid { get; set; }
        [Column("whid")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Whid { get; set; }
        [Column("curid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Curid { get; set; } = null!;
        [Column("paymid")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Paymid { get; set; }
        [Column("paycid")]
        public int? Paycid { get; set; }
        [Column("sordstat")]
        public int Sordstat { get; set; }
        [Column("note")]
        [StringLength(200)]
        [Unicode(false)]
        public string? Note { get; set; }
        [Column("projid")]
        public int? Projid { get; set; }
        [Column("paritycode")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Paritycode { get; set; }
        [Column("parityplace")]
        [StringLength(100)]
        [Unicode(false)]
        public string? Parityplace { get; set; }
        [Column("gen")]
        public int Gen { get; set; }
        [Column("langid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Langid { get; set; } = null!;
        [Column("lastlinenum")]
        public int Lastlinenum { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }

        [ForeignKey("Addrid")]
        [InverseProperty("OlsSordhead")]
        public virtual OlsPartnaddr Addr { get; set; } = null!;
        [ForeignKey("Addusrid")]
        [InverseProperty("OlsSordhead")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [ForeignKey("Cmpid")]
        [InverseProperty("OlsSordhead")]
        public virtual OlsCompany Cmp { get; set; } = null!;
        [ForeignKey("Partnid")]
        [InverseProperty("OlsSordhead")]
        public virtual OlsPartner Partn { get; set; } = null!;
        [InverseProperty("Sord")]
        public virtual OlcSordhead OlcSordhead { get; set; } = null!;
        [InverseProperty("Sord")]
        public virtual ICollection<OlsSordline> OlsSordline { get; set; }
    }
}