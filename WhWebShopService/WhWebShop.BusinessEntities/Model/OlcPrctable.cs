using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("olc_prctable")]
    public partial class OlcPrctable : Entity
    {
        [Key]
        [Column("prcid")]
        public int Prcid { get; set; }
        [Column("ptid")]
        public int Ptid { get; set; }
        [Column("prctype")]
        public int Prctype { get; set; }
        [Column("wid")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Wid { get; set; }
        [Column("partnid")]
        public int? Partnid { get; set; }
        [Column("addrid")]
        public int? Addrid { get; set; }
        [Column("curid")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Curid { get; set; }
        [Column("startdate", TypeName = "datetime")]
        public DateTime Startdate { get; set; }
        [Column("enddate", TypeName = "datetime")]
        public DateTime Enddate { get; set; }
        [Column("prc", TypeName = "numeric(16, 9)")]
        public decimal? Prc { get; set; }
        [Column("imid")]
        public int? Imid { get; set; }
        [Column("isid")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Isid { get; set; }
        [Column("icid")]
        [StringLength(3)]
        [Unicode(false)]
        public string? Icid { get; set; }
        [Column("itemid")]
        public int? Itemid { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Addusrid { get; set; }
        [Column("adddate", TypeName = "datetime")]
        public DateTime? Adddate { get; set; }
        [Column("delstat")]
        public int Delstat { get; set; }

        [ForeignKey("Addrid")]
        [InverseProperty("OlcPrctable")]
        public virtual OlsPartnaddr? Addr { get; set; }
        [ForeignKey("Addusrid")]
        [InverseProperty("OlcPrctable")]
        public virtual CfwUser? Addusr { get; set; }
        [ForeignKey("Itemid")]
        [InverseProperty("OlcPrctable")]
        public virtual OlsItem? Item { get; set; }
        [ForeignKey("Partnid")]
        [InverseProperty("OlcPrctable")]
        public virtual OlsPartner? Partn { get; set; }
    }
}