using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
using Microsoft.EntityFrameworkCore;


namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;


[Table("olc_itemtmp")]
public class ItemTmp : BusinessEntity
{
    [Key]
    [Column("itemid", TypeName = "int")]
    public int? Itemid { get; set; } = null!;

    [Column("productcode", TypeName = "varchar(max)")]
    public string? ProductCode { get; set; } = null!;

    [Column("cat1", TypeName = "varchar(max)")]
    public string? Cat1 { get; set; } = null!;

    [Column("cat2", TypeName = "varchar(max)")]
    public string? Cat2 { get; set; } = null!;

    [Column("productname", TypeName = "varchar(max)")]
    public string? ProductName { get; set; } = null!; 

    [Column("descriptionhu", TypeName = "varchar(max)")]
    public string? DescriptionHu { get; set; } = null!; 

    [Column("descriptioen", TypeName = "varchar(max)")]
    public string? DescriptioEn { get; set; } = null!; 

    [Column("descriptinsk", TypeName = "varchar(max)")]
    public string? DescriptinSk { get; set; } = null!;
     
    [Column("descriptincz", TypeName = "varchar(max)")]
    public string? DescriptinCz { get; set; } = null!; 
      
    [Column("descriptinro", TypeName = "varchar(max)")]
    public string? DescriptinRo { get; set; } = null!; 

    [Column("ean", TypeName = "varchar(max)")]
    public string? EAN { get; set; } = null!; 

    [Column("season", TypeName = "varchar(max)")]
    public string? Season { get; set; } = null!;

    [Column("priceGrossHuf", TypeName = "numeric(19, 6)")]
    public decimal? PriceGrossHuf { get; set; }


    [Column("priceGrossHuf", TypeName = "numeric(19, 6)")]
    public decimal? PriceSaleGrossHuf { get; set; }


    [Column("priceGrossHuf", TypeName = "numeric(19, 6)")]
    public decimal? RetailPriceGrossHuf { get; set; }


    [Column("priceGrossHuf", TypeName = "numeric(19, 6)")]
    public decimal? RetailPriceSaleGrossHuf { get; set; }


    [Column("priceGrossHuf", TypeName = "numeric(19, 6)")]
    public decimal? PriceGrossEurEn { get; set; }


    [Column("priceGrossHuf", TypeName = "numeric(19, 6)")]
    public decimal? PriceSaleGrossEurEn { get; set; }


    [Column("priceGrossHuf", TypeName = "numeric(19, 6)")]
    public decimal? RetailPriceSaleGrossEurEn { get; set; }


    [Column("priceGrossHuf", TypeName = "numeric(19, 6)")]
    public decimal? RetailPriceGrossEurEn { get; set; }


    [Column("priceGrossEurSK", TypeName = "numeric(19, 6)")]
    public decimal? PriceGrossEurSK { get; set; }


    [Column("priceSaleGrossEurSK", TypeName = "numeric(19, 6)")]
    public decimal? PriceSaleGrossEurSK { get; set; }


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
}
