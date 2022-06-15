using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item.MainGroup
{
    class OlcItemSizeRangeInfo : TabInfoPartTemplate1
    {
        public OlcItemSizeRangeInfo()
            : base("info", OlcItemSizeRangeBL.ID + ".GetInfo")
        {
        } 
    }
}
