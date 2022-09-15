using eProjectWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Reports.Financials.ReminderLetter
{
    public class ReminderLetter2 : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(ReminderLetter2).FullName;

        public ReminderLetter2() : base("ReminderLetter2")
        {
            Tabs.AddTab(() => ReminderLetterParamsTab2.New("ReminderLetter2"));
        }
    }

}
