using eLog.Base.Masters.Item;
using eLog.HeavyTools.Warehouse.WhLocPrio;
using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Setup.Warehouse
{
    public class OlcWhLocationSelectTab : SearchTabTemplate1
    {
        public static OlcWhLocationSelectTab New(DefaultPageSetup setup, PageMode mode)
        {
            var t = ObjectFactory.New<OlcWhLocationSelectTab>();
            t.Initialize(nameof(OlcWhLocation), setup, DefaultActions.None, mode);
            return t;
        }
    }
}
