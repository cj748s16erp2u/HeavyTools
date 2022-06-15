using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.ImportBase;
using Newtonsoft.Json;

namespace eLog.HeavyTools.Masters.Partner.Import
{
    [System.Diagnostics.DebuggerStepThrough]
    public class ImportConditionalConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(ImportConditional) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize<PartnerImportConditional>(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
