using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Reports.Financials.ReminderLetter
{
    public class ReminderLetterTextSearchTab : SearchTabTemplate1
    {
        public static ReminderLetterTextSearchTab New(DefaultPageSetup setup)
        {
            var t = ObjectFactory.New<ReminderLetterTextSearchTab>();
            t.Initialize("ReminderLetterText", setup, DefaultActions.Basic | DefaultActions.Delete);
            return t;
        }

        protected ReminderLetterTextSearchTab() { }
    }
}
