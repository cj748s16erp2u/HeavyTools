using eLog.HeavyTools.Common.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.InterfaceCaller
{

    [Common.Json.JsonObjectAttributes("")]
    public class RetailOrderResultDto
    {
        [JsonFieldAttribute(true)]
        public int? RetaulCount { get; set; } = null;
    }
}
