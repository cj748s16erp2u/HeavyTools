using System;
using OfficeOpenXml;

namespace eLog.HeavyTools.ImportBase.Xlsx
{
    public class XlsxWorksheet : IDisposable
    {
        private ExcelWorksheet worksheet;
        private bool isChanged;

        public string Name => this.worksheet?.Name;
        public bool IsChanged => this.isChanged;

        public XlsxWorksheet(ExcelWorksheet worksheet)
        {
            if (worksheet == null)
            {
                throw new ArgumentNullException(nameof(worksheet));
            }

            this.worksheet = worksheet;
        }

        public ExcelRange Cells(string address)
        {
            return this.worksheet.Cells[address];
        }

        public ExcelRange Cells(int fromRow, int fromCol)
        {
            return this.worksheet.Cells[fromRow, fromCol];
        }

        public ExcelRange Cells(int fromRow, int fromCol, int toRow, int toCol)
        {
            return this.worksheet.Cells[fromRow, fromCol, toRow, toCol];
        }

        public object GetCellValue(int row, string col)
        {
            if (this.worksheet == null)
            {
                return null;
            }

            var colIndex = GetColumnIndex(col);
            return this.GetCellValue(row, colIndex);
        }

        public object GetCellValue(int row, int colIndex)
        {
            if (this.worksheet == null)
            {
                return null;
            }

            return this.worksheet.GetValue(row, colIndex);
        }

        public T GetCellValue<T>(int row, string col)
        {
            if (this.worksheet == null)
            {
                return default(T);
            }

            var colIndex = GetColumnIndex(col);
            return this.GetCellValue<T>(row, colIndex);
        }

        public T GetCellValue<T>(int row, int colIndex)
        {
            if (this.worksheet == null)
            {
                return default(T);
            }

            return this.worksheet.GetValue<T>(row, colIndex);
        }

        public ExcelRange CopyCell(int row, int col, ExcelRange cell)
        {
            var destCell = this.worksheet.Cells[row, col];
            cell.Copy(destCell);
            return destCell;
        }

        public void AddColumn(int row, string col, string name)
        {
            var colIndex = GetColumnIndex(col);

            this.worksheet.InsertColumn(colIndex, 1);
            this.worksheet.SetValue(row, colIndex, name);

            this.isChanged = true;
        }

        public ExcelColumn Column(int col)
        {
            return this.worksheet.Column(col);
        }

        public ExcelRow Row(int row)
        {
            return this.worksheet.Row(row);
        }

        public int RowCount => this.worksheet?.Dimension.End.Row ?? -1;

        const char StartChar = 'A';
        const char EndChar = 'Z';

        public static string GetColumnName(int index)
        {
            var range = (byte)EndChar - (byte)StartChar + 1;
            var prefix = index / range;
            var current = index % range;

            var result = ((char)((byte)StartChar + current)).ToString();
            if (prefix > 0)
            {
                result = GetColumnName(prefix - 1) + result;
            }

            return result;
        }

        public static int GetColumnIndex(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return 0;
            }

            var chr = name.Substring(name.Length - 1).ToUpperInvariant()[0];

            if (chr < StartChar || chr > EndChar)
            {
                return -1;
            }

            var current = chr - StartChar + 1;
            if (name.Length > 1)
            {
                var prefix = GetColumnIndex(name.Substring(0, name.Length - 1));
                var range = (byte)EndChar - (byte)StartChar + 1;
                prefix *= range;
                current += prefix;
            }

            return current;
        }

        public void Dispose()
        {
            this.worksheet?.Dispose();
            this.worksheet = null;
        }

        public static implicit operator ExcelWorksheet(XlsxWorksheet ws) => ws.worksheet;
    }
}
