using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFC = Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    [Table("olc_whzone")]
    [EFC.Index("Whid", Name = "idx_olc_whzone_whid")]
    [EFC.Index("Whid", "Whzonecode", Name = "idx_olc_whzone_whzonecode")]
    public partial class OlcWhzone
    {
        public OlcWhzone()
        {
            OlcWhlocations = new HashSet<OlcWhlocation>();
            OlcWhzstockmaps = new HashSet<OlcWhzstockmap>();
            OlcWhztranheadFromwhzs = new HashSet<OlcWhztranhead>();
            OlcWhztranheadTowhzs = new HashSet<OlcWhztranhead>();
            OlcWhztranlocs = new HashSet<OlcWhztranloc>();
        }

        [Key]
        [Column("whzoneid")]
        public int Whzoneid { get; set; }
        [Column("whid")]
        [StringLength(12)]
        [EFC.Unicode(false)]
        public string Whid { get; set; } = null!;
        [Column("whzonecode")]
        [StringLength(12)]
        [EFC.Unicode(false)]
        public string Whzonecode { get; set; } = null!;
        [Column("name")]
        [StringLength(40)]
        [EFC.Unicode(false)]
        public string Name { get; set; } = null!;
        [Column("pickingtype")]
        public int Pickingtype { get; set; }
        [Column("pickingcartaccessible")]
        public int Pickingcartaccessible { get; set; }
        [Column("isbackground")]
        public int Isbackground { get; set; }
        [Column("ispuffer")]
        public int Ispuffer { get; set; }
        [Column("locdefvolume", TypeName = "numeric(19, 6)")]
        public decimal? Locdefvolume { get; set; }
        [Column("locdefoverfillthreshold", TypeName = "numeric(19, 6)")]
        public decimal? Locdefoverfillthreshold { get; set; }
        [Column("locdefismulti")]
        public int? Locdefismulti { get; set; }
        [Column("note")]
        [StringLength(200)]
        [EFC.Unicode(false)]
        public string? Note { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlcWhzones")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [ForeignKey("Whid")]
        [InverseProperty("OlcWhzones")]
        public virtual OlsWarehouse Wh { get; set; } = null!;
        [InverseProperty("Whzone")]
        public virtual ICollection<OlcWhlocation> OlcWhlocations { get; set; }
        [InverseProperty("Whzone")]
        public virtual ICollection<OlcWhzstockmap> OlcWhzstockmaps { get; set; }
        [InverseProperty("Fromwhz")]
        public virtual ICollection<OlcWhztranhead> OlcWhztranheadFromwhzs { get; set; }
        [InverseProperty("Towhz")]
        public virtual ICollection<OlcWhztranhead> OlcWhztranheadTowhzs { get; set; }
        [InverseProperty("Whzone")]
        public virtual ICollection<OlcWhztranloc> OlcWhztranlocs { get; set; }
    }
}
