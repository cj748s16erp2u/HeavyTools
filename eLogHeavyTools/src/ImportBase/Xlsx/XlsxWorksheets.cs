using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;

namespace eLog.HeavyTools.ImportBase.Xlsx
{
    public class XlsxWorksheets : IEnumerable<XlsxWorksheet>, IDisposable
    {
        private XlsxWorkbook workbook;

        private IList<XlsxWorksheet> worksheets = new List<XlsxWorksheet>();

        public XlsxWorksheet this[int index] => this.worksheets?[index];
        public XlsxWorksheet this[string name] => this.worksheets?.FirstOrDefault(ws => string.Equals(ws.Name, name, StringComparison.InvariantCultureIgnoreCase));
        public int Count => this.worksheets?.Count ?? 0;
        public bool IsChanged => this.worksheets?.Any(ws => ws.IsChanged) ?? false;

        public XlsxWorksheets(XlsxWorkbook workbook)
        {
            this.workbook = workbook ?? throw new ArgumentNullException(nameof(workbook));
        }

        public IEnumerator<XlsxWorksheet> GetEnumerator()
        {
            return this.worksheets.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.worksheets).GetEnumerator();
        }

        public XlsxWorksheet Add(ExcelWorksheet worksheet)
        {
            var xlsxWorksheet = new XlsxWorksheet(worksheet);
            this.worksheets.Add(xlsxWorksheet);
            return xlsxWorksheet;
        }

        public XlsxWorksheet Add(string name)
        {
            var worksheet = this.workbook.Package.Workbook.Worksheets.Add(name);
            return this.Add(worksheet);
        }

        public void Dispose()
        {
            if (this.worksheets != null)
            {
                foreach (var worksheet in this.worksheets)
                {
                    worksheet.Dispose();
                }

                this.worksheets.Clear();
                this.worksheets = null;
            }

            this.workbook = null;
        }
    }
}
