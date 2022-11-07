using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    [Table("ols_sysval")]
    public partial class OlsSysval
    {
        [Key]
        [Column("sysvalid")]
        [StringLength(40)]
        [Unicode(false)]
        public string Sysvalid { get; set; } = null!;
        [Column("valueint")]
        public int? Valueint { get; set; }
        [Column("valuenum", TypeName = "numeric(19, 6)")]
        public decimal? Valuenum { get; set; }
        [Column("valuestr")]
        [StringLength(40)]
        [Unicode(false)]
        public string? Valuestr { get; set; }
        [Column("valuedate", TypeName = "datetime")]
        public DateTime? Valuedate { get; set; }
        [Column("valuevar", TypeName = "sql_variant")]
        public object? Valuevar { get; set; }
    }
}
