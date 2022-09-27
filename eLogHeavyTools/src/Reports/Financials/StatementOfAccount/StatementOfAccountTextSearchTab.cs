using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Reports.Financials.StatementOfAccount
{
    public class StatementOfAccountTextSearchTab : SearchTabTemplate1
    {
        public static StatementOfAccountTextSearchTab New(DefaultPageSetup setup)
        {
            var t = ObjectFactory.New<StatementOfAccountTextSearchTab>();
            t.Initialize("StatementOfAccountText", setup, DefaultActions.Basic | DefaultActions.Delete);
            return t;
        }

        protected StatementOfAccountTextSearchTab() { }
    }
}
