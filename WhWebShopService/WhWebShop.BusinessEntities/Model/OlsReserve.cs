using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("ols_reserve")]
    public partial class OlsReserve : Entity
    {
        public OlsReserve()
        {
            OlcSordlineRes = new HashSet<OlcSordlineRes>();
            OlsSordline = new HashSet<OlsSordline>();
        }

        [Key]
        [Column("resid")]
        public int Resid { get; set; }
        [Column("cmpid")]
        public int Cmpid { get; set; }
        [Column("partnid")]
        public int Partnid { get; set; }
        [Column("addrid")]
        public int Addrid { get; set; }
        [Column("whid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Whid { get; set; } = null!;
        [Column("itemid")]
        public int Itemid { get; set; }
        [Column("lotid")]
        public int? Lotid { get; set; }
        [Column("restype")]
        public int Restype { get; set; }
        [Column("resqty", TypeName = "numeric(19, 6)")]
        public decimal Resqty { get; set; }
        [Column("resdate", TypeName = "datetime")]
        public DateTime Resdate { get; set; }
        [Column("freedate", TypeName = "datetime")]
        public DateTime? Freedate { get; set; }
        [Column("note")]
        [StringLength(200)]
        [Unicode(false)]
        public string? Note { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }

        [ForeignKey("Addrid")]
        [InverseProperty("OlsReserve")]
        public virtual OlsPartnaddr Addr { get; set; } = null!;
        [ForeignKey("Addusrid")]
        [InverseProperty("OlsReserve")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [ForeignKey("Cmpid")]
        [InverseProperty("OlsReserve")]
        public virtual OlsCompany Cmp { get; set; } = null!;
        [ForeignKey("Itemid")]
        [InverseProperty("OlsReserve")]
        public virtual OlsItem Item { get; set; } = null!;
        [ForeignKey("Partnid")]
        [InverseProperty("OlsReserve")]
        public virtual OlsPartner Partn { get; set; } = null!;
        [InverseProperty("Res")]
        public virtual ICollection<OlcSordlineRes> OlcSordlineRes { get; set; }
        [InverseProperty("Res")]
        public virtual ICollection<OlsSordline> OlsSordline { get; set; }
    }
}