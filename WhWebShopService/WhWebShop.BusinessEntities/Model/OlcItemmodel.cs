using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("olc_itemmodel")]
    public partial class OlcItemmodel : Entity
    {
        public OlcItemmodel()
        {
            OlcItemmodelseason = new HashSet<OlcItemmodelseason>();
            OlcPrctable = new HashSet<OlcPrctable>();
        }

        [Key]
        [Column("imid")]
        public int Imid { get; set; }
        [Column("imgid")]
        public int Imgid { get; set; }
        [Column("code")]
        [StringLength(8)]
        [Unicode(false)]
        public string Code { get; set; } = null!;
        [Column("name")]
        [StringLength(200)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column("unitid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Unitid { get; set; } = null!;
        [Column("exclusivetype")]
        public int? Exclusivetype { get; set; }
        [Column("netweight", TypeName = "numeric(19, 6)")]
        public decimal? Netweight { get; set; }
        [Column("grossweight", TypeName = "numeric(19, 6)")]
        public decimal? Grossweight { get; set; }
        [Column("volume", TypeName = "numeric(19, 6)")]
        public decimal? Volume { get; set; }
        [Column("isimported")]
        public int? Isimported { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Addusrid { get; set; }
        [Column("adddate", TypeName = "datetime")]
        public DateTime? Adddate { get; set; }
        [Column("delstat")]
        public int Delstat { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlcItemmodel")]
        public virtual CfwUser? Addusr { get; set; }
        [InverseProperty("Im")]
        public virtual ICollection<OlcItemmodelseason> OlcItemmodelseason { get; set; }
        [InverseProperty("Im")]
        public virtual ICollection<OlcPrctable> OlcPrctable { get; set; }
    }
}