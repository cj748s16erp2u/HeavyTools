using eProjectWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Reports.Financials.ReminderLetter
{
    public class ReminderLetterEditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(ReminderLetterEditPage).FullName;

        public ReminderLetterEditPage() : base("ReminderLetter")
        {
            Tabs.AddTab(() => ReminderLetterEditTab.New(ReminderLetterSearchPage.Setup));
        }
    }
}
