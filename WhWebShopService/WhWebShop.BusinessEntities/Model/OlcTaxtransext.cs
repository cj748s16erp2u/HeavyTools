using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("olc_taxtransext")]
    public partial class OlcTaxtransext : Entity
    {
        [Key]
        [Column("tteid")]
        public int Tteid { get; set; }
        [Column("ttid")]
        public int Ttid { get; set; }
        [Column("countryid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Countryid { get; set; } = null!;
        [Column("taxid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Taxid { get; set; } = null!;
        [Column("ttefrom", TypeName = "datetime")]
        public DateTime Ttefrom { get; set; }
        [Column("tteto", TypeName = "datetime")]
        public DateTime Tteto { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Addusrid { get; set; }
        [Column("adddate", TypeName = "datetime")]
        public DateTime? Adddate { get; set; }
        [Column("delstat")]
        public int Delstat { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlcTaxtransext")]
        public virtual CfwUser? Addusr { get; set; }
        [ForeignKey("Countryid")]
        [InverseProperty("OlcTaxtransext")]
        public virtual OlsCountry Country { get; set; } = null!;
        [ForeignKey("Taxid")]
        [InverseProperty("OlcTaxtransext")]
        public virtual OlsTax Tax { get; set; } = null!;
        [ForeignKey("Ttid")]
        [InverseProperty("OlcTaxtransext")]
        public virtual OlsTaxtrans Tt { get; set; } = null!;
    }
}