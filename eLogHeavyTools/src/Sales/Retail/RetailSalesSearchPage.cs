using eLog.Base.Common;
using eLog.Base.Masters.Item;
using eLog.HeavyTools.Sales.Retail.Cart;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.PageParts;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;

namespace eLog.HeavyTools.Sales.Retail
{
    internal class RetailSalesSearchPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(RetailSalesSearchPage).FullName;

        public static DefaultPageSetup Setup = new DefaultPageSetup("RetailSales", RetailSalesBL.ID,
                                                                    RetailSalesSearchProvider.ID, RetailSalesEditPage.ID,
                                                                    typeof(RetailSalesRules));


        public RetailSalesSearchPage()
            : base("RetailSales")
        {
            Tabs.AddTab(delegate { return RetailSalesSearchTab.New(Setup); });
        }
    }

    internal class RetailSalesRules : TypedBaseRuleSet<OlcCart>
    {
        public RetailSalesRules()
            : base(true)
        {

        }
    }

    internal class RetailSalesEditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(RetailSalesEditPage).FullName;

        public RetailSalesEditPage()
            : base("RetailSales")
        {
            Tabs.AddTab(delegate { return RetailSalesEditTab.New(RetailSalesSearchPage.Setup); });
        }
    }

    internal class RetailSalesEditTab : EditTabTemplate1<OlcCart, RetailSalesRules, RetailSalesBL>
    {
        protected RetailSalesEditTab()
        {
        }

        public static RetailSalesEditTab New(DefaultPageSetup setup)
        {
            var t = (RetailSalesEditTab)ObjectFactory.New(typeof(RetailSalesEditTab));
            t.Initialize("RetailSales", setup);
            return t;
        }
    }

    internal class RetailSalesSearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(RetailSalesSearchProvider).FullName;

        public static string DefaultOrderByStr = null;

        protected static string m_queryString = @"select c.*, i.itemcode, i.name01, a.name, p.name pname, p.partncode
  from olc_cart c
  left join ols_item i on i.itemid=c.itemid
  left join olc_action a on a.aid=c.aid
  left join olc_partner cp on cp.loyaltycardno=c.loyaltyCardNo
  left join ols_partner p on p.partnid=cp.partnid

";

        protected static QueryArg[] m_argsTemplate = new QueryArg[]
        {
            new QueryArg("cartid", "c", FieldType.Integer, QueryFlags.Equals),
            new QueryArg("addusrid", "c", FieldType.String, QueryFlags.Equals),
        };

        protected RetailSalesSearchProvider()
            : base(m_queryString, m_argsTemplate, SearchProviderType.Default)
        {
        }
        protected override string CreateQueryString(Dictionary<string, object> args, bool fmtonly)
        {
            args.Add("addusrid", Session.Current.UserID);
            return base.CreateQueryString(args, fmtonly);
        }
    }

    internal class RetailSalesBL : DefaultBL1<OlcCart, RetailSalesRules>
    {
        public static readonly string ID = typeof(RetailSalesBL).FullName;

        protected RetailSalesBL()
            : base(DefaultBLFunctions.BasicHideUnhide)
        {
        }

        public static RetailSalesBL New()
        {
            return (RetailSalesBL)ObjectFactory.New(typeof(RetailSalesBL));
        }
        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);

            object x = objects[typeof(OlcCart).Name];
            if (x != null)
                RuleServer.Validate(objects, typeof(RetailSalesRules));

        }
    }

    internal class RetailSalesSearchTab : SearchTabTemplate1
    {
        DialogBox dbCupon = new DialogBox(DialogBoxType.InputOkCancel);
        DialogBox dbLoyaltyCardNo = new DialogBox(DialogBoxType.InputOkCancel);
        
        public static RetailSalesSearchTab New(DefaultPageSetup setup)
        {
            var t = (RetailSalesSearchTab)ObjectFactory.New(typeof(RetailSalesSearchTab));
            t.Initialize("RetailSales", setup, DefaultActions.None);
            return t;
        }
        protected override void CreateBase()
        {
            base.CreateBase();
            var addItemButton = new Button("AddItem");
            AddCmd(addItemButton);
            addItemButton.SetOnClick(OnAddItem);

            var recalcItemButton = new Button("Recalc");
            AddCmd(recalcItemButton);
            recalcItemButton.SetOnClick(OnRecalc);

            var deleteButton = new Button("Delete");
            AddCmd(deleteButton);
            deleteButton.SetOnClick(OnDelete);

            var cuponButton = new Button("Cupon");
            AddCmd(cuponButton);
            cuponButton.SetOnClick(OnCupon);

            var loyaltycardnoButton = new Button("LoyaltyCardNo");
            AddCmd(loyaltycardnoButton);
            loyaltycardnoButton.SetOnClick(OnLoyaltyCardNo);

            RegisterDialog(dbCupon);
            RegisterDialog(dbLoyaltyCardNo);

            dbCupon.OnButton1Clicked += this.DbCupon_OnButton1Clicked;
            dbLoyaltyCardNo.OnButton1Clicked += this.DbLoyaltyCardNo_OnButton1Clicked;


        }

        private void DbLoyaltyCardNo_OnButton1Clicked(PageUpdateArgs args)
        {
            var loyaltycardno = dbCupon.GetStringValue("loyaltycardno", args);

            new CartBL().SetLoyaltyCardNo(loyaltycardno);
            SearchResults.ForceRefresh = true;
            args.Continue = true;
        }

        private void DbCupon_OnButton1Clicked(PageUpdateArgs args)
        { 
            var cupon = dbCupon.GetStringValue("cupon", args);

            new CartBL().AddCupon(cupon);
            SearchResults.ForceRefresh = true;
            args.Continue = true;
        }

        private void OnLoyaltyCardNo(PageUpdateArgs args)
        {
            args.ShowInputDialog(dbLoyaltyCardNo,
                 "addLoyaltyCardNo", "", new Control[] {
                   new Textbox("loyaltycardno") { Width = 200, Value=null},
             });
        }

        private void OnCupon(PageUpdateArgs args)
        {
            args.ShowInputDialog(dbCupon,
                  "addCupon", "", new Control[] {
                   new Textbox("cupon") { Width = 200, Value=null},
              });
        }

        private void OnDelete(PageUpdateArgs args)
        {
            if (SearchResults.SelectedPK.Count > 0)
            {
                foreach (var pk in SearchResults.SelectedPKS)
                {
                    OlcCart.Delete(pk);
                }
            }
            OnRecalc(args);
        }

        private void OnRecalc(PageUpdateArgs args)
        {
            new CartBL().Recalc();

            SearchResults.ForceRefresh = true;
        }

        private void OnAddItem(PageUpdateArgs args)
        {
            var item = (Selector)SrcBar["itemid_itemcode"];

            if (item.Value!= null)
            {
                var itemid = (int)item.Value;
                new CartBL().AddNewItem(itemid);
                SearchResults.ForceRefresh = true;
            }
        }
    }
}
