using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("olc_partner")]
    public partial class OlcPartner : Entity
    {
        [Key]
        [Column("partnid")]
        public int Partnid { get; set; }
        [Column("oldcode")]
        [StringLength(10)]
        [Unicode(false)]
        public string? Oldcode { get; set; }
        [Column("wsemail")]
        [Unicode(false)]
        public string? Wsemail { get; set; }
        [Column("invlngid")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Invlngid { get; set; }
        [Column("loyaltycardno")]
        [StringLength(20)]
        [Unicode(false)]
        public string? Loyaltycardno { get; set; }
        [Column("loyaltydiscpercnt", TypeName = "numeric(9, 4)")]
        public decimal? Loyaltydiscpercnt { get; set; }
        [Column("loyaltyturnover", TypeName = "numeric(19, 6)")]
        public decimal? Loyaltyturnover { get; set; }
        [Column("regreprempid")]
        public int? Regreprempid { get; set; }
        [Column("taxid")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Taxid { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime? Adddate { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlcPartner")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [ForeignKey("Partnid")]
        [InverseProperty("OlcPartner")]
        public virtual OlsPartner Partn { get; set; } = null!;
        [ForeignKey("Taxid")]
        [InverseProperty("OlcPartner")]
        public virtual OlsTax? Tax { get; set; }
    }
}