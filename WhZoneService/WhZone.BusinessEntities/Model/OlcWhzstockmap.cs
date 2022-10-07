using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFC = Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    [Table("olc_whzstockmap")]
    [EFC.Index("Itemid", "Whid", "Whzoneid", "Whlocid", Name = "uq_olc_whzstockmap", IsUnique = true)]
    public partial class OlcWhzstockmap
    {
        [Key]
        [Column("whzstockmapid")]
        public int Whzstockmapid { get; set; }
        [Column("itemid")]
        public int Itemid { get; set; }
        [Column("whid")]
        [StringLength(12)]
        [EFC.Unicode(false)]
        public string Whid { get; set; } = null!;
        [Column("whzoneid")]
        public int? Whzoneid { get; set; }
        [Column("whlocid")]
        public int? Whlocid { get; set; }
        [Column("recqty", TypeName = "numeric(19, 6)")]
        public decimal Recqty { get; set; }
        [Column("reqqty", TypeName = "numeric(19, 6)")]
        public decimal Reqqty { get; set; }
        [Column("actqty", TypeName = "numeric(19, 6)")]
        public decimal Actqty { get; set; }
        [Column("resqty", TypeName = "numeric(19, 6)")]
        public decimal Resqty { get; set; }
        [Column("provqty", TypeName = "numeric(21, 6)")]
        public decimal? Provqty { get; set; }

        [ForeignKey("Itemid")]
        [InverseProperty("OlcWhzstockmaps")]
        public virtual OlsItem Item { get; set; } = null!;
        [ForeignKey("Whid")]
        [InverseProperty("OlcWhzstockmaps")]
        public virtual OlsWarehouse Wh { get; set; } = null!;
        [ForeignKey("Whlocid")]
        [InverseProperty("OlcWhzstockmaps")]
        public virtual OlcWhlocation Whloc { get; set; } = null!;
        [ForeignKey("Whzoneid")]
        [InverseProperty("OlcWhzstockmaps")]
        public virtual OlcWhzone? Whzone { get; set; }
    }
}
