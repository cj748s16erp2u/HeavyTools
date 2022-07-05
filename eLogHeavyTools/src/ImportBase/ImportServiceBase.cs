using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using eLog.HeavyTools.ImportBase.Xlsx;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Extensions;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace eLog.HeavyTools.ImportBase
{
    public abstract class ImportServiceBase<TResultSet, TResultSets, TRowContext>
        where TRowContext : RowContextBase, new()
        where TResultSet : ImportResultSetBase, new()
        where TResultSets : ImportResultSetsBase<TResultSet>, new()
    {
        protected Logger logger;

        protected Dictionary<string, object> lookupCache = new Dictionary<string, object>();
        protected Dictionary<string, DictionaryProxy> dictionaryCache = new Dictionary<string, DictionaryProxy>();
        private string importDescrFileName;
        private string importXlsxFileName;

        protected ImportServiceBase()
        {
        }

        public abstract ProcessResult Import(string importDescrFileName, string importXlsxFileName);

        protected ProcessResult Import(string prefix, string importDescrFileName, string importXlsxFileName)
        {
            this.importDescrFileName = importDescrFileName;
            this.importXlsxFileName = importXlsxFileName;

            var directory = Path.Combine(Globals.LogFolder, "Import");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var processResult = new ProcessResult();

            using (this.logger = new Logger(directory, prefix))
            {
                this.logger.LogLine($"Executing at {DateTime.Now:yyyy.MM.dd. HH:mm:ss} by {Session.UserID}");
                this.logger.LogLine($"Current directory: {directory}");

                this.Initialize();

                var importDescr = this.GetImportDescr();
                var fileName = this.GetXlsxFileName();

                var logXlsxFileName = this.CreateLogXlsxFileName(directory, fileName, prefix);

                processResult.LogFileName = this.logger.LogFile;
                processResult.ErrorFileName = this.logger.ErrorFile;
                processResult.LogXlsxFileName = logXlsxFileName;

                // TODO
                //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                using (var logWorkbook = this.CreateLogExcel(logXlsxFileName))
                using (var workbook = this.OpenExcel(fileName))
                {
                    foreach (var sheet in importDescr.Where(id => id.Ignore != true))
                    {
                        var worksheet = workbook.Sheets[sheet.Sheet];
                        if (worksheet == null)
                        {
                            this.logger.LogLine($"Excel missing sheet: {sheet.Sheet}");
                            continue;
                        }

                        this.logger.LogLine($"Processing sheet: {sheet.Sheet}");
                        var logWorksheet = logWorkbook.Sheets.FirstOrDefault(s => string.Equals(s.Name, worksheet.Name, StringComparison.OrdinalIgnoreCase));
                        if (logWorksheet == null)
                        {
                            logWorksheet = logWorkbook.Sheets.Add(worksheet.Name);
                        }

                        var results = this.ProcessWorksheet(worksheet, sheet, logWorksheet);

                        this.logger.LogLine($"  ... {results.ProcessedRows} of {results.TotalRows} rows processed");
                        this.logger.LogLine($"Saving sheet: {sheet.Sheet}");

                        var success = this.SaveImport(results);

                        this.WriteLogTextToXlsx(logWorksheet, results);

                        processResult.TotalRows += results.TotalRows.GetValueOrDefault(0);
                        processResult.ProcessedRows += results.ProcessedRows.GetValueOrDefault(0);
                        processResult.SavedRows += success;
                    }

                    logWorkbook.Save();
                }
            }

            return processResult;
        }

        protected virtual IEnumerable<ImportSheet> GetImportDescr()
        {
            var impDescrFileName = this.GetImportDescrFileName();
            var descrText = File.ReadAllText(impDescrFileName);
            return this.ParseImportDescr(descrText);
        }

        protected virtual IEnumerable<ImportSheet> ParseImportDescr(string descrText, params JsonConverter[] converters)
        {
            if (converters == null)
            {
                converters = new JsonConverter[0];
            }

            return JsonConvert.DeserializeObject<IEnumerable<ImportSheet>>(descrText, converters);
        }

        private string GetImportDescrFileName()
        {
            var fileName = this.importDescrFileName;
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new FileNotFoundException("Import Description's FileName is not set.");
            }

            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException($"Import Description's File not found: {fileName}");
            }

            this.logger?.LogLine($"Using import descriptions: {fileName}");

            return fileName;
        }

        protected string CreateLogXlsxFileName(string directory, string fileName, string prefix)
        {
            fileName = $"{prefix}_{Path.GetFileNameWithoutExtension(fileName)}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(fileName)}";
            fileName = Path.Combine(directory, fileName);
            return fileName;
        }

        protected string GetXlsxFileName()
        {
            var fileName = this.importXlsxFileName;
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new FileNotFoundException("FileName is not set.");
            }

            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException($"File not found: {fileName}");
            }

            this.logger?.LogLine($"Processing file: {fileName}");

            return fileName;
        }

        protected virtual void Initialize()
        {
        }

        protected void WriteLogTextToXlsx(XlsxWorksheet logWorksheet, TResultSets results)
        {
            if (results.LogColIndex != null && results.FirstHeaderRow != null)
            {
                logWorksheet.Cells(results.FirstHeaderRow.Value, results.LogColIndex.Value).Value = "$import_importresult".eLogTransl();

                foreach (var r in results)
                {
                    var logText = r.LogText;
                    if (string.IsNullOrWhiteSpace(logText))
                    {
                        logText = "$import_importresult_ok".eLogTransl();
                    }

                    logWorksheet.Cells(r.Row, r.LogCol.Value).Value = logText;
                }
            }
        }

        protected XlsxWorkbook CreateLogExcel(string fileName)
        {
            return XlsxWorkbook.Create(fileName);
        }

        protected XlsxWorkbook OpenExcel(string fileName)
        {
            return XlsxWorkbook.Open(fileName);
        }

        private TResultSets ProcessWorksheet(XlsxWorksheet worksheet, ImportSheet sheet, XlsxWorksheet logWorksheet)
        {
            this.ResolveColumnNames(worksheet, sheet);

            this.CreateDictionaries(sheet);

            int? firstHeaderRow = null;
            var firstDataRow = 1;
            if (!string.IsNullOrWhiteSpace(sheet.HeaderRow))
            {
                var colIndexes = sheet.HeaderRow.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(hr => Convert.ToInt32(hr));
                firstHeaderRow = colIndexes.Min();
                firstDataRow = colIndexes.Max() + 1;
            }

            var logColIndex = this.CopyHeaderRowToLog(worksheet, logWorksheet, firstHeaderRow, firstDataRow - 1);

            var totalRows = worksheet.RowCount - firstDataRow;
            var resultSets = new TResultSets
            {
                TotalRows = totalRows,
                FirstHeaderRow = firstHeaderRow,
                LastHeaderRow = firstDataRow - 1,
                LogColIndex = logColIndex,
            };

            for (var i = firstDataRow; i <= worksheet.RowCount; i++)
            {
                using (var rowContext = new TRowContext
                {
                    Sheet = worksheet,
                    Row = i,
                    LogCol = logColIndex
                })
                {
                    this.CopyRowToLog(worksheet, logWorksheet, i, maxColCount: logColIndex.GetValueOrDefault() - 1);

                    var procResult = true;
                    foreach (var table in sheet.Tables)
                    {
                        rowContext.CurrentTable = table;
                        procResult = this.ProcessTable(rowContext);
                        if (!procResult)
                        {
                            break;
                        }
                    }

                    if (procResult)
                    {
                        var result = this.CreateResultSet(rowContext);
                        resultSets.Add(result);
                    }
                }
            }

            resultSets.ProcessedRows = resultSets.Count();

            return resultSets;
        }

        protected abstract int SaveImport(TResultSets results);

        protected abstract TResultSet CreateResultSet(TRowContext rowContext);

        protected virtual void ResolveColumnNames(XlsxWorksheet worksheet, ImportSheet sheet)
        {
            if (sheet.ColumnNameRow != null)
            {
                /* nem lehet validálni, mert akkor 1 oszlop adatát csak 1x lehet felhasználni
                var duplicatedColumnNames = sheet.Tables
                    .SelectMany(t => t.Fields.Where(f => !string.IsNullOrWhiteSpace(f.ColumnName)))
                    .GroupBy(g => g.ColumnName, StringComparer.InvariantCultureIgnoreCase)
                    .Where(x => x.Count() > 1)
                    .Select(x => x.Key)
                    .ToList();

                var errorMsg = string.Join(", ", duplicatedColumnNames.Select(c => $"'{c}'"));
                if (!string.IsNullOrWhiteSpace(errorMsg))
                {
                    throw new InvalidDataException("$import_columnname_not_unique".eLogTransl(errorMsg));
                }
                */

                var pattern = $"^([A-Za-z]+){sheet.ColumnNameRow}$";
                var cells = worksheet.Cells($"{sheet.ColumnNameRow}:{sheet.ColumnNameRow}");
                var cellObjs = cells
                    .Select(c => new { value = c.GetValue<string>(), address = Regex.Replace(c.Address, pattern, "$1") })
                    .Where(c => !string.IsNullOrWhiteSpace(c.value))
                    .Select(c => new Tuple<string, string>(c.value, c.address))
                    .ToList();

                foreach (var table in sheet.Tables)
                {
                    this.ResolveColumnNames(worksheet, cellObjs, table);
                }
            }
        }

        protected virtual void ResolveColumnNames(XlsxWorksheet worksheet, IEnumerable<Tuple<string, string>> cellObjs, ImportTable table)
        {
            foreach (var field in table.Fields)
            {
                this.ResolveColumnNames(worksheet, cellObjs, field);
            }

            if (table.Conditionals != null)
            {
                foreach (var cond in table.Conditionals)
                {
                    this.ResolveColumnNames(worksheet, cellObjs, cond);
                }
            }

            if (table.Splits != null)
            {
                foreach (var split in table.Splits)
                {
                    this.ResolveColumnNames(worksheet, cellObjs, split.Field);
                }
            }
        }

        protected virtual void ResolveColumnNames(XlsxWorksheet worksheet, IEnumerable<Tuple<string, string>> cellObjs, ImportConditional cond)
        {
            this.ResolveColumnNames(worksheet, cellObjs, (IImportExcelColumn)cond);

            if (!string.IsNullOrWhiteSpace(cond.Field))
            {
                cond.Valid = true;
            }
        }

        protected virtual void ResolveColumnNames(XlsxWorksheet worksheet, IEnumerable<Tuple<string, string>> cellObjs, IImportExcelColumn column)
        {
            if (string.IsNullOrWhiteSpace(column.Column) && !string.IsNullOrWhiteSpace(column.ColumnName))
            {
                var obj = cellObjs.FirstOrDefault(c => string.Equals(c.Item1, column.ColumnName, StringComparison.InvariantCultureIgnoreCase));
                column.Column = obj?.Item2;
                column.Valid = !string.IsNullOrWhiteSpace(column.Column);
            }

            if (!string.IsNullOrWhiteSpace(column.Column))
            {
                column.ColumnIndex = XlsxWorksheet.GetColumnIndex(column.Column);
            }

            if (column is ImportField field && field.Columns?.Any() == true)
            {
                foreach (var c in field.Columns)
                {
                    this.ResolveColumnNames(worksheet, cellObjs, c);
                }
            }
        }

        private int? CopyHeaderRowToLog(XlsxWorksheet worksheet, XlsxWorksheet logWorksheet, int? firstHeaderRow, int lastHeaderRow)
        {
            if (firstHeaderRow != null && firstHeaderRow <= lastHeaderRow)
            {
                var lastValuableCell = 0;
                var cells = worksheet.Cells($"{firstHeaderRow.Value}:{lastHeaderRow}");
                var start = cells.Start.Row;
                var end = cells.End.Row;
                for (var i = start; i <= end; i++)
                {
                    var valuableCell = this.CopyRowToLog(worksheet, logWorksheet, i, i == 1);
                    if (lastValuableCell < valuableCell)
                    {
                        lastValuableCell = valuableCell;
                    }

                    logWorksheet.Row(i).Height = worksheet.Row(i).Height;
                }

                return lastValuableCell + 1;
            }

            return null;
        }

        private int CopyRowToLog(XlsxWorksheet worksheet, XlsxWorksheet logWorksheet, int row, bool setColWidth = false, int maxColCount = 0)
        {
            var noMaxColCount = maxColCount <= 0;

            var lastValuableCell = 0;
            var cells = worksheet.Cells($"{row}:{row}");
            var start = cells.Start.Column;
            var end = cells.End.Column;
            if (noMaxColCount)
            {
                maxColCount = end;
            }

            var emptyCell = 0;
            for (var i = start; i <= end && i <= maxColCount; i++)
            {
                var cell = cells[row, i];
                var destCell = logWorksheet.CopyCell(row, i, cell);
                if (string.IsNullOrWhiteSpace(Convert.ToString(destCell.Value)) &&
                    string.IsNullOrWhiteSpace(destCell.Formula))
                {
                    emptyCell++;
                }
                else
                {
                    emptyCell = 0;

                    if (i > lastValuableCell)
                    {
                        lastValuableCell = i;
                    }
                }

                if (setColWidth)
                {
                    logWorksheet.Column(i).Width = worksheet.Column(i).Width;
                }

                if (noMaxColCount && emptyCell >= 5)
                {
                    maxColCount = lastValuableCell;
                }
            }

            return lastValuableCell;
        }

        private void CreateDictionaries(ImportSheet sheet)
        {
            var dictDescrs = sheet.Dictionaries;
            if (dictDescrs?.Any() == true)
            {
                foreach (var dictDescr in dictDescrs)
                {
                    this.CreateDictionary(dictDescr);
                }
            }
        }

        private void CreateDictionary(ImportDictionary dictDescr)
        {
            var key = $"CUST|{dictDescr.Name}".ToUpperInvariant();
            if (dictionaryCache.ContainsKey(key))
            {
                return;
            }

            var sql = new StringBuilder();
            sql.Append($"select [t].[{dictDescr.KeyField}]");
            foreach (var f in dictDescr.ValueFields)
            {
                sql.Append($", [t].[{f.Field}]");
            }

            sql.AppendLine();
            sql.Append($"from [{dictDescr.Table}] [t] (nolock)");

            var list = new DictionaryProxy(dictDescr);
            Base.Common.SqlFunctions.QueryData(DB.Main, sql.ToString(), r =>
            {
                var values = new object[r.FieldCount];
                r.GetValues(values);
                list.Add(values);
            });

            foreach (var i in list)
            {
                foreach (var f in dictDescr.ValueFields)
                {
                    if (f.Lookup != null)
                    {
                        i[f.Field] = this.CreateDictionaryValue(f.Type, i[f.Field], f.Lookup);
                    }
                }
            }

            dictionaryCache[key] = list;
        }

        private object CreateDictionaryValue(ImportFieldBaseType? type, object value, ImportLookup lookup)
        {
            if (string.Equals(ConvertUtils.ToString(value), "#N/A", StringComparison.InvariantCultureIgnoreCase) ||
                value == null || string.IsNullOrWhiteSpace(ConvertUtils.ToString(value)))
            {
                return null;
            }

            switch (type)
            {
                case ImportFieldBaseType.Company:
                    return this.DetermineCompanyValue(value, lookup);
                case ImportFieldBaseType.Type:
                    return this.DetermineTypeValue(value, lookup);
                case ImportFieldBaseType.Lookup:
                    return this.DetermineLookupValue(value, lookup);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, nameof(this.CreateDictionaryValue));
            }
        }

        private bool ProcessTable(TRowContext rowContext)
        {
            rowContext.Sequences = new List<FieldValue>();
            rowContext.Splits = new List<SplitValue>();

            this.ProcessSplits(rowContext);

            this.CreateEntity(rowContext);
            this.ProcessFields(rowContext);
            if (rowContext.CurrentEntry.Valid)
            {
                var condResult = this.ProcessConditionals(rowContext);
                if (!condResult)
                {
                    this.RemoveEntity(rowContext);
                    if (rowContext.CurrentTable.Optional != true)
                    {
                        return false;
                    }
                }
                else if (rowContext.Sequences.Count > 0)
                {
                    this.ApplySequences(rowContext);
                }

                return true;
            }
            else
            {
                this.RemoveEntity(rowContext);
                return false;
            }
        }

        protected abstract void CreateEntity(TRowContext rowContext);

        protected abstract void RemoveEntity(TRowContext rowContext);

        private void ProcessSplits(TRowContext rowContext)
        {
            if (rowContext.CurrentTable.Splits?.Any() == true)
            {
                foreach (var split in rowContext.CurrentTable.Splits)
                {
                    var count = split.PartsCount.GetValueOrDefault(1);
                    if (count < 1)
                    {
                        count = 1;
                    }

                    rowContext.CurrentField = split.Field;
                    var value = this.DetermineFieldValue(rowContext);
                    if (value?.Valid == true && value.Value != null)
                    {
                        if (!(value.Value is string))
                        {
                            value.Value = ConvertUtils.ToString(value.Value);
                        }

                        var text = (string)value.Value;
                        var sep = split.Separator;
                        if (string.IsNullOrEmpty(sep))
                        {
                            sep = " ";
                        }

                        var parts = text.Split(sep.ToCharArray(), count, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length < count)
                        {
                            // ha kevesebb adatot tartalmaz, mint az elvart, akkor a maradekot null-al toltjuk fel
                            var temp = new string[count];
                            Array.Copy(parts, temp, parts.Length);
                            parts = temp;
                        }

                        rowContext.Splits.Add(new SplitValue
                        {
                            Split = split,
                            Value = parts,
                            Valid = true
                        });
                    }
                    else
                    {
                        rowContext.Splits.Add(new SplitValue
                        {
                            Split = split,
                            Valid = false
                        });
                    }
                }
            }
        }

        private void ProcessFields(TRowContext rowContext)
        {
            foreach (var field in rowContext.CurrentTable.ValidFields)
            {
                rowContext.CurrentField = field;
                var value = this.DetermineFieldValue(rowContext);
                if (value != null)
                {
                    if (value.Valid)
                    {
                        string xmlpath = null;
                        var fld = value.Field.Field;
                        if (!string.IsNullOrWhiteSpace(fld))
                        {
                            var fldparts = fld.Split(new[] { ':' }, 3);
                            if (fldparts.Length == 3 && string.Equals(fldparts[0], "xml", StringComparison.InvariantCultureIgnoreCase))
                            {
                                fld = fldparts[1];
                                xmlpath = fldparts[2];
                            }
                        }

                        if (string.IsNullOrWhiteSpace(xmlpath))
                        {
                            if (rowContext.CurrentEntry.Schema.Fields.Any(f => string.Equals(f.Name, value.Field.Field)))
                            {

                                if (value.Value != null)
                                {
                                    var fieldType = rowContext.CurrentEntry.Schema.Fields[value.Field.Field].ClassType;
                                    if (fieldType == typeof(StringN))
                                    {
                                        fieldType = typeof(string);
                                    }

                                    try
                                    {
                                        var val = Convert.ChangeType(value.Value, fieldType);
                                        rowContext.CurrentEntry.Entity[value.Field.Field] = val;
                                    }
                                    catch (Exception ex)
                                    {
                                        var message = "$import_invalidfieldvalue".eLogTransl(rowContext.CurrentTable.Table, value.Field.Field, value.Value, ex.Message);
                                        this.logger.LogErrorLine(message);
                                        rowContext.AddLogText(message);

                                        rowContext.CurrentEntry.Valid = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    rowContext.CurrentEntry.Entity[value.Field.Field] = null;
                                }
                            }
                            else
                            {
                                var message = "$import_invalidfield".eLogTransl(rowContext.CurrentTable.Table, value.Field.Field);
                                this.logger.LogErrorLine(message);
                                rowContext.AddLogText(message);

                                rowContext.CurrentEntry.Valid = false;
                                break;
                            }
                        }
                        else
                        {
                            var xmlParts = xmlpath.Split(new[] { '/' }, 2);
                            var x = new eProjectWeb.Framework.Xml.XmlManiputeStr(() => new StringN(rowContext.CurrentEntry.Entity[fld]), (v) => rowContext.CurrentEntry.Entity[fld] = v, xmlParts[0]);
                            x.SetOrRemove(xmlParts[1], value.Value);
                        }

                        if (field.DefIfExists == true)
                        {
                            rowContext.CurrentEntry.DefaultIfExists.Add(field);
                        }
                    }
                    else
                    {
                        var message = "$import_invalidfieldvalue".eLogTransl(rowContext.CurrentTable.Table, value.Field.Field, value.Value, string.Empty);
                        rowContext.AddLogText(message);

                        rowContext.CurrentEntry.Valid = false;
                        break;
                    }
                }
                else if (rowContext.CurrentField.Required == true)
                {
                    var message = "$import_missingfieldvalue".eLogTransl(rowContext.CurrentTable.Table, value.Field.Field);
                    rowContext.AddLogText(message);

                    rowContext.CurrentEntry.Valid = false;
                    break;
                }
            }
        }

        private bool ProcessConditionals(TRowContext rowContext)
        {
            var result = true;
            if (rowContext.CurrentTable.ValidConditionals?.Any() ?? false)
            {
                foreach (var cond in rowContext.CurrentTable.ValidConditionals)
                {
                    object value = null;
                    if (!string.IsNullOrWhiteSpace(cond.Field))
                    {
                        value = rowContext.CurrentEntry.Entity[cond.Field];
                    }
                    else if (cond.ColumnIndex != null)
                    {
                        value = rowContext.Sheet.GetCellValue(rowContext.Row, cond.ColumnIndex.Value);
                        if (string.IsNullOrWhiteSpace(ConvertUtils.ToString(value)))
                        {
                            value = null;
                        }
                    }

                    switch (cond.Type)
                    {
                        case ImportConditionalType.IsNotEmpty:
                            result = !string.IsNullOrWhiteSpace(ConvertUtils.ToString(value));
                            break;
                        case ImportConditionalType.Expression:
                            result = this.ProcessExpressionConditional(Convert.ToString(value), cond.Expression);
                            break;
                        default:
                            result = this.ProcessCustomConditionals(rowContext, cond, value);
                            break;
                    }

                    if (!result)
                    {
                        return result;
                    }
                }
            }

            return result;
        }

        protected virtual bool ProcessCustomConditionals(TRowContext rowContext, ImportConditional cond, object value)
        {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
            throw new ArgumentOutOfRangeException(nameof(cond.Type), cond.Type, nameof(this.ProcessConditionals));
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
        }

        private bool ProcessExpressionConditional(string value, string expression)
        {
            var regex = new Regex(expression, RegexOptions.IgnoreCase);
            return regex.IsMatch(value);
        }

        private void ApplySequences(TRowContext rowContext)
        {
            var sequence = rowContext.Sequences.First();

            var origEntry = rowContext.CurrentEntry;
            var entryUsed = false;
            foreach (var i in sequence.Value as System.Collections.IEnumerable)
            {
                if (entryUsed)
                {
                    this.CloneEntry(origEntry, rowContext);
                }

                rowContext.CurrentEntry.Entity[sequence.Field.Field] = i;

                entryUsed = true;
            }
        }

        private void CloneEntry(TableEntry entry, TRowContext rowContext)
        {
            this.CreateEntity(rowContext);

            entry.Entity.CopyTo(rowContext.CurrentEntry.Entity);

            rowContext.CurrentEntry.Entity.ApplyChanges();
        }
        private object DetermineColumnValue(TRowContext rowContext, int col)
        {
            var value = rowContext.Sheet.GetCellValue(rowContext.Row, col);
            if (string.IsNullOrWhiteSpace(ConvertUtils.ToString(value)))
            {
                value = null;
            }

            return value;
        }

        private object DetermineColumnValue(TRowContext rowContext, string[] splitId)
        {
            object value = null;

            var split = rowContext.Splits.FirstOrDefault(s => string.Equals(s.Split.Name, splitId[0], StringComparison.InvariantCultureIgnoreCase));
            var partId = ConvertUtils.ToInt32(splitId[1]).GetValueOrDefault(-1);
            if (split?.Valid == true && partId >= 0 && partId < split.Split.PartsCount)
            {
                value = split.Value[partId];
            }
            else
            {
                value = null;
            }

            return value;
        }

        private object DetermineColumnValueLeft(TRowContext rowContext, int left, object value)
        {
            var str = ConvertUtils.ToString(value);
            var len = str.Length;
            if (len > rowContext.CurrentField.Left)
            {
                value = str.Substring(0, left);
            }

            return value;
        }

        private object ProcessColumnValue(TRowContext rowContext, ImportFieldType type, object value)
        {
            switch (type)
            {
                case ImportFieldType.Company:
                    value = this.DetermineCompanyValue(value, rowContext.CurrentField.Lookup);
                    break;
                case ImportFieldType.Type:
                    value = this.DetermineTypeValue(value, rowContext.CurrentField.Lookup);
                    break;
                case ImportFieldType.FlagType:
                    value = this.DetermineFlagTypeValue(value, rowContext.CurrentField.Lookup);
                    break;
                case ImportFieldType.Lookup:
                    value = this.DetermineLookupValue(value, rowContext.CurrentField.Lookup, rowContext);
                    break;
                case ImportFieldType.SelfLookup:
                    value = this.DetermineSelfLookupValue(value, rowContext);
                    break;
                case ImportFieldType.Dictionary:
                    value = this.DetermineDictionaryValue(value, rowContext.CurrentField.Lookup);
                    break;
                case ImportFieldType.Sequence:
                    value = this.DetermineSequenceValue(rowContext);
                    break;
                case ImportFieldType.Boolean:
                    value = this.DetermineBooleanValue(value, rowContext);
                    break;
                case ImportFieldType.Concat:
                    value = this.DetermineConcatValue(value, rowContext);
                    break;
                case ImportFieldType.VirtualID:
                    value = this.DetermineVirtualID(rowContext);
                    break;
                default:
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                    throw new ArgumentOutOfRangeException(nameof(rowContext.CurrentField.Type), rowContext.CurrentField.Type, nameof(this.ProcessColumnValue));
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
            }

            return value;
        }

        private FieldValue DetermineFieldValue(TRowContext rowContext)
        {
            object value = null;
            if (rowContext.CurrentField.ColumnIndex != null)
            {
                value = this.DetermineColumnValue(rowContext, rowContext.CurrentField.ColumnIndex.Value);
            }

            if (!string.IsNullOrWhiteSpace(rowContext.CurrentField.SplitPart))
            {
                var splitId = rowContext.CurrentField.SplitPart.Split(new[] { '#' }, 2);
                if (splitId.Length == 2)
                {
                    value = this.DetermineColumnValue(rowContext, splitId);
                }
                else
                {
                    value = null;
                }
            }

            if (value != null && rowContext.CurrentField.Left != null)
            {
                value = this.DetermineColumnValueLeft(rowContext, rowContext.CurrentField.Left.Value, value);
            }
            if (value != null && !string.IsNullOrEmpty(rowContext.CurrentField.SubString))
            {
                var pars = rowContext.CurrentField.SubString.Split(',');
                if (pars.Length == 2)
                {
                    var startIndex = int.Parse(pars[0]);
                    var length = int.Parse(pars[1]); 
                    var str = ConvertUtils.ToString(value);
                    var len = str.Length;
                    if (len > startIndex + length)
                    {
                        value = str.Substring(startIndex, length);
                    }
                } else if (pars.Length == 1)
                {
                    var startIndex = int.Parse(pars[0]); 
                    var str = ConvertUtils.ToString(value);
                    var len = str.Length;
                    if (len > startIndex)
                    {
                        value = str.Substring(startIndex);
                    }
                }
            }

            if (rowContext.CurrentField.Type.HasValue)
            {
                value = this.ProcessColumnValue(rowContext, rowContext.CurrentField.Type.Value, value);
            }

            if (value == null)
            {
                value = rowContext.CurrentField.Const;
            }

            if (value != null && !string.IsNullOrWhiteSpace(rowContext.CurrentField.Prefix))
            {
                value = $"{rowContext.CurrentField.Prefix}{Convert.ToString(value)}";
            }

            return new FieldValue
            {
                Field = rowContext.CurrentField,
                Value = value,
                Valid = !rowContext.CurrentField.Required.GetValueOrDefault() ||
                    !(value is string) && value != null ||
                    value is string strValue && !string.IsNullOrWhiteSpace(strValue)
            };
        }

        private object DetermineFieldValue(ImportFieldBase dependentField, TRowContext rowContext)
        {
            switch (dependentField.Type)
            {
                case ImportFieldBaseType.Current:
                    return this.DetermineCurrentValue(rowContext, dependentField.Lookup);
                default:
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                    throw new ArgumentOutOfRangeException(nameof(rowContext.CurrentField.Type), rowContext.CurrentField.Type, nameof(this.DetermineFieldValue));
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
            }
        }

        private object DetermineCompanyValue(object value, ImportLookup lookup)
        {
            if (string.IsNullOrWhiteSpace(ConvertUtils.ToString(value)))
            {
                return null;
            }

            if (string.Equals(ConvertUtils.ToString(value), Base.Initializer.CMPCODEALL, StringComparison.InvariantCultureIgnoreCase))
            {
                return Base.Initializer.CMPCODEALL;
            }

            IEnumerable<Base.Setup.Company.Company> companies;

            var cached = Base.Setup.Company.CompanyCache.GetAllCached();
            if (cached.Count == 0)
            {
                // betoltjuk az osszes vallalatot
                var allCmpIds = Base.Setup.Company.Company.GetAllCmpIds();
                var key = new Key
                {
                    [Base.Setup.Company.Company.FieldCmpid.Name] = new Key.InAtToSql(allCmpIds)
                };
                companies = SqlDataAdapter.GetList<Base.Setup.Company.Company>(key);
                Base.Setup.Company.CompanyCache.Add(companies.ToArray());
            }

            switch (lookup.KeyField.ToLowerInvariant())
            {
                case "cmpcode":
                    var cmpcodes = ConvertUtils.ToString(value).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    companies = Base.Setup.Company.CompanyCache.GetList(cmpcodes);
                    return string.Join("", companies.Select(c => (string)c.Cmpcode).OrderBy(c => c, StringComparer.InvariantCultureIgnoreCase));
                case "codacode":
                    var codacodes = ConvertUtils.ToString(value).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    companies = Base.Setup.Company.CompanyCache.GetListByCodaCode(codacodes);
                    return string.Join("", companies.Select(c => (string)c.Cmpcode).OrderBy(c => c, StringComparer.InvariantCultureIgnoreCase));
                default:
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                    throw new ArgumentOutOfRangeException(nameof(lookup.KeyField), lookup.KeyField, nameof(this.DetermineCompanyValue));
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
            }
        }

        private object DetermineTypeValue(object value, ImportLookup lookup)
        {
            var str = $"{value}";
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }

            var key = $"TYPE|{lookup.TypeKey}|{lookup.KeyField}|{lookup.ValueField}|{value}".ToUpperInvariant();
            if (lookupCache.TryGetValue(key, out var o))
            {
                return o;
            }

            var sql = $@"select top 1 [tl].[{lookup.ValueField}]
from [ols_typehead] [th] (nolock)
  join [ols_typeline] [tl] (nolock) on [tl].[typegrpid] = [th].[typegrpid]
where [th].[typekey] = {Utils.SqlToString(lookup.TypeKey)}
  and (";

            var sep = "";
            foreach (string kf in lookup.KeyField.Split(','))
            {
                sql += sep + $"[tl].[{kf}] = {Utils.SqlToString(value)}";
                sep = " or ";
            }

            sql += ")";

            o = SqlDataAdapter.ExecuteSingleValue(DB.Main, sql);

            if (o == null)
            {
                this.logger.LogErrorLine($"Type value is not found: {value} [key: {lookup.TypeKey}]");
            }

            return lookupCache[key] = o;
        }

        private object DetermineFlagTypeValue(object value, ImportLookup lookup)
        {
            var str = $"{value}";
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }

            var strs = str.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            var key = $"TYPE|{lookup.TypeKey}|{lookup.KeyField}|{lookup.ValueField}|{value}".ToUpperInvariant();
            if (lookupCache.TryGetValue(key, out var o))
            {
                return o;
            }

            var sql = $@"select [tl].[{lookup.ValueField}]
from [ols_typehead] [th] (nolock)
  join [ols_typeline] [tl] (nolock) on [tl].[typegrpid] = [th].[typegrpid]
where [th].[typekey] = {Utils.SqlToString(lookup.TypeKey)}
  and [tl].[{lookup.KeyField}] {Utils.SqlEqualOrInToString2(strs)}";

            var result = 0;
            Base.Common.SqlFunctions.QueryData(DB.Main, sql, r =>
            {
                result |= Convert.ToInt32(r[0]);
            });

            if (result == 0)
            {
                this.logger.LogErrorLine($"Flag value is not found: ({string.Join(",", strs)}) [key: {lookup.TypeKey}]");
            }

            return lookupCache[key] = result;
        }

        private object DetermineLookupValue(object value, ImportLookup lookup, TRowContext rowContext = null)
        {
            if (string.Equals(ConvertUtils.ToString(value), "#N/A", StringComparison.InvariantCultureIgnoreCase) ||
                value == null || string.IsNullOrWhiteSpace(ConvertUtils.ToString(value)))
            {
                return null;
            }

            var cacheKeyBldr = new StringBuilder();
            cacheKeyBldr.Append($"CUST|{lookup.Table}|{lookup.KeyField}|{lookup.ValueField}");

            var key = new Key();
            if (lookup.DependentFields?.Any() == true && rowContext != null)
            {
                foreach (var df in lookup.DependentFields)
                {
                    var dfValue = this.DetermineFieldValue(df, rowContext);
                    key[$"[{df.Field}]"] = dfValue;
                    cacheKeyBldr.Append($"|{dfValue}");
                }
            }

            cacheKeyBldr.Append($"|{value}");
            var cacheKey = cacheKeyBldr.ToString().ToUpperInvariant();
            if (lookupCache.TryGetValue(cacheKey, out var o))
            {
                return o;
            }

            if (!string.IsNullOrWhiteSpace(lookup.KeyField))
            {
                key[$"[{lookup.KeyField}]"] = value;
            }
            else if (lookup.KeyFieldExpr != null)
            {
                var field = lookup.KeyFieldExpr.Expr;
                field = Regex.Replace(field, @"(\{value\})", $"{value}");
                key[$"[{lookup.KeyFieldExpr.Field}]"] = field;
            }

            var sql = $@"select top 2 [t].[{lookup.ValueField}]
from [{lookup.Table}] [t] (nolock)
where {key.ToSql("[t]")}";

            var list = new List<object>();
            Base.Common.SqlFunctions.QueryData(DB.Main, sql, r =>
            {
                list.Add(r.GetValue(0));
            });

            // ha 1-nel tobb talalat van, akkor az nem egyertelmu talalat
            o = null;
            if (list.Count == 1)
            {
                o = list[0];
            }

            if (o == null)
            {
                this.logger.LogErrorLine($"Lookup value is not found: {value} [object: {lookup.Table}, key: {lookup.KeyField}]");
            }

            return lookupCache[cacheKey] = o;
        }

        private object DetermineDictionaryValue(object value, ImportLookup lookup)
        {
            if (string.Equals(ConvertUtils.ToString(value), "#N/A", StringComparison.InvariantCultureIgnoreCase) ||
                value == null || string.IsNullOrWhiteSpace(ConvertUtils.ToString(value)))
            {
                return null;
            }

            var key = $"CUST|{lookup.Alias}".ToUpperInvariant();
            if (!dictionaryCache.TryGetValue(key, out var dict))
            {
                this.logger.LogErrorLine($"Dictionary is not found for {lookup.Alias}");

                return null;
            }

            var values = new[] { value };
            if (value is string strValue)
            {
                values = strValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Cast<object>().ToArray();
            }

            var foundValues = dict.Where(d => this.EqualsOrInValue(d.KeyValue, values)).ToList();
            object o = null;
            if (foundValues.Count == values.Length)
            {
                foreach (var ev in foundValues
                    .Select(fv => fv.FirstOrDefault(v => string.Equals(v.Key, lookup.ValueField, StringComparison.InvariantCultureIgnoreCase)))
                    .Where(e => !string.IsNullOrWhiteSpace(e.Key))
                    .Select(e => e.Value))
                {
                    if (dict.Description.Type == ImportDictionaryType.FlagType && int.TryParse(ConvertUtils.ToString(ev), out var i))
                    {
                        o = (o == null ? 0 : Convert.ToInt32(o)) | i;
                    }
                    else
                    {
                        o = ev;
                        break;
                    }
                }
            }

            if (o == null)
            {
                this.logger.LogErrorLine($"Dictionary value is not found: {value} [object: {lookup.Alias}, key: {lookup.KeyField}]");
            }

            return o;
        }

        private bool EqualsOrInValue(object a, object[] bArray)
        {
            if (a == null || bArray == null || bArray.Length == 0)
            {
                return false;
            }

            var aType = a.GetType();
            var aTypeCode = Type.GetTypeCode(aType);

            var b = bArray[0];
            var bType = b.GetType();
            var bTypeCode = Type.GetTypeCode(bType);

            switch (aTypeCode)
            {
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Byte:
                case TypeCode.SByte:
                    switch (bTypeCode)
                    {
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                        case TypeCode.Int16:
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                        case TypeCode.UInt16:
                        case TypeCode.UInt32:
                        case TypeCode.UInt64:
                        case TypeCode.Byte:
                        case TypeCode.SByte:
                        case TypeCode.Boolean:
                            return bArray.Any(b1 => decimal.Equals(Convert.ToDecimal(a), Convert.ToDecimal(b1)));
                        case TypeCode.String:
                            return bArray.Any(b1 => decimal.Equals(Convert.ToDecimal(a), decimal.Parse((string)b1)));
                    }

                    return false;
                case TypeCode.Boolean:
                    switch (bTypeCode)
                    {
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                        case TypeCode.Int16:
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                        case TypeCode.UInt16:
                        case TypeCode.UInt32:
                        case TypeCode.UInt64:
                        case TypeCode.Byte:
                        case TypeCode.SByte:
                        case TypeCode.Boolean:
                            return bArray.Any(b1 => bool.Equals(Convert.ToBoolean(a), Convert.ToBoolean(b1)));
                        case TypeCode.String:
                            return bArray.Any(b1 => bool.Equals(Convert.ToDecimal(a), bool.Parse((string)b1)));
                    }

                    return false;
                case TypeCode.String:
                    return bArray.Any(b1 => string.Equals(Convert.ToString(a), Convert.ToString(b1)));
                case TypeCode.DateTime:
                    switch (aTypeCode)
                    {
                        case TypeCode.DateTime:
                            return bArray.Any(b1 => DateTime.Equals(Convert.ToDateTime(a), Convert.ToDateTime(b1)));
                        default:
                            return bArray.Any(b1 => DateTime.Equals(Convert.ToDateTime(a), DateTime.Parse(Convert.ToString(b))));
                    }
            }

            return false;
        }

        protected virtual object DetermineSelfLookupValue(object value, TRowContext rowContext)
        {
            return value;
        }

        private object DetermineCurrentValue(TRowContext rowContext, ImportLookup lookup)
        {
            var entity = rowContext.CurrentEntry?.Entity;

            if (entity != null)
            {
                return entity[lookup.ValueField];
            }

            return null;
        }

        private object DetermineSequenceValue(TRowContext rowContext)
        {
            if (rowContext.Sequences.Count > 0)
            {
                throw new InvalidOperationException("Multiple sequences is not supported yet");
            }

            if (!string.Equals(rowContext.CurrentField.Sequence.Table, "ols_company", StringComparison.InvariantCultureIgnoreCase))
            {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                throw new ArgumentOutOfRangeException(nameof(rowContext.CurrentField.Sequence.Table), rowContext.CurrentField.Sequence.Table, nameof(this.DetermineSequenceValue));
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
            }

            var cached = Base.Setup.Company.CompanyCache.GetAllCached();
            if (cached.Count == 0)
            {
                // betoltjuk az osszes vallalatot
                var allCmpIds = Base.Setup.Company.Company.GetAllCmpIds();
                var key = new Key
                {
                    [Base.Setup.Company.Company.FieldCmpid.Name] = new Key.InAtToSql(allCmpIds)
                };
                var companies = SqlDataAdapter.GetList<Base.Setup.Company.Company>(key);
                Base.Setup.Company.CompanyCache.Add(companies.ToArray());

                cached = companies;
            }

            var cmpIds = cached.Select(c => c.Cmpid.Value).ToArray();

            var value = new FieldValue
            {
                Field = rowContext.CurrentField,
                Value = cmpIds
            };

            rowContext.Sequences.Add(value);

            return null;
        }

