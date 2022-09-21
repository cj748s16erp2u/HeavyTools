using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocPrio
{
    public partial class OlcWhLocPrio
    {
        public override void SetDefaultValues()
        {
            
        }

        public enum OlcWhLocPrio_PrioType
        { 
            /// <summary>
            /// Primary location - 1
            /// </summary>
            Primary = 1,
            /// <summary>
            /// Secondary location - 2
            /// </summary>
            Secondary = 2
        }
    }
}
