using eProjectWeb.Framework.UI.Templates;
using eProjectWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhZone.WhZTran
{
    public abstract class OlcWhZTranLocEditTab<TRule, TBL> : EditTabTemplate1<OlcWhZTranLoc, TRule, TBL>
        where TRule : OlcWhZTranLocRules
        where TBL : OlcWhZTranLocBL<TRule>
    {

        public static TTab New<TTab>(DefaultPageSetup setup) where TTab : OlcWhZTranLocEditTab<TRule, TBL>
        {
            var tTabType = typeof(TTab);

            var t = ObjectFactory.New<TTab>();
            t.Initialize(tTabType.Name, setup);
            return t;
        }
    }
}
