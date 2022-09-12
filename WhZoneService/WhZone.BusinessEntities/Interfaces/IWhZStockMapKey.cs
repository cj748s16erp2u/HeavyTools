using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model.Interfaces;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Interfaces;

public interface IWhZStockMapKey : IEntity, IEquatable<IWhZStockMapKey>
{
    int Itemid { get; }
    string Whid { get; }
    int? Whzoneid { get; }
    int? Whlocid { get; }

    IWhZStockMapKey CreateKey();
    string KeyString();
}
