using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
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
    public class ReminderLetterTextEditTab : EditTabTemplate1<ReminderLetterText, ReminderLetterTextRules, ReminderLetterTextBL>
    {
        public static ReminderLetterTextEditTab New(DefaultPageSetup setup)
        {
            var t = ObjectFactory.New<ReminderLetterTextEditTab>();
            t.Initialize(nameof(ReminderLetterText), setup);
            return t;
        }

        protected ReminderLetterTextEditTab() { }

        protected Control ctrlXCVExtCode;
        protected Control ctrlNoteLegend;

        protected override void CreateBase()
        {
            base.CreateBase();

            this.ctrlXCVExtCode = this.EditGroup1["xcvextcode"];
            this.ctrlNoteLegend = this.EditGroup1["note_legend"];
        }

        protected override ReminderLetterText DefaultPageLoad(PageUpdateArgs args)
        {
            var e = base.DefaultPageLoad(args);

            if (args.ActionID == ActionID.Modify)
            {
                this.ctrlXCVExtCode.SetDisabled(true);
            }

            this.ctrlNoteLegend.SetValue("$info_note_legend".eLogTransl().Replace("\\n", "\n"));
            this.ctrlNoteLegend.SetCustomHtmlStyles(new Dictionary<string, object>
            {
                { "cssText", "background-color: transparent !important; border: 0 !important; height: 94px;" },
            });

            return e;
        }
    }
}
