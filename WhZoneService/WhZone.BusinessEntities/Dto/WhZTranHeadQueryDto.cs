using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;

public class WhZTranHeadQueryDto
{
    public int? Cmpid { get; set; } = null!;
    public DateTime? Fromdate { get; set; } = null!;
    public DateTime? Todate { get; set; } = null!;
    public int? Fromwhzid { get; set; } = null!;
    public int? Towhzid { get; set; } = null!;
    public int? Stid { get; set; } = null!;
}
