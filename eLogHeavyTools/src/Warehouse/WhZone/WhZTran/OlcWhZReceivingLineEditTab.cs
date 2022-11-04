using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhZone.WhZTran
{
    public class OlcWhZReceivingLineEditTab : OlcWhZTranLineEditTab<OlcWhZReceivingLineRules, OlcWhZReceivingLineBL>
    {
        protected OlcWhZReceivingLineEditTab()
        {

        }
        public static OlcWhZReceivingLineEditTab New(DefaultPageSetup setup)
        {
            return New<OlcWhZReceivingLineEditTab>(setup);
        }

        protected override void CreateBase()
        {
            base.CreateBase();
        }

        protected override OlcWhZTranLine DefaultPageLoad(PageUpdateArgs args)
        {
            return base.DefaultPageLoad(args);
        }
    }
}
