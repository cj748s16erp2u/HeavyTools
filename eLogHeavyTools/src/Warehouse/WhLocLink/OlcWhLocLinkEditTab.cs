using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocLink
{
    public class OlcWhLocLinkEditTab : EditTabTemplate1<OlcWhLocLink, OlcWhLocLinkRules, OlcWhLocLinkBL>
    {
        public static OlcWhLocLinkEditTab New(DefaultPageSetup setup)
        {
            var t = ObjectFactory.New<OlcWhLocLinkEditTab>();
            t.Initialize("OlcWhLocLink", setup);
            return t;
        }

        protected OlcWhLocLinkEditTab() { }

    }
}
