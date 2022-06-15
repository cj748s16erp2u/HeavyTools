using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Setup.Warehouse
{
    public partial class OlcWhLocation
    {
        public override void SetDefaultValues()
        {
        }
    }

    public enum OlcWhLocation_LocType
    {
        /// <summary>
        /// Normál - 1
        /// </summary>
        Normal = 1,
        /// <summary>
        /// Mozgó - 2
        /// </summary>
        Moving = 2,
        /// <summary>
        /// Átadó terület - 3
        /// </summary>
        TransferArea = 3,
    }
}
