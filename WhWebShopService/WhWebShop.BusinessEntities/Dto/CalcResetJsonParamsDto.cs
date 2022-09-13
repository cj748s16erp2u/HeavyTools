using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;

[JsonObjectAttributes("Reset")] 
public class CalcResetJsonParamsDto
{
    [JsonField(MandotaryType.No)]
    public int? Aid { get; set; } = null!;
}
