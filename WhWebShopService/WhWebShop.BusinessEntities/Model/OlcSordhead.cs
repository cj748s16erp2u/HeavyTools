using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("olc_sordhead")]
    public partial class OlcSordhead : Entity
    {
        [Key]
        [Column("sordid")]
        public int Sordid { get; set; }
        [Column("sordapprovalstat")]
        public int Sordapprovalstat { get; set; }
        [Column("loyaltycardno")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Loyaltycardno { get; set; }
        [Column("transfcond")]
        public int? Transfcond { get; set; }
        [Column("deliverylocation")]
        [StringLength(200)]
        [Unicode(false)]
        public string? Deliverylocation { get; set; }
        [Column("data", TypeName = "xml")]
        public string? Data { get; set; }
        [Column("regreprempid")]
        public int? Regreprempid { get; set; }
        [Column("clerkempid")]
        public int? Clerkempid { get; set; }
        [Column("wid")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Wid { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlcSordhead")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [ForeignKey("Sordid")]
        [InverseProperty("OlcSordhead")]
        public virtual OlsSordhead Sord { get; set; } = null!;
    }
}