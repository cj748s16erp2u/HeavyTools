using eLog.Base.Common;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Rules;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.Templates;

namespace eLog.HeavyTools.Masters.Item.MainGroup
{
    internal class OlcItemMainGroupType2SearchPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcItemMainGroupType2SearchPage).FullName;

        public static DefaultPageSetup Setup = new DefaultPageSetup("OlcItemMainGroupType2", OlcItemMainGroupType2BL.ID,
                                                                    OlcItemMainGroupType2SearchProvider.ID, OlcItemMainGroupType2EditPage.ID,
                                                                    typeof(OlcItemMainGroupType2Rules));


        public OlcItemMainGroupType2SearchPage()
            : base("OlcItemMainGroupType2")
        {
            Tabs.AddTab(delegate { return OlcItemMainGroupType2SearchTab.New(Setup); });
        }
    }

    internal class OlcItemMainGroupType2Rules : TypedBaseRuleSet<OlcItemMainGroupType2>
    {
        public OlcItemMainGroupType2Rules()
            : base(true)
        {
            AddCustomRule(Duplcate);
        }

        private void Duplcate(RuleValidateContext ctx, OlcItemMainGroupType2 value)
        {
            if (value.State == DataRowState.Added)
            {
                var o = OlcItemMainGroupType2.Load(value.Imgt2id);
                if (o != null)
                {
                    ctx.AddError("$duplicaterecord");
                }
            }
        }
    }

    internal class OlcItemMainGroupType2EditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcItemMainGroupType2EditPage).FullName;

        public OlcItemMainGroupType2EditPage()
            : base("OlcItemMainGroupType2")
        {
            Tabs.AddTab(delegate { return OlcItemMainGroupType2EditTab.New(OlcItemMainGroupType2SearchPage.Setup); });
        }
    }

    internal class OlcItemMainGroupType2EditTab : EditTabTemplate1<OlcItemMainGroupType2, OlcItemMainGroupType2Rules, OlcItemMainGroupType2BL>
    {
        protected OlcItemMainGroupType2EditTab()
        {
        }

        public static OlcItemMainGroupType2EditTab New(DefaultPageSetup setup)
        {
            var t = (OlcItemMainGroupType2EditTab)ObjectFactory.New(typeof(OlcItemMainGroupType2EditTab));
            t.Initialize("OlcItemMainGroupType2", setup);
            return t;
        }
        protected override void CreateBase()
        {
            base.CreateBase();
            OnPageLoad += OlcItemMainGroupType1EditTab_OnPageLoad;
        }

        private void OlcItemMainGroupType1EditTab_OnPageLoad(PageUpdateArgs args)
        {
            if (args.ActionID != ActionID.New)
            {
                FindRenderable<Textbox>("imgt2id").Disabled = true;
            }
        }
    }

    internal class OlcItemMainGroupType2SearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(OlcItemMainGroupType2SearchProvider).FullName;

        public static string DefaultOrderByStr = null;

        protected static string m_queryString = @"select * from olc_ItemMainGroupType2";

        protected static QueryArg[] m_argsTemplate = new QueryArg[]
        {
            new QueryArg("imgt2id", FieldType.String, QueryFlags.Equals),
            new QueryArg("delstat", FieldType.Integer, QueryFlags.Equals)
        };

        protected OlcItemMainGroupType2SearchProvider()
            : base(m_queryString, m_argsTemplate, SearchProviderType.Default)
        {
        }
    }

    internal class OlcItemMainGroupType2BL : DefaultBL1<OlcItemMainGroupType2, OlcItemMainGroupType2Rules>
    {
        public static readonly string ID = typeof(OlcItemMainGroupType2BL).FullName;

        protected OlcItemMainGroupType2BL()
            : base(DefaultBLFunctions.BasicHideUnhide)
        {
        }

        public static OlcItemMainGroupType2BL New()
        {
            return (OlcItemMainGroupType2BL)ObjectFactory.New(typeof(OlcItemMainGroupType2BL));
        }
        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);

            object x = objects[typeof(OlcItemMainGroupType2).Name];
            if (x != null)
                RuleServer.Validate(objects, typeof(OlcItemMainGroupType2Rules));

        }
    }

    internal class OlcItemMainGroupType2SearchTab : SearchTabTemplate1
    {
        public static OlcItemMainGroupType2SearchTab New(DefaultPageSetup setup)
        {
            var t = (OlcItemMainGroupType2SearchTab)ObjectFactory.New(typeof(OlcItemMainGroupType2SearchTab));
            t.Initialize("OlcItemMainGroupType2", setup, DefaultActions.Basic | DefaultActions.HideUnhide);
            return t;
        }
    }
}