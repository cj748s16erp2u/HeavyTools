using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto.Base;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Enums;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;

public class CalcJsonResultDto : ResultDto
{
    [Newtonsoft.Json.JsonIgnore] 
    public string CountryId { get; set; } = null!;

    public string Curid { get; set; } = null!;

    public CalcItemJsonResultDto[] Items { get; set; } = null!;

    public bool FreeShipping { get; set; } = false;
    public bool FreeFee { get; set; } = false;


    [Newtonsoft.Json.JsonIgnore]
    public bool TempFreeShipping { get; set; } = false;

    [Newtonsoft.Json.JsonIgnore]
    public bool TempFreeFee { get; set; } = false;




    [Newtonsoft.Json.JsonIgnore]
    public bool IsNet { get; set; } = false;

    [Newtonsoft.Json.JsonIgnore]
    public int? Round { get; set; }
}
public class CalcItemJsonResultDto
{
    public int? CartId { get; set; } = null!;
    public int? Itemid { get; set; } = null!;
    public string ItemCode { get; set; } = null!;
    public int? Quantity { get; set; } = null!;

    /// <summary>
    /// Eredeti egységár (lehet nettó vagy bruttó)
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public decimal? RawOrigSelPrc { get; set; } = null!;


    
    /// <summary>
    /// Egység ár (lehet nettó vagy bruttó)
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public decimal? RawSelPrc { get; set; } = null!;


    
    /// <summary>
    /// Érték  (lehet nettó vagy bruttó)
    /// </summary> 
    [Newtonsoft.Json.JsonIgnore]
    public decimal? RawSelVal { get; set; } = null!;

    /// <summary>
    /// Eredeti nettó egységár
    /// </summary>
    public decimal? OrignalSelPrc { get; set; } = null!;


    /// <summary>
    /// Eredeti bruttó egységár
    /// </summary>
    public decimal? OrignalGrossPrc { get; set; } = null!;
  
    /// <summary>
    /// Eredeti bruttó érték
    /// </summary>
    public decimal? OrignalTotVal{ get; set; } = null!;
  
    /// <summary>
    /// Nettó egységár
    /// </summary>
    public decimal? SelPrc { get; set; } = null!;

    /// <summary>
    /// Bruttó egységár
    /// </summary>
    public decimal? GrossPrc { get; set; } = null!;


    /// <summary>
    /// Nettó érték  (Nettó egységár * Mennyiség)
    /// </summary>
    public decimal? NetVal { get; set; } = null!;

    /// <summary>
    /// Áfa érték
    /// </summary>
    public decimal? TaxVal { get; set; } = null!;

    /// <summary>
    /// Bruttó érték
    /// </summary>
    public decimal? TotVal  { get; set; } = null!;


    [Newtonsoft.Json.JsonIgnore]
    public CartActionType CartActionType { get; set; } = CartActionType.None;

    /// <summary>
    /// Kedvezmény az.
    /// </summary>
    public int? Aid { get; set; } = null!;

    /// <summary>
    /// Kedvezmény név
    /// </summary>
    public string? AidName { get; set; } = null!;


}

