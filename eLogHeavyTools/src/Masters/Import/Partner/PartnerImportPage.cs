using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Import.Partner
{
    public class PartnerImportPage : eProjectWeb.Framework.TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(PartnerImportPage).FullName;

        public PartnerImportPage() : base("partnerimport")
        {
            this.Tabs.AddTab(() => PartnerImportTab.New());
        }
    }
}
