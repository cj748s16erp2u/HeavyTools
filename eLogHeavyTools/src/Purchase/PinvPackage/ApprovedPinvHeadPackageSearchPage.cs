using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Templates;

namespace eLog.HeavyTools.Purchase.PinvPackage
{
    public class ApprovedPinvHeadPackageSearchPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(ApprovedPinvHeadPackageSearchPage).FullName;

        public static DefaultPageSetup Head = new DefaultPageSetup("ApprovedPinvHead", null, ApprovedPinvHeadPackageSearchProvider.ID, null, null);
        public static DefaultPageSetup Line = new DefaultPageSetup("ApprovedPinvLines", null, ApprovedPinvLinePackageSearchProvider.ID, null, null);

        public ApprovedPinvHeadPackageSearchPage() : base("ApprovedPinvHead")
        {
            Tabs.AddTab(() => ApprovedPinvHeadPackageSearchTab.New(Head));
            Tabs.AddTab(() => ApprovedPinvLinePackageSearchTab.New(Line));
        }

    }
}
