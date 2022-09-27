using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.PageParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Sales.Retail.Cart
{
    internal class BarcodeSelectPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(BarcodeSelectPage).FullName;
         
        public BarcodeSelectPage()
            : base("RetailSales")
        {
            Tabs.AddTab(delegate { return BarcodeSelectTab.New(); });
        }
    }
    internal class BarcodeSelectTab : TabPage2
    {
        public static BarcodeSelectTab New()
        {
            var t = (BarcodeSelectTab)ObjectFactory.New(typeof(BarcodeSelectTab));
            t.Initialize("RetailSales");
            return t;
        }
        protected override void Initialize(string labelID)
        {
            base.Initialize(labelID);
            CreateControls();
            var b = new Button("close");
            AddCmd(b);
            b.SetOnClick(OnClose);

        }

        private void OnClose(PageUpdateArgs args)
        {
            args.ClosePage();
        }

        protected override void CreateControls()
        {
            base.CreateControls(); 
            OnPageActivate += this.BarcodeSelectTab_OnPageActivate;
        }

        private void BarcodeSelectTab_OnPageActivate(PageUpdateArgs args)
        {
            args.ClosePage();
        }
        
    }
} 
