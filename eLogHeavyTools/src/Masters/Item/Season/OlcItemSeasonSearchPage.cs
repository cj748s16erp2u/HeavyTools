using eLog.Base.Common;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.Templates;


namespace eLog.HeavyTools.Masters.Item.Season
{
    internal class OlcItemSeasonSearchPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcItemSeasonSearchPage).FullName;

        public static DefaultPageSetup Setup = new DefaultPageSetup("OlcItemSeason", OlcItemSeasonBL.ID,
                                                                    OlcItemSeasonSearchProvider.ID, OlcItemSeasonEditPage.ID,
                                                                    typeof(OlcItemSeasonRules));


        public OlcItemSeasonSearchPage()
            : base("OlcItemSeason")
        {
            Tabs.AddTab(delegate { return OlcItemSeasonSearchTab.New(Setup); });
        }
    }

    internal class OlcItemSeasonRules : TypedBaseRuleSet<OlcItemSeason>
    {
        public OlcItemSeasonRules()
            : base(true)
        {

        }
    }

    internal class OlcItemSeasonEditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcItemSeasonEditPage).FullName;

        public OlcItemSeasonEditPage()
            : base("OlcItemSeason")
        {
            Tabs.AddTab(delegate { return OlcItemSeasonEditTab.New(OlcItemSeasonSearchPage.Setup); });
        }
    }

    internal class OlcItemSeasonEditTab : EditTabTemplate1<OlcItemSeason, OlcItemSeasonRules, OlcItemSeasonBL>
    {
        protected OlcItemSeasonEditTab()
        {
        }

        public static OlcItemSeasonEditTab New(DefaultPageSetup setup)
        {
            var t = (OlcItemSeasonEditTab)ObjectFactory.New(typeof(OlcItemSeasonEditTab));
            t.Initialize("OlcItemSeason", setup);
            return t;
        }
        protected override void CreateBase()
        {
            base.CreateBase();
            OnPageLoad += this.OlcItemSeasonEditTab_OnPageLoad;
        }

        private void OlcItemSeasonEditTab_OnPageLoad(PageUpdateArgs args)
        {
            if (args.ActionID != ActionID.New)
            {
                FindRenderable<Textbox>("isid").Disabled = true;
            }
        }
    }

    internal class OlcItemSeasonSearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(OlcItemSeasonSearchProvider).FullName;

        public static string DefaultOrderByStr = null;

        protected static string m_queryString = @"select * from olc_ItemSeason";

        protected static QueryArg[] m_argsTemplate = new QueryArg[]
        {
            new QueryArg("isid", FieldType.String, QueryFlags.Equals),
            new QueryArg("name", FieldType.String, QueryFlags.Like),
            new QueryArg("delstat", FieldType.Integer, QueryFlags.Equals),
        };

        protected OlcItemSeasonSearchProvider()
            : base(m_queryString, m_argsTemplate, SearchProviderType.Default)
        {
        }
    }

    internal class OlcItemSeasonBL : DefaultBL1<OlcItemSeason, OlcItemSeasonRules>
    {
        public static readonly string ID = typeof(OlcItemSeasonBL).FullName;

        protected OlcItemSeasonBL()
            : base(DefaultBLFunctions.BasicHideUnhide)
        {
        }

        public static OlcItemSeasonBL New()
        {
            return (OlcItemSeasonBL)ObjectFactory.New(typeof(OlcItemSeasonBL));
        }
        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);

            object x = objects[typeof(OlcItemSeason).Name];
            if (x != null)
                RuleServer.Validate(objects, typeof(OlcItemSeasonRules));

        }
    }

    internal class OlcItemSeasonSearchTab : SearchTabTemplate1
    {
        public static OlcItemSeasonSearchTab New(DefaultPageSetup setup)
        {
            var t = (OlcItemSeasonSearchTab)ObjectFactory.New(typeof(OlcItemSeasonSearchTab));
            t.Initialize("OlcItemSeason", setup, DefaultActions.Basic | DefaultActions.HideUnhide);
            return t;
        }
    }
}