using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhZone.WhZTran
{
    public class OlcWhZReceivingLineSearchTab : SearchTabTemplate1
    {
        public static OlcWhZReceivingLineSearchTab New(DefaultPageSetup setup, PageMode pageMode = PageMode.Default)
        {
            var t = (OlcWhZReceivingLineSearchTab)ObjectFactory.New(typeof(OlcWhZReceivingLineSearchTab));
            t.Initialize("OlcWhZReceivingLine", setup, DefaultActions.View | DefaultActions.Basic | DefaultActions.Delete | DefaultActions.Print /*| DefaultActions.Print2*/, pageMode);
            return t;
        }
    }
}
