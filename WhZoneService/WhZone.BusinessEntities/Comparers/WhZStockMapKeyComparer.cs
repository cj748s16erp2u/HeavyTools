using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Interfaces;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Comparers;

[System.Diagnostics.DebuggerStepThrough]
public sealed class WhZStockMapKeyComparer : IEqualityComparer<IWhZStockMapKey>
{
    public static IEqualityComparer<IWhZStockMapKey> Instance { get; } = new WhZStockMapKeyComparer();

    private WhZStockMapKeyComparer() { }

    bool IEqualityComparer<IWhZStockMapKey>.Equals(IWhZStockMapKey? x, IWhZStockMapKey? y) => this.Equals(x, y);

    int IEqualityComparer<IWhZStockMapKey>.GetHashCode([DisallowNull] IWhZStockMapKey obj) => this.GetHashCode(obj);

    public bool Equals(IWhZStockMapKey? x, IWhZStockMapKey? y)
    {
        if (x is null || y is null)
        {
            return false;
        }

        if (ReferenceEquals(x, y))
        {
            return true;
        }

        return this.GetHashCode(x) == this.GetHashCode(y);
    }

    public int GetHashCode([DisallowNull] IWhZStockMapKey obj)
    {
        var hashcode = (obj.Itemid, obj.Whid, obj.Whlocid).GetHashCode();
        if (obj.Whzoneid is not null)
        {
            hashcode *= obj.Whzoneid.GetHashCode();
        }

        return hashcode;
    }
}
