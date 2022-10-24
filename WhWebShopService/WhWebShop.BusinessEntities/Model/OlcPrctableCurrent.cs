using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("olc_prctable_current")]
    public partial class OlcPrctableCurrent : Entity
    {
        [Key]
        [Column("prccid")]
        public int Prccid { get; set; }
        [Column("itemid")]
        public int Itemid { get; set; }
        [Column("date", TypeName = "datetime")]
        public DateTime Date { get; set; }
        [Column("priceGrossHuf", TypeName = "numeric(19, 6)")]
        public decimal? PriceGrossHuf { get; set; }
        [Column("priceSaleGrossHuf", TypeName = "numeric(19, 6)")]
        public decimal? PriceSaleGrossHuf { get; set; }
        [Column("retailPriceGrossHuf", TypeName = "numeric(19, 6)")]
        public decimal? RetailPriceGrossHuf { get; set; }
        [Column("retailPriceSaleGrossHuf", TypeName = "numeric(19, 6)")]
        public decimal? RetailPriceSaleGrossHuf { get; set; }
        [Column("priceGrossEurEn", TypeName = "numeric(19, 6)")]
        public decimal? PriceGrossEurEn { get; set; }
        [Column("priceSaleGrossEurEn", TypeName = "numeric(19, 6)")]
        public decimal? PriceSaleGrossEurEn { get; set; }
        [Column("retailPriceGrossEurEn", TypeName = "numeric(19, 6)")]
        public decimal? RetailPriceGrossEurEn { get; set; }
        [Column("retailPriceSaleGrossEurEn", TypeName = "numeric(19, 6)")]
        public decimal? RetailPriceSaleGrossEurEn { get; set; }
        [Column("priceGrossEurSK", TypeName = "numeric(19, 6)")]
        public decimal? PriceGrossEurSk { get; set; }
        [Column("priceSaleGrossEurSK", TypeName = "numeric(19, 6)")]
        public decimal? PriceSaleGrossEurSk { get; set; }
        [Column("priceGrossCzkCz", TypeName = "numeric(19, 6)")]
        public decimal? PriceGrossCzkCz { get; set; }
        [Column("priceSaleGrossCzkCz", TypeName = "numeric(19, 6)")]
        public decimal? PriceSaleGrossCzkCz { get; set; }
        [Column("retailPriceGrossCzkCz", TypeName = "numeric(19, 6)")]
        public decimal? RetailPriceGrossCzkCz { get; set; }
        [Column("retailPriceSaleGrossCzkCz", TypeName = "numeric(19, 6)")]
        public decimal? RetailPriceSaleGrossCzkCz { get; set; }
        [Column("priceGrossRonRo", TypeName = "numeric(19, 6)")]
        public decimal? PriceGrossRonRo { get; set; }
        [Column("priceSaleGrossRonRo", TypeName = "numeric(19, 6)")]
        public decimal? PriceSaleGrossRonRo { get; set; }
        [Column("retailPriceGrossRonRo", TypeName = "numeric(19, 6)")]
        public decimal? RetailPriceGrossRonRo { get; set; }
        [Column("retailPriceSaleGrossRonRo", TypeName = "numeric(19, 6)")]
        public decimal? RetailPriceSaleGrossRonRo { get; set; }

        [ForeignKey("Itemid")]
        [InverseProperty("OlcPrctableCurrent")]
        public virtual OlsItem Item { get; set; } = null!;
    }
}