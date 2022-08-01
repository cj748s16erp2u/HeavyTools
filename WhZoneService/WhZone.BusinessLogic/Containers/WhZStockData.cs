using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Containers.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Enums;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Containers;

internal class WhZStockData : IWhZStockData
{
    public IWhZStockMapData StockMapData { get; init; } = null!;

    public IWhZStockKey Key { get; init; } = null!;
    public WhZStockMovement Movement { get; init; }
    public decimal Qty { get; init; }
}
