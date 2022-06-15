using System;
using System.IO;
using OfficeOpenXml;

namespace eLog.HeavyTools.ImportBase.Xlsx
{
    public class XlsxWorkbook : IDisposable
    {
        private MemoryStream stream;
        private readonly string fileName;

        public ExcelPackage Package { get; private set; }
        public XlsxWorksheets Sheets { get; private set; }

        //public bool IsChanged => this.sheets?.IsChanged ?? false;

        private XlsxWorkbook(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            this.fileName = fileName;
        }

        public static XlsxWorkbook Open(string fileName)
        {
            var workbook = new XlsxWorkbook(fileName);
            workbook.Open();

            return workbook;
        }

        public static XlsxWorkbook Create(string fileName)
        {
            var workbook = new XlsxWorkbook(fileName);
            workbook.Create();

            return workbook;
        }

        public void Dispose()
        {
            this.Sheets?.Dispose();
            this.Sheets = null;

            this.Package?.Dispose();
            this.Package = null;

            this.stream?.Dispose();
            this.stream = null;
        }

        private void Open()
        {
            if (!File.Exists(this.fileName))
            {
                throw new FileNotFoundException(this.fileName);
            }

            this.stream = new MemoryStream();
            using (var fileStream = File.Open(this.fileName, FileMode.Open))
            {
                fileStream.CopyTo(this.stream);
            }

            this.stream.Seek(0, SeekOrigin.Begin);

            this.InitExcelPackage();
        }

        private void Create()
        {
            this.stream = new MemoryStream();

            if (File.Exists(this.fileName))
            {
                using (var fileStream = File.Open(this.fileName, FileMode.Open))
                {
                    fileStream.CopyTo(this.stream);
                }

                this.stream.Seek(0, SeekOrigin.Begin);
            }

            this.InitExcelPackage();
        }

        private void InitExcelPackage()
        {
            this.Package = new ExcelPackage(this.stream);
            this.Sheets = new XlsxWorksheets(this);

            foreach (var worksheet in this.Package.Workbook.Worksheets)
            {
                this.Sheets.Add(worksheet);
            }
        }

        public void Save(string fileName = null)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = this.fileName;
            }

            using (var fileStream = File.Create(fileName))
            {
                this.Package.SaveAs(fileStream);
                fileStream.Flush();
            }
        }
    }
}
