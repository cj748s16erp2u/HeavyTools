using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Reports.Financials.StatementOfAccount
{
    public class StatementOfAccountTextList : DefaultListProvider
    {
        public static readonly string ID = typeof(StatementOfAccountTextList).FullName;

        protected static string queryString = $@"select [v].[xcvextcode]
from [ofc_xcval] [v] (nolock)
where [v].[xcvcode] = {Utils.SqlToString(StatementOfAccountBL.STATEMENTOFACCOUNT_TEXT_XCVCODE)}
  and [v].[xcvextcode] is not null
";

        protected static ListColumn[] listColumns = new[]
        {
            new ListColumn("xcvextcode", 280)
        };

        protected StatementOfAccountTextList() : base(queryString, listColumns)
        {
            DBConnID = CodaInt.Base.Module.CodaDBConnID;
        }
    }
}
