using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("ols_taxtrans")]
    public partial class OlsTaxtrans : Entity
    {
        public OlsTaxtrans()
        {
            OlcTaxtransext = new HashSet<OlcTaxtransext>();
        }

        [Key]
        [Column("ttid")]
        public int Ttid { get; set; }
        [Column("taxid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Taxid { get; set; } = null!;
        [Column("bustypeid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Bustypeid { get; set; } = null!;
        [Column("cmpid")]
        public int Cmpid { get; set; }
        [Column("cmpcodes")]
        [StringLength(2000)]
        [Unicode(false)]
        public string Cmpcodes { get; set; } = null!;
        [Column("startdate", TypeName = "datetime")]
        public DateTime Startdate { get; set; }
        [Column("enddate", TypeName = "datetime")]
        public DateTime Enddate { get; set; }
        [Column("realtaxid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Realtaxid { get; set; } = null!;
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlsTaxtrans")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [InverseProperty("Tt")]
        public virtual ICollection<OlcTaxtransext> OlcTaxtransext { get; set; }
    }
}