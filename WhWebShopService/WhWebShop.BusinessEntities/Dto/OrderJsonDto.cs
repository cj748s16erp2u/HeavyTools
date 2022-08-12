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

    [JsonField(true)]
    public string Wid { get; set; } = null!;

    [JsonField(true)]
    public string OrderNumber { get; set; } = null!;

    [JsonField(true, RegexpType.Date)]
    public DateTime? OrderDate { get; set; } = null!;

    [JsonField(true)]
    public string Sinv_name { get; set; } = null!;
    [JsonField(false)]
    public string Note { get; set; } = null!;

    [JsonField(true)]
    public int? GiftCardLogId { get; set; } = null!;
    
    [JsonField(true)]
    public string Sinv_countryid { get; set; } = null!;
    [JsonField(true)]
    public string Sinv_postcode { get; set; } = null!;
    [JsonField(true)]
    public string Sinv_city { get; set; } = null!;

    [JsonField(false)]
    public string Sinv_building { get; set; } = null!;

    [JsonField(false)]
    public string Sinv_district { get; set; } = null!;

    [JsonField(false)]
    public string Sinv_door { get; set; } = null!;

    [JsonField(true)]
    public string Sinv_hnum { get; set; } = null!;

    [JsonField(false)]
    public string Sinv_floor { get; set; } = null!;

    [JsonField(true)]
    public string Sinv_place { get; set; } = null!;

    [JsonField(true)]
    public string Sinv_placetype { get; set; } = null!;

    [JsonField(false)]
    public string Sinv_stairway { get; set; } = null!;

    [JsonField(true)]
    public string Shipping_name { get; set; } = null!;
    [JsonField(true)]
    public string Shipping_countryid { get; set; } = null!;
    [JsonField(true)]
    public string Shipping_postcode { get; set; } = null!;
    [JsonField(true)]
    public string Shipping_city { get; set; } = null!;

    [JsonField(false)]
    public string Shipping_building { get; set; } = null!;

    [JsonField(false)]
    public string Shipping_district { get; set; } = null!;

    [JsonField(false)]
    public string Shipping_door { get; set; } = null!;

    [JsonField(true)]
    public string Shipping_hnum { get; set; } = null!;

    [JsonField(false)]
    public string Shipping_floor { get; set; } = null!;

    [JsonField(true)]
    public string Shipping_place { get; set; } = null!;

    [JsonField(true)]
    public string Shipping_placetype { get; set; } = null!;

    [JsonField(false)]
    public string Shipping_stairway { get; set; } = null!;

    [JsonField(true)]
    public string Phone { get; set; } = null!;

    [JsonField(true, RegexpType.Email)]
    public string Email { get; set; } = null!;

    [JsonField(false)]
    public decimal? ShippinPrc { get; set; } = null!;

    [JsonField(false)]
    public string Paymenttransaciondata { get; set; } = null!;

    [JsonField(true)]
    public string Netgopartnid { get; set; } = null!;

    [JsonField(false)]
    public string Pppid { get; set; } = null!;

    [JsonField(false)]
    public string Glsid { get; set; } = null!;

    [JsonField(false)]
    public string Foxpostid { get; set; } = null!;

    [JsonField(false)]
    public string CentralRetailType { get; set; } = null!;

    [JsonField(false)]
    public int? Exchangepackagesnumber { get; set; } = null!;

    [JsonField(true)]
    public string ShippingId { get; set; } = null!;

    [JsonField(true)]
    public string PaymentId { get; set; } = null!;

    [JsonField(true)]
    public CalcJsonParamsDto Cart { get; set; } = null!;
}
