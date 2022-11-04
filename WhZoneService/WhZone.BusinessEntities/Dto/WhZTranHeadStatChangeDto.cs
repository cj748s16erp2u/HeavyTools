using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Enums;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;

public class WhZTranHeadStatChangeDto : Base.EntityDto
{
    public int? Whztid { get; set; }
    public int? Stid { get; set; }
    public WhZTranHead_Whztstat NewStat { get; set; }

    public string AuthUser { get; set; } = null!;
}
