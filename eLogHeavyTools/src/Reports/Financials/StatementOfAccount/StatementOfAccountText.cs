using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Reports.Financials.StatementOfAccount
{
    public class StatementOfAccountText : Common.xcval.OfcXcval
    {
        public override void PreSave()
        {
            base.PreSave();

            if (this.State == eProjectWeb.Framework.Data.DataRowState.Added)
            {
                this.Xcvcode = StatementOfAccountBL.STATEMENTOFACCOUNT_TEXT_XCVCODE;
                this.Xmldata = "<x />";
            }
        }
    }
}
