using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    [Table("ols_warehouse")]
    public partial class OlsWarehouse
    {
        public OlsWarehouse()
        {
            OlcWhlocations = new HashSet<OlcWhlocation>();
            OlcWhzones = new HashSet<OlcWhzone>();
            OlcWhzstockmaps = new HashSet<OlcWhzstockmap>();
            OlcWhztranlocs = new HashSet<OlcWhztranloc>();
            OlsStheadFromwhs = new HashSet<OlsSthead>();
            OlsStheadIntransittowhs = new HashSet<OlsSthead>();
            OlsStheadIntransitwhs = new HashSet<OlsSthead>();
            OlsStheadTowhs = new HashSet<OlsSthead>();
        }

        [Key]
        [Column("whid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Whid { get; set; } = null!;
        [Column("cmpid")]
        public int Cmpid { get; set; }
        [Column("cmpcodes")]
        [StringLength(2000)]
        [Unicode(false)]
        public string Cmpcodes { get; set; } = null!;
        [Column("partnid")]
        public int Partnid { get; set; }
        [Column("addrid")]
        public int Addrid { get; set; }
        [Column("name")]
        [StringLength(40)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column("whtype")]
        public int? Whtype { get; set; }
        [Column("loctype")]
        public int Loctype { get; set; }
        [Column("storemtype")]
        public int Storemtype { get; set; }
        [Column("pickmtype")]
        public int Pickmtype { get; set; }
        [Column("backordertype")]
        public int Backordertype { get; set; }
        [Column("projid")]
        public int? Projid { get; set; }
        [Column("note")]
        [StringLength(200)]
        [Unicode(false)]
        public string? Note { get; set; }
        [Column("codacode")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Codacode { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlsWarehouses")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [InverseProperty("Wh")]
        public virtual ICollection<OlcWhlocation> OlcWhlocations { get; set; }
        [InverseProperty("Wh")]
        public virtual ICollection<OlcWhzone> OlcWhzones { get; set; }
        [InverseProperty("Wh")]
        public virtual ICollection<OlcWhzstockmap> OlcWhzstockmaps { get; set; }
        [InverseProperty("Wh")]
        public virtual ICollection<OlcWhztranloc> OlcWhztranlocs { get; set; }
        [InverseProperty("Fromwh")]
        public virtual ICollection<OlsSthead> OlsStheadFromwhs { get; set; }
        [InverseProperty("Intransittowh")]
        public virtual ICollection<OlsSthead> OlsStheadIntransittowhs { get; set; }
        [InverseProperty("Intransitwh")]
        public virtual ICollection<OlsSthead> OlsStheadIntransitwhs { get; set; }
        [InverseProperty("Towh")]
        public virtual ICollection<OlsSthead> OlsStheadTowhs { get; set; }
    }
}
