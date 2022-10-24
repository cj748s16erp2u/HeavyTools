using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Templates;

namespace eLog.HeavyTools.Warehouse.StockTranLocation
{
    public class ReceivingStLocCustomEditTab : EditTabTemplate1<ReceivingStLocCustomDto, ReceivingStLocCustomRules, ReceivingStLocCustomBL>
    {
        public static ReceivingStLocCustomEditTab New(DefaultPageSetup setup)
        {
            var t = ObjectFactory.New<ReceivingStLocCustomEditTab>();
            t.Initialize(setup.MainEntity, setup);
            return t;
        }

        protected ReceivingStLocCustomEditTab()
        {
        }
    }
}
