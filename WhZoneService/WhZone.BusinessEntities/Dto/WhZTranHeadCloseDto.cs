using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;

public class WhZTranHeadCloseDto
{
    public int? Whztid { get; set; }
    public int? Stid { get; set; }

    public string AuthUser { get; set; } = null!;
}
