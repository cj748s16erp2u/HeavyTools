using eLog.Base.Common;
using eLog.HeavyTools.InterfaceCaller;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Lang;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.PageParts;
using eProjectWeb.Framework.UI.Templates;
using System;

namespace eLog.HeavyTools.Masters.ItmWh
{
    internal class RetailOrderSearchPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(RetailOrderSearchPage).FullName;
         
        public RetailOrderSearchPage()
            : base("RetailOrder")
        {
            Tabs.AddTab(delegate { return RetailOrderSearchTab.New(); });
        }
    } 

    internal class RetailOrderSearchTab : TabPage2
    {
        DialogBox dialog=new DialogBox(DialogBoxType.Ok);

        public static RetailOrderSearchTab New()
        {
            var t = (RetailOrderSearchTab)ObjectFactory.New(typeof(RetailOrderSearchTab));
            t.Initialize("RetailOrder");
            return t;
        }
        protected override void Initialize(string labelID)
        {
            base.Initialize(labelID);
            CreateControls();
            var c = new Button("createretailorder");
            AddCmd(c);
            c.SetOnClick(OnCreateRetailOrder);
            RegisterDialog(dialog);

        }

        private void OnCreateRetailOrder(PageUpdateArgs args)
        {
            var bl=new InterfaceCallerBL();
            var r = bl.CreateRetailOrder(Session.UserID);

            args.ShowDialog(dialog, "$retailorderresulttitle", Translator.Translate("$retailorderresult", r.RetaulCount));

        }
    }
}
