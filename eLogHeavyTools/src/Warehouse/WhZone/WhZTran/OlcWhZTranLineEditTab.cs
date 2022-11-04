using eProjectWeb.Framework.UI.Templates;
using eProjectWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhZone.WhZTran
{
    public abstract class OlcWhZTranLineEditTab<TRule, TBL> : EditTabTemplate1<OlcWhZTranLine, TRule, TBL>
        where TRule : OlcWhZTranLineRules
        where TBL : OlcWhZTranLineBL<TRule>
    {

        public static TTab New<TTab>(DefaultPageSetup setup) where TTab : OlcWhZTranLineEditTab<TRule, TBL>
        {
            var tTabType = typeof(TTab);

            var t = ObjectFactory.New<TTab>();
            t.Initialize(tTabType.Name, setup);
            return t;
        }
    }
}
