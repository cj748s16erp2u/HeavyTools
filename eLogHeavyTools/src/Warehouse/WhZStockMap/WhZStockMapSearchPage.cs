using eLog.Base.Warehouse.StockMap;
using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhZStockMap
{
    public class WhZStockMapSearchPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(WhZStockMapSearchPage).FullName;
        public static DefaultPageSetup Setup = new DefaultPageSetup(nameof(WhZStockMapDto), null, WhZStockMapSearchProvider.ID, null);

        public WhZStockMapSearchPage() : base(nameof(WhZStockMapDto))
        {
            Tabs.AddTab(()=>WhZStockMapSearchTab.New(Setup));
        }
    }
}
