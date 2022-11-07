using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    [Table("ols_unit")]
    public partial class OlsUnit
    {
        public OlsUnit()
        {
            OlcWhlocations = new HashSet<OlcWhlocation>();
            OlcWhztranlines = new HashSet<OlcWhztranline>();
            OlsItems = new HashSet<OlsItem>();
            OlsStlines = new HashSet<OlsStline>();
        }

        [Key]
        [Column("unitid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Unitid { get; set; } = null!;
        [Column("name")]
        [StringLength(40)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column("code2")]
        [StringLength(12)]
        public string? Code2 { get; set; }
        [Column("unittype")]
        public int? Unittype { get; set; }
        [Column("note")]
        [StringLength(200)]
        [Unicode(false)]
        public string? Note { get; set; }
        [Column("navcode")]
        [StringLength(30)]
        [Unicode(false)]
        public string? Navcode { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlsUnits")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [InverseProperty("Capunit")]
        public virtual ICollection<OlcWhlocation> OlcWhlocations { get; set; }
        [InverseProperty("Unitid2Navigation")]
        public virtual ICollection<OlcWhztranline> OlcWhztranlines { get; set; }
        [InverseProperty("Unit")]
        public virtual ICollection<OlsItem> OlsItems { get; set; }
        [InverseProperty("Unitid2Navigation")]
        public virtual ICollection<OlsStline> OlsStlines { get; set; }
    }
}
