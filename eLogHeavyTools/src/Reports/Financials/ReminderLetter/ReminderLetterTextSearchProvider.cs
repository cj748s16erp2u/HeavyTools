using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Reports.Financials.ReminderLetter
{
    public class ReminderLetterTextSearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(ReminderLetterTextSearchProvider).FullName;

        protected static string queryString = $@"select [v].*
from [ofc_xcval] [v] (nolock)
";

        protected static QueryArg[] argsTemplate = new[]
        {
            new QueryArg("xcvid", "v", FieldType.Integer),
            new QueryArg("xcvcode", "v", FieldType.String, QueryFlags.Equals),
            new QueryArg("xcvextcode", "v", FieldType.String, QueryFlags.Equals)
        };

        protected ReminderLetterTextSearchProvider() : base(queryString, argsTemplate, SearchProviderType.Default)
        {
            this.DBConnID = CodaInt.Base.Module.CodaDBConnID;
            this.SetCustomFunc("xcvextcode", XCVExtCodeCustomFunc);
        }

        private void XCVExtCodeCustomFunc(StringBuilder sb, QueryArg arg, string quotedFieldName, object argValue)
        {
            var v = ConvertUtils.ToString(argValue);
            if (string.IsNullOrWhiteSpace(v))
            {
                sb.Append($"{quotedFieldName} is not null");
            }
            else
            {
                QueryArg.BuildQueryArgString(sb, arg, quotedFieldName, argValue);
            }
        }

        protected override void PreSearch(Dictionary<string, object> args)
        {
            base.PreSearch(args);

            args["xcvcode"] = ReminderLetterBL.REMINDERLETTER_TEXT_XCVCODE;
            if (!args.ContainsKey("xcvextcode"))
            {
                args["xcvextcode"] = null;
            }
        }
    }
}
