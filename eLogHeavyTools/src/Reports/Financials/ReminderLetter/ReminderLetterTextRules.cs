using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Reports.Financials.ReminderLetter
{
    public class ReminderLetterTextRules : Common.xcval.OfcXcvalRules<ReminderLetterText>
    {
        public ReminderLetterTextRules() : base(true, false)
        {
            this.ERules[ReminderLetterText.FieldXcvcode.Name].Mandatory = false;
            this.ERules[ReminderLetterText.FieldXmldata.Name].Mandatory = false;
        }
    }
}
