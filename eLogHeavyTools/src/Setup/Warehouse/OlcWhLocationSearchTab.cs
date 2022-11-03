using eLog.Base.Masters.Item;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.PageParts;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace eLog.HeavyTools.Setup.Warehouse
{
    public class OlcWhLocationSearchTab : DetailSearchTabTemplate1
    {
        public static OlcWhLocationSearchTab New(DefaultPageSetup setup)
        {
            var t = ObjectFactory.New<OlcWhLocationSearchTab>();
            t.Initialize(nameof(OlcWhLocation), setup, "$noroot_OlcWhLocation", DefaultActions.Basic | DefaultActions.Delete);
            return t;
        }

        protected OlcWhLocationSearchTab() { }

        protected Button genNextCmd;
        protected DialogBox dlgMessage;


        protected override void CreateBase()
        {
            this.ParentEntityKey = Consts.DetailEntityKey;
            this.DetailEntityKey = null;

            base.CreateBase();

            this.dlgMessage = new DialogBox(DialogBoxType.Ok);
            this.RegisterDialog(this.dlgMessage);

            AddCmd(genNextCmd = new Button(OlcWhLocationBL.GENNEXT_ACTIONID, 90));
            var loadArgs = "this.getObjectValue(\"" + SearchGridID + "\")";
            SetButtonAction(genNextCmd.ID, new eProjectWeb.Framework.UI.Actions.NewRecordAction(Setup.EditPageName, Consts.DetailEntityKey,
                loadArgs, true), new ControlEvent(m_ActionGenNextEventHandler));
            genNextCmd.Shortcut = eProjectWeb.Framework.UI.Commands.ShortcutKeys.Key_F9;
        }

        private void m_ActionGenNextEventHandler(PageUpdateArgs args)
        {
            if (this.SearchResults.SelectedPK != null)
            {
                var selectedRow = OlcWhLocation.Load(this.SearchResults.SelectedPK);
                if (selectedRow != null)
                {
                    if (selectedRow.Loctype != (int)OlcWhLocation_LocType.Moving)
                    {
                        args.ShowDialog(this.dlgMessage, "$msg_gennext_title", "$msg_gennext_message");
                    }
                    else
                    {
                        args.Continue = true;
                    }
                }
            }
        }

        protected override void CreateCheckForRootEntityKey()
        {
            //this.OnActivate.AddStep(new eProjectWeb.Framework.UI.Script.CopyEntityKeyStep(Consts.RootEntityKey, Consts.DetailEntityKey));
            this.OnActivate.AddStep(new MergeEntityKeyStep(Consts.RootEntityKey, Consts.DetailEntityKey));
            this.OnActivate.AddStep(new eProjectWeb.Framework.UI.Script.CheckEntityKeyStep(this.TabID, this.ParentEntityKey));

            this.CreateTabInfoPartStep();
        }

        protected void CreateTabInfoPartStep()
        {
            foreach (eProjectWeb.Framework.UI.IRenderable r in this.EnumRenderable)
            {
                if (r is TabInfoPartTemplate1)
                {
                    var t = (TabInfoPartTemplate1)r;
                    this.OnActivate.AddStep(new eProjectWeb.Framework.UI.Script.ShowRootEntityInfoStep(t.ControlGroup, t.InfoBusinessLogicID, Consts.DetailEntityKey, true));
                }
            }
        }

        class MergeEntityKeyStep : eProjectWeb.Framework.UI.Script.ScriptStep
        {
            public MergeEntityKeyStep(string source, string target)
                : base($@"
var p = this.getPage(), s = p.getEntityKey('{source}'), t = p.getEntityKey('{target}');

if (!t) {{
    if (s) {{
        p.setEntityKey('{target}', s);
    }}
}} else {{
    if (s) {{
        var e = {{}};

        for (var i in s) {{
            if (s.hasOwnProperty(i)) {{
                e[i] = s[i];
            }}
        }}

        for (var i in t) {{
            if (t.hasOwnProperty(i)) {{
                e[i] = t[i];
            }}
        }}

        p.setEntityKey('{target}', e);
    }}
}}
")
            {
            }
        }
    }
}
