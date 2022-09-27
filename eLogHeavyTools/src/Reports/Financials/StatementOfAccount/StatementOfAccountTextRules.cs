using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Reports.Financials.StatementOfAccount
{
    public class StatementOfAccountTextRules : Common.xcval.OfcXcvalRules<StatementOfAccountText>
    {
        public StatementOfAccountTextRules() : base(true, false)
        {
            this.ERules[StatementOfAccountText.FieldXcvcode.Name].Mandatory = false;
            this.ERules[StatementOfAccountText.FieldXmldata.Name].Mandatory = false;
        }
    }
}
