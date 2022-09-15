using eProjectWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Reports.Financials.StatementOfAccount
{
    public class StatementOfAccountEditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(StatementOfAccountEditPage).FullName;

        public StatementOfAccountEditPage() : base("StatementOfAccount")
        {
            Tabs.AddTab(() => StatementOfAccountEditTab.New(StatementOfAccountSearchPage.Setup));
        }
    }
}
