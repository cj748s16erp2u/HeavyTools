using eLog.Base.Common;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.UI.Templates;

namespace eLog.HeavyTools.Setup.PriceTable
{
    internal class OlcPrcTypeSearchPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcPrcTypeSearchPage).FullName;

        public static DefaultPageSetup Setup = new DefaultPageSetup("OlcPrcType", OlcPrcTypeBL.ID,
                                                                    OlcPrcTypeSearchProvider.ID, OlcPrcTypeEditPage.ID,
                                                                    typeof(OlcPrcTypeRules));


        public OlcPrcTypeSearchPage()
            : base("OlcPrcType")
        {
            Tabs.AddTab(delegate { return OlcPrcTypeSearchTab.New(Setup); });
        }
    }

    internal class OlcPrcTypeRules : TypedBaseRuleSet<OlcPrcType>
    {
        public OlcPrcTypeRules()
            : base(true)
        {

        }
    }

    internal class OlcPrcTypeEditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcPrcTypeEditPage).FullName;

        public OlcPrcTypeEditPage()
            : base("OlcPrcType")
        {
            Tabs.AddTab(delegate { return OlcPrcTypeEditTab.New(OlcPrcTypeSearchPage.Setup); });
        }
    }

    internal class OlcPrcTypeEditTab : EditTabTemplate1<OlcPrcType, OlcPrcTypeRules, OlcPrcTypeBL>
    {
        protected OlcPrcTypeEditTab()
        {
        }

        public static OlcPrcTypeEditTab New(DefaultPageSetup setup)
        {
            var t = (OlcPrcTypeEditTab)ObjectFactory.New(typeof(OlcPrcTypeEditTab));
            t.Initialize("OlcPrcType", setup);
            return t;
        }
    }

    internal class OlcPrcTypeSearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(OlcPrcTypeSearchProvider).FullName;

        public static string DefaultOrderByStr = null;

        protected static string m_queryString = @"select * from olc_PrcType";

        protected static QueryArg[] m_argsTemplate = new QueryArg[]
        {
            new QueryArg("tpid", FieldType.Integer, QueryFlags.Equals),
            new QueryArg("name", FieldType.String, QueryFlags.Like),
        };

        protected OlcPrcTypeSearchProvider()
            : base(m_queryString, m_argsTemplate, SearchProviderType.Default)
        {
        }
    }

    internal class OlcPrcTypeBL : DefaultBL1<OlcPrcType, OlcPrcTypeRules>
    {
        public static readonly string ID = typeof(OlcPrcTypeBL).FullName;

        protected OlcPrcTypeBL()
            : base(DefaultBLFunctions.BasicHideUnhide)
        {
        }

        public static OlcPrcTypeBL New()
        {
            return (OlcPrcTypeBL)ObjectFactory.New(typeof(OlcPrcTypeBL));
        }
        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);

            object x = objects[typeof(OlcPrcType).Name];
            if (x != null)
                RuleServer.Validate(objects, typeof(OlcPrcTypeRules));

        }
    }

    internal class OlcPrcTypeSearchTab : SearchTabTemplate1
    {
        public static OlcPrcTypeSearchTab New(DefaultPageSetup setup)
        {
            var t = (OlcPrcTypeSearchTab)ObjectFactory.New(typeof(OlcPrcTypeSearchTab));
            t.Initialize("OlcPrcType", setup, DefaultActions.Basic | DefaultActions.HideUnhide);
            return t;
        }
    }
}
