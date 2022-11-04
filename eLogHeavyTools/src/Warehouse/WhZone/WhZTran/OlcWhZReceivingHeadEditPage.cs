using eLog.HeavyTools.Setup.Warehouse;
using eProjectWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhZone.WhZTran
{
    public class OlcWhZReceivingHeadEditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcWhZReceivingHeadEditPage).FullName;

        public OlcWhZReceivingHeadEditPage() : base(nameof(OlcWhZTranHead))
        {
            Tabs.AddTab(() => OlcWhZReceivingHeadEditTab.New(OlcWhZReceivingSearchPage.Setup));
        }
    }
}
