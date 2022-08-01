using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Interfaces;

public interface IWhZStockMapKey : IWhZStockKey, IEquatable<IWhZStockMapKey>
{
    int Whlocid { get; }

    new IWhZStockMapKey CreateKey();
}
