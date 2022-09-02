using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Reports.Financials.ReminderLetter
{
    public class ReminderLetterTextList : DefaultListProvider
    {
        public static readonly string ID = typeof(ReminderLetterTextList).FullName;

        protected static string queryString = $@"select [v].[xcvextcode]
from [ofc_xcval] [v] (nolock)
where [v].[xcvcode] = {Utils.SqlToString(ReminderLetterBL.REMINDERLETTER_TEXT_XCVCODE)}
  and [v].[xcvextcode] is not null
";

        protected static ListColumn[] listColumns = new[]
        {
            new ListColumn("xcvextcode", 280)
        };

        protected ReminderLetterTextList() : base(queryString, listColumns)
        {
            DBConnID = CodaInt.Base.Module.CodaDBConnID;
        }
    }
}
