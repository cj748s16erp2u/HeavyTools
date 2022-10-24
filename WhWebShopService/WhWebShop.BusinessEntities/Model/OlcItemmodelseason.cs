using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("olc_itemmodelseason")]
    public partial class OlcItemmodelseason : Entity
    {
        public OlcItemmodelseason()
        {
            OlcItem = new HashSet<OlcItem>();
        }

        [Key]
        [Column("imsid")]
        public int Imsid { get; set; }
        [Column("imid")]
        public int? Imid { get; set; }
        [Column("isid")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Isid { get; set; }
        [Column("icid")]
        [StringLength(3)]
        [Unicode(false)]
        public string? Icid { get; set; }
        [Column("oldcode")]
        public int? Oldcode { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Addusrid { get; set; }
        [Column("adddate", TypeName = "datetime")]
        public DateTime? Adddate { get; set; }
        [Column("delstat")]
        public int Delstat { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlcItemmodelseason")]
        public virtual CfwUser? Addusr { get; set; }
        [ForeignKey("Imid")]
        [InverseProperty("OlcItemmodelseason")]
        public virtual OlcItemmodel? Im { get; set; }
        [InverseProperty("Ims")]
        public virtual ICollection<OlcItem> OlcItem { get; set; }
    }
}