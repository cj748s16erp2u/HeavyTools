using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("olc_prctype")]
    public partial class OlcPrctype : Entity
    {
        public OlcPrctype()
        {
            OlcPrctable = new HashSet<OlcPrctable>();
        }

        [Key]
        [Column("ptid")]
        public int Ptid { get; set; }
        [Column("name")]
        [StringLength(100)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column("isnet")]
        public int Isnet { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Addusrid { get; set; }
        [Column("adddate", TypeName = "datetime")]
        public DateTime? Adddate { get; set; }
        [Column("delstat")]
        public int Delstat { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlcPrctype")]
        public virtual CfwUser? Addusr { get; set; }
        [InverseProperty("Pt")]
        public virtual ICollection<OlcPrctable> OlcPrctable { get; set; }
    }
}