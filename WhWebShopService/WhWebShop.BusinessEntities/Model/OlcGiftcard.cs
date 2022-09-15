using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("olc_giftcard")]
    public partial class OlcGiftcard : Entity
    {
        public OlcGiftcard()
        {
            OlcGiftcardlog = new HashSet<OlcGiftcardlog>();
        }

        [Key]
        [Column("gcid")]
        public int Gcid { get; set; }
        [Column("barcode")]
        [StringLength(40)]
        [Unicode(false)]
        public string? Barcode { get; set; }
        [Column("pincode")]
        [StringLength(4)]
        [Unicode(false)]
        public string? Pincode { get; set; }
        [Column("prc", TypeName = "numeric(19, 6)")]
        public decimal Prc { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }
        [Column("delstat")]
        public int Delstat { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlcGiftcard")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [InverseProperty("Gc")]
        public virtual ICollection<OlcGiftcardlog> OlcGiftcardlog { get; set; }
    }
}