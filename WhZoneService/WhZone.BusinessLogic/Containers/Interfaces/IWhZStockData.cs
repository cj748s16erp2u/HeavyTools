using eLog.HeavyTools.Services.WhZone.BusinessEntities.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Enums;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Containers.Interfaces;

public interface IWhZStockData
{
    IWhZStockMapData StockMapData { get; init; }

    IWhZStockKey Key { get; init; }
    WhZStockMovement Movement { get; init; }
    decimal Qty { get; init; }
}