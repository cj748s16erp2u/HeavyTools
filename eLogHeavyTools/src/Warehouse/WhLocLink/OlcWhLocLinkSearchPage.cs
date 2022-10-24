using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.PageParts;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocLink
{
    public class OlcWhLocLinkSearchPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcWhLocLinkSearchPage).FullName;

        /// <summary>
        /// Delegát az Helykódok oldal számára.
        /// </summary>
        public static DefaultPageSetup Setup = new DefaultPageSetup("OlcWhLocLink", OlcWhLocLinkBL.ID, OlcWhLocLinkSearchProvider.ID, OlcWhLocLinkEditPage.ID, typeof(OlcWhLocLinkRules));

        /// <summary>
        /// Delegát a Kapcsolt helykódok oldal számára.
        /// </summary>
        public static DefaultPageSetup SetupLine = new DefaultPageSetup("OlcWhLocLinkLine", OlcWhLocLinkLineBL.ID, OlcWhLocLinkLineSearchProvider.ID, null, typeof(OlcWhLocLinkLineRules));

        public OlcWhLocLinkSearchPage() : base("OlcWhLoc")
        {
            Tabs.AddTab(() => OlcWhLocLinkSearchTab.New(Setup));
            Tabs.AddTab(() => OlcWhLocLinkLineSearchTab.New(SetupLine));
        }
    }
}
