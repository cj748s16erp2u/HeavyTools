using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Templates;

namespace eLog.HeavyTools.Warehouse.StockTranLocation
{
    public class ReceivingStLocCustomSearchTab : DetailSearchTabTemplate1
    {
        public static DefaultPageSetup SetupStLocCustom = new DefaultPageSetup("StLocCustom", ReceivingStLocCustomBL.ID, ReceivingStLocCustomSearchProvider.ID, null);

        public static ReceivingStLocCustomSearchTab New(DefaultPageSetup setup)
        {
            var t = ObjectFactory.New<ReceivingStLocCustomSearchTab>();
            t.Initialize("StLocCustom", setup, "$noroot_ReceivingStLocCustom", DefaultActions.Basic | DefaultActions.Delete);
            return t;
        }

        protected ReceivingStLocCustomSearchTab() { }
    }
}
