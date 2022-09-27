using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocLink
{
    public partial class OlcWhLocLinkLine
    {    
        public override void SetDefaultValues()
        {
            this.Whllinktype = (int)OlcWhLocLinkLine_WhlLinkType.Detail;
        }
    }

    public enum OlcWhLocLinkLine_WhlLinkType
    {
        Master = 1,
        Detail = 2
    }
}
