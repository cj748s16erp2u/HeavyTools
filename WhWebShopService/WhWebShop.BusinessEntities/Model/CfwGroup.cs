using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("cfw_group")]
    public partial class CfwGroup : Entity
    {
        public CfwGroup()
        {
            CfwUsergroup = new HashSet<CfwUsergroup>();
        }

        [Key]
        [Column("grpid")]
        public int Grpid { get; set; }
        [Column("name")]
        [StringLength(40)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column("options")]
        public int Options { get; set; }
        [Column("lev")]
        public int Lev { get; set; }
        [Column("note")]
        [StringLength(200)]
        [Unicode(false)]
        public string? Note { get; set; }
        [Column("xmldata", TypeName = "xml")]
        public string? Xmldata { get; set; }
        [Column("cs")]
        [StringLength(50)]
        [Unicode(false)]
        public string Cs { get; set; } = null!;

        [InverseProperty("Grp")]
        public virtual ICollection<CfwUsergroup> CfwUsergroup { get; set; }
    }
}