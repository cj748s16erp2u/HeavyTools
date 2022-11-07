using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFC = Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    [Table("olc_whlocation")]
    [EFC.Index("Whid", "Whzoneid", "Loctype", Name = "idx_olc_whlocation_loctype")]
    [EFC.Index("Whid", Name = "idx_olc_whlocation_whid")]
    [EFC.Index("Whid", "Whzoneid", Name = "idx_olc_whlocation_whzoneid")]
    [EFC.Index("Whid", "Whzoneid", "Whloccode", Name = "uq_olc_whlocation_whloccode", IsUnique = true)]
    public partial class OlcWhlocation
    {
        public OlcWhlocation()
        {
            OlcWhzstockmaps = new HashSet<OlcWhzstockmap>();
            OlcWhztranlocs = new HashSet<OlcWhztranloc>();
        }

        [Key]
        [Column("whlocid")]
        public int Whlocid { get; set; }
        [Column("whid")]
        [StringLength(12)]
        [EFC.Unicode(false)]
        public string Whid { get; set; } = null!;
        [Column("whzoneid")]
        public int? Whzoneid { get; set; }
        [Column("whloccode")]
        [StringLength(20)]
        [EFC.Unicode(false)]
        public string Whloccode { get; set; } = null!;
        [Column("name")]
        [StringLength(200)]
        [EFC.Unicode(false)]
        public string? Name { get; set; }
        [Column("loctype")]
        public int Loctype { get; set; }
        [Column("movloctype")]
        public int? Movloctype { get; set; }
        [Column("volume", TypeName = "numeric(19, 6)")]
        public decimal? Volume { get; set; }
        [Column("overfillthreshold", TypeName = "numeric(19, 6)")]
        public decimal? Overfillthreshold { get; set; }
        [Column("ismulti")]
        public int? Ismulti { get; set; }
        [Column("crawlorder")]
        public int? Crawlorder { get; set; }
        [Column("capacity", TypeName = "numeric(19, 6)")]
        public decimal? Capacity { get; set; }
        [Column("capunitid")]
        [StringLength(12)]
        [EFC.Unicode(false)]
        public string? Capunitid { get; set; }
        [Column("note")]
        [StringLength(200)]
        [EFC.Unicode(false)]
        public string? Note { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlcWhlocations")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [ForeignKey("Capunitid")]
        [InverseProperty("OlcWhlocations")]
        public virtual OlsUnit? Capunit { get; set; }
        [ForeignKey("Whid")]
        [InverseProperty("OlcWhlocations")]
        public virtual OlsWarehouse Wh { get; set; } = null!;
        [ForeignKey("Whzoneid")]
        [InverseProperty("OlcWhlocations")]
        public virtual OlcWhzone? Whzone { get; set; }
        [InverseProperty("Whloc")]
        public virtual ICollection<OlcWhzstockmap> OlcWhzstockmaps { get; set; }
        [InverseProperty("Whloc")]
        public virtual ICollection<OlcWhztranloc> OlcWhztranlocs { get; set; }
    }
}
