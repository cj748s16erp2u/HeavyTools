using eProjectWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhZone.WhZTran
{
    public class OlcWhZReceivingLineEditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcWhZReceivingLineEditPage).FullName;

        public OlcWhZReceivingLineEditPage() : base(nameof(OlcWhZTranLine))
        {
            Tabs.AddTab(() => OlcWhZReceivingLineEditTab.New(OlcWhZReceivingSearchPage.SetupLine));
        }
    }
}
