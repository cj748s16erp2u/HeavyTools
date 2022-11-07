using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Comparers;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Interfaces;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;

public partial class OlcWhzstockmap : Base.Entity, IWhZStockMapKey
{
    public static IEqualityComparer<IWhZStockMapKey> Comparer { get; } = WhZStockMapKeyComparer.Instance;

    public IWhZStockMapKey CreateKey()
    {
        return new WhZStockMapKey
        {
            Itemid = this.Itemid,
            Whid = this.Whid,
            Whzoneid = this.Whzoneid,
            Whlocid = this.Whlocid,
        };
    }

    public string KeyString()
    {
        return $"ItemId: {this.Itemid}, Whid: {this.Whid}, Whzoneid: {this.Whzoneid}, Whlocid: {this.Whlocid}";
    }

    bool IEquatable<IWhZStockMapKey>.Equals(IWhZStockMapKey? other) => Comparer.Equals(this, other);
}
