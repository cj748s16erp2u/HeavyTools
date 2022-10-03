using eProjectWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocPrio
{
    public class OlcWhLocPrioEditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcWhLocPrioEditPage).FullName;

        public OlcWhLocPrioEditPage() : base(nameof(OlcWhLocPrio))
        {
            this.Tabs.AddTab(() => OlcWhLocPrioEditTab.New(OlcWhLocPrioSearchPage.Setup));
        }
    }
}
