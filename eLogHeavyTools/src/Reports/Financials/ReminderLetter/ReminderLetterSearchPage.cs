using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Reports.Financials.ReminderLetter
{
    public class ReminderLetterSearchPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(ReminderLetterSearchPage).FullName;
        public static DefaultPageSetup Setup = new DefaultPageSetup("ReminderLetter", ReminderLetterBL.ID, ReminderLetterSearchProvider.ID, ReminderLetterEditPage.ID, typeof(ReminderLetterRules));
        public static DefaultPageSetup SetupText = new DefaultPageSetup("ReminderLetterText", ReminderLetterTextBL.ID, ReminderLetterTextSearchProvider.ID, ReminderLetterTextEditPage.ID, typeof(ReminderLetterTextRules));

        public ReminderLetterSearchPage() : base("ReminderLetter")
        {
            Tabs.AddTab(() => ReminderLetterSearchTab.New(Setup));
            Tabs.AddTab(() => ReminderLetterTextSearchTab.New(SetupText));
        }
    }
}
