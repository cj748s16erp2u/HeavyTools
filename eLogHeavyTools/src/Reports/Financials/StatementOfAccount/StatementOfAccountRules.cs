using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Reports.Financials.StatementOfAccount
{
    public class StatementOfAccountRules : eLog.Base.Common.TypedBaseRuleSet<StatementOfAccountItem>
    {
        public StatementOfAccountRules() : base(true, false)
        {
            this.ERules[StatementOfAccountItem.FieldXcvid.Name].Mandatory = false;
            this.ERules[StatementOfAccountItem.FieldSeqno.Name].Mandatory = false;
        }
    }
}
