using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;

public class CalcJsonResultDto : ResultDto
{
    public string Curid { get; set; } = null!;

    public CalcItemJsonResultDto[] Items { get; set; } = null!;

}
public class CalcItemJsonResultDto
{
    public string ItemCode { get; set; } = null!;
    public int? Quantity { get; set; } = null!;
    public decimal? OrigSelVal { get; set; } = null!;
     
    /// <summary>
    /// Egység ár
    /// </summary>
    public decimal? SelPrc { get; set; } = null!;
     
    /// <summary>
    /// Bruttó egység ár
    /// </summary>
    public decimal? SetTotPrc { get; set; } = null!;

    /// <summary>
    /// Nettó érték
    /// </summary> 
    public decimal? SelVal { get; set; } = null!;


    /// <summary>
    /// Bruttó érték
    /// </summary>
    public decimal? SelTotVal { get; set; } = null!;

}
