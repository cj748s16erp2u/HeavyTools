using eLog.Base.Common;
using eLog.Base.Masters.Item;
using eLog.Finance.Base.PettyCash.PayMethod;
using eLog.HeavyTools.InterfaceCaller;
using eLog.HeavyTools.Sales.Retail.Cart;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.UI.Actions;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.PageParts;
using eProjectWeb.Framework.UI.Script;
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


    internal class RetailSalesPaySearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(RetailSalesPaySearchProvider).FullName;

        public static string DefaultOrderByStr = null;

        protected static string m_queryString = @"

select cp.*, m.name 
  from olc_cartpay cp
  join fin_paymethod m on m.finpaymid=cp.finpaymid

";

        protected static QueryArg[] m_argsTemplate = new QueryArg[]
        {
            new QueryArg("cartpayid", "cp", FieldType.Integer, QueryFlags.Equals),
            new QueryArg("addusrid", "cp", FieldType.String, QueryFlags.Equals),
        };

        protected RetailSalesPaySearchProvider()
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
        Grid paySearchResults;
        DialogBox dbItem = new DialogBox(DialogBoxType.InputOkCancel);
        DialogBox dbPayCustom = new DialogBox(DialogBoxType.InputOkCancel);

        Numberbox originalValue;
        Numberbox discValue;
        Numberbox totValue;
        Numberbox payValue;
        Numberbox missingValue;
        Textbox barcode;
         

        public static RetailSalesSearchTab New(DefaultPageSetup setup)
        {
            var t = (RetailSalesSearchTab)ObjectFactory.New(typeof(RetailSalesSearchTab));
            t.Initialize("RetailSales", setup, DefaultActions.None);
            return t;
        }
        protected override void CreateBase()
        {
            base.CreateBase();

            FindButton(ActionID.Refresh).Remove = true;
            AddCmd(new Button(ActionID.Refresh + "2", 10) {Shortcut= eProjectWeb.Framework.UI.Commands.ShortcutKeys.Key_F10, ImageUrl = "images/refresh.gif" });
            SetButtonAction(ActionID.Refresh + "2", new RefreshTwinGridAction(SearchGridID, "paySearchResults"));


            var addButton = new PopupButton("add");
            AddCmd(addButton);

            var itemButton = new PopupChildButton("Item");
            addButton.Add(itemButton);
            itemButton.SetOnClick(OnItemButton);
            addButton.DefaultButton = itemButton;
            addButton.AllowDropDown = true;

            var cuponButton = new PopupChildButton("Cupon");
            addButton.Add(cuponButton);
            cuponButton.SetOnClick(OnCupon);

            var loyaltycardnoButton = new PopupChildButton("LoyaltyCardNo");
            addButton.Add(loyaltycardnoButton);
            loyaltycardnoButton.SetOnClick(OnLoyaltyCardNo);


            var payButton = new PopupButton("pay");
            AddCmd(payButton);

            var paycardButton = new PopupChildButton("PayCard");
            payButton.Add(paycardButton);
            paycardButton.SetOnClick(OnPayCard);
            payButton.DefaultButton = paycardButton;
            payButton.AllowDropDown = true;

            var paycashButton = new PopupChildButton("PayCash");
            payButton.Add(paycashButton);
            paycashButton.SetOnClick(OnPayCash);

            var paycustomButton = new PopupChildButton("PayCustom");
            payButton.Add(paycustomButton);
            paycustomButton.SetOnClick(OnPayCustom);
            dbPayCustom.OnButton1Clicked += this.DbPayCustom_OnButton1Clicked;



            var deleteButton = new PopupButton("pay2");
            AddCmd(deleteButton);
            deleteButton.AllowDropDown = true;


            var deleteItemButton = new PopupChildButton("Delete");
            deleteButton.Add(deleteItemButton);
            deleteItemButton.SetOnClick(OnDelete);
            deleteButton.DefaultButton = deleteItemButton;

            var deletePayButton = new PopupChildButton("PayDelete");
            deleteButton.Add(deletePayButton); 
            deletePayButton.SetOnClick(OnPayDelete);

            var deletePayButtonAll = new PopupChildButton("PayDeleteAll");
            deleteButton.Add(deletePayButtonAll);
            deletePayButtonAll.SetOnClick(OnPayDeleteAll);
              
            var deleteItemButtonAll = new PopupChildButton("DeleteAll");
            deleteButton.Add(deleteItemButtonAll);
            deleteItemButtonAll.SetOnClick(OnDeleteAll); 

            RegisterDialog(dbCupon);
            RegisterDialog(dbLoyaltyCardNo);
            RegisterDialog(dbItem);
            RegisterDialog(dbPayCustom);

            dbCupon.OnButton1Clicked += this.DbCupon_OnButton1Clicked;
            dbLoyaltyCardNo.OnButton1Clicked += this.DbLoyaltyCardNo_OnButton1Clicked;
            dbItem.OnButton1Clicked += this.DbItem_OnButton1Clicked;

            var adminButton = new PopupButton("admin");
            AddCmd(adminButton);

    
            var recalcItemButton = new PopupChildButton("Recalc");
            adminButton.Add(recalcItemButton);
            recalcItemButton.SetOnClick(OnRecalc);

            var clearCartCache = new PopupChildButton("clearCartCache");
            adminButton.Add(clearCartCache);
            clearCartCache.SetOnClick(OnCartCache);

            var reloadCactions = new PopupChildButton("reloadActions");
            adminButton.Add(reloadCactions);
            reloadCactions.SetOnClick(OnReloadCactions);

            paySearchResults = (Grid)FindRenderable("paySearchResults");
           // paySearchResults.ControlGroup = "vsplit_right";
            paySearchResults.SearchID = RetailSalesPaySearchProvider.ID;
            paySearchResults.SetColumns(SearchServer.Get(RetailSalesPaySearchProvider.ID).GetColumns());
            
            OnPageLoad += this.RetailSalesSearchTab_OnPageLoad;

            originalValue = FindRenderable<Numberbox>("originalValue");
            discValue = FindRenderable<Numberbox>("discValue");
            totValue = FindRenderable<Numberbox>("totValue");
            payValue = FindRenderable<Numberbox>("payValue");
            missingValue = FindRenderable<Numberbox>("missingValue");


            barcode = FindRenderable<Textbox>("barcode");
            barcode.SetOnEnterPressed(OnBarcode);
             
            var sinvbutton = new Button("sinv");
            AddCmd(sinvbutton);

            SetButtonAction("sinv", new EditRecordCallbackAction(RetailSinvPage.ID, 
                eEditRecordCallbackFlags.CheckForContinue), new ControlEvent(OnSinv));



        }

        private void OnSinv(PageUpdateArgs args)
        {
            var bl = GetCartBL();
            var t = bl.GetCartTotal();
            if (t.GetMissingValue() != 0)
            {
                throw new MessageException("$sinvpaynotequal");
            } else
            {
                args.Continue = true;
            }
        }

        private void OnPayDeleteAll(PageUpdateArgs args)
        {
            SqlDataAdapter.ExecuteNonQuery(DB.Main, $@"delete olc_cartpay where addusrid='{Session.Current.UserID}'");

            GetCartBL().FillTotal();
            paySearchResults.ForceRefresh = true;
            SearchResults.ForceRefresh = true;
        }

        private void OnDeleteAll(PageUpdateArgs args)
        {
            SqlDataAdapter.ExecuteNonQuery(DB.Main, $@"delete olc_cart where addusrid='{Session.Current.UserID}'");
            SqlDataAdapter.ExecuteNonQuery(DB.Main, $@"delete olc_cartpay where addusrid='{Session.Current.UserID}'");

            GetCartBL().Recalc();
            paySearchResults.ForceRefresh = true;
            SearchResults.ForceRefresh = true;
        }

        private void OnPayDelete(PageUpdateArgs args)
        { 
            if (paySearchResults.SelectedPK.Count > 0)
            {
                foreach (var pk in paySearchResults.SelectedPKS)
                {
                    OlcCartPay.Delete(pk);
                }
            }
             
            GetCartBL().FillTotal();
            paySearchResults.ForceRefresh = true;
            args.Continue = true;
        }

        private void DbPayCustom_OnButton1Clicked(PageUpdateArgs args)
        {
            var finpaymid = dbPayCustom.GetStringValue("finpaymid", args); 
            var barcode = dbPayCustom.GetStringValue("barcode", args);

            decimal payvalue = 0;

            object inputValueObject = dbPayCustom.GetInputValueObject("payvalue", args);
            if (inputValueObject != null)
            {
                try
                {
                    payvalue = (ConvertUtils.ToDecimal(inputValueObject)).Value;
                }
                catch (Exception)
                {
                    throw new MessageException("$invalidPayvalue");
                }
            }

            var pm = PayMethod.Load(finpaymid);
            if (pm==null)
            {
                throw new MessageException("$invalidFinpaymid");
            }
            
             
            var cp = OlcCartPay.CreateNew();
            cp.Finpaymid = finpaymid;
            cp.Payvalue = payvalue;
            cp.Barcode = barcode;
            cp.Save();

            GetCartBL().FillTotal();
            paySearchResults.ForceRefresh = true;
            args.Continue = true;
        }

        private void OnPayCustom(PageUpdateArgs args)
        {
            args.ShowInputDialog(dbPayCustom,
              "$paycustom", "", new Control[] {
                   new Combo("finpaymid", PayMethodList.ID){ Width=200},
                   new ForceNextRow(),
                   new Numberbox("payvalue"){ Width=200},
                   new ForceNextRow(),
                   new Textbox("barcode"){ Width=200}
          });
        }

        private void OnPayCash(PageUpdateArgs args)
        {
            var bl = GetCartBL();
            var m = bl.GetCartTotal().GetMissingValue();
            if (m > 0)
            {
                var cp = OlcCartPay.CreateNew();
                cp.Finpaymid = "KP";
                cp.Payvalue = m;
                cp.Save();
            }
            bl.FillTotal();
            paySearchResults.ForceRefresh = true;
        }

        private void OnPayCard(PageUpdateArgs args)
        {
            var bl = GetCartBL();
            var m = bl.GetCartTotal().GetMissingValue();
            if (m > 0)
            {
                var cp=OlcCartPay.CreateNew();
                cp.Finpaymid = "KARTYA";
                cp.Payvalue = m;
                cp.Save();
            }
            bl.FillTotal();
            paySearchResults.ForceRefresh = true;
        }

        private void OnBarcode(PageUpdateArgs args)
        {
            var ie=ItemExt.Load(new Key() { { ItemExt.FieldExtcode.Name, barcode.Value } });

            if (ie != null)
            {
                GetCartBL().AddNewItem(ie.Itemid.Value);
                SearchResults.ForceRefresh = true;
                paySearchResults.ForceRefresh = true;
                args.Continue = true;

                barcode.Value = "";
            }
        }

        private CartBL GetCartBL()
        {
            return new CartBL(originalValue, discValue, totValue, payValue, missingValue);
        } 
        private void OnReloadCactions(PageUpdateArgs args)
        {
            new InterfaceCallerBL().ReloadActions();
            GetCartBL().Recalc();
        }

        private void OnItemButton(PageUpdateArgs args)
        {
            args.ShowInputDialog(dbItem,
               "$additem", "", new Control[] {
                   new Selector() { Field="itemid", LabelId="$item_code", ValueField="itemid", TextField="itemcode" ,SelectionID="eLog.Base.Masters.Item.ItemSelectionProvider"},
                    new Selector() { Field="itemid", LabelId="$item_name", ValueField="itemid", TextField="name01" ,SelectionID="eLog.Base.Masters.Item.ItemSelectionProvider"},
           });
        }

        private void DbItem_OnButton1Clicked(PageUpdateArgs args)
        { 

            var item = dbCupon.GetStringValue("itemid_itemcode", args);

            if (!string.IsNullOrEmpty(item))
            {
                var itemid = int.Parse(item);
                GetCartBL().AddNewItem(itemid);
                SearchResults.ForceRefresh = true;
                args.Continue = true;
            }


        }

        private void RetailSalesSearchTab_OnPageLoad(PageUpdateArgs args)
        {
            paySearchResults.ForceRefresh = true;
            SearchResults.ForceRefresh = true;
            GetCartBL().FillTotal();

        }

        private void OnCartCache(PageUpdateArgs args)
        {
            new InterfaceCallerBL().ClearCartCache();
            GetCartBL().Recalc();
        }

        private void DbLoyaltyCardNo_OnButton1Clicked(PageUpdateArgs args)
        {
            var loyaltycardno = dbCupon.GetStringValue("loyaltycardno", args);

            GetCartBL().SetLoyaltyCardNo(loyaltycardno);
            SearchResults.ForceRefresh = true;
            args.Continue = true;
        }

        private void DbCupon_OnButton1Clicked(PageUpdateArgs args)
        { 
            var cupon = dbCupon.GetStringValue("cupon", args);

            GetCartBL().AddCupon(cupon);
            SearchResults.ForceRefresh = true;
            args.Continue = true;
        }

        private void OnLoyaltyCardNo(PageUpdateArgs args)
        {
            args.ShowInputDialog(dbLoyaltyCardNo,
                 "$addLoyaltyCardNo", "", new Control[] {
                   new Textbox("loyaltycardno") { Width = 200, Value=null},
             });
        }

        private void OnCupon(PageUpdateArgs args)
        {
            args.ShowInputDialog(dbCupon,
                  "$addCupon", "", new Control[] {
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
            GetCartBL().Recalc();

            SearchResults.ForceRefresh = true;
            paySearchResults.ForceRefresh = true;
        }
         
    }
}
