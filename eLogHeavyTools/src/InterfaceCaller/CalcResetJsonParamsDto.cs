using eLog.HeavyTools.Common.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.InterfaceCaller
{
    [JsonObjectAttributes("Reset")]
    public class CalcResetJsonParamsDto
    {
        [JsonField(false)]
        public int? Aid { get; set; } = null;
    }
}
