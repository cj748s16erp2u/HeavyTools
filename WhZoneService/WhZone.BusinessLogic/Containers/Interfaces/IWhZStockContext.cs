using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Containers.Interfaces;

public interface IWhZStockContext : IDisposable
{
    IEnumerable<IWhZStockData> MovementList { get; }

    void Clear();
}
