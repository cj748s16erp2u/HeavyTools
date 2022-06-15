using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Setup.Warehouse
{
    public class OlcWhZoneInfoPart : TabInfoPartTemplate1
    {
        public OlcWhZoneInfoPart() : base("info", $"{OlcWhZoneBL.ID}.GetInfo")
        {
        }
    }
}
