using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("olc_actioncouponnumber")]
    public partial class OlcActioncouponnumber : Entity
    {
        [Key]
        [Column("acnid")]
        public int Acnid { get; set; }
        [Column("aid")]
        public int Aid { get; set; }
        [Column("couponnumber")]
        [StringLength(100)]
        [Unicode(false)]
        public string Couponnumber { get; set; } = null!;
        [Column("used")]
        public int Used { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }
        [Column("delstat")]
        public int Delstat { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlcActioncouponnumber")]
        public virtual CfwUser Addusr { get; set; } = null!;
    }
}