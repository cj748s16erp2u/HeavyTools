using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Containers.Interfaces;

public interface IWhZStockMapContext : IDisposable
{
    IEnumerable<IWhZStockMapData> MovementList { get; }

    void Clear();
}
