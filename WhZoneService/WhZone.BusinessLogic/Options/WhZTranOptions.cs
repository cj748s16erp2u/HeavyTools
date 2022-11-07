using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Options;

public class WhZTranOptions
{
    public const string NAME = "WhZTran";

    public bool? AllowDirectClose { get; set; }
    public string? DefaultReceivingLocCode { get; set; }
}
