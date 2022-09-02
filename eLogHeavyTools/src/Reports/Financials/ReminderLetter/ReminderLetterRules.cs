using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Reports.Financials.ReminderLetter
{
    public class ReminderLetterRules : eLog.Base.Common.TypedBaseRuleSet<ReminderLetterItem>
    {
        public ReminderLetterRules() : base(true, false)
        {
            this.ERules[ReminderLetterItem.FieldXcvid.Name].Mandatory = false;
            this.ERules[ReminderLetterItem.FieldSeqno.Name].Mandatory = false;
        }
    }
}
