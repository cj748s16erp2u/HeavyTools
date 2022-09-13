using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;

[JsonObjectAttributes("Create")]
public class OrderJsonParamsDto
{

    [JsonField(MandotaryType.No, MandotaryType.Yes)]
    public string Wid { get; set; } = null!;

    [JsonField(MandotaryType.Yes)]
    public string OrderNumber { get; set; } = null!;

    [JsonField(RegexpType.Date, MandotaryType.Yes)]
    public DateTime? OrderDate { get; set; } = null!;

    [JsonField(MandotaryType.Yes)]
    public string Sinv_name { get; set; } = null!;
    [JsonField(MandotaryType.No)]
    public string Note { get; set; } = null!;

    [JsonField(MandotaryType.Yes)]
    public int? GiftCardLogId { get; set; } = null!;
    
    [JsonField(MandotaryType.Yes)]
    public string Sinv_countryid { get; set; } = null!;
    [JsonField(MandotaryType.Yes)]
    public string Sinv_postcode { get; set; } = null!;
    [JsonField(MandotaryType.Yes)]
    public string Sinv_city { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public string Sinv_building { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public string Sinv_district { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public string Sinv_door { get; set; } = null!;

    [JsonField(MandotaryType.Yes)]
    public string Sinv_hnum { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public string Sinv_floor { get; set; } = null!;

    [JsonField(MandotaryType.Yes)]
    public string Sinv_place { get; set; } = null!;

    [JsonField(MandotaryType.Yes)]
    public string Sinv_placetype { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public string Sinv_stairway { get; set; } = null!;

    [JsonField(MandotaryType.Yes)]
    public string Shipping_name { get; set; } = null!;
    [JsonField(MandotaryType.Yes)]
    public string Shipping_countryid { get; set; } = null!;
    [JsonField(MandotaryType.Yes)]
    public string Shipping_postcode { get; set; } = null!;
    [JsonField(MandotaryType.Yes)]
    public string Shipping_city { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public string Shipping_building { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public string Shipping_district { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public string Shipping_door { get; set; } = null!;

    [JsonField(MandotaryType.Yes)]
    public string Shipping_hnum { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public string Shipping_floor { get; set; } = null!;

    [JsonField(MandotaryType.Yes)]
    public string Shipping_place { get; set; } = null!;

    [JsonField(MandotaryType.Yes)]
    public string Shipping_placetype { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public string Shipping_stairway { get; set; } = null!;

    [JsonField(MandotaryType.Yes)]
    public string Phone { get; set; } = null!;

    [JsonField(RegexpType.Email, MandotaryType.Yes)]
    public string Email { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public decimal? ShippinPrc { get; set; } = null!; 
    
    [JsonField(MandotaryType.No)]
    public decimal? PaymentFee { get; set; } = null!;


    [JsonField(MandotaryType.No)]
    public string Paymenttransaciondata { get; set; } = null!;

    [JsonField(MandotaryType.Yes)]
    public string Netgopartnid { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public string Pppid { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public string Glsid { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public string Foxpostid { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public string CentralRetailType { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public int? Exchangepackagesnumber { get; set; } = null!;

    [JsonField(MandotaryType.Yes)]
    public string ShippingId { get; set; } = null!;

    [JsonField(MandotaryType.Yes)]
    public string PaymentId { get; set; } = null!;

    [JsonField(MandotaryType.Yes)]
    public CalcJsonParamsDto Cart { get; set; } = null!;
}
