using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;

namespace eLog.HeavyTools.Masters.Partner
{
    public class OlcPartnCmpCustEditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcPartnCmpCustEditPage).FullName;

        protected OlcPartnCmpCustEditPage() : base("olcpartncmp")
        {
            Tabs.AddTab(() => OlcPartnCmpCustEditTab.New(PartnerSearchPage3.Custom));
        }

    }
}
