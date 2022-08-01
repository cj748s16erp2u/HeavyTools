using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model.Interfaces;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Interfaces;

public interface IWhZStockKey : IEntity, IEquatable<IWhZStockKey>
{
    int Itemid { get; }
    string Whid { get; }
    int? Whzoneid { get; }

    IWhZStockKey CreateKey();
    string KeyString();
}
