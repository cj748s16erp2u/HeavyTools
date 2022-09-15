using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;

public class CalcResultDto
{
    public int? Id { get; set; }
    public int Result { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
}
