using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;

public class WhZTranHeadDeleteDto
{
    public int? Whztid { get; set; }
    public int? Stid { get; set; }
    public bool? DeleteLine { get; set; }
}
