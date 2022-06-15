namespace eLog.HeavyTools.ImportBase
{
    public class ProcessResult
    {
        public int TotalRows { get; set; } = 0;
        public int ProcessedRows { get; set; } = 0;
        public int SavedRows { get; set; } = 0;
        
        public string LogFileName { get; set; }
        public string ErrorFileName { get; set; }
        public string LogXlsxFileName { get; set; }
    }
}
