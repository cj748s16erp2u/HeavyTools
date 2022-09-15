using eLog.Base.Common;
using eLog.Base.Masters.Item;
using eLog.HeavyTools.Common;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Lang;
using eProjectWeb.Framework.Rules;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eLog.HeavyTools.Sales.Action
{
    internal class ActionExtRules : TypedBaseRuleSet<OlcActionExt>
    {
        public ActionExtRules()
            : base(true)
        {
            AddCustomRule(CanAddRule);
            AddCustomRule(DiscounttypeRule);
            AddCustomRule(FilteritemsRule);
        }

        private void DiscounttypeRule(RuleValidateContext ctx, OlcActionExt value)
        { 
            if (value.Isdiscount.GetValueOrDefault(0)==1)
            {
                if (!value.Discounttype.HasValue)
                {
                    ctx.AddErrorField(OlcActionExt.FieldDiscounttype,
                          Translator.Translate("$validation_error_mandatory",
                              Translator.Translate("$" + OlcActionExt.FieldDiscounttype.Name)));
                }
                if (!value.Discountcalculationtype.HasValue)
                {
                    ctx.AddErrorField(OlcActionExt.FieldDiscountcalculationtype,
                          Translator.Translate("$validation_error_mandatory",
                              Translator.Translate("$" + OlcActionExt.FieldDiscountcalculationtype.Name)));
                }
            }
        }

        private void FilteritemsRule(eProjectWeb.Framework.Rules.RuleValidateContext ctx, OlcActionExt value)
        {
            if (value.Filteritems != null)
            {
                ActionRules.CheckItemids(value.Filteritems, ctx, OlcAction.FieldFilteritems);
            }
            if (value.Filteritemsblock != null)
            {
                ActionRules.CheckItemids(value.Filteritemsblock, ctx, OlcAction.FieldFilteritemsblock);
            }
        }
        private void CanAddRule(RuleValidateContext ctx, OlcActionExt value)
        {
            var a = OlcAction.Load(value.Aid);

            if (value.Isdiscount.GetValueOrDefault(0)==1)
            {
                if (a.Isextdiscount.Value == 0)
                {
                    ctx.AddError("$cannot_use");
                }
            } else
            {
                if (a.Isextcondition.Value == 0)
                {
                    ctx.AddError("$cannot_use");
                }
            }

            

        }
    }

    internal class ActionExtEditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(ActionExtEditPage).FullName;

        public ActionExtEditPage()
            : base("ActionExt")
        {
            Tabs.AddTab(delegate { return ActionExtEditTab.New(ActionSearchPage.SetupExt); });
        }
    }

    internal class ActionExtEditTab : EditTabTemplate1<OlcActionExt, ActionExtRules, ActionExtBL>
    {
        protected ActionExtEditTab()
        {
        }

        public static ActionExtEditTab New(DefaultPageSetup setup)
        {
            var t = (ActionExtEditTab)ObjectFactory.New(typeof(ActionExtEditTab));
            t.Initialize("ActionExt", setup);
            return t;
        }

        Combo isdiscount;

        protected override void CreateBase()
        {
            base.CreateBase();

            isdiscount = FindRenderable<Combo>("isdiscount");
            isdiscount.SetOnChanged(OnIsDiscountChanged);
        }

        private void OnIsDiscountChanged(PageUpdateArgs args)
        { 
            var hide = false; 
            if (isdiscount.Value != null)
            {
                var v = int.Parse(isdiscount.Value.ToString());
                if (v == 1)
                {
                    hide = true;
                }
            }


            var l = new List<Control>();
            l.Add(FindRenderable<Combo>("discounttype"));
            l.Add(FindRenderable<Numberbox>("discountval"));
            l.Add(FindRenderable<Combo>("discountcalculationtype"));
             
            ShowHideAll(l, !hide);
        }


        private void ShowHideAll(List<Control> l, bool hide)
        {
            var defVaule = OlcAction.CreateNew();

            foreach (Control c in l)
            {
                c.Visible = !hide;
                if (hide)
                {
                    c.Value = defVaule[c.Field];
                }

            }
        }

    }

    internal class ActionExtSearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(ActionExtSearchProvider).FullName;

        public static string DefaultOrderByStr = null;

        protected static string m_queryString = @"select * from olc_ActionExt";

        protected static QueryArg[] m_argsTemplate = new QueryArg[]
        {
            new QueryArg("aid", FieldType.Integer, QueryFlags.Equals),
            new QueryArg("axid", FieldType.Integer, QueryFlags.Equals), 
        };

        protected ActionExtSearchProvider()
            : base(m_queryString, m_argsTemplate, SearchProviderType.Default)
        {
        }
    }

    internal class ActionExtBL : DefaultBL1<OlcActionExt, ActionExtRules>
    {
        public static readonly string ID = typeof(ActionExtBL).FullName;

        protected ActionExtBL()
            : base(DefaultBLFunctions.BasicHideUnhide)
        {
        }

        public static ActionExtBL New()
        {
            return (ActionExtBL)ObjectFactory.New(typeof(ActionExtBL));
        }
        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);

            object x = objects[typeof(OlcActionExt).Name];
            if (x != null)
                RuleServer.Validate(objects, typeof(ActionExtRules));

        }
    }

    internal class ActionExtSearchTab : DetailSearchTabTemplate1
    {
        public static ActionExtSearchTab New(DefaultPageSetup setup)
        {
            var t = (ActionExtSearchTab)ObjectFactory.New(typeof(ActionExtSearchTab));
            t.Initialize("ActionExt", setup,"$noActionselected" ,DefaultActions.Basic | DefaultActions.HideUnhide);
            return t;
        }
    }
}
