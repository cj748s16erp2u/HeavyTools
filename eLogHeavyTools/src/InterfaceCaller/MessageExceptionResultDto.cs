using eLog.HeavyTools.Common.Json;

namespace eLog.HeavyTools.InterfaceCaller
{
    public class MessageExceptionResultDto
    {
        [JsonFieldAttribute(false)]
        public string Id { get; set; }

        [JsonFieldAttribute(false)]
        public string NameSpace { get; set; }


        [JsonFieldAttribute(false)]
        public object[] Param { get; set; }


        [JsonFieldAttribute(false)]
        public string ErrKey { get; internal set; }
    }
}
