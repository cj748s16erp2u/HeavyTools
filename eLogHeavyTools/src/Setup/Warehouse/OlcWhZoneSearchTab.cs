using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Setup.Warehouse
{
    public class OlcWhZoneSearchTab : DetailSearchTabTemplate1
    {
        public static OlcWhZoneSearchTab New(DefaultPageSetup setup)
        {
            var t = ObjectFactory.New<OlcWhZoneSearchTab>();
            t.Initialize(nameof(OlcWhZone), setup, "$noroot_OlcWhZone", DefaultActions.Basic | DefaultActions.HideUnhide);
            return t;
        }

        protected OlcWhZoneSearchTab() { }
    }
}
