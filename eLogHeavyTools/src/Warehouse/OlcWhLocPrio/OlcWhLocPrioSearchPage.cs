using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocPrio
{
    public class OlcWhLocPrioSearchPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcWhLocPrioSearchPage).FullName;
        public static DefaultPageSetup Setup = new DefaultPageSetup(nameof(OlcWhLocPrio), OlcWhLocPrioBL.ID, OlcWhLocPrioSearchProvider.ID, OlcWhLocPrioEditPage.ID, typeof(OlcWhLocPrioRules));
        public static DefaultPageSetup SetupAll = new DefaultPageSetup(nameof(OlcWhLocPrio), OlcWhLocPrioBL.ID, OlcWhLocPrioAllSearchProvider.ID, OlcWhLocPrioEditPage.ID, typeof(OlcWhLocPrioRules));

        public OlcWhLocPrioSearchPage() : base(nameof(OlcWhLocPrio))
        {
            Tabs.AddTab(() => OlcWhLocPrioSearchTab.New(Setup));
            Tabs.AddTab(() => OlcWhLocPrioAllSearchTab.New(SetupAll));
        }
    }
}
