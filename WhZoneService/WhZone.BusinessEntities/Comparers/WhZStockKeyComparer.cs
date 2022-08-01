using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Interfaces;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Comparers;

[System.Diagnostics.DebuggerStepThrough]
public sealed class WhZStockKeyComparer : IEqualityComparer<IWhZStockKey>
{
    public static IEqualityComparer<IWhZStockKey> Instance { get; } = new WhZStockKeyComparer();

    private WhZStockKeyComparer() { }

    bool IEqualityComparer<IWhZStockKey>.Equals(IWhZStockKey? x, IWhZStockKey? y) => this.Equals(x, y);

    int IEqualityComparer<IWhZStockKey>.GetHashCode([DisallowNull] IWhZStockKey obj) => this.GetHashCode(obj);

    public bool Equals(IWhZStockKey? x, IWhZStockKey? y)
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

    public int GetHashCode([DisallowNull] IWhZStockKey obj)
    {
        var hashcode = (obj.Itemid, obj.Whid).GetHashCode();
        if (obj.Whzoneid is not null)
        {
            hashcode *= obj.Whzoneid.GetHashCode();
        }

        return hashcode;
    }
}
