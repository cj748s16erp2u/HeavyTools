using com.coda.eassets.schemas.asset;
using eLog.HeavyTools.Warehouse.WhZone.WhZTranService;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Data.Colorizer;
using eProjectWeb.Framework.Formatters;
using eProjectWeb.Framework.JSON;
using eProjectWeb.Framework.Rights;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Collections.Generic;
using DataRowState = eProjectWeb.Framework.Data.DataRowState;

namespace eLog.HeavyTools.Common.Data
{
    public abstract class HTSearchProviderBase<TEntity> : ISearchProvider 
        where TEntity : Entity
    {
        public enum SearchProviderType
        {
            Default = 1, // normal grid, statusszal
            Inline = 2,  // inline grid, inline statusszal
        }
        protected SearchProviderType Type;

        public int MaxRecords { get; set; }
        protected string QueryString { get; set; }
        protected QueryArg[] ArgsTemplate { get; set; }
        protected bool ChangingColumns; // valtozo oszlopok, ne cache-elje az oszlop informaciot, es mindig kuldje le a kereses eredmenyevel egyutt
        protected FormatterSet FormatterSet;
        public const DataRowState Detached = EntityReader<TEntity>.Detached;
        protected IList<TEntity> List { get; set; }

        public HTSearchProviderBase(string queryString, QueryArg[] argsTemplate, SearchProviderType type)
        {
            QueryString = queryString;
            Type = type;
            MaxRecords = -1;
            ArgsTemplate = argsTemplate;
        }

        public ColumnCollection GetColumns()
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            return GetColumns(args);
        }

        public ColumnCollection GetColumns(Dictionary<string, object> args)
        {
            string columnCacheKey = GetColumnCacheKey();
            if (!ChangingColumns)
            {
                ColumnCollection cachedColumns = ColumnCache.Get(columnCacheKey);
                if (cachedColumns != null)
                    return cachedColumns;
            }

            Dictionary<string, object> argsCopy = new Dictionary<string, object>();
            foreach (string key in args.Keys)
                argsCopy.Add(key, args[key]);


            ColumnCollection columns = ColumnCollection.ReadXml(GetSearchXmlFileName());

            SetupColumns(columns);

            switch (Type)
            {
                case SearchProviderType.Default:
                    columns.Insert(0, DefaultColumns.SearchRowStatusColumn);
                    break;
                case SearchProviderType.Inline:
                    columns.Insert(0, DefaultColumns.InlineEditRowStatusColumn);
                    break;
            }

            columns.FormatterSet = FormatterSet;
            if (!ChangingColumns)
            {
                ColumnCache.Add(columnCacheKey, columns);
            }
            return columns;
        }

        public string SearchJSON(Dictionary<string, object> args)
        {
            List<string> list = new List<string>();
            Search(args, ProcessSearchResultsJSON, list);
            return list[0];
        }

        public void Search(Dictionary<string, object> args, ProcessSearchResultsDelegate proc, object userData)
        {
            Search(args, proc, userData, true);
        }
        public virtual void Search(Dictionary<string, object> args, ProcessSearchResultsDelegate proc, object userData, bool useReader)
        {
            List = PrepareList(new MSPCreateListArgs(args, MSPCreateListCallType.Search));

            ColumnCollection columns = GetColumns(args); // precache, hogy ne legyen nyitva ket DataReader
            PreSearch1(args);
            PreGetResults(args, columns);

            ACPath path = new ACPath(Session.AccessControl);
            path.Push("G");
            path.Push(GetACName());

            var sa = new SearchResultsArgs
            {
                Args = args,
                UserData = userData,
                Columns = columns,
                SearchProvider = this,
                ACPath = path
            };
            proc(sa);
        }
        
