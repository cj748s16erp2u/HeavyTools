using eLog.Base.Common;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Rules;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Text.RegularExpressions;

namespace eLog.HeavyTools.Masters.Item.MainGroup
{
    internal class OlcItemMainGroupType1SearchPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcItemMainGroupType1SearchPage).FullName;

        public static DefaultPageSetup Setup = new DefaultPageSetup("OlcItemMainGroupType1", OlcItemMainGroupType1BL.ID,
                                                                    OlcItemMainGroupType1SearchProvider.ID, OlcItemMainGroupType1EditPage.ID,
                                                                    typeof(OlcItemMainGroupType1Rules));


        public OlcItemMainGroupType1SearchPage()
            : base("OlcItemMainGroupType1")
        {
            Tabs.AddTab(delegate { return OlcItemMainGroupType1SearchTab.New(Setup); });
        }
    }

    internal class OlcItemMainGroupType1Rules : TypedBaseRuleSet<OlcItemMainGroupType1>
    {
        public OlcItemMainGroupType1Rules()
            : base(true)
        {
            AddCustomRule(LetterRule);
            AddCustomRule(Duplcate);
        }

        private void Duplcate(RuleValidateContext ctx, OlcItemMainGroupType1 value)
        {
            if (value.State == DataRowState.Added)
            {
                var o=OlcItemMainGroupType1.Load(value.Imgt1id);
                if (o != null)
                {
                    ctx.AddError("$duplicaterecord");
                }
            }
        }

        private void LetterRule(RuleValidateContext ctx, OlcItemMainGroupType1 oimgt)
        {
            if (!string.IsNullOrEmpty(oimgt.Imgt1id.Value))
            {
                if (!Regex.IsMatch(oimgt.Imgt1id.Value, @"^[a-zA-Z]+$"))
                {
                    ctx.AddErrorField(OlcItemMainGroupType1.FieldImgt1id, "$notletter");
                }
            }
        }
    }

    internal class OlcItemMainGroupType1EditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcItemMainGroupType1EditPage).FullName;

        public OlcItemMainGroupType1EditPage()
            : base("OlcItemMainGroupType1")
        {
            Tabs.AddTab(delegate { return OlcItemMainGroupType1EditTab.New(OlcItemMainGroupType1SearchPage.Setup); });
        }
    }

    internal class OlcItemMainGroupType1EditTab : EditTabTemplate1<OlcItemMainGroupType1, OlcItemMainGroupType1Rules, OlcItemMainGroupType1BL>
    {
        protected OlcItemMainGroupType1EditTab()
        {
        }

        public static OlcItemMainGroupType1EditTab New(DefaultPageSetup setup)
        {
            var t = (OlcItemMainGroupType1EditTab)ObjectFactory.New(typeof(OlcItemMainGroupType1EditTab));
            t.Initialize("OlcItemMainGroupType1", setup);
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
                FindRenderable<Textbox>("imgt1id").Disabled = true;
            }
        }
    }

    internal class OlcItemMainGroupType1SearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(OlcItemMainGroupType1SearchProvider).FullName;

        public static string DefaultOrderByStr = null;

        protected static string m_queryString = @"select * from olc_itemmaingrouptype1";

        protected static QueryArg[] m_argsTemplate = new QueryArg[]
        {
            new QueryArg("imgt1id", FieldType.String, QueryFlags.Equals),
            new QueryArg("delstat", FieldType.Integer, QueryFlags.Equals)
        };

        protected OlcItemMainGroupType1SearchProvider()
            : base(m_queryString, m_argsTemplate, SearchProviderType.Default)
        {
        }
    }

    internal class OlcItemMainGroupType1BL : DefaultBL1<OlcItemMainGroupType1, OlcItemMainGroupType1Rules>
    {
        public static readonly string ID = typeof(OlcItemMainGroupType1BL).FullName;

        protected OlcItemMainGroupType1BL()
            : base(DefaultBLFunctions.BasicHideUnhide)
        {
        }

        public static OlcItemMainGroupType1BL New()
        {
            return (OlcItemMainGroupType1BL)ObjectFactory.New(typeof(OlcItemMainGroupType1BL));
        }
        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);

            object x = objects[typeof(OlcItemMainGroupType1).Name];
            if (x != null)
                RuleServer.Validate(objects, typeof(OlcItemMainGroupType1Rules));

        }
    }

    internal class OlcItemMainGroupType1SearchTab : SearchTabTemplate1
    {
        public static OlcItemMainGroupType1SearchTab New(DefaultPageSetup setup)
        {
            var t = (OlcItemMainGroupType1SearchTab)ObjectFactory.New(typeof(OlcItemMainGroupType1SearchTab));
            t.Initialize("OlcItemMainGroupType1", setup, DefaultActions.Basic | DefaultActions.HideUnhide);
            return t;
        }
    }
}