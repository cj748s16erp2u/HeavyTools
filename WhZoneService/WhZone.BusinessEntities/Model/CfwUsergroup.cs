using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    [Table("cfw_usergroup")]
    public partial class CfwUsergroup
    {
        [Key]
        [Column("usrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Usrid { get; set; } = null!;
        [Key]
        [Column("grpid")]
        public int Grpid { get; set; }
        [Column("cs")]
        [StringLength(50)]
        [Unicode(false)]
        public string Cs { get; set; } = null!;

        [ForeignKey("Grpid")]
        [InverseProperty("CfwUsergroups")]
        public virtual CfwGroup Grp { get; set; } = null!;
        [ForeignKey("Usrid")]
        [InverseProperty("CfwUsergroups")]
        public virtual CfwUser Usr { get; set; } = null!;
    }
}
