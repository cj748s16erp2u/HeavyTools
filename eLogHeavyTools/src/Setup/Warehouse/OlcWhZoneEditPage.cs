using eProjectWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Setup.Warehouse
{
    public class OlcWhZoneEditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcWhZoneEditPage).FullName;

        public OlcWhZoneEditPage() : base(nameof(OlcWhZone))
        {
            Tabs.AddTab(() => OlcWhZoneEditTab.New(WarehouseSearchPage3.SetupOlcWhZone));
        }
    }
}
