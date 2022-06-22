namespace eLog.HeavyTools.ImportBase
{
    [System.Diagnostics.DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class ImportConditional : IImportExcelColumn
    {
        public string Column { get; set; }
        public string ColumnName { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public int? ColumnIndex { get; set; }
        public bool Valid { get; set; } = true;
        public string Field { get; set; }
        public ImportConditionalType? Type { get; set; }
        public string Expression { get; set; }

        private string DebuggerDisplay => this.ColumnName ?? this.Column ?? this.Field;
    }

    public enum ImportConditionalType
    {
        Unknown = 0,
        IsNotEmpty = 1,
        Expression = 2
    }
}
