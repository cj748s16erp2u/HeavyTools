using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocPrio
{
    public class OlcWhLocPrioAllSearchTab : SearchTabTemplate1
    {
        public static OlcWhLocPrioAllSearchTab New(DefaultPageSetup setup)
        {
            var t = ObjectFactory.New<OlcWhLocPrioAllSearchTab>();
            t.Initialize(nameof(OlcWhLocPrio), setup, DefaultActions.View);
            return t;
        }

        protected override void CreateBase()
        {
            base.CreateBase();
        }
    }
}
