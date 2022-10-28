using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("ols_stock")]
    public partial class OlsStock : Entity
    {
        [Key]
        [Column("itemid")]
        public int Itemid { get; set; }
        [Key]
        [Column("whid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Whid { get; set; } = null!;
        [Column("actqty", TypeName = "numeric(19, 6)")]
        public decimal Actqty { get; set; }
        [Column("resqty", TypeName = "numeric(19, 6)")]
        public decimal Resqty { get; set; }

        [ForeignKey("Itemid")]
        [InverseProperty("OlsStock")]
        public virtual OlsItem Item { get; set; } = null!;
    }
}