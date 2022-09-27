using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;

public class OrderParamsDto
{
    public string Content { get; set; } = null!;
}

public class OrderResultDto : ResultDto
{
    public int? Sordid { get; set; } = null!;
}
