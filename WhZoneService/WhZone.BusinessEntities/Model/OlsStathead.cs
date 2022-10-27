using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    [Table("ols_stathead")]
    public partial class OlsStathead
    {
        public OlsStathead()
        {
            OlsStatlines = new HashSet<OlsStatline>();
        }

        [Key]
        [Column("statgrpid")]
        public int Statgrpid { get; set; }
        [Column("statkey")]
        [StringLength(50)]
        [Unicode(false)]
        public string Statkey { get; set; } = null!;
        [Column("descr")]
        [StringLength(50)]
        [Unicode(false)]
        public string Descr { get; set; } = null!;
        [Column("protect")]
        public int Protect { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }
        [Column("delstat")]
        public int Delstat { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlsStatheads")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [InverseProperty("Statgrp")]
        public virtual ICollection<OlsStatline> OlsStatlines { get; set; }
    }
}