#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable S1172 // Unused method parameters should be removed
        private object DetermineBooleanValue(object value, TRowContext rowContext)
#pragma warning restore S1172 // Unused method parameters should be removed
#pragma warning restore IDE0060 // Remove unused parameter
        {
            if (value == null)
            {
                return null;
            }
            else if (value is bool)
            {
                return value as bool?;
            }
            else if (value is int || value is long)
            {
                return Convert.ToBoolean(Convert.ToInt64(value));
            }
            else if (value is string strValue)
            {
                switch (strValue.ToUpperInvariant())
                {
                    case "I":
                    case "T":
                    case "Y":
                    case "+":
                    case "1":
                    case "-1":
                    case "TRUE":
                    case "YES":
                    case "IGEN":
                    case "ON":
                        return true;
                    case "N":
                    case "F":
                    case "-":
                    case "0":
                    case "FALSE":
                    case "NO":
                    case "NEM":
                    case "OFF":
                        return false;
                }
            }

            return null;
        }

        private object DetermineConcatValue(object value, TRowContext rowContext)
        {
            if (rowContext.CurrentField.Columns?.Any(c => c.Valid) == true)
            {
                var bldr = new StringBuilder();
                foreach (var c in rowContext.CurrentField.Columns.Where(c => c.Valid && c.ColumnIndex != null))
                {
                    var val = ConvertUtils.ToString(this.DetermineColumnValue(rowContext, c.ColumnIndex.Value));
                    if (!string.IsNullOrWhiteSpace(val))
                    {
                        bldr.AppendLine(val);
                    }
                }

                return bldr.ToString();
            }

            return value;
        }

        private object DetermineVirtualID(TRowContext rowContext)
        {
            return --rowContext.VirtualID;
        }
    }
}
