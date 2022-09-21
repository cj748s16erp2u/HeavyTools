using eProjectWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocPrio
{
    public class WhLocPrioEditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(WhLocPrioEditPage).FullName;

        public WhLocPrioEditPage() : base(nameof(OlcWhLocPrio))
        {
            this.Tabs.AddTab(() => WhLocPrioEditTab.New(WhLocPrioSearchPage.Setup));
        }
    }
}
