using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Reports.Financials.StatementOfAccount
{
    public class StatementOfAccountSearchPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(StatementOfAccountSearchPage).FullName;
        public static DefaultPageSetup Setup = new DefaultPageSetup("StatementOfAccount", StatementOfAccountBL.ID, StatementOfAccountSearchProvider.ID, StatementOfAccountEditPage.ID, typeof(StatementOfAccountRules));
        public static DefaultPageSetup SetupText = new DefaultPageSetup("StatementOfAccountText", StatementOfAccountTextBL.ID, StatementOfAccountTextSearchProvider.ID, StatementOfAccountTextEditPage.ID, typeof(StatementOfAccountTextRules));

        public StatementOfAccountSearchPage() : base("StatementOfAccount")
        {
            Tabs.AddTab(() => StatementOfAccountSearchTab.New(Setup));
            Tabs.AddTab(() => StatementOfAccountTextSearchTab.New(SetupText));
        }
    }
}
