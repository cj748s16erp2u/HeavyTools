using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFC = Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    [Table("olc_whztranline")]
    [EFC.Index("Stlineid", Name = "idx_olc_whztranline_stlineid")]
    public partial class OlcWhztranline
    {
        public OlcWhztranline()
        {
            OlcWhztranlocs = new HashSet<OlcWhztranloc>();
        }

        [Key]
        [Column("whztlineid")]
        public int Whztlineid { get; set; }
        [Column("whztid")]
        public int Whztid { get; set; }
        [Column("linenum")]
        public int Linenum { get; set; }
        [Column("itemid")]
        public int Itemid { get; set; }
        [Column("ordqty", TypeName = "numeric(19, 6)")]
        public decimal Ordqty { get; set; }
        [Column("dispqty", TypeName = "numeric(19, 6)")]
        public decimal Dispqty { get; set; }
        [Column("movqty", TypeName = "numeric(19, 6)")]
        public decimal Movqty { get; set; }
        [Column("inqty", TypeName = "numeric(19, 6)")]
        public decimal Inqty { get; set; }
        [Column("outqty", TypeName = "numeric(19, 6)")]
        public decimal Outqty { get; set; }
        [Column("unitid2")]
        [StringLength(12)]
        [EFC.Unicode(false)]
        public string Unitid2 { get; set; } = null!;
        [Column("change", TypeName = "numeric(19, 6)")]
        public decimal Change { get; set; }
        [Column("ordqty2", TypeName = "numeric(19, 6)")]
        public decimal Ordqty2 { get; set; }
        [Column("dispqty2", TypeName = "numeric(19, 6)")]
        public decimal Dispqty2 { get; set; }
        [Column("movqty2", TypeName = "numeric(19, 6)")]
        public decimal Movqty2 { get; set; }
        [Column("note")]
        [StringLength(1000)]
        [EFC.Unicode(false)]
        public string? Note { get; set; }
        [Column("stlineid")]
        public int? Stlineid { get; set; }
        [Column("sordlineid")]
        public int? Sordlineid { get; set; }
        [Column("pordlineid")]
        public int? Pordlineid { get; set; }
        [Column("taskitemid")]
        public int? Taskitemid { get; set; }
        [Column("gen")]
        public int? Gen { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlcWhztranlines")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [ForeignKey("Itemid")]
        [InverseProperty("OlcWhztranlines")]
        public virtual OlsItem Item { get; set; } = null!;
        [ForeignKey("Stlineid")]
        [InverseProperty("OlcWhztranlines")]
        public virtual OlsStline? Stline { get; set; }
        [ForeignKey("Unitid2")]
        [InverseProperty("OlcWhztranlines")]
        public virtual OlsUnit Unitid2Navigation { get; set; } = null!;
        [ForeignKey("Whztid")]
        [InverseProperty("OlcWhztranlines")]
        public virtual OlcWhztranhead Whzt { get; set; } = null!;
        [InverseProperty("Whztline")]
        public virtual ICollection<OlcWhztranloc> OlcWhztranlocs { get; set; }
    }
}
