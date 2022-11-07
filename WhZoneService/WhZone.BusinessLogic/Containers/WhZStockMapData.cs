using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Containers.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Enums;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Containers;

internal class WhZStockMapData : IWhZStockMapData
{
    public IWhZStockMapKey Key { get; init; } = null!;
    public WhZStockMovement Movement { get; init; }
    public decimal Qty { get; init; }
}
