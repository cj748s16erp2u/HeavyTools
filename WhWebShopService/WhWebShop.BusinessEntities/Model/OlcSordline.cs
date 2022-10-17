using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("olc_sordline")]
    public partial class OlcSordline : Entity
    {
        [Key]
        [Column("sordlineid")]
        public int Sordlineid { get; set; }
        [Column("confqty", TypeName = "numeric(19, 6)")]
        public decimal? Confqty { get; set; }
        [Column("confdeldate", TypeName = "datetime")]
        public DateTime? Confdeldate { get; set; }
        [Column("data", TypeName = "xml")]
        public string? Data { get; set; }
        [Column("preordersordlineid")]
        public int? Preordersordlineid { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlcSordline")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [ForeignKey("Preordersordlineid")]
        [InverseProperty("OlcSordlinePreordersordline")]
        public virtual OlsSordline? Preordersordline { get; set; }
        [ForeignKey("Sordlineid")]
        [InverseProperty("OlcSordlineSordline")]
        public virtual OlsSordline Sordline { get; set; } = null!;
    }
}