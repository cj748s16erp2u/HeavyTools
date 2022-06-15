using System;
using System.Collections.Generic;
using System.Text;
using eLog.HeavyTools.ImportBase.Xlsx;

namespace eLog.HeavyTools.ImportBase
{
    public class RowContextBase : IDisposable
    {
        private StringBuilder logText = new StringBuilder();

        public int Row { get; internal set; }
        public int? LogCol { get; internal set; }
        public XlsxWorksheet Sheet { get; internal set; }
        public ImportTable CurrentTable { get; internal set; }
        public ImportField CurrentField { get; internal set; }

        public List<FieldValue> Sequences { get; internal set; }
        public List<SplitValue> Splits { get; internal set; }

        public int VirtualID { get; internal set; } = 0;
        public TableEntry CurrentEntry { get; internal set; }

        public void AddLogText(string text)
        {
            string alias = null;
            if (this.CurrentTable != null)
            {
                alias = this.CurrentTable.Alias;
                if (!string.IsNullOrWhiteSpace(alias))
                {
                    alias = $"[{this.CurrentTable.Table} ({alias})]";
                }

                alias = $"{alias}: ";
            }

            this.logText.Append(alias);
            this.logText.AppendLine(text);
        }

        public string LogText => this.logText?.ToString();

        public virtual void Dispose()
        {
            this.Sheet = null;
            this.CurrentField = null;
            this.CurrentTable = null;
            this.CurrentEntry = null;

            this.Splits?.Clear();
            this.Splits = null;

            this.Sequences?.Clear();
            this.Sequences = null;

            this.logText = null;
        }
    }
}
