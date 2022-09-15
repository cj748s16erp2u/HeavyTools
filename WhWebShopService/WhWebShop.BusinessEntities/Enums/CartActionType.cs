using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Enums;

public enum CartActionType
{
    /// <summary>
    /// Aktuális ár
    /// </summary>
    None,
    /// <summary>
    /// Ártábla akció
    /// </summary>
    PrcTable,
    /// <summary>
    /// Kedvezményben részt vesz
    /// </summary>
    Calculated,
    /// <summary>
    /// Aktuális számolásban részvesz
    /// </summary>
    Reserved,
    /// <summary>
    /// Kedvezmény rászámolva, de visszavonható
    /// </summary>
    TempCalculated,
    /// <summary>
    /// Egyedi ár
    /// </summary>
    HandPrice,
}
