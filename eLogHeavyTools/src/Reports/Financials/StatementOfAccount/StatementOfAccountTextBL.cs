using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Reports.Financials.StatementOfAccount
{
    public class StatementOfAccountTextBL : Common.xcval.OfcXcvalBL<StatementOfAccountText, StatementOfAccountTextRules>
    {
        public static readonly string ID = typeof(StatementOfAccountTextBL).FullName;

        public static T New<T>()
            where T : StatementOfAccountTextBL
        {
            return ObjectFactory.New<T>();
        }

        public static StatementOfAccountTextBL New()
        {
            return New<StatementOfAccountTextBL>();
        }

        public StatementOfAccountTextBL() : base(DefaultBLFunctions.Basic)
        {
        }

        protected static readonly string SQL_DeleteUseCheck = $@"select count(0)
from [ofc_xcval] [v] (nolock)
where [v].[xcvcode] = {Utils.SqlToString(StatementOfAccountBL.STATEMENTOFACCOUNT_TEXT_XCVCODE)}
  and [v].[xcvextcode] is not null
  and exists (select 0 from [ofc_xcval] [v1]
      cross apply [v1].[xmldata].nodes('/items/item') [xml]([x])
      outer apply (select [x].value('template[1]', 'nvarchar(100)') [template]) [val]
    where [val].[template] = [v].[xcvextcode])
  and {{0}}
";

        public override bool IsDeletePossible(Key k, out string reason)
        {
            var b = base.IsDeletePossible(k, out reason);

            if (b)
            {
                var sql = string.Format(SQL_DeleteUseCheck, k.ToSql("[v]"));
                var count = ConvertUtils.ToInt32(SqlDataAdapter.ExecuteSingleValue(CodaInt.Base.Module.CodaDBConnID, sql));
                if (!(b = !(count.GetValueOrDefault() > 0)))
                {
                    reason = "$err_statementofaccounttext_isused".eLogTransl();
                }
            }

            return b;
        }
    }
}
