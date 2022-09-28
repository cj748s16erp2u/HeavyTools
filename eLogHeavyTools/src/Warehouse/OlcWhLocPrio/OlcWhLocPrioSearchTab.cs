using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocPrio
{
    public class OlcWhLocPrioSearchTab : SearchTabTemplate1
    {
        public static OlcWhLocPrioSearchTab New(DefaultPageSetup setup)
        {
            var t = ObjectFactory.New<OlcWhLocPrioSearchTab>();
            t.Initialize(nameof(OlcWhLocPrio), setup, DefaultActions.NewModify);
            return t;
        }

        protected override void CreateBase()
        {
            base.CreateBase();
        }

    }
}
