using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.UI.Actions;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.Script;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Sales.Sord
{
    public class SordSordPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(SordSordPage).FullName;

        public static DefaultPageSetup Setup = new DefaultPageSetup("SordSord", SordSordBL.ID, SordSordSearchProvider.ID, null);

        public SordSordPage()
            : base("sordsord")
        {
            base.Tabs.AddTab(() => SordSordInlineEditTab.New());
        }
    }

    public class SordSordSearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(SordSordSearchProvider).FullName;

        public static string DefaultOrderByStr = null;

        protected static string m_queryString = @"select * from olc_tmp_sordsord tmp ";

        protected static QueryArg[] m_argsTemplate = new QueryArg[]
        {
            new QueryArg("ssid", FieldType.Guid, QueryFlags.Equals),
            new QueryArg("sordlineid", "tmp", FieldType.Integer),
            new QueryArg("nonzero", FieldType.Integer),
            new QueryArg("docnum", FieldType.String, QueryFlags.Like),
            new QueryArg("itemcode", FieldType.String, QueryFlags.Like),
            new QueryArg("name01", FieldType.String, QueryFlags.Like),
            new QueryArg("name02", FieldType.String, QueryFlags.Like),
            new QueryArg("ref2", FieldType.String),
            new QueryArg("ref2_like", "ref2", null, FieldType.String, QueryFlags.Like),
            new QueryArg("reqdatefrom", "reqdate", null, FieldType.DateTime, QueryFlags.Greater | QueryFlags.Equals),
            new QueryArg("reqdateto", "reqdate", null, FieldType.DateTime, QueryFlags.Less | QueryFlags.Equals)
        };

        protected SordSordSearchProvider()
            : base(m_queryString, m_argsTemplate, SearchProviderType.Default)
        {
            SetCustomFunc("nonzero", NonZeroFilter);
        }
        protected void NonZeroFilter(StringBuilder sb, QueryArg arg, string quotedFieldName, object argValue)
        {
            bool nonzero;
            if (argValue is bool)
                nonzero = (bool)argValue;
            else
                nonzero = ((argValue as int?).GetValueOrDefault() != 0);

            if (nonzero)
                sb.Append("qty <> 0");
        }
        protected override string CreateQueryString(Dictionary<string, object> args, bool fmtonly)
        {
            var sql=base.CreateQueryString(args, fmtonly);

            return sql;
        }
    }

    public class SordSordInlineEditTab : InlineEditTabTemplate1<OlcTmpSordSord, Base.Common.TypedBaseRuleSet<OlcTmpSordSord>, SordSordBL>
    {

        protected Button cmdOK;

        public static SordSordInlineEditTab New()
        {
            SordSordInlineEditTab t = (SordSordInlineEditTab)ObjectFactory.New(typeof(SordSordInlineEditTab));
            t.Initialize("sordst", SordSordPage.Setup, DefaultActions.Modify);
            return t;
        }

        protected SordSordInlineEditTab()
        {
        }

        protected override void CreateDefaultActions()
        {
            base.CreateDefaultActions();

            AddCmd(new Button("clear"));
            SetButtonAction("clear", new ControlEvent(OnCmdClear));

            AddCmd(new Button("fill"));
            SetButtonAction("fill", new ControlEvent(OnCmdFill));

            AddCmd(new Button("cancel"));
            SetButtonAction("cancel", EditCancelAction.Instance);

            cmdOK = new Button("ok");
            AddCmd(cmdOK);
            SetButtonAction("ok", new ControlEvent(OnCmdOK));

            SearchResults.MergePageData = Consts.RootEntityKey;

            this.OnActivate.AddStep(new LoadEntityKeyStep(Consts.RootEntityKey));

            this.OnActivate.AddStep(new ShowObjectStep(SearchGridID));
        }

        protected override void RenderTextResources(/*RenderContext ctx*/)
        {
            base.RenderTextResources(/*ctx*/);

            TabSettings.TextResource["tabName_sordst"] = eProjectWeb.Framework.Lang.Translator.Translate("$tabName_sordst");
        }

        protected virtual void OnCmdOK(PageUpdateArgs args)
        {
            Key k = new Key(args.PageData[Consts.RootEntityKey]);
            var bl = SordSordBL.New();
            List<Key> newRecordPKs = bl.CommitSordSord(k);
            // frissiteni kell a grid-et a kivet tetel fulon
            args.ClosePage(new ClosePageStep(Key.ToJSONKeyArray(newRecordPKs), Consts.EditedPK));
        }

        protected virtual void OnCmdClear(PageUpdateArgs args)
        {
            Key k = new Key(args.PageData[Consts.RootEntityKey]);
            var bl = SordSordBL.New();

            bl.ClearOlcTmpSordSord(k, SearchResults.SelectedPKS);
            args.AddExecCommand(new eProjectWeb.Framework.UI.Script.RefreshObjectStep(SearchResults.ID));
        }

        protected virtual void OnCmdFill(PageUpdateArgs args)
        {
            Key k = new Key(args.PageData[Consts.RootEntityKey]);
            var bl = SordSordBL.New(); 
            bl.FillOlcTmpSordSord(k, SearchResults.SelectedPKS);
            args.AddExecCommand(new eProjectWeb.Framework.UI.Script.RefreshObjectStep(SearchResults.ID));
        }
    }

}
