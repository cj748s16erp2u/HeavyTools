using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.ImportBase
{
    public class ImportExcelColumn : IImportExcelColumn
    {
        public string Column { get; set; }
        public string ColumnName { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public int? ColumnIndex { get; set; }
        public bool Valid { get; set; } = true;
    }
}
