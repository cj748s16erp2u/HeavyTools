using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Comparers;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Interfaces;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;

[System.Diagnostics.DebuggerDisplay("{KeyString(),nq}")]
public class WhZStockMapKey : Base.EntityDto, IWhZStockMapKey
{
    public static IEqualityComparer<IWhZStockMapKey> Comparer { get; } = WhZStockMapKeyComparer.Instance;

    public int Itemid { get; init; }
    public string Whid { get; init; } = null!;
    public int? Whzoneid { get; init; }
    public int Whlocid { get; init; }

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

    IWhZStockKey IWhZStockKey.CreateKey()
    {
        return new WhZStockKey
        {
            Itemid = this.Itemid,
            Whid = this.Whid,
            Whzoneid = this.Whzoneid
        };
    }

    bool IEquatable<IWhZStockKey>.Equals(IWhZStockKey? other) => WhZStockKeyComparer.Instance.Equals(this, other);
}
