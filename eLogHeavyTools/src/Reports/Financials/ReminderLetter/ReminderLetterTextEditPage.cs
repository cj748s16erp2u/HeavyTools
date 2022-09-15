using eProjectWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Reports.Financials.ReminderLetter
{
    public class ReminderLetterTextEditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(ReminderLetterTextEditPage).FullName;

        public ReminderLetterTextEditPage() : base(nameof(ReminderLetterText))
        {
            Tabs.AddTab(() => ReminderLetterTextEditTab.New(ReminderLetterSearchPage.SetupText));
        }
    }
}
