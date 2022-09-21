using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocPrio
{
    public class WhLocPrioSearchPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(WhLocPrioSearchPage).FullName;
        public static DefaultPageSetup Setup = new DefaultPageSetup(nameof(OlcWhLocPrio), OlcWhLocPrioBL.ID, WhLocPrioSearchProvider.ID, WhLocPrioEditPage.ID,typeof(OlcWhLocPrioRules));
        public static DefaultPageSetup SetupAll = new DefaultPageSetup(nameof(OlcWhLocPrio), OlcWhLocPrioBL.ID, WhLocPrioAllSearchProvider.ID, WhLocPrioEditPage.ID, typeof(OlcWhLocPrioRules));
        
        public WhLocPrioSearchPage():base(nameof(OlcWhLocPrio))
        {
            Tabs.AddTab(() => WhLocPrioSearchTab.New(Setup));
            Tabs.AddTab(() => WhLocPrioAllSearchTab.New(SetupAll));
        }
    }
}
