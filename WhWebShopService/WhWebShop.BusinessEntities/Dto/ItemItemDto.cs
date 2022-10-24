namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;

public  class ItemItemDto
{
    public string ProductCode { get; set; } = null!;
    public string Cat1 { get; set; } = null!;
    public string Cat2 { get; set; } = null!;
    public string ProductName { get; set; } = null!;

    //public decimal? StockPc { get; set; } = null!;
     
    public string DescriptionHu { get; set; } = null!;

    //public string VatHu { get; set; } = null!;

    public decimal? PriceGrossHuf { get; set; } = null!;

    public decimal? PriceSaleGrossHuf { get; set; } = null!;

    public decimal? RetailPriceGrossHuf { get; set; } = null!;

    public decimal? RetailPriceSaleGrossHuf { get; set; } = null!;
 
    public string DescriptioEn { get; set; } = null!;

    //public string VatEn { get; set; } = null!;

    public decimal? PriceGrossEurEn { get; set; } = null!;

    public decimal? PriceSaleGrossEurEn { get; set; } = null!;

    public decimal? RetailPriceGrossEurEn { get; set; } = null!;

    public decimal? RetailPriceSaleGrossEurEn { get; set; } = null!;
 
    public string DescriptinSk { get; set; } = null!;

    //public string VatSk { get; set; } = null!;

    public decimal? PriceGrossEurSK { get; set; } = null!;

    public decimal? PriceSaleGrossEurSK { get; set; } = null!;
 
    public string DescriptinCz { get; set; } = null!;

    //public string VatCz { get; set; } = null!;

    public decimal? PriceGrossCzkCz { get; set; } = null!;

    public decimal? PriceSaleGrossCzkCz { get; set; } = null!;

    public decimal? RetailPriceGrossCzkCz { get; set; } = null!;

    public decimal? RetailPriceSaleGrossCzkCz { get; set; } = null!;

    public string DescriptinRo { get; set; } = null!;

    //public string VatRo { get; set; } = null!;

    public decimal? PriceGrossRonRo { get; set; } = null!;

    public decimal? PriceSaleGrossRonRo { get; set; } = null!;

    public decimal? RetailPriceGrossRonRo { get; set; } = null!;

    public decimal? RetailPriceSaleGrossRonRo { get; set; } = null!;

    //public string Cat3 { get; set; } = null!;
    //public string Cat4 { get; set; } = null!;

    public string EAN { get; set; } = null!;

    public string Season { get; set; } = null!;

    public DateTime? PriceDate { get; set; } = null;

}
