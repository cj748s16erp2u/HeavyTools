namespace eLog.HeavyTools.ImportBase
{
    [System.Diagnostics.DebuggerDisplay("{Field,nq}")]
    public class ImportSplit
    {
        public string Name { get; set; }
        public ImportField Field { get; set; }
        public string Separator { get; set; }
        public int? PartsCount { get; set; }
    }
}
