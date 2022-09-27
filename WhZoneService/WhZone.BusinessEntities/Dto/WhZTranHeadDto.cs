using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Enums;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;

public abstract class WhZTranHeadDto : Base.EntityDto
{
    public int? Whztid { get; set; } = null!;
    public int Cmpid { get; set; }
    public virtual WhZTranHead_Whzttype Whzttype { get; }
    public DateTime Whztdate { get; set; }
    //public int? Towhzid { get; set; } = null!;
    public string? Closeusrid { get; set; } = null!; 
    public DateTime? Closedate { get; set; } = null!;
    public int? Whztstat { get; set; } = null!;
    public string? Note { get; set; } = null!;
}
