using eLog.Base.Common;
using eLog.HeavyTools.Masters.Item;
using eLog.HeavyTools.Masters.Item.Model;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Rules;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Text;

namespace eLog.HeavyTools.Masters.PriceTable
{
    internal class OlcPrctableSearchPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcPrctableSearchPage).FullName;

        public static DefaultPageSetup Setup = new DefaultPageSetup("OlcPrctable", OlcPrctableBL.ID,
                                                                    OlcPrctableSearchProvider.ID, OlcPrctableEditPage.ID,
                                                                    typeof(OlcPrctableRules));


        public OlcPrctableSearchPage()
            : base("OlcPrctable")
        {
            Tabs.AddTab(delegate { return OlcPrctableSearchTab.New(Setup); });
        }
    }

    internal class OlcPrctableRules : TypedBaseRuleSet<OlcPrctable>
    {
        public OlcPrctableRules()
            : base(true)
        {
            AddCustomRule(UniqRule);
            AddCustomRule(ItemRule);
        }

        private void ItemRule(RuleValidateContext ctx, OlcPrctable pt)
        {
            if (pt.Itemid.HasValue)
            {
                var ok = false;
                var ci = OlcItem.Load(pt.Itemid);

                if (ci != null)
                {
                    var ims = OlcItemModelSeason.Load(ci.Imsid);
                    if (ims != null)
                    {
                        if (ims.Imid == pt.Imid)
                        {
                            ok = true;
                        }
                    }
                }

                if (!ok)
                {
                    ctx.AddErrorField(OlcPrctable.FieldItemid, "$wringitemid");
                }
            }
        }

        private void UniqRule(RuleValidateContext ctx, OlcPrctable pt)
        {
            var sql = "select count(0) from olc_Prctable where 1=1 ";

            if (pt.Prcid.HasValue)
            {
                sql += " and prcid<>" + pt.Prcid;
            }

            sql += " and ptid=" + pt.Ptid;
            sql += " and curid='" + pt.Curid + "'";

            AddIntNullFilter(ref sql, OlcPrctable.FieldPartnid, pt);
            AddIntNullFilter(ref sql, OlcPrctable.FieldAddrid, pt);
            AddIntNullFilter(ref sql, OlcPrctable.FieldImid, pt);
            AddIntNullFilter(ref sql, OlcPrctable.FieldImid, pt);



            sql += string.Format("  and ( convert(datetime, '{0}')  <=  enddate and convert(datetime, '{1}') >= startdate )",
                    pt.Startdate.Value.ToString("yyyy-MM-dd"),
                    pt.Enddate.Value.ToString("yyyy-MM-dd"));

            AddStringNullFilter(ref sql, OlcPrctable.FieldIsid, pt);
            AddStringNullFilter(ref sql, OlcPrctable.FieldIcid, pt);

            var c = (int)SqlDataAdapter.ExecuteSingleValue(DB.Main, sql);
            if (c > 0)
            {
                ctx.AddError("$overmaperror");
            }
        }

        private void AddStringNullFilter(ref string sql, Field f, OlcPrctable pt)
        {
            var v = ConvertUtils.ToString(pt[f]);

            if (string.IsNullOrEmpty(v))
            { 
                sql += " and " + f.Name + " is null ";
            }
            else
            {
                sql += " and " + f.Name + "= " + Utils.SqlToString(v);
            }
        }

        private void AddIntNullFilter(ref string sql, Field f, OlcPrctable pt)
        {
            var v = ConvertUtils.ToInt32(pt[f]);

            if (v.HasValue)
            {
                sql += " and " + f.Name + " = " + v;
            } else
            {
                sql += " and " + f.Name + " is null ";
            }
        }
    }

    internal class OlcPrctableEditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcPrctableEditPage).FullName;

        public OlcPrctableEditPage()
            : base("OlcPrctable")
        {
            Tabs.AddTab(delegate { return OlcPrctableEditTab.New(OlcPrctableSearchPage.Setup); });
        }
    }

    internal class OlcPrctableEditTab : EditTabTemplate1<OlcPrctable, OlcPrctableRules, OlcPrctableBL>
    {
        protected OlcPrctableEditTab()
        {
        }

        public static OlcPrctableEditTab New(DefaultPageSetup setup)
        {
            var t = (OlcPrctableEditTab)ObjectFactory.New(typeof(OlcPrctableEditTab));
            t.Initialize("OlcPrctable", setup);
            return t;
        }
        protected override BLObjectMap SaveControlsToBLObjectMap(PageUpdateArgs args, OlcPrctable e)
        {
            var map= base.SaveControlsToBLObjectMap(args, e);

            FindRenderable("EditGroup2").DataBind(e, true);

            FindRenderable("EditGroup3").DataBind(e, true);

            return map;
        }

        protected override OlcPrctable DefaultPageLoad(PageUpdateArgs args)
        {
            var e= base.DefaultPageLoad(args);


            FindRenderable("EditGroup2").DataBind(e, false);

            FindRenderable("EditGroup3").DataBind(e, false);
            return e;
        }

        protected override void CreateBase()
        {
            base.CreateBase();
            OnPageLoad += OlcPrctableEditTab_OnPageLoad;
        }

        private void OlcPrctableEditTab_OnPageLoad(PageUpdateArgs args)
        {
             if (args.PageData.ContainsKey(Consts.RootEntityKey))
            {
                FindRenderable("imid").Disabled = true;
            }
        }
    }

    internal class OlcPrctableSearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(OlcPrctableSearchProvider).FullName;

        public static string DefaultOrderByStr = null;

        protected static string m_queryString = @"select pt.*, p.name as pname , a.name aname, i.itemcode, i.name01, im.code
  from olc_Prctable pt
  left join ols_partner p on p.partnid=pt.partnid
  left join ols_partnaddr a on a.addrid= pt.addrid
  left join ols_item i on i.itemid=pt.itemid
  left join olc_itemmodel im on im.imid= pt.imid";

        protected static QueryArg[] m_argsTemplate = new QueryArg[]
        {
            new QueryArg("prcid", FieldType.Integer, QueryFlags.Equals),

            new QueryArg("partnname", "name","p", FieldType.String, QueryFlags.Like),
            new QueryArg("addrname", "name","a", FieldType.String, QueryFlags.Like),
            new QueryArg("curid", FieldType.String, QueryFlags.Equals),
            new QueryArg("date", FieldType.DateTime), 
            new QueryArg("imid","pt", FieldType.Integer, QueryFlags.Equals),
            new QueryArg("isid", FieldType.String, QueryFlags.Equals),
            new QueryArg("ptid", FieldType.Integer, QueryFlags.Equals),
            new QueryArg("icid", FieldType.String, QueryFlags.Equals),
            new QueryArg("itemcode", FieldType.String, QueryFlags.Like),
            new QueryArg("itemname", "name01", "i", FieldType.String, QueryFlags.Like),
            new QueryArg("delstat", "pt", FieldType.Integer, QueryFlags.Equals),
        };

        protected OlcPrctableSearchProvider()
            : base(m_queryString, m_argsTemplate, SearchProviderType.Default)
        {

            SetCustomFunc("date", DateFilter); 
        }


        private void DateFilter(StringBuilder sb, QueryArg arg, string quotedFieldName, object argValue)
        {
            if (argValue != null) {
                sb.Append(string.Format("{0} between startdate and enddate", Utils.SqlToString(argValue)));
            } }
    }

    internal class OlcPrctableBL : DefaultBL1<OlcPrctable, OlcPrctableRules>
    {
        public static readonly string ID = typeof(OlcPrctableBL).FullName;

        protected OlcPrctableBL()
            : base(DefaultBLFunctions.BasicHideUnhide)
        {
        }

        public static OlcPrctableBL New()
        {
            return (OlcPrctableBL)ObjectFactory.New(typeof(OlcPrctableBL));
        }
        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);

            object x = objects[typeof(OlcPrctable).Name];
            if (x != null)
                RuleServer.Validate(objects, typeof(OlcPrctableRules));

        }
    }

    internal class OlcPrctableSearchTab : SearchTabTemplate1
    {
        public static OlcPrctableSearchTab New(DefaultPageSetup setup)
        {
            var t = (OlcPrctableSearchTab)ObjectFactory.New(typeof(OlcPrctableSearchTab));
            t.Initialize("OlcPrctable", setup, DefaultActions.Basic | DefaultActions.HideUnhide);
            return t;
        }
    }

    internal class OlcPrctableSearchTab2 : DetailSearchTabTemplate1
    {
        public static OlcPrctableSearchTab2 New(DefaultPageSetup setup)
        {
            var t = (OlcPrctableSearchTab2)ObjectFactory.New(typeof(OlcPrctableSearchTab2));
            t.Initialize("OlcPrctable", setup, "$noroot", DefaultActions.Basic | DefaultActions.HideUnhide);
            return t;
        }
    }
} 