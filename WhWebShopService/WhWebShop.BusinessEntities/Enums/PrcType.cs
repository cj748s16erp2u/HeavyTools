using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Enums;

public enum PrcType
{
    /// <summary>
    /// Eredeti
    /// </summary>
    Original=1,
    /// <summary>
    /// Aktuális
    /// </summary>
    Actual=2,
    /// <summary>
    /// Akció alapja
    /// </summary>
    BasisOfAction=3,
    /// <summary>
    /// Akciós ár
    /// </summary>
    SalePrice=4
}
