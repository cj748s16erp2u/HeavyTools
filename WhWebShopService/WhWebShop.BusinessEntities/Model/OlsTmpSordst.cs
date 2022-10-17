using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("ols_tmp_sordst")]
    public partial class OlsTmpSordst : Entity
    {
        [Key]
        [Column("ssid")]
        public Guid Ssid { get; set; }
        [Key]
        [Column("sordlineid")]
        public int Sordlineid { get; set; }
        [Column("stid")]
        public int Stid { get; set; }
        [Column("sordid")]
        public int Sordid { get; set; }
        [Column("itemid")]
        public int Itemid { get; set; }
        [Column("qty", TypeName = "numeric(19, 6)")]
        public decimal Qty { get; set; }
        [Column("recqty", TypeName = "numeric(19, 6)")]
        public decimal? Recqty { get; set; }
        [Column("remqty", TypeName = "numeric(19, 6)")]
        public decimal Remqty { get; set; }
        [Column("selprc", TypeName = "numeric(19, 6)")]
        public decimal? Selprc { get; set; }
        [Column("selprctype")]
        public int? Selprctype { get; set; }
        [Column("selprcprcid")]
        public int? Selprcprcid { get; set; }
        [Column("discpercnt", TypeName = "numeric(9, 4)")]
        public decimal? Discpercnt { get; set; }
        [Column("discpercntprcid")]
        public int? Discpercntprcid { get; set; }
        [Column("discval", TypeName = "numeric(19, 6)")]
        public decimal? Discval { get; set; }
        [Column("taxid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Taxid { get; set; } = null!;
        [Column("ordresqty", TypeName = "numeric(19, 6)")]
        public decimal Ordresqty { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlsTmpSordst")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [ForeignKey("Itemid")]
        [InverseProperty("OlsTmpSordst")]
        public virtual OlsItem Item { get; set; } = null!;
        [ForeignKey("Sordid")]
        [InverseProperty("OlsTmpSordst")]
        public virtual OlsSordhead Sord { get; set; } = null!;
        [ForeignKey("Sordlineid")]
        [InverseProperty("OlsTmpSordst")]
        public virtual OlsSordline Sordline { get; set; } = null!;
        [ForeignKey("Taxid")]
        [InverseProperty("OlsTmpSordst")]
        public virtual OlsTax Tax { get; set; } = null!;
    }
}