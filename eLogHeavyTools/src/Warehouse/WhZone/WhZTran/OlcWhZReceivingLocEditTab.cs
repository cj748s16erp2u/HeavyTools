using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhZone.WhZTran
{
    public class OlcWhZReceivingLocEditTab : OlcWhZTranLocEditTab<OlcWhZReceivingLocRules, OlcWhZReceivingLocBL>
    {
        protected OlcWhZReceivingLocEditTab()
        {

        }
        public static OlcWhZReceivingLocEditTab New(DefaultPageSetup setup)
        {
            return New<OlcWhZReceivingLocEditTab>(setup);
        }
    }
}
