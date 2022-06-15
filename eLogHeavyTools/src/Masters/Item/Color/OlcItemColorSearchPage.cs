using eLog.Base.Common;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.UI.Templates;
namespace eLog.HeavyTools.Masters.Item.Color
{
    internal class OlcItemColorSearchPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcItemColorSearchPage).FullName;

        public static DefaultPageSetup Setup = new DefaultPageSetup("OlcItemColor", OlcItemColorBL.ID,
                                                                    OlcItemColorSearchProvider.ID, OlcItemColorEditPage.ID,
                                                                    typeof(OlcItemColorRules));


        public OlcItemColorSearchPage()
            : base("OlcItemColor")
        {
            Tabs.AddTab(delegate { return OlcItemColorSearchTab.New(Setup); });
        }
    }

    internal class OlcItemColorRules : TypedBaseRuleSet<OlcItemColor>
    {
        public OlcItemColorRules()
            : base(true)
        {

        }
    }

    internal class OlcItemColorEditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcItemColorEditPage).FullName;

        public OlcItemColorEditPage()
            : base("OlcItemColor")
        {
            Tabs.AddTab(delegate { return OlcItemColorEditTab.New(OlcItemColorSearchPage.Setup); });
        }
    }

    internal class OlcItemColorEditTab : EditTabTemplate1<OlcItemColor, OlcItemColorRules, OlcItemColorBL>
    {
        protected OlcItemColorEditTab()
        {
        }

        public static OlcItemColorEditTab New(DefaultPageSetup setup)
        {
            var t = (OlcItemColorEditTab)ObjectFactory.New(typeof(OlcItemColorEditTab));
            t.Initialize("OlcItemColor", setup);
            return t;
        }
    }

    internal class OlcItemColorSearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(OlcItemColorSearchProvider).FullName;

        public static string DefaultOrderByStr = null;

        protected static string m_queryString = @"select * from olc_ItemColor";

        protected static QueryArg[] m_argsTemplate = new QueryArg[]
        {
            new QueryArg("icid", FieldType.String, QueryFlags.Equals),
            new QueryArg("delstat", FieldType.Integer, QueryFlags.Equals),
        };

        protected OlcItemColorSearchProvider()
            : base(m_queryString, m_argsTemplate, SearchProviderType.Default)
        {
        }
    }

    internal class OlcItemColorBL : DefaultBL1<OlcItemColor, OlcItemColorRules>
    {
        public static readonly string ID = typeof(OlcItemColorBL).FullName;

        protected OlcItemColorBL()
            : base(DefaultBLFunctions.BasicHideUnhide)
        {
        }

        public static OlcItemColorBL New()
        {
            return (OlcItemColorBL)ObjectFactory.New(typeof(OlcItemColorBL));
        }
        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);

            object x = objects[typeof(OlcItemColor).Name];
            if (x != null)
                RuleServer.Validate(objects, typeof(OlcItemColorRules));

        }
    }

    internal class OlcItemColorSearchTab : SearchTabTemplate1
    {
        public static OlcItemColorSearchTab New(DefaultPageSetup setup)
        {
            var t = (OlcItemColorSearchTab)ObjectFactory.New(typeof(OlcItemColorSearchTab));
            t.Initialize("OlcItemColor", setup, DefaultActions.Basic | DefaultActions.HideUnhide);
            return t;
        }
    }
}
