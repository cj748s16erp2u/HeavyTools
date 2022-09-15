using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;

public class PriceCalcOriginalResultDto
{
    public string? Curid { get; set; } = null!;
    public decimal? Prc { get; set; } = null!;
    public bool? IsNet { get; set; } = null!;
    public PrcType? Prctype { get; set; } = null!;
}
