using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Reports.Financials.ReminderLetter
{
    public class ReminderLetterSearchTab : SearchTabTemplate1
    {
        public static ReminderLetterSearchTab New(DefaultPageSetup setup)
        {
            var t = ObjectFactory.New<ReminderLetterSearchTab>();
            t.Initialize("ReminderLetter", setup, DefaultActions.Basic | DefaultActions.Delete);
            return t;
        }

        protected ReminderLetterSearchTab() { }

        protected override void Delete_ConfirmYes(PageUpdateArgs args)
        {
            base.Delete_ConfirmYes(args);
        }
    }
}
