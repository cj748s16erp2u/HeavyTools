using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("olc_tmp_sordsord")]
    public partial class OlcTmpSordsord : Entity
    {
        [Column("ssid")]
        public Guid Ssid { get; set; }
        [Key]
        [Column("sordlineid")]
        public int Sordlineid { get; set; }
        [Column("sordid")]
        public int Sordid { get; set; }
        [Column("linenum")]
        public int Linenum { get; set; }
        [Column("itemid")]
        public int Itemid { get; set; }
        [Column("itemcode")]
        [StringLength(25)]
        [Unicode(false)]
        public string? Itemcode { get; set; }
        [Column("name01")]
        [StringLength(100)]
        [Unicode(false)]
        public string? Name01 { get; set; }
        [Column("name02")]
        [StringLength(200)]
        public string? Name02 { get; set; }
        [Column("docnum")]
        [StringLength(12)]
        [Unicode(false)]
        public string Docnum { get; set; } = null!;
        [Column("qty", TypeName = "numeric(19, 6)")]
        public decimal? Qty { get; set; }
        [Column("reqdate", TypeName = "datetime")]
        public DateTime Reqdate { get; set; }
        [Column("confqty", TypeName = "numeric(19, 6)")]
        public decimal? Confqty { get; set; }
        [Column("confdeldate", TypeName = "datetime")]
        public DateTime? Confdeldate { get; set; }
        [Column("ref2")]
        [StringLength(30)]
        [Unicode(false)]
        public string? Ref2 { get; set; }
        [Column("pendingqty", TypeName = "numeric(19, 6)")]
        public decimal Pendingqty { get; set; }
        [Column("fullordqty", TypeName = "numeric(19, 6)")]
        public decimal Fullordqty { get; set; }
        [Column("fullmovqty", TypeName = "numeric(19, 6)")]
        public decimal Fullmovqty { get; set; }
        [Column("ordqty", TypeName = "numeric(19, 6)")]
        public decimal Ordqty { get; set; }
        [Column("movqty", TypeName = "numeric(19, 6)")]
        public decimal Movqty { get; set; }
        [Column("selprc", TypeName = "numeric(19, 6)")]
        public decimal Selprc { get; set; }
        [Column("seltotprc", TypeName = "numeric(19, 6)")]
        public decimal? Seltotprc { get; set; }
        [Column("selprctype")]
        public int? Selprctype { get; set; }
        [Column("selprcprcid")]
        public int? Selprcprcid { get; set; }
        [Column("discpercnt", TypeName = "numeric(9, 4)")]
        public decimal Discpercnt { get; set; }
        [Column("discpercntprcid")]
        public int? Discpercntprcid { get; set; }
        [Column("discval", TypeName = "numeric(19, 6)")]
        public decimal Discval { get; set; }
        [Column("disctotval", TypeName = "numeric(19, 6)")]
        public decimal? Disctotval { get; set; }
        [Column("taxid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Taxid { get; set; } = null!;
        [Column("sordlinestat")]
        public int Sordlinestat { get; set; }
        [Column("note")]
        [StringLength(200)]
        [Unicode(false)]
        public string? Note { get; set; }
        [Column("resid")]
        public int? Resid { get; set; }
        [Column("ucdid")]
        public int? Ucdid { get; set; }
        [Column("pjpid")]
        public int? Pjpid { get; set; }
        [Column("gen")]
        public int Gen { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }
    }
}