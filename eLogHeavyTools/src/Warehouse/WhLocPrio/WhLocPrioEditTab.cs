using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocPrio
{
    public class WhLocPrioEditTab:EditTabTemplate1<OlcWhLocPrio,OlcWhLocPrioRules,OlcWhLocPrioBL>
    {
        public static WhLocPrioEditTab New(DefaultPageSetup setup)
        {
            var t = ObjectFactory.New<WhLocPrioEditTab>();
            t.Initialize(nameof(OlcWhLocPrio), setup);
            return t;
        }

        protected override void CreateBase()
        {
            base.CreateBase();
        }

        protected override OlcWhLocPrio DefaultPageLoad(PageUpdateArgs args)
        {
            var e =  base.DefaultPageLoad(args);
            return e;
        }

        protected override BLObjectMap SaveControlsToBLObjectMap(PageUpdateArgs args, OlcWhLocPrio e)
        {
            return base.SaveControlsToBLObjectMap(args, e);
        }
    }
}
