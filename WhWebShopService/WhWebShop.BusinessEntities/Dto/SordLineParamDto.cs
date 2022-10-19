using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto.Attributes;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;

[JsonObjectAttributes("SordLine")]
public class SordLineParamDto
{
    [JsonField(MandotaryType.No)]
    public int? Sordlineid { get; set; } = null!;

}
