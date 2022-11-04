using eLog.HeavyTools.Setup.Warehouse;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhZone.WhZTran
{
    public abstract class OlcWhZTranHeadEditTab<TRule, TBL> : EditTabTemplate1<OlcWhZTranHead, TRule, TBL>
        where TRule: OlcWhZTranHeadRules
        where TBL: OlcWhZTranHeadBL<TRule>
    {

        public static TTab New<TTab>(DefaultPageSetup setup) where TTab : OlcWhZTranHeadEditTab<TRule, TBL>
        {
            var tTabType = typeof(TTab);

            var t = ObjectFactory.New<TTab>();
            t.Initialize(tTabType.Name, setup);
            return t;
        }      
    }
}
