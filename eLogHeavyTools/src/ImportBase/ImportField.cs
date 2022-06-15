namespace eLog.HeavyTools.ImportBase
{
    [System.Diagnostics.DebuggerDisplay("{Field,nq}")]
    public class ImportField : ImportFieldBase
    {
        public string Column { get; set; }
        public string ColumnName { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public int? ColumnIndex { get; set; }
        public bool Valid { get; set; } = true;
        public bool? Required { get; set; }
        public object Const { get; set; }
        public int? Left { get; set; }
        public string SplitPart { get; set; }
        public bool? DefIfExists { get; set; }
        public ImportSequence Sequence { get; set; }

        public new ImportFieldType? Type
        {
            get
            {
                var type = base.Type;
                if (type.HasValue)
                {
                    return (ImportFieldType)type.Value;
                }

                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    base.Type = (ImportFieldBaseType)value.Value;
                }
                else
                {
                    base.Type = null;
                }
            }
        }
    }

    public enum ImportFieldType
    {
        Unknown = ImportFieldBaseType.Unknown,
        Current = ImportFieldBaseType.Current,
        Company = ImportFieldBaseType.Company,
        Type = ImportFieldBaseType.Type,
        Lookup = ImportFieldBaseType.Lookup,
        SelfLookup = ImportFieldBaseType.SelfLookup,
        FlagType,
        Dictionary,
        Sequence,
        Boolean,
        VirtualID
    }
}
