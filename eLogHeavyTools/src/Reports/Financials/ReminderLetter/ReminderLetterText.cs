using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Reports.Financials.ReminderLetter
{
    public class ReminderLetterText : Common.xcval.OfcXcval
    {
        public override void PreSave()
        {
            base.PreSave();

            if (this.State == eProjectWeb.Framework.Data.DataRowState.Added)
            {
                this.Xcvcode = ReminderLetterBL.REMINDERLETTER_TEXT_XCVCODE;
                this.Xmldata = "<x />";
            }
        }
    }
}
