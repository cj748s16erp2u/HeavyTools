namespace eLog.HeavyTools.ImportBase
{
    public interface IImportExcelColumn
    {
        string Column { get; set; }
        int? ColumnIndex { get; set; }
        string ColumnName { get; set; }
        bool Valid { get; set; }
    }
}