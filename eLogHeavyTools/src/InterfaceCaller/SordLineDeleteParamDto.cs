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
    public class SordLineDeleteParamDto
    {
        public int? Sordlineid { get; set; }
    }

    [Common.Json.JsonObjectAttributes("")]
    public class SordLineDeleteResultDto : ErrorMessageDto
    {
        [JsonFieldAttribute(false)]
        public int? Sordlineid { get; set; }
    }

}
