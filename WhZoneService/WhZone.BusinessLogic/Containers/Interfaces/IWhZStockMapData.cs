using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Enums;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Containers.Interfaces;

public interface IWhZStockMapData
{
    IWhZStockMapKey Key { get; init; }
    WhZStockMovement Movement { get; init; }
    decimal Qty { get; init; }
}
