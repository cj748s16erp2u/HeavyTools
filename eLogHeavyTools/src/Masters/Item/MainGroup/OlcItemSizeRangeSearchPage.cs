using eLog.Base.Common;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.Templates;
using System.Collections.Generic;

namespace eLog.HeavyTools.Masters.Item.MainGroup
{
    internal class OlcItemSizeRangeSearchPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcItemSizeRangeSearchPage).FullName;

        public static DefaultPageSetup Setup = new DefaultPageSetup("OlcItemSizeRange", OlcItemSizeRangeBL.ID,
                                                                    OlcItemSizeRangeSearchProvider.ID, OlcItemSizeRangeEditPage.ID,
                                                                    typeof(OlcItemSizeRangeRules));

        public static DefaultPageSetup SetupLine = new DefaultPageSetup("OlcItemSizeRangeLine", OlcItemSizeRangeLineBL.ID,
                                                                 OlcItemSizeRangeLineSearchProvider.ID, OlcItemSizeRangeLineEditPage.ID,
                                                                 typeof(OlcItemSizeRangeRules));

        public OlcItemSizeRangeSearchPage()
            : base("OlcItemSizeRange")
        {
            Tabs.AddTab(delegate { return OlcItemSizeRangeSearchTab.New(Setup); });
            Tabs.AddTab(delegate { return OlcItemSizeRangeLineSearchTab.New(SetupLine); });
        }
    }

    internal class OlcItemSizeRangeRules : TypedBaseRuleSet<OlcItemSizeRangeHead>
    {
        public OlcItemSizeRangeRules()
            : base(true)
        {

        }
    }

    internal class OlcItemSizeRangeLineRules : TypedBaseRuleSet<OlcItemSizeRangeLine>
    {
        public OlcItemSizeRangeLineRules()
            : base(true)
        {

        }
    }
    internal class OlcItemSizeRangeEditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcItemSizeRangeEditPage).FullName;

        public OlcItemSizeRangeEditPage()
            : base("OlcItemSizeRange")
        {
            Tabs.AddTab(delegate { return OlcItemSizeRangeEditTab.New(OlcItemSizeRangeSearchPage.Setup); });
        }
    }
    internal class OlcItemSizeRangeLineEditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcItemSizeRangeLineEditPage).FullName;

        public OlcItemSizeRangeLineEditPage()
            : base("OlcItemSizeRangeLine")
        {
            Tabs.AddTab(delegate { return OlcItemSizeRangeLineEditTab.New(OlcItemSizeRangeSearchPage.SetupLine); });
        }
    }

    internal class OlcItemSizeRangeEditTab : EditTabTemplate1<OlcItemSizeRangeHead, OlcItemSizeRangeRules, OlcItemSizeRangeBL>
    {
        protected OlcItemSizeRangeEditTab()
        {
        }

        public static OlcItemSizeRangeEditTab New(DefaultPageSetup setup)
        {
            var t = (OlcItemSizeRangeEditTab)ObjectFactory.New(typeof(OlcItemSizeRangeEditTab));
            t.Initialize("OlcItemSizeRange", setup);
            return t;
        }
    }

    internal class OlcItemSizeRangeLineEditTab : EditTabTemplate1<OlcItemSizeRangeLine, OlcItemSizeRangeLineRules, OlcItemSizeRangeLineBL>
    {
        protected OlcItemSizeRangeLineEditTab()
        {
        }

        public static OlcItemSizeRangeLineEditTab New(DefaultPageSetup setup)
        {
            var t = (OlcItemSizeRangeLineEditTab)ObjectFactory.New(typeof(OlcItemSizeRangeLineEditTab));
            t.Initialize("OlcItemSizeRangeLine", setup);
            return t;
        }
        protected override void CreateBase()
        {
            base.CreateBase();
        }

        protected override OlcItemSizeRangeLine DefaultPageLoad_CreateNewEntity(PageUpdateArgs args)
        {
            var e= base.DefaultPageLoad_CreateNewEntity(args);

            var lastonum = ConvertUtils.ToInt32(SqlDataAdapter.ExecuteSingleValue(DB.Main, @"select max(ordernum) ordernum
  from olc_itemsizerangeline 
 where isrhid=" + (int)(long)new Key(args.PageData[Consts.RootEntityKey])["isrhid"]));

            if (!lastonum.HasValue)
            {
                lastonum = 10;
            }
            else
            {
                lastonum = lastonum.Value + 10;
            }

            FindRenderable<Intbox>("ordernum").Value = lastonum;

            return e;
        }
    }
     

    internal class OlcItemSizeRangeSearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(OlcItemSizeRangeSearchProvider).FullName;

        public static string DefaultOrderByStr = null;

        protected static string m_queryString = @"select * from olc_itemsizerangehead";

        protected static QueryArg[] m_argsTemplate = new QueryArg[]
        {
            new QueryArg("isrhid", FieldType.Integer, QueryFlags.Equals),
            new QueryArg("delstat", FieldType.Integer, QueryFlags.Equals)

        };

        protected OlcItemSizeRangeSearchProvider()
            : base(m_queryString, m_argsTemplate, SearchProviderType.Default)
        {
        }
    }
    internal class OlcItemSizeRangeLineSearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(OlcItemSizeRangeLineSearchProvider).FullName;

        public static string DefaultOrderByStr = null;

        protected static string m_queryString = @"select * from olc_itemsizerangeline";

        protected static QueryArg[] m_argsTemplate = new QueryArg[]
        {
            new QueryArg("isrlid", FieldType.Integer, QueryFlags.Equals),
            new QueryArg("isrhid", FieldType.Integer, QueryFlags.Equals),
            new QueryArg("delstat", FieldType.Integer, QueryFlags.Equals),
        };

        protected OlcItemSizeRangeLineSearchProvider()
            : base(m_queryString, m_argsTemplate, SearchProviderType.Default)
        {
        }
        protected override string GetOrderByString(Dictionary<string, object> args)
        {
            return " order by ordernum";
        }
    }

    internal class OlcItemSizeRangeBL : DefaultBL1<OlcItemSizeRangeHead, OlcItemSizeRangeRules>
    {
        public static readonly string ID = typeof(OlcItemSizeRangeBL).FullName;

        protected OlcItemSizeRangeBL()
            : base(DefaultBLFunctions.BasicHideUnhide)
        {
        }

        public static OlcItemSizeRangeBL New()
        {
            return (OlcItemSizeRangeBL)ObjectFactory.New(typeof(OlcItemSizeRangeBL));
        }
        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);

            object x = objects[typeof(OlcItemSizeRangeHead).Name];
            if (x != null)
                RuleServer.Validate(objects, typeof(OlcItemSizeRangeRules));

        }
    }

    internal class OlcItemSizeRangeLineBL : DefaultBL1<OlcItemSizeRangeLine, OlcItemSizeRangeLineRules>
    {
        public static readonly string ID = typeof(OlcItemSizeRangeLineBL).FullName;

        protected OlcItemSizeRangeLineBL()
            : base(DefaultBLFunctions.BasicHideUnhide)
        {
        }

        public static OlcItemSizeRangeLineBL New()
        {
            return (OlcItemSizeRangeLineBL)ObjectFactory.New(typeof(OlcItemSizeRangeLineBL));
        }
        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);

            object x = objects[typeof(OlcItemSizeRangeLine).Name];
            if (x != null)
                RuleServer.Validate(objects, typeof(OlcItemSizeRangeLineRules));

        }
    }


    internal class OlcItemSizeRangeSearchTab : SearchTabTemplate1
    {
        public static OlcItemSizeRangeSearchTab New(DefaultPageSetup setup)
        {
            var t = (OlcItemSizeRangeSearchTab)ObjectFactory.New(typeof(OlcItemSizeRangeSearchTab));
            t.Initialize("OlcItemSizeRange", setup, DefaultActions.Basic | DefaultActions.HideUnhide);
            return t;
        }
    }
    internal class OlcItemSizeRangeLineSearchTab : DetailSearchTabTemplate1
    {
        public static OlcItemSizeRangeLineSearchTab New(DefaultPageSetup setup)
        {
            var t = (OlcItemSizeRangeLineSearchTab)ObjectFactory.New(typeof(OlcItemSizeRangeLineSearchTab));
            t.Initialize("OlcItemSizeRangeLine", setup, "$noroot", DefaultActions.Basic | DefaultActions.HideUnhide);
            return t;
        }
    }
}