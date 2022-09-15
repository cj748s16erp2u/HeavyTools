using eProjectWeb.Framework;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Reports.Financials.ReminderLetter
{
    public class ReminderLetterEditTab : EditTabTemplate1<ReminderLetterItem, ReminderLetterRules, ReminderLetterBL>
    {
        public static ReminderLetterEditTab New(DefaultPageSetup setup)
        {
            var t = ObjectFactory.New<ReminderLetterEditTab>();
            t.Initialize("ReminderLetter", setup);
            return t;
        }

        protected ReminderLetterEditTab() { }

        protected Control ctrlLedgerLegend;
        protected Combo ctrlTemplate;

        protected override void CreateBase()
        {
            base.CreateBase();

            this.ctrlLedgerLegend = this.EditGroup1["ledger_legend"];
            this.ctrlTemplate = this.EditGroup1["template"] as Combo;

            if (this.ctrlTemplate != null)
            {
                this.OnPageActivate += this.ReminderLetterEditTab_OnPageActivate;
            }
        }

        private void ReminderLetterEditTab_OnPageActivate(PageUpdateArgs args)
        {
            this.ctrlTemplate.ForceReloadList = true;
        }

        protected override ReminderLetterItem DefaultPageLoad(PageUpdateArgs args)
        {
            var e = base.DefaultPageLoad(args);

            this.ctrlLedgerLegend.SetValue("$info_ledger_legend".eLogTransl());
            this.ctrlLedgerLegend.SetCustomHtmlStyles(new Dictionary<string, object>
            {
                { "cssText", "background-color: transparent !important; border: 0 !important;" },
            });

            return e;
        }
    }
}
