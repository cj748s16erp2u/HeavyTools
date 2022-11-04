using eProjectWeb.Framework.UI.Templates;
using eProjectWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhZone.WhZTran
{
    public class OlcWhZReceivingLocSearchTab : SearchTabTemplate1
    {
        public static OlcWhZReceivingLocSearchTab New(DefaultPageSetup setup, PageMode pageMode = PageMode.Default)
        {
            var t = (OlcWhZReceivingLocSearchTab)ObjectFactory.New(typeof(OlcWhZReceivingLocSearchTab));
            t.Initialize("OlcWhZReceivingLoc", setup, DefaultActions.Basic /*| DefaultActions.Print2*/, pageMode);
            return t;
        }
    }
}
