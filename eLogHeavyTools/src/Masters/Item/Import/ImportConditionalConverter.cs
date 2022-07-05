using eLog.HeavyTools.ImportBase;
using Newtonsoft.Json;
using System;

namespace eLog.HeavyTools.Masters.Item.Import
{
    [System.Diagnostics.DebuggerStepThrough]
    class ImportConditionalConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(ImportConditional) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize<ItemImportConditional>(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