        /// <summary>
        /// Lista feldolgozása és megfelelő json formátumba konvertálása.
        /// </summary>
        /// <param name="sa"></param>
        /// <exception cref="ArgumentException"></exception>
        protected virtual void ProcessSearchResultsJSON(SearchResultsArgs sa)
        {
            if (!(sa.UserData is List<string>))
            {
                throw new ArgumentException("userdata must be a List<string>");
            }

            List<string> list = (List<string>)sa.UserData;
            using (var sw = new System.IO.StringWriter())
            using (var jw = new JsonTextWriter(sw))
            {
                jw.WriteStartObject();
                jw.WritePropertyName("list");
                int num = GetResults(sa, jw);
                if (num >= MaxRecords && MaxRecords >= -1)
                {
                    num = GetRecordCount();
                }

                if (ChangingColumns)
                {
                    jw.WritePropertyName("columns");
                    JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        StringEscapeHandling = StringEscapeHandling.EscapeHtml
                    };
                    jsonSerializerSettings.Converters.Add(new ColumnCollectionConverter());
                    jw.WriteRawValue(JsonConvert.SerializeObject(sa.Columns, Formatting.None, jsonSerializerSettings));
                }

                jw.WritePropertyName("records");
                jw.WriteValue(num);
                jw.WriteEndObject();
                jw.Flush();
                list.Add(sw.ToString());
            }
        }
        protected virtual void PreSearch1(Dictionary<string, object> args)
        {
            PreSearch(args);
        }
        protected virtual void PreSearch(Dictionary<string, object> args)
        {
        }
        protected virtual string GetColumnCacheKey()
        {
            return this.GetType().FullName;
        }
        protected virtual void SetupColumns(ColumnCollection schema)
        {

        }
        protected virtual void PreGetResults(Dictionary<string, object> args, ColumnCollection columns)
        {
        }
        protected virtual string GetSearchXmlFileName()
        {
            return GetNamespaceName() + "." + this.GetType().Name;
        }
        public virtual string GetNamespaceName()
        {
            return this.GetType().Namespace;
        }
        protected virtual int GetResults(SearchResultsArgs sa, Newtonsoft.Json.JsonWriter jw)
        {
            var adapter = GetReader(sa);
            return JSONSerializer.ToJsonWriter(jw, adapter, MaxRecords, sa.Columns, true);
        }
        protected virtual int GetRecordCount()
        {
            var i = 0;
            foreach (var v in List)
            {
                if (v.State != Detached)
                {
                    i++;
                }
            }
            return i;
        }

        /// <summary>
        /// Dictionaryból a kívánt Entity-be konvertálás.
        /// </summary>
        /// <typeparam name="TQueryDto"></typeparam>
        /// <param name="args"></param>
        /// <returns>TQueryDTO</returns>
        protected virtual TQueryDto ConvertToDto<TQueryDto>(IDictionary<string, object> args)
            where TQueryDto : new()
        {
            var dto = new TQueryDto();
            if (args == null)
            {
                return dto;
            }

            var type = typeof(TQueryDto);
            var props = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty);
            foreach (var p in props)
            {
                if (args.ContainsKey(p.Name))
                {
                    var temp = p.PropertyType;
                    //if (temp.IsGenericType && temp.GetGenericTypeDefinition() == typeof(Nullable<>))
                    //{
                    //    temp = Nullable.GetUnderlyingType(temp);                   
                    //}
                    if (temp == typeof(decimal?))
                    {
                        p.SetValue(dto, Convert.ToDecimal(args[p.Name]));
                    }
                    else if (temp == typeof(int?))
                    {
                        p.SetValue(dto, Convert.ToInt32(args[p.Name]));
                    }
                    else if (temp == typeof(DateTime?))
                    {
                        p.SetValue(dto, Convert.ToDateTime(args[p.Name]));
                    }
                    else if (temp == typeof(DateTimeOffset?))
                    {
                        DateTimeOffset offset = DateTime.SpecifyKind(Convert.ToDateTime(args[p.Name]), DateTimeKind.Local);
                        p.SetValue(dto, offset);
                    }
                    else
                    {
                        StringN s = new StringN(args[p.Name]);
                        p.SetValue(dto, s);
                    }
                }
            }

            return dto;
        }
        protected virtual string GetACName()
        {
            Type t = this.GetType();

            System.Reflection.FieldInfo fi = null;
            while (t != typeof(object) && (fi = t.GetField("ID")) == null)
                t = t.BaseType;

            if (fi != null)
                return Convert.ToString(fi.GetValue(this));

            return this.GetType().FullName;
        }
        protected virtual void AfterFinallySearch(Dictionary<string, object> args)
        {
        }
        public eProjectWeb.Framework.Data.IDataReader GetReader(SearchResultsArgs sa)
        {
            return new EntityReader<TEntity>(List, sa.ACPath);
        }
        public Colorizer GetColorizer()
        {
            return Colorizer.Get(GetColorizerID());
        }
        protected virtual string GetColorizerID()
        {
            return GetType().Name;
        }

        /// <summary>
        /// A metódus megvalósításai a leszármazottakban elkészít az argumentumok alapján egy Entity listát.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected abstract IList<TEntity> PrepareList(MSPCreateListArgs args);

        //public virtual IList<TEntity> CreateList(MSPCreateListArgs args)
        //{
        //    if (List == null)
        //    {
        //        //string items = PrepareSql(args);
        //        List = PrepareList(args);
        //        PostFilterList(List, args);
        //    }

        //    return List;
        //}
        protected virtual void PostFilterList(IList<TEntity> list, MSPCreateListArgs args)
        {
        }
        public ProcessRecordDelegate GetProcessRecordFunc()
        {
            throw new NotImplementedException();
        }

        public System.Data.DataSet SearchDataSet(Dictionary<string, object> args)
        {
            return SearchDataSet(args, true);
        }

        public System.Data.DataSet SearchDataSet(Dictionary<string, object> args, bool fillSchema)
        {
            throw new NotImplementedException();
        }
    }
}
