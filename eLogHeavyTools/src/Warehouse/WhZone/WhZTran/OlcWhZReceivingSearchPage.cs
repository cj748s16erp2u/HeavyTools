using eLog.Base.Warehouse.StCostPriceCalc;
using eLog.Base.Warehouse.StockTran;
using eLog.HeavyTools.Setup.PriceTable;
using eLog.HeavyTools.Setup.Warehouse;
using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.PageParts;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhZone.WhZTran
{
    public class OlcWhZReceivingSearchPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcWhZReceivingSearchPage).FullName;

        public static DefaultPageSetup Setup = new DefaultPageSetup("OlcWhZReceivingHead", OlcWhZReceivingHeadBL.ID, OlcWhZReceivingHeadSearchProvider.ID, OlcWhZReceivingHeadEditPage.ID, typeof(OlcWhZReceivingHeadRules));
        public static DefaultPageSetup SetupLine = new DefaultPageSetup("OlcWhZReceivingLine", OlcWhZReceivingLineBL.ID, OlcWhZReceivingLineSearchProvider.ID, OlcWhZReceivingLineEditPage.ID, typeof(OlcWhZReceivingLineRules));
        public static DefaultPageSetup SetupLoc = new DefaultPageSetup("OlcWhZReceivingLoc", OlcWhZReceivingLocBL.ID, OlcWhZReceivingLocSearchProvider.ID, OlcWhZReceivingLocEditPage.ID, typeof(OlcWhZReceivingLocRules));

        //public static DefaultPageSetup SetupLot = new DefaultPageSetup("OlcWhZReceivingLot", LotBL.ID, LotSearchProvider.ID, ReceivingLotEditPage.ID);

        //public static DefaultPageSetup SetupLineCost = new DefaultPageSetup("CostLine", eLog.Base.Purchase.Pinv.CostLineBL.ID, ReceivingLineCostSearchProvider.ID, null);
        //public static DefaultPageSetup SetupLineReq = new DefaultPageSetup("Requirement", eLog.Base.Manufacture.Mord.RequirementBL.ID, ReceivingReqSearchProvider.ID, ReceivingReqEditPage.ID, typeof(eLog.Base.Manufacture.Mord.RequirementRules));

        protected static TabCreatorDelegate OlcWhZReceivingHeadDelegate = () => OlcWhZReceivingHeadSearchTab.New(Setup);
        protected static TabCreatorDelegate OlcWhZReceivingLineDelegate = () => OlcWhZReceivingLineSearchTab.New(SetupLine);
        protected static TabCreatorDelegate OlcWhZReceivingLocDelegate = () => OlcWhZReceivingLocSearchTab.New(SetupLoc);
        //protected static TabCreatorDelegate OlcWhZReceivingLotDelegate;
        //protected static TabCreatorDelegate OlcWhZPrcTestDelegate;
        //protected static TabCreatorDelegate OlcWhZReceivingCostDelegate;
        //protected static TabCreatorDelegate OlcWhZReceivingReqDelegate;

        public OlcWhZReceivingSearchPage() : base("OlcWhZTran")
        {
            this.Tabs.AddTab(OlcWhZReceivingHeadDelegate);
            this.Tabs.AddTab(OlcWhZReceivingLineDelegate);
            this.Tabs.AddTab(OlcWhZReceivingLocDelegate);
        }
    }
}
