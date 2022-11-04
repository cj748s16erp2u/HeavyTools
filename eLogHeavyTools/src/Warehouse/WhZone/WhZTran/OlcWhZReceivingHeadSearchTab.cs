using eLog.HeavyTools.Setup.PriceTable;
using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace eLog.HeavyTools.Warehouse.WhZone.WhZTran
{
    public class OlcWhZReceivingHeadSearchTab : SearchTabTemplate1
    {
        public static OlcWhZReceivingHeadSearchTab New(DefaultPageSetup setup, PageMode pageMode = PageMode.Default)
        {
            var t = (OlcWhZReceivingHeadSearchTab)ObjectFactory.New(typeof(OlcWhZReceivingHeadSearchTab));
            t.Initialize("OlcWhZReceivingHead", setup, DefaultActions.View | DefaultActions.Basic);
            return t;
        }
    }
}
