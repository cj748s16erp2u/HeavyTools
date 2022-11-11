using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    [Table("ols_statline")]
    public partial class OlsStatline
    {
        [Key]
        [Column("statgrpid")]
        public int Statgrpid { get; set; }
        [Key]
        [Column("value")]
        public int Value { get; set; }
        [Column("seqno")]
        public int Seqno { get; set; }
        [Column("name")]
        [StringLength(40)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column("abbr")]
        [StringLength(5)]
        [Unicode(false)]
        public string Abbr { get; set; } = null!;
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]

        [ForeignKey("Addusrid")]
        [InverseProperty("OlsStatlines")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [ForeignKey("Statgrpid")]
        [InverseProperty("OlsStatlines")]
        public virtual OlsStathead Statgrp { get; set; } = null!;
    }
}
