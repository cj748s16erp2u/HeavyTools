using eProjectWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhZone.WhZTran
{
    public class OlcWhZReceivingLocEditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcWhZReceivingLocEditPage).FullName;

        public OlcWhZReceivingLocEditPage() : base(nameof(OlcWhZTranLoc))
        {
            Tabs.AddTab(() => OlcWhZReceivingLocEditTab.New(OlcWhZReceivingSearchPage.SetupLoc));
        }
    }
}
