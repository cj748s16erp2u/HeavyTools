using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("olc_actionext")]
    public partial class OlcActionext : Entity
    {
        [Key]
        [Column("axid")]
        public int Axid { get; set; }
        [Column("aid")]
        public int Aid { get; set; }
        [Column("filteritems")]
        [Unicode(false)]
        public string Filteritems { get; set; } = null!;
        [Column("filteritemsblock")]
        [Unicode(false)]
        public string? Filteritemsblock { get; set; }
        [Column("count")]
        public int Count { get; set; }
        [Column("discounttype")]
        public int Discounttype { get; set; }
        [Column("discountval", TypeName = "numeric(19, 6)")]
        public decimal? Discountval { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }
        [Column("delstat")]
        public int Delstat { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlcActionext")]
        public virtual CfwUser Addusr { get; set; } = null!;
    }
}