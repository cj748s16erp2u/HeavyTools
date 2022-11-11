using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("ols_itemgroup")]
    public partial class OlsItemgroup : Entity
    {
        public OlsItemgroup()
        {
            OlsItem = new HashSet<OlsItem>();
        }

        [Key]
        [Column("itemgrpid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Itemgrpid { get; set; } = null!;
        [Column("name")]
        [StringLength(40)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column("taxid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Taxid { get; set; } = null!;
        [Column("stockhtype")]
        public int Stockhtype { get; set; }
        [Column("sttosinvtype")]
        public int? Sttosinvtype { get; set; }
        [Column("uqcardtype")]
        public int? Uqcardtype { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }
        [Column("delstat")]
        public int Delstat { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlsItemgroup")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [ForeignKey("Taxid")]
        [InverseProperty("OlsItemgroup")]
        public virtual OlsTax Tax { get; set; } = null!;
        [InverseProperty("Itemgrp")]
        public virtual ICollection<OlsItem> OlsItem { get; set; }
    }
}