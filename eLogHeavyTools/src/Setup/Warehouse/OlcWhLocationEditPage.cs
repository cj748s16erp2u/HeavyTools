using eProjectWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Setup.Warehouse
{
    public class OlcWhLocationEditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcWhLocationEditPage).FullName;

        public OlcWhLocationEditPage() : base(nameof(OlcWhLocation))
        {
            Tabs.AddTab(() => OlcWhLocationEditTab.New(WarehouseSearchPage3.SetupOlcWhLocation));
        }
    }
}
