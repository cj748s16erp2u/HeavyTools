using eProjectWeb.Framework;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Reports.Financials.StatementOfAccount
{
    public class StatementOfAccountEditTab : EditTabTemplate1<StatementOfAccountItem, StatementOfAccountRules, StatementOfAccountBL>
    {
        public static StatementOfAccountEditTab New(DefaultPageSetup setup)
        {
            var t = ObjectFactory.New<StatementOfAccountEditTab>();
            t.Initialize("StatementOfAccount", setup);
            return t;
        }

        protected StatementOfAccountEditTab() { }

        protected Combo ctrlTemplate;

        protected override void CreateBase()
        {
            base.CreateBase();

            this.ctrlTemplate = this.EditGroup1["template"] as Combo;

            if (this.ctrlTemplate != null)
            {
                this.OnPageActivate += this.StatementOfAccountEditTab_OnPageActivate;
            }
        }

        private void StatementOfAccountEditTab_OnPageActivate(PageUpdateArgs args)
        {
            this.ctrlTemplate.ForceReloadList = true;
        }
    }
}
