using eLog.HeavyTools.Masters.Item.Model;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item
{
    class ItemSearchTab3Info : TabInfoPartTemplate1
    {
        public ItemSearchTab3Info()
            : base("info", OlcItemModelBL.ID + ".GetInfo")
        {
        }

    }
}
