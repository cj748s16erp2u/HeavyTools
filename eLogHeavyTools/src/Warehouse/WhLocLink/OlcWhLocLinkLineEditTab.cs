using eProjectWeb.Framework.UI.Templates;
using eProjectWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocLink
{
    public class OlcWhLocLinkLineEditTab : EditTabTemplate1<OlcWhLocLinkLine, OlcWhLocLinkLineRules, OlcWhLocLinkLineBL>
    {
        public static OlcWhLocLinkLineEditTab New(DefaultPageSetup setup)
        {
            var t = ObjectFactory.New<OlcWhLocLinkLineEditTab>();
            t.Initialize("LocLineTab", setup);
            return t;
        }
    }
}
