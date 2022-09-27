using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.PageParts;
using eProjectWeb.Framework.UI.Templates;

namespace eLog.HeavyTools.Masters.Partner
{
    public class PartnerSearchPage3: Base.Masters.Partner.PartnerSearchPage
    {
        public static DefaultPageSetup Custom = new DefaultPageSetup("OlcPartnCmp", OlcPartnCmpCustBL.ID, OlcPartnCmpCustSearchProvider.ID, OlcPartnCmpCustEditPage.ID, null);

        protected TabCreatorDelegate m_PartnerCustomTab;

        public PartnerSearchPage3() : base()
        {
            m_PartnerCustomTab = delegate { return OlcPartnCmpCustTab.New(Custom); };
            Tabs.AddTab(m_PartnerCustomTab);
        }

    }
}