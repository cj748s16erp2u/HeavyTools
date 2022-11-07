using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;

namespace eLog.HeavyTools.Warehouse.StockTranLocation
{
    public class ReceivingStLocCustomEditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(ReceivingStLocCustomEditPage).FullName;

        public ReceivingStLocCustomEditPage() : base(ReceivingStLocCustomSearchTab.SetupStLocCustom.MainEntity)
        {
            Tabs.AddTab(() => ReceivingStLocCustomEditTab.New(ReceivingStLocCustomSearchTab.SetupStLocCustom));
        }
    }
}
