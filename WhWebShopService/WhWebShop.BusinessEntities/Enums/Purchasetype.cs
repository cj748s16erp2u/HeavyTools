using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Enums;

public enum Purchasetype
{
    /// <summary>
    /// Összes vásárlás
    /// </summary>
    All = 0,
    /// <summary>
    /// Első vásárlás
    /// </summary>
    First=1,
    /// <summary>
    /// Csak a legolcsóbb termék megvásárlásához, több termék vásárlásakor
    /// </summary>
    Cheapest = 2
}
