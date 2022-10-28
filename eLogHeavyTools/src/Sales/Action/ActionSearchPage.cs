using eLog.Base.Common;
using eLog.Base.Masters.Item;
using eLog.HeavyTools.Common;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Lang;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eLog.HeavyTools.Sales.Action
{
    internal class ActionSearchPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(ActionSearchPage).FullName;

        public static DefaultPageSetup Setup = new DefaultPageSetup("Action", ActionBL.ID,
                                                                    ActionSearchProvider.ID, ActionEditPage.ID,
                                                                    typeof(ActionRules));

        public static DefaultPageSetup SetupExt = new DefaultPageSetup("ActionExt", ActionExtBL.ID,
                                                                  ActionExtSearchProvider.ID, ActionExtEditPage.ID,
                                                                  typeof(ActionExtRules));


        public ActionSearchPage()
            : base("Action")
        {
            Tabs.AddTab(delegate { return ActionSearchTab.New(Setup); });
            Tabs.AddTab(delegate { return ActionExtSearchTab.New(SetupExt); });
        }
    }

    internal class ActionRules : TypedBaseRuleSet<OlcAction>
    {
        public ActionRules()
            : base(true)
        {
            ERules["isactive"].Mandatory = true;
            ERules["isextcondition"].Mandatory = true;
            ERules["isextdiscount"].Mandatory = true;
            ERules["discounttype"].Mandatory = false;
            ERules["discountval"].Mandatory = false;


            AddCustomRule(CustomMandotaryFilterRule);
            AddCustomRule(FilteritemsRule);

            AddCustomRule(DiscounttypeRule);
        }

        private void DiscounttypeRule(eProjectWeb.Framework.Rules.RuleValidateContext ctx, OlcAction value)
        {
            if (value.Isextdiscount==0)
            {
                if (!value.Discounttype.HasValue)
                {
                    ctx.AddErrorField(OlcAction.FieldDiscounttype,
                            Translator.Translate("$validation_error_mandatory", 
                                Translator.Translate("$" + OlcAction.FieldDiscounttype.Name)));  
                }

                var cnt = 0;

                if (value.Discountval.HasValue)
                {
                    cnt++;
                }

                if (value.Discountforfree.HasValue)
                {
                    cnt++;
                }

                if (value.Discountfreetransportation.HasValue)
                {
                    cnt++;
                }
                
                if (value.Discounttype.Value==2)
                {
                    if (cnt > 0)
                    {
                        ctx.AddError(Translator.Translate("$cupon_not_single_discount"));
                       
                    }
                    if (!value.Discountaid.HasValue)
                    {
                        ctx.AddErrorField(OlcAction.FieldDiscountaid,
                           Translator.Translate("$validation_error_mandatory",
                               Translator.Translate("$" + OlcAction.FieldDiscountaid.Name)));
                    }
                } else
                {
                    if (cnt == 0)
                    {
                        ctx.AddError(Translator.Translate("$discount_missing"));
                    }
                }
            }
        }

        private void FilteritemsRule(eProjectWeb.Framework.Rules.RuleValidateContext ctx, OlcAction value)
        {
            if (value.Filteritems != null)
            {
                CheckItemids(value.Filteritems, ctx, OlcAction.FieldFilteritems);
            }
            if (value.Filteritemsblock != null)
            {
                CheckItemids(value.Filteritemsblock, ctx, OlcAction.FieldFilteritemsblock);
            }
        }

        public static void CheckItemids(StringN filteritemsblock, eProjectWeb.Framework.Rules.RuleValidateContext ctx, Field field)
        {
            var s = filteritemsblock.Value;
            if (string.IsNullOrEmpty(s))
            {
                return;
            }

            foreach (var itemcode in s.Split(','))
            {
                var ic = itemcode.Replace('*', '%');

                if (ic.Contains('%')) {
                    var i = Item.Load(new Key() { { Item.FieldItemcode.Name, new Key.LikeAtToSql(ic) } });
                    if (i == null)
                    {
                        ctx.AddErrorField(field, Translator.Translate("$missingitemcode", itemcode));
                    } 
                }
                else {
                    var i = Item.Load(ic);
                    if (i == null)
                    {
                        ctx.AddErrorField(field, Translator.Translate("$missingitemcode", itemcode));
                    } 
                }
            }
        }

        private void CustomMandotaryFilterRule(eProjectWeb.Framework.Rules.RuleValidateContext ctx, OlcAction olcaction)
        {
            var arm = new ActionRulesMandotary();

            arm.Add(OlcAction.FieldActiontype,
                    (int)ActionType.Action,
                        new[] {
                            OlcAction.FieldName,
                            OlcAction.FieldPriority,
                            //OlcAction.FieldDiscounttype,
                            //OlcAction.FieldDiscountval,
                            OlcAction.FieldValiddatefrom,
                            OlcAction.FieldValiddateto,
                            OlcAction.FieldPurchasetype}
                        );

            arm.Add(OlcAction.FieldActiontype,
                      (int)ActionType.Cupon,
                      new[] {
                            OlcAction.FieldName,
                            OlcAction.FieldPriority,
                            //OlcAction.FieldDiscounttype,
                            //OlcAction.FieldDiscountval,
                            OlcAction.FieldValiddatefrom,
                            OlcAction.FieldValiddateto,
                            OlcAction.FieldPurchasetype,
                            OlcAction.FieldSinglecouponnumber,
                            OlcAction.FieldCouponunlimiteduse}
                      );
            arm.Add(OlcAction.FieldActiontype,
                     (int)ActionType.Loyaltycardno,
                     new[] {
                            OlcAction.FieldName,
                            OlcAction.FieldPriority,
                            //OlcAction.FieldDiscounttype,
                            //OlcAction.FieldDiscountval,
                            OlcAction.FieldValiddatefrom,
                            OlcAction.FieldValiddateto,
                            OlcAction.FieldValidtotvalfrom}
                     );
            arm.Add(OlcAction.FieldActiontype,
                     (int)ActionType.VIP,
                     new[] {
                                        OlcAction.FieldName,
                                        OlcAction.FieldPriority,
                                        //OlcAction.FieldDiscounttype,
                                        //OlcAction.FieldDiscountval,
                                        OlcAction.FieldValiddatefrom,
                                        OlcAction.FieldValiddateto
                     }
                     );
            arm.Validate(ctx, olcaction);
        }
    }

    internal class ActionEditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(ActionEditPage).FullName;

        public ActionEditPage()
            : base("Action")
        {
            Tabs.AddTab(delegate { return ActionEditTab.New(ActionSearchPage.Setup); });
        }
    }

    internal class ActionEditTab : EditTabTemplate1<OlcAction, ActionRules, ActionBL>
    {
        protected ActionEditTab()
        {
        }

        public static ActionEditTab New(DefaultPageSetup setup)
        {
            var t = (ActionEditTab)ObjectFactory.New(typeof(ActionEditTab));
            t.Initialize("Action", setup);
            return t;
        }

        Combo actiontype;
        Combo isextcondition;
        Combo isextdiscount;
        Combo discounttype;

        protected override void CreateBase()
        {
            base.CreateBase();
            actiontype = FindRenderable<Combo>("actiontype");
            actiontype.SetOnChanged(OnActionTypeChange); 
            isextcondition = FindRenderable<Combo>("isextcondition");
            isextdiscount = FindRenderable<Combo>("isextdiscount");
            isextcondition.SetOnChanged(OnIsExtcondition);
            isextdiscount.SetOnChanged(OnIsDiscount);
            discounttype = FindRenderable<Combo>("discounttype");
            discounttype.SetOnChanged(OnDiscountType);
        }

        private void OnDiscountType(PageUpdateArgs args)
        {
            var isPercent = false;

            if (discounttype.Value != null)
            {
                switch ((Discounttype)int.Parse(discounttype.Value.ToString()))
                {
                  
                    case Discounttype.Percent:
                        isPercent = true;
                        break; 
                }
            }

            var discountcalculationtype = FindRenderable<Combo>("discountcalculationtype");

            if (isPercent)
            {
                discountcalculationtype.Disabled = false;
            } else
            {
                discountcalculationtype.Value = (int)Discountcalculationtype.Division;
                discountcalculationtype.Disabled = true;
            }
        }

        private void OnIsDiscount(PageUpdateArgs args)
        {
            var l = new List<Control>();
            l.Add(FindRenderable<Combo>("discounttype"));
            l.Add(FindRenderable<Numberbox>("discountval"));
            l.Add(FindRenderable<Combo>("discountforfree"));
            l.Add(FindRenderable<Combo>("discountfreetransportation"));
            l.Add(FindRenderable<Combo>("discountcalculationtype"));

            ShowHideAll(l, isextdiscount.Value);
        }

        private void OnIsExtcondition(PageUpdateArgs args)
        {
            var l = new List<Control>();
            l.Add(FindRenderable<Textbox>("filteritems"));
            l.Add(FindRenderable<Textbox>("filteritemsblock"));
            l.Add(FindRenderable<Numberbox>("validtotvalfrom"));
            l.Add(FindRenderable<Numberbox>("validtotvalto"));
            l.Add(FindRenderable<Intbox>("count"));

            ShowHideAll(l, isextcondition.Value); 
        }

        private void ShowHideAll(List<Control> l, object value)
        {
            var hide = false;

            if (value != null)
            {
                var v = int.Parse(value.ToString());
                if (v == 1)
                {
                    hide = true;
                }
            }
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

        private void OnActionTypeChange(PageUpdateArgs args)
        {
            var cs = this.EditGroup1?.ControlArray.Where(c => c.CustomData != "");
            
            if (actiontype.Value == null)
            {
                foreach (var c in cs)
                {
                    c.Visible = false;
                }
                return;
            }

            var at = (ActionType)int.Parse(actiontype.Value.ToString());

            OnIsDiscount(args);
            OnIsExtcondition(args);

            foreach (var c in cs)
            {
                var visible = false;

                var en = c.CustomData.Split(',');
                foreach (var e in en)
                {
                    var a = (ActionType)Enum.Parse(typeof(ActionType), e);
                    if (a == at)
                    {
                        visible = true;
                    } 
                }
                var defVaule=OlcAction.CreateNew();
                c.Visible = visible;
                if (!visible)
                {
                    try
                    {
                        c.Value = defVaule[c.Field];
                    }
                    catch (Exception)
                    {
                        c.Value = null;
                    }
                }
            }
            
        }

        protected override OlcAction DefaultPageLoad_LoadEntity(PageUpdateArgs args)
        {
            var e= base.DefaultPageLoad_LoadEntity(args);

            FillCombo("country", OlcActioncountry.FieldCountryid, 
                OlcActioncountries.Load(new Key() { { OlcActioncountry.FieldAid.Name, e.Aid } }));
           
            FillCombo("retail", OlcActionRetail.FieldWhid,
                OlcActionRetails.Load(new Key() { { OlcActionRetail.FieldAid.Name, e.Aid } }));
            
            FillCombo("webshop", OlcActionWebhop.FieldWid,
                OlcActionWebhops.Load(new Key() { { OlcActionWebhop.FieldAid.Name, e.Aid } }));

            if (e.Netgoid.HasValue)
            {
                foreach (var item in this.m_contents)
                {
                    item.Disabled = true;
                }
            }

            return e;
        }

        private void FillCombo(string field, Field key, EntityCollection items)
        {
            var c = FindRenderable<Combo>(field);
            var l = new List<object>();
            foreach (var item in items.AllRows)
            {
                l.Add(item[key.Name]);
            }
            c.Value = l;
        }

        protected override BLObjectMap SaveControlsToBLObjectMap(PageUpdateArgs args, OlcAction e)
        {
            var map= base.SaveControlsToBLObjectMap(args, e);

            map.Add(typeof(OlcActioncountries).FullName,
                SaveCombo("country", OlcActioncountry.FieldCountryid, OlcActioncountry.FieldAid, e.Aid,
                    OlcActioncountries.Load(new Key() { { OlcActioncountry.FieldAid.Name, e.Aid } }), OlcActioncountry.CreateNew)
            );

            map.Add(typeof(OlcActionRetails).FullName,
                SaveCombo("retail", OlcActionRetail.FieldWhid, OlcActionRetail.FieldAid, e.Aid,
                    OlcActionRetails.Load(new Key() { { OlcActionRetail.FieldAid.Name, e.Aid } }), OlcActionRetail.CreateNew)
            );

            map.Add(typeof(OlcActionWebhops).FullName,
                SaveCombo("webshop", OlcActionWebhop.FieldWid, OlcActionWebhop.FieldAid, e.Aid,
                    OlcActionWebhops.Load(new Key() { { OlcActionWebhop.FieldAid.Name, e.Aid } }), OlcActionWebhop.CreateNew)
            );

            return map;
        }

        private EntityCollection SaveCombo(string field, Field key, Field rootKey, object rootValue ,EntityCollection items, Func<Entity> newEntity)
        {
            var c = FindRenderable<Combo>(field);
            var l = new List<object>();
            if (c.Value == null)
            {
                foreach (var item in items.AllRows)
                {
                    item.State = DataRowState.Deleted;
                }
                return items;
            }

            if (c.Value is string)
            {
                l.Add(c.Value);
            } else
            {
                l.AddRange((List<object>)c.Value);
            }
             
            var toInsert = new List<Entity>();

            foreach (var comboValue in l)
            {
                var found = false;
                foreach (var item in items.AllRows)
                {
                    if (item[key.Name].Equals(comboValue))
                    {
                        found = true;
                        break;
                    }
                }    
                if (!found)
                {
                    var n = newEntity();
                    n[rootKey] = rootValue;
                    n[key] = comboValue;
                    toInsert.Add(n);
                }
            }

            foreach (var item in items.AllRows)
            {
                var found = false;
                foreach (var comboValue in l)
                {
                    if (item[key.Name].Equals(comboValue))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    item.State = DataRowState.Deleted;
                }
            }
            foreach (var item in toInsert)
            {
                items.Add(item);
            }

            return items;
        }


    }

    internal class ActionSearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(ActionSearchProvider).FullName;

        public static string DefaultOrderByStr = null;

        protected static string m_queryString = @"select * from olc_action";

        protected static QueryArg[] m_argsTemplate = new QueryArg[]
        {
            new QueryArg("aid", FieldType.Integer, QueryFlags.Equals),
            new QueryArg("delstat", FieldType.Integer, QueryFlags.Equals),
            new QueryArg("actiontype", FieldType.Integer, QueryFlags.Equals),
            new QueryArg("name", FieldType.String, QueryFlags.Like),
        };

        protected ActionSearchProvider()
            : base(m_queryString, m_argsTemplate, SearchProviderType.Default)
        {
        }
    }

    internal class ActionBL : DefaultBL1<OlcAction, ActionRules>
    {
        public static readonly string ID = typeof(ActionBL).FullName;

        protected ActionBL()
            : base(DefaultBLFunctions.BasicHideUnhide)
        {
        }

        public static ActionBL New()
        {
            return (ActionBL)ObjectFactory.New(typeof(ActionBL));
        }
        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);

            object x = objects[typeof(OlcAction).Name];
            if (x != null)
                RuleServer.Validate(objects, typeof(ActionRules));

        }
        protected override bool PreSave(BLObjectMap objects, Entity e)
        {
            var b=base.PreSave(objects, e);


            var olcAction = (OlcAction)objects.Default;


            var c = e as OlcActioncountry;
            if (c!= null)
            {
                c.Aid = olcAction.Aid; 
            }

            var r = e as OlcActionRetail;
            if (r != null)
            {
                r.Aid = olcAction.Aid;
            }

            var w = e as OlcActionWebhop;
            if (w != null)
            {
                w.Aid = olcAction.Aid;
            }


            return b;
        }
    }

    internal class ActionSearchTab : SearchTabTemplate1
    {
        public static ActionSearchTab New(DefaultPageSetup setup)
        {
            var t = (ActionSearchTab)ObjectFactory.New(typeof(ActionSearchTab));
            t.Initialize("Action", setup, DefaultActions.Basic | DefaultActions.HideUnhide);
            return t;
        }
    }
} 