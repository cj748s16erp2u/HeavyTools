using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;

public class WhZTranLineDeleteDto
{
    public int? Whztlineid { get; set; }
    public int? Stlineid { get; set; }
    public bool? DeleteLoc { get; set; }
}
