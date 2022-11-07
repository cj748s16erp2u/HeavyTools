using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Enums;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;

public abstract class WhZTranHeadDto : Base.EntityDto
{
    public int? Whztid { get; set; }
    public int Cmpid { get; set; }
    public virtual WhZTranHead_Whzttype Whzttype { get; }
    public DateTime Whztdate { get; set; }
    //public int? Towhzid { get; set; } = null!;
    public string? Closeusrid { get; set; }
    public DateTime? Closedate { get; set; }
    public WhZTranHead_Whztstat? Whztstat { get; set; }
    public string? Note { get; set; }

    public string AuthUser { get; set; } = null!;
}
