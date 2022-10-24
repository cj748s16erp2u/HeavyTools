using eLog.HeavyTools.Setup.Warehouse;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocLink
{
    public class OlcWhLocLinkLineInfoPart : TabInfoPartTemplate1
    {
        public OlcWhLocLinkLineInfoPart() : base("info", $"{OlcWhLocLinkBL.ID}.GetInfo")
        {

        }

        
    }
}
