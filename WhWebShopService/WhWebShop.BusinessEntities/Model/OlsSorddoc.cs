using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("ols_sorddoc")]
    public partial class OlsSorddoc : Entity
    {
        public OlsSorddoc()
        {
            OlsSordhead = new HashSet<OlsSordhead>();
        }

        [Key]
        [Column("sorddocid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Sorddocid { get; set; } = null!;
        [Column("cmpcodes")]
        [StringLength(2000)]
        [Unicode(false)]
        public string Cmpcodes { get; set; } = null!;
        [Column("type")]
        public int Type { get; set; }
        [Column("name")]
        [StringLength(40)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column("bomdecomptype")]
        public int? Bomdecomptype { get; set; }
        [Column("visibleinsttype")]
        public int Visibleinsttype { get; set; }
        [Column("autoreservetype")]
        public int Autoreservetype { get; set; }
        [Column("autoreseventtype")]
        public int Autoreseventtype { get; set; }
        [Column("stclosemovqtychtype")]
        public int Stclosemovqtychtype { get; set; }
        [Column("bustypeid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Bustypeid { get; set; } = null!;
        [Column("headdefstat")]
        public int Headdefstat { get; set; }
        [Column("hcreatedmod")]
        public int? Hcreatedmod { get; set; }
        [Column("paymtype")]
        public int? Paymtype { get; set; }
        [Column("paymdeftype")]
        public int? Paymdeftype { get; set; }
        [Column("ref1")]
        [StringLength(30)]
        [Unicode(false)]
        public string? Ref1 { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }
        [Column("delstat")]
        public int Delstat { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlsSorddoc")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [InverseProperty("Sorddoc")]
        public virtual ICollection<OlsSordhead> OlsSordhead { get; set; }
    }
}