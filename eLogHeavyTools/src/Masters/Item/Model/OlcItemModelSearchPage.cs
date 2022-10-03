using eLog.Base.Common;
using eLog.Base.Masters.Item;
using eLog.HeavyTools.Masters.Item.Assortment;
using eLog.HeavyTools.Masters.Item.ItemMatrix;
using eLog.HeavyTools.Masters.Item.MainGroup;
using eLog.HeavyTools.Masters.PriceTable;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.UI.Templates;

namespace eLog.HeavyTools.Masters.Item.Model
{
    internal class OlcItemModelSearchPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcItemModelSearchPage).FullName;

        public static DefaultPageSetup Setup = new DefaultPageSetup("OlcItemModel", OlcItemModelBL.ID,
                                                                    OlcItemModelSearchProvider.ID, OlcItemModelEditPage.ID,
                                                                    typeof(OlcItemModelRules));

        public static DefaultPageSetup SetupModelSeason = new DefaultPageSetup("OlcItemModelSeason", OlcItemModelSeasonBL.ID, null, null);

        public OlcItemModelSearchPage()
            : base("OlcItemModel")
        {
            Tabs.AddTab(delegate { return OlcItemModelSearchTab.New(Setup); });
            Tabs.AddTab(delegate { return OlcItemModelSeasonSearchTab.New(); });
            Tabs.AddTab(delegate { return OlcItemSearchTab.New(); });
            Tabs.AddTab(delegate { return ItemSearchTab3.NewDetail(PageMode.Default, ItemSearchPage.Setup, "$nomodelselected"); });
            Tabs.AddTab(delegate { return OlcPrctableSearchTab2.New(OlcPrctableSearchPage.Setup); });

            Tabs.AddTab(delegate { return OlcItemAssortmentSearchTab.New(); });
        }
    }

    internal class OlcItemModelRules : TypedBaseRuleSet<OlcItemModel>
    {
        public OlcItemModelRules()
            : base(true)
        {
            ERules["code"].Mandatory = false;
            ERules["exclusivetype"].Mandatory = true;
        }
    }

    internal class OlcItemModelEditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcItemModelEditPage).FullName;

        public OlcItemModelEditPage()
            : base("OlcItemModel")
        {
            Tabs.AddTab(delegate { return OlcItemModelEditTab.New(OlcItemModelSearchPage.Setup); });
        }
    }

    internal class OlcItemModelEditTab : EditTabTemplate1<OlcItemModel, OlcItemModelRules, OlcItemModelBL>
    {
        protected OlcItemModelEditTab()
        {
        }

        public static OlcItemModelEditTab New(DefaultPageSetup setup)
        {
            var t = (OlcItemModelEditTab)ObjectFactory.New(typeof(OlcItemModelEditTab));
            t.Initialize("OlcItemModel", setup);
            return t;
        }
    }

    internal class OlcItemModelSearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(OlcItemModelSearchProvider).FullName;

        public static string DefaultOrderByStr = null;

        protected static string m_queryString = @"select * from olc_ItemModel";

        protected static QueryArg[] m_argsTemplate = new QueryArg[]
        {
            new QueryArg("imid", FieldType.Integer, QueryFlags.Equals),
            new QueryArg("delstat", FieldType.Integer, QueryFlags.Equals),
            new QueryArg("code", FieldType.String, QueryFlags.Like),
            new QueryArg("name", FieldType.String, QueryFlags.Like),
        };

        protected OlcItemModelSearchProvider()
            : base(m_queryString, m_argsTemplate, SearchProviderType.Default)
        {
        }
    }

    internal class OlcItemModelBL : DefaultBL1<OlcItemModel, OlcItemModelRules>
    {
        public static readonly string ID = typeof(OlcItemModelBL).FullName;

        protected OlcItemModelBL()
            : base(DefaultBLFunctions.BasicHideUnhide)
        {
        }

        public static OlcItemModelBL New()
        {
            return (OlcItemModelBL)ObjectFactory.New(typeof(OlcItemModelBL));
        }
        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);

            object x = objects[typeof(OlcItemModel).Name];
            if (x != null)
                RuleServer.Validate(objects, typeof(OlcItemModelRules));
        }
        protected override bool PreSave(BLObjectMap objects, Entity e)
        {
            var ee = base.PreSave(objects, e);

            var itemmodel = e as OlcItemModel;
            if (itemmodel != null)
            {
                if (itemmodel.State == DataRowState.Added)
                {
                    var t1 = OlcItemMainGroup.Load(itemmodel.Imgid);
                    t1.Maingrouplastnum = t1.Maingrouplastnum.GetValueOrDefault(0) + 1;
                    t1.Save();
                    var num = t1.Maingrouplastnum.ToString();
                    if (num.Length < 2)
                    {
                        num = "0" + num;
                    }
                    if (num.Length < 3)
                    {
                        num = "0" + num;
                    }
                    if (num.Length > 3)
                    {
                        throw new MessageException("$itemmaingrouptoolong");
                    }
                    itemmodel.Code = t1.Code + num;
                }
            }

            return ee;
        }
    }

    internal class OlcItemModelSearchTab : SearchTabTemplate1
    {
        public static OlcItemModelSearchTab New(DefaultPageSetup setup)
        {
            var t = (OlcItemModelSearchTab)ObjectFactory.New(typeof(OlcItemModelSearchTab));
            t.Initialize("OlcItemModel", setup, DefaultActions.Basic);
            return t;
        }
        protected override void CreateBase()
        {
            base.CreateBase();
            OnPageLoad += this.OlcItemModelSearchTab_OnPageLoad;
        }

        private void OlcItemModelSearchTab_OnPageLoad(PageUpdateArgs args)
        {
            //SrcBar["code"].Value = "A1W22300";
        }
    }
}