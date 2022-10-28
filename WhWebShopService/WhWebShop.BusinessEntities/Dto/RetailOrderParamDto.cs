using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;


[JsonObjectAttributes("Retail")]

public class RetailOrderParamDto
{
    [JsonField(MandotaryType.Yes)]
    public string? Addusrid { get; set; } = null!;

}
