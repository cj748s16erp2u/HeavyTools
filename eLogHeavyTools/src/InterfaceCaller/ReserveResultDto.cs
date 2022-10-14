using eLog.Base.Common;
using eLog.HeavyTools.Common.Json;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.InterfaceCaller
{
    [Common.Json.JsonObjectAttributes("")]
    public class ReserveResultDto : ErrorMessageDto
    {

        [JsonFieldAttribute(false)]
        public int? Resid { get; set; }
         
    }
}
