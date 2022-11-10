using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;


[JsonObjectAttributes("Partner")]
public class PartnerParamsDto
{   
    [JsonField(MandotaryType.Yes)]
    public string? WebpartnerID { get; set; }

    [JsonField(MandotaryType.Yes)]
    public string Name { get; set; } = null!;

    [JsonField(MandotaryType.Yes)]
    public string Ptvattypid { get; set; } = null!; 

    [JsonField(MandotaryType.Yes)]
    public string? Email { get; set; } = null!; 

    [JsonField(MandotaryType.Yes)]
    public string? Loyaltycardno { get; set; } = null!;


    [JsonField(MandotaryType.Yes)]
    public string Sinv_name { get; set; } = null!;

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

    [JsonField(MandotaryType.No)]
    public string Sinv_hnum { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public string Sinv_floor { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public string Sinv_place { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public string Sinv_placetype { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public string Sinv_stairway { get; set; } = null!;


    [JsonField(MandotaryType.Yes)]
    public string Shipping_countryid { get; set; } = null!;

    [JsonField(MandotaryType.Yes)]
    public string Shipping_name { get; set; } = null!;

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

    [JsonField(MandotaryType.No)]
    public string Shipping_hnum { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public string Shipping_floor { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public string Shipping_place { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public string Shipping_placetype { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public string Shipping_stairway { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public string? Phone { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public string? Vatnum { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public string? Vatnumeu { get; set; } = null!;

    [JsonField(MandotaryType.No)]
    public string? Groupvatnum { get; set; } = null!;
}
