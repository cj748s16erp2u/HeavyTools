using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Templates;

namespace eLog.HeavyTools.Purchase.PinvPackage
{
    public class ApprovedPackageSearchPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(ApprovedPackageSearchPage).FullName;

        public static DefaultPageSetup Package = new DefaultPageSetup("ApprovedPackage", null, ApprovedPackageSearchProvider.ID, null, null);
        public static DefaultPageSetup Head = new DefaultPageSetup("ApprovedPinvHead", null, ApprovedPinvHeadPackageSearchProvider.ID, null, null);
        public static DefaultPageSetup Line = new DefaultPageSetup("ApprovedPinvLine", null, ApprovedPinvLinePackageSearchProvider.ID, null, null);
        public static DefaultPageSetup Attachment = new DefaultPageSetup("ApprovedAttachment", null, ApprovedPackageAttachmentSearchProvider.ID, null, null);

        public ApprovedPackageSearchPage() : base("ApprovedPackage")
        {
            Tabs.AddTab(() => ApprovedPackageSearchTab.New(Package));
            Tabs.AddTab(() => ApprovedPinvHeadPackageSearchTab.New(Head));
            Tabs.AddTab(() => ApprovedPinvLinePackageSearchTab.New(Line));
            Tabs.AddTab(() => ApprovedPackageAttachmentSearchTab.New(Attachment));
        }

    }
}
