using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Templates;
namespace eLog.HeavyTools.Warehouse.StockTran
{
    internal class TransferingOnRoadPage : TabbedPageInfoProvider
    {
        public static string ID = typeof(TransferingOnRoadPage).FullName;

        public static DefaultPageSetup Setup = new DefaultPageSetup("TransferingOnRoad", TransferingOnRoadBL.ID, null, null, typeof(TransferingHeadRules3));

        public TransferingOnRoadPage()
            : base("TransferingOnRoad")
        {
            Tabs.AddTab(delegate () { return TransferingOnRoadTab.New(Setup); });
        }
    }
}
