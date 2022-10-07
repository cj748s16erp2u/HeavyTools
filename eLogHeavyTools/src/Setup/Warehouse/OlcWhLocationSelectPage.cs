using eLog.Base.Masters.Partner;
using eLog.HeavyTools.Warehouse.WhLocPrio;
using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Setup.Warehouse
{
    public class OlcWhLocationSelectPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcWhLocationSelectPage).FullName;

        public static DefaultPageSetup Setup = new DefaultPageSetup(nameof(OlcWhLocation), OlcWhLocationBL.ID, OlcWhLocationSearchProvider.ID, null, typeof(OlcWhLocationRules));

        public OlcWhLocationSelectPage() : base(nameof(OlcWhLocation))
        {
            Tabs.AddTab(()=> OlcWhLocationSelectTab.New(Setup,PageMode.Selection));

        }

        public override string ACGetTypeFullName()
        {
            return typeof(WarehouseSearchPage3).FullName;
        }
    }
}
