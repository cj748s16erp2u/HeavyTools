using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Comparers;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Interfaces;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;

[System.Diagnostics.DebuggerDisplay("{KeyString(),nq}")]
public class WhZStockKey : Base.EntityDto, IWhZStockKey
{
    public static IEqualityComparer<IWhZStockKey> Comparer { get; } = WhZStockKeyComparer.Instance;

    public int Itemid { get; init; }
    public string Whid { get; init; } = null!;
    public int? Whzoneid { get; init; }

    public IWhZStockKey CreateKey()
    {
        return new WhZStockKey
        {
            Itemid = this.Itemid,
            Whid = this.Whid,
            Whzoneid = this.Whzoneid,
        };
    }

    public string KeyString()
    {
        return $"Itemid: {this.Itemid}, Whid: {this.Whid}, Whzoneid: {this.Whzoneid}";
    }

    bool IEquatable<IWhZStockKey>.Equals(IWhZStockKey? other) => Comparer.Equals(this, other);
}