using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("olc_sordline_res")]
    public partial class OlcSordlineRes : Entity
    {
        [Key]
        [Column("sordlineidres")]
        public int Sordlineidres { get; set; }
        [Column("sordlineid")]
        public int Sordlineid { get; set; }
        [Column("resid")]
        public int Resid { get; set; }
        [Column("preordersordlineid")]
        public int? Preordersordlineid { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlcSordlineRes")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [ForeignKey("Preordersordlineid")]
        [InverseProperty("OlcSordlineResPreordersordline")]
        public virtual OlsSordline? Preordersordline { get; set; }
        [ForeignKey("Resid")]
        [InverseProperty("OlcSordlineRes")]
        public virtual OlsReserve Res { get; set; } = null!;
        [ForeignKey("Sordlineid")]
        [InverseProperty("OlcSordlineResSordline")]
        public virtual OlsSordline Sordline { get; set; } = null!;
    }
}