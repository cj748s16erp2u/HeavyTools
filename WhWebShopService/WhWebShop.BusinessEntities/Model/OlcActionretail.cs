using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("olc_actionretail")]
    public partial class OlcActionretail : Entity
    {
        [Key]
        [Column("arid")]
        public int Arid { get; set; }
        [Column("aid")]
        public int Aid { get; set; }
        [Column("whid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Whid { get; set; } = null!;
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }
        [Column("delstat")]
        public int Delstat { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlcActionretail")]
        public virtual CfwUser Addusr { get; set; } = null!;
    }
}