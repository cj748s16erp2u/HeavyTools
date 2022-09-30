using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Enums;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;

public class WhZReceivingTranHeadDto : WhZTranHeadDto
{
    public int? Stid { get; set; }
    public int? Towhzid { get; set; }
    public override WhZTranHead_Whzttype Whzttype => WhZTranHead_Whzttype.Receiving;
}
