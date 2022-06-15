using eLog.Base.Common; 
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.Templates;

namespace eLog.HeavyTools.Masters.Item.MainGroup
{
    internal class OlcItemMainGroupSearchPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcItemMainGroupSearchPage).FullName;

        public static DefaultPageSetup Setup = new DefaultPageSetup("OlcItemMainGroup", OlcItemMainGroupBL.ID,
                                                                    OlcItemMainGroupSearchProvider.ID, OlcItemMainGroupEditPage.ID,
                                                                    typeof(OlcItemMainGroupRules));


        public OlcItemMainGroupSearchPage()
            : base("OlcItemMainGroup")
        {
            Tabs.AddTab(delegate { return OlcItemMainGroupSearchTab.New(Setup); });
        }
    }

    internal class OlcItemMainGroupRules : TypedBaseRuleSet<OlcItemMainGroup>
    {
        public OlcItemMainGroupRules()
            : base(true)
        {
            ERules["code"].Mandatory = false;
        }
    }

    internal class OlcItemMainGroupEditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcItemMainGroupEditPage).FullName;

        public OlcItemMainGroupEditPage()
            : base("OlcItemMainGroup")
        {
            Tabs.AddTab(delegate { return OlcItemMainGroupEditTab.New(OlcItemMainGroupSearchPage.Setup); });
        }
    }

    internal class OlcItemMainGroupEditTab : EditTabTemplate1<OlcItemMainGroup, OlcItemMainGroupRules, OlcItemMainGroupBL>
    {
        protected OlcItemMainGroupEditTab()
        {
        }

        public static OlcItemMainGroupEditTab New(DefaultPageSetup setup)
        {
            var t = (OlcItemMainGroupEditTab)ObjectFactory.New(typeof(OlcItemMainGroupEditTab));
            t.Initialize("OlcItemMainGroup", setup);
            return t;
        }
        protected override OlcItemMainGroup DefaultPageLoad_LoadEntity(PageUpdateArgs args)
        {
            var e= base.DefaultPageLoad_LoadEntity(args);

            FindRenderable<Combo>("imgt1id").Disabled = true;

            return e;
        }
    }

    internal class OlcItemMainGroupSearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(OlcItemMainGroupSearchProvider).FullName;

        public static string DefaultOrderByStr = null;

        protected static string m_queryString = @"select img.*,  isrh.name isrhname, imgt2.groupname groupname2
  from olc_itemmaingroup img
  left join olc_itemsizerangehead isrh on isrh.isrhid=img.isrhid
  left join olc_itemmaingrouptype2 imgt2 on imgt2.imgt2id=img.imgt2id";

        protected static QueryArg[] m_argsTemplate = new QueryArg[]
        {
            new QueryArg("imgid", "img", FieldType.String, QueryFlags.Like),
            new QueryArg("code", "img", FieldType.String, QueryFlags.Like),
            new QueryArg("name", "img", FieldType.String, QueryFlags.Like),
            new QueryArg("delstat","img", FieldType.Integer, QueryFlags.Equals),
        };

        protected OlcItemMainGroupSearchProvider()
            : base(m_queryString, m_argsTemplate, SearchProviderType.Default)
        {
        }
    }

    internal class OlcItemMainGroupBL : DefaultBL1<OlcItemMainGroup, OlcItemMainGroupRules>
    {
        public static readonly string ID = typeof(OlcItemMainGroupBL).FullName;

        protected OlcItemMainGroupBL()
            : base(DefaultBLFunctions.BasicHideUnhide)
        {
        }

        public static OlcItemMainGroupBL New()
        {
            return (OlcItemMainGroupBL)ObjectFactory.New(typeof(OlcItemMainGroupBL));
        }
        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);

            object x = objects[typeof(OlcItemMainGroup).Name];
            if (x != null)
                RuleServer.Validate(objects, typeof(OlcItemMainGroupRules));

        }
        protected override bool PreSave(BLObjectMap objects, Entity e)
        {
            var ee=base.PreSave(objects, e);

            var olcitemmaingroup = e as OlcItemMainGroup;
            if (olcitemmaingroup != null)
            {
                if (olcitemmaingroup.State == DataRowState.Added)
                {
                    var t1 = OlcItemMainGroupType1.Load(olcitemmaingroup.Imgt1id);
                    t1.Grouplastnum = t1.Grouplastnum.GetValueOrDefault(0) + 1;
                    t1.Save();
                    var num = t1.Grouplastnum.ToString();
                    if (num.Length < 2)
                    {
                        num = "0" + num;
                    }
                    if (num.Length > 3)
                    {
                        throw new MessageException("$itemmaingrouptoolong");
                    }
                    olcitemmaingroup.Code = olcitemmaingroup.Imgt1id + num;
                }
            } 

            return ee;
        }
    }

    internal class OlcItemMainGroupSearchTab : SearchTabTemplate1
    {
        public static OlcItemMainGroupSearchTab New(DefaultPageSetup setup)
        {
            var t = (OlcItemMainGroupSearchTab)ObjectFactory.New(typeof(OlcItemMainGroupSearchTab));
            t.Initialize("OlcItemMainGroup", setup, DefaultActions.Basic);
            return t;
        }
    }
}