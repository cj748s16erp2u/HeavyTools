using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        protected override void CreateBase()
        {
            this.ParentEntityKey = Consts.DetailEntityKey;
            this.DetailEntityKey = null;

            base.CreateBase();
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
