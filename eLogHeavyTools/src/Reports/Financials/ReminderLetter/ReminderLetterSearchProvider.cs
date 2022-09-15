using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Reports.Financials.ReminderLetter
{
    public class ReminderLetterSearchProvider : MemorySearchProvider<ReminderLetterItem>
    {
        public static readonly string ID = typeof(ReminderLetterSearchProvider).FullName;

        protected static string queryString = $@"select [v].[xcvid], [val].*
from [ofc_xcval] [v] (nolock)
  cross apply [v].[xmldata].nodes('/items/item') [xml]([x])
  outer apply (select 
    [x].value('seqno[1]', 'int') [seqno],
    [x].value('company[1]', 'nvarchar(100)') [company],
    [x].value('ledger[1]', 'nvarchar(100)') [ledger],
    [x].value('severity[1]', 'int') [severity],
    [x].value('lettertype[1]', 'int') [lettertype],
    [x].value('template[1]', 'nvarchar(100)') [template]) [val]
";

        protected static QueryArg[] argsTemplate = new[]
        {
            new QueryArg("xcvid", "v", FieldType.Integer),
            new QueryArg("seqno", "val", FieldType.Integer, QueryFlags.MultipleAllowed),
            new QueryArg("xcvcode", "v", FieldType.String, QueryFlags.Equals),
            new QueryArg("xcvextcode", "v", FieldType.String, QueryFlags.Equals | QueryFlags.SearchForNull)
        };

        protected ReminderLetterSearchProvider() : base(queryString, argsTemplate, SearchProviderType.Default)
        {
        }

        protected override void PreSearch(MSPCreateListArgs args)
        {
            base.PreSearch(args);

            foreach (var f in args.Filters.Keys.ToArray())
            {
                args.Filters[f.ToLowerInvariant()] = args.Filters[f];
            }

            args.Filters["xcvcode"] = ReminderLetterBL.REMINDERLETTER_TEXT_XCVCODE;
            args.Filters["xcvextcode"] = null;
        }

        protected override IList<ReminderLetterItem> PrepareList(string sql, MSPCreateListArgs args)
        {
            var list = SqlDataAdapter.GetList<ReminderLetterItem>(sql);

            return list;
        }

        protected override string GetOrderByString(MSPCreateListArgs args)
        {
            return "\norder by [val].[seqno]";
        }
    }
}
