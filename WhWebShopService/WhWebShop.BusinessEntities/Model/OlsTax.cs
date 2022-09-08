using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("ols_tax")]
    public partial class OlsTax : Entity
    {
        public OlsTax()
        {
            OlcTaxtransext = new HashSet<OlcTaxtransext>();
            OlsSordline = new HashSet<OlsSordline>();
            OlsTaxtransRealtax = new HashSet<OlsTaxtrans>();
            OlsTaxtransTax = new HashSet<OlsTaxtrans>();
        }

        [Key]
        [Column("taxid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Taxid { get; set; } = null!;
        [Column("name")]
        [StringLength(40)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column("taxtype")]
        public int Taxtype { get; set; }
        [Column("vistype")]
        public int? Vistype { get; set; }
        [Column("percnt", TypeName = "numeric(9, 4)")]
        public decimal Percnt { get; set; }
        [Column("navsubtype")]
        public int? Navsubtype { get; set; }
        [Column("exemptionreason")]
        [StringLength(200)]
        [Unicode(false)]
        public string? Exemptionreason { get; set; }
        [Column("invnote")]
        [StringLength(500)]
        [Unicode(false)]
        public string? Invnote { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }
        [Column("delstat")]
        public int Delstat { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlsTax")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [InverseProperty("Tax")]
        public virtual ICollection<OlcTaxtransext> OlcTaxtransext { get; set; }
        [InverseProperty("Tax")]
        public virtual ICollection<OlsSordline> OlsSordline { get; set; }
        [InverseProperty("Realtax")]
        public virtual ICollection<OlsTaxtrans> OlsTaxtransRealtax { get; set; }
        [InverseProperty("Tax")]
        public virtual ICollection<OlsTaxtrans> OlsTaxtransTax { get; set; }
    }
}