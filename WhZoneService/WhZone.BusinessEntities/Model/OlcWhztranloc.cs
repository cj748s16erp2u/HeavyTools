using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    [Table("olc_whztranloc")]
    public partial class OlcWhztranloc
    {
        [Key]
        [Column("whztlocid")]
        public int Whztlocid { get; set; }
        [Column("whztid")]
        public int Whztid { get; set; }
        [Column("whztlineid")]
        public int Whztlineid { get; set; }
        [Column("whid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Whid { get; set; } = null!;
        [Column("whzoneid")]
        public int Whzoneid { get; set; }
        [Column("whlocid")]
        public int Whlocid { get; set; }
        [Column("whztltype")]
        public int Whztltype { get; set; }
        [Column("ordqty", TypeName = "numeric(19, 6)")]
        public decimal Ordqty { get; set; }
        [Column("dispqty", TypeName = "numeric(19, 6)")]
        public decimal Dispqty { get; set; }
        [Column("movqty", TypeName = "numeric(19, 6)")]
        public decimal Movqty { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlcWhztranlocs")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [ForeignKey("Whid")]
        [InverseProperty("OlcWhztranlocs")]
        public virtual OlsWarehouse Wh { get; set; } = null!;
        [ForeignKey("Whlocid")]
        [InverseProperty("OlcWhztranlocs")]
        public virtual OlcWhlocation Whloc { get; set; } = null!;
        [ForeignKey("Whzoneid")]
        [InverseProperty("OlcWhztranlocs")]
        public virtual OlcWhzone Whzone { get; set; } = null!;
        [ForeignKey("Whztid")]
        [InverseProperty("OlcWhztranlocs")]
        public virtual OlcWhztranhead Whzt { get; set; } = null!;
        [ForeignKey("Whztlineid")]
        [InverseProperty("OlcWhztranlocs")]
        public virtual OlcWhztranline Whztline { get; set; } = null!;
    }
}
