using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("olc_giftcardlog")]
    public partial class OlcGiftcardlog : Entity
    {
        [Key]
        [Column("gclid")]
        public int Gclid { get; set; }
        [Column("gcid")]
        public int Gcid { get; set; }
        [Column("sinvlineid")]
        public int? Sinvlineid { get; set; }
        [Column("sinvid")]
        public int? Sinvid { get; set; }
        [Column("val", TypeName = "numeric(19, 6)")]
        public decimal Val { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }
        [Column("delstat")]
        public int Delstat { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlcGiftcardlog")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [ForeignKey("Gcid")]
        [InverseProperty("OlcGiftcardlog")]
        public virtual OlcGiftcard Gc { get; set; } = null!;
    }
}