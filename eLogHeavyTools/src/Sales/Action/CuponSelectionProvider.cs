using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Sales.Action
{
    public class CuponSelectionProvider : DefaultSelectionProvider
    {
        public static readonly string ID = typeof(CuponSelectionProvider).FullName;

        private static string m_queryString = "select * from olc_action  (nolock) ";
        private static new QueryArg[] m_argsTemplate = new QueryArg[] {
            new QueryArg("aid", FieldType.String),
            new QueryArg("name", FieldType.String,QueryFlags.Like)
        };

        public CuponSelectionProvider()
            : base(m_queryString, m_argsTemplate, CuponSelectPage.ID, "aid")
        {
        }
    }

    class CuponSelectPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(CuponSelectPage).FullName;

        public CuponSelectPage()
            : base("postcodeselect")
        {
            Tabs.AddTab(delegate { return CuponSelectTab.New(); });
        }
    }

    public class CuponSelectTab : SearchTabTemplate1
    {
        public new static DefaultPageSetup Setup = new DefaultPageSetup("aid", ActionSearchProvider.ID, ActionSearchProvider.ID, null);

        public static CuponSelectTab New()
        {
            CuponSelectTab t = (CuponSelectTab)ObjectFactory.New(typeof(CuponSelectTab));
            t.Initialize("postcode", Setup, DefaultActions.None, PageMode.Selection);
            return t;
        }

        protected CuponSelectTab()
        {
        }

        protected override void CreateBase()
        {
            base.CreateBase();
            this.OnPageLoad += CuponSelectPage_OnPageLoad;
        }

        void CuponSelectPage_OnPageLoad(PageUpdateArgs args)
        {
        } 
    }
     
}
