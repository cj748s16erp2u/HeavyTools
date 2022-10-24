using eLog.Base.Warehouse.StCostPriceCalc;
using eProjectWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocLink
{
    public class OlcWhLocLinkEditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcWhLocLinkEditPage).FullName;
        public OlcWhLocLinkEditPage() : base("OlcWhLoc")
        {
            Tabs.AddTab(() => OlcWhLocLinkEditTab.New(OlcWhLocLinkSearchPage.Setup));
        }
    }
}
