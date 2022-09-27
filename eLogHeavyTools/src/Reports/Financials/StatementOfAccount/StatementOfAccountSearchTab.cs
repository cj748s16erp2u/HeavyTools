using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Reports.Financials.StatementOfAccount
{
    public class StatementOfAccountSearchTab : SearchTabTemplate1
    {
        public static StatementOfAccountSearchTab New(DefaultPageSetup setup)
        {
            var t = ObjectFactory.New<StatementOfAccountSearchTab>();
            t.Initialize("StatementOfAccount", setup, DefaultActions.Basic | DefaultActions.Delete);
            return t;
        }

        protected StatementOfAccountSearchTab() { }

        protected override void Delete_ConfirmYes(PageUpdateArgs args)
        {
            base.Delete_ConfirmYes(args);
        }
    }
}
