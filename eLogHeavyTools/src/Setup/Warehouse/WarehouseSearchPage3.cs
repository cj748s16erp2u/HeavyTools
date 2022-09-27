using eProjectWeb.Framework.UI.PageParts;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Setup.Warehouse
{
    public class WarehouseSearchPage3 : Base.Setup.Warehouse.WarehouseSearchPage
    {
        protected static readonly System.Type baseType = typeof(Base.Setup.Warehouse.WarehouseSearchPage);

        protected override string GetNamespaceName()
        {
            return baseType.Namespace;
        }

        public override string ACGetTypeFullName()
        {
            return baseType.FullName;
        }

        public static DefaultPageSetup SetupOlcWhZone = new DefaultPageSetup(nameof(OlcWhZone), OlcWhZoneBL.ID, OlcWhZoneSearchProvider.ID, OlcWhZoneEditPage.ID, typeof(OlcWhZoneRules));
        public static DefaultPageSetup SetupOlcWhLocation = new DefaultPageSetup(nameof(OlcWhLocation), OlcWhLocationBL.ID, OlcWhLocationSearchProvider.ID, OlcWhLocationEditPage.ID, typeof(OlcWhLocationRules));

        protected static TabCreatorDelegate OlcWhZoneDelegate = () => OlcWhZoneSearchTab.New(SetupOlcWhZone);
        protected static TabCreatorDelegate OlcWhLocationDelegate = () => OlcWhLocationSearchTab.New(SetupOlcWhLocation);

        protected WarehouseSearchPage3() : base()
        {
            // Minden Tab-ot eltavolitunk, ami nem a Warehouse alap fulet teszi fel
            var tabCreators = this.Tabs.GetTabCreators();
            for (var i = tabCreators.Length - 1; i >= 1; i--)
            {
                this.Tabs.RemoveTab(tabCreators[i]);
            }

            this.Tabs.AddTab(OlcWhZoneDelegate);
            this.Tabs.AddTab(OlcWhLocationDelegate);
        }
    }
}
