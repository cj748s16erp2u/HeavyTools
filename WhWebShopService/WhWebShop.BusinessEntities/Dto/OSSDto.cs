using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;

public class OSSParamsDto
{
    public string? CoundtyId { get; set; } = null!;
}


public class OSSResultDto : ResultDto
{
    public string? Bustypeid { get; set; } = null!;
    public string? Taxid { get; set; } = null!;
}
