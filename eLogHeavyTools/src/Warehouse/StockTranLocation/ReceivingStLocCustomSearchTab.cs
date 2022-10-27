using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Templates;

namespace eLog.HeavyTools.Warehouse.StockTranLocation
{
    public class ReceivingStLocCustomSearchTab : DetailSearchTabTemplate1
    {
        public static DefaultPageSetup SetupStLocCustom = new DefaultPageSetup("ReceivingStLocCustom", ReceivingStLocCustomBL.ID, ReceivingStLocCustomSearchProvider.ID, ReceivingStLocCustomEditPage.ID);

        public static ReceivingStLocCustomSearchTab New(DefaultPageSetup setup)
        {
            var t = ObjectFactory.New<ReceivingStLocCustomSearchTab>();
            t.Initialize(setup.MainEntity, setup, $"$noroot_{setup.MainEntity}", DefaultActions.View);
            return t;
        }

        protected ReceivingStLocCustomSearchTab() { }

        protected override void CreateBase()
        {
            this.ParentEntityKey = Consts.DetailEntityKey;
            this.DetailEntityKey = null;

            base.CreateBase();
        }

        protected override void CreateCheckForRootEntityKey()
        {
            this.OnActivate.AddStep(new eProjectWeb.Framework.UI.Script.CopyEntityKeyStep(Consts.RootEntityKey, Consts.DetailEntityKey));
            this.OnActivate.AddStep(new eProjectWeb.Framework.UI.Script.CheckEntityKeyStep(TabID, ParentEntityKey));
        }
    }
}
