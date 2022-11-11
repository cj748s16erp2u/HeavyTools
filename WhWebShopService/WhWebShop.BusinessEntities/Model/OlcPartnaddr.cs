using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("olc_partnaddr")]
    public partial class OlcPartnaddr : Entity
    {
        [Key]
        [Column("addrid")]
        public int Addrid { get; set; }
        [Column("oldcode")]
        [StringLength(10)]
        [Unicode(false)]
        public string? Oldcode { get; set; }
        [Column("glnnum")]
        [StringLength(15)]
        [Unicode(false)]
        public string? Glnnum { get; set; }
        [Column("buildingname")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Buildingname { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime? Adddate { get; set; }

        [ForeignKey("Addrid")]
        [InverseProperty("OlcPartnaddr")]
        public virtual OlsPartnaddr Addr { get; set; } = null!;
        [ForeignKey("Addusrid")]
        [InverseProperty("OlcPartnaddr")]
        public virtual CfwUser Addusr { get; set; } = null!;
    }
}