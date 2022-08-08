using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers.CSVParser;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Sord;

public class SordOrderCSV : CSFile<SordHeadOrderCSV, SordOrderLineCSV>
{
   
}

public class SordHeadOrderCSV
{
    [CSV(0)]
    public string? Ordernum { get; set; }

    [CSV(1)]
    public string? OrderDate { get; set; }
    
    [CSV(2)]
    public string? OrderName { get; set; }

    [CSV(3)]
    public string? CountryName { get; set; }
    
    [CSV(4)]
    public string? SinvAddr { get; set; }
    
    [CSV(5)]
    public string? SinvCity { get; set; }

    [CSV(6)]
    public string? SinvPostcode { get; set; }

    [CSV(7)]
    public string? ShippingAddr { get; set; }

    [CSV(8)]
    public string? ShippingCity { get; set; }

    [CSV(9)]
    public string? ShippingPostcode { get; set; }
    
    [CSV(10)]
    public string? ContactPerson { get; set; }
    [CSV(11)]
    public string? Phone { get; set; }
    [CSV(12)]
    public string? Email { get; set; }
    [CSV(13)]
    public string? LoyaltyCardno { get; set; }
    [CSV(14)]
    public string? Note { get; set; }

    [CSV(21)]
    public string? Curid { get; set; }
    [CSV(22)]
    public string? Rate { get; set; }
    [CSV(23)]
    public string? ShippinPrc { get; set; }
    [CSV(24)]
    public string? PaymentAndShippingMethod { get; set; }
    [CSV(25)]
    public string? PaymentTransacionData { get; set; }
    [CSV(26)]
    public string? NetGoPartnId { get; set; }
    [CSV(27)]
    public string? DestinationCountryCode { get; set; }
    [CSV(28)]
    public string? PPPid { get; set; }

    [CSV(29)]
    public string? ShippingPlace { get; set; }
    [CSV(30)]
    public string? ShippingPlaceType { get; set; }
    [CSV(31)]
    public string? ShippingHNum { get; set; }
    [CSV(32)]
    public string? ShippingBuilding { get; set; }
    [CSV(33)]
    public string? ShippingStairs { get; set; }
    [CSV(34)]
    public string? ShippingLevel{ get; set; }
    [CSV(35)]
    public string? ShippingDoor { get; set; }
    [CSV(36)]
    public string? ShippingDistrict { get; set; }
    [CSV(37)]
    public string? FoxPostId { get; set; }
    [CSV(38)]
    public string? GLSId { get; set; }
    [CSV(39)]
    public string? Taxnum { get; set; }
    [CSV(40)]
    public string? HTShipId { get; set; }
    [CSV(41)]
    public string? CentralRetailType{ get; set; }
    [CSV(42)]
    public string? ExchangePackagesNumber { get; set; }
    [CSV(43)]
    public string? ShippingMagicCode { get; set; }
    [CSV(44)]
    public string? PaymentMagicCode { get; set; }
}

public class SordOrderLineCSV
{
    [CSV(15)]
    public string? ItemName { get; set; }
    [CSV(16)]
    public string? OrdQty { get; set; }
    [CSV(17)]
    public string? NetPrc { get; set; }
    [CSV(18)]
    public string? NetVal { get; set; }
    [CSV(19)]
    public string? TaxVal { get; set; }
    [CSV(20)]
    public string? ItemCode { get; set; } 


}
