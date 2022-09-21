using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocPrio
{
    public class WhLocPrioAllSearchTab: SearchTabTemplate1
    {
        public static WhLocPrioAllSearchTab New(DefaultPageSetup setup)
        {
            var t = ObjectFactory.New<WhLocPrioAllSearchTab>();
            t.Initialize(nameof(OlcWhLocPrio), setup, DefaultActions.View);
            return t;
        }

        protected override void CreateBase()
        {
            base.CreateBase();
        }
    }
}
