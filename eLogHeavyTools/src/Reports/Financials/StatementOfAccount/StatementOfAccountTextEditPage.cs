using eProjectWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Reports.Financials.StatementOfAccount
{
    public class StatementOfAccountTextEditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(StatementOfAccountTextEditPage).FullName;

        public StatementOfAccountTextEditPage() : base(nameof(StatementOfAccountText))
        {
            Tabs.AddTab(() => StatementOfAccountTextEditTab.New(StatementOfAccountSearchPage.SetupText));
        }
    }
}
