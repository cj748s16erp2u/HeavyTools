using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Enums;

public enum PtidPrcType
{
    /// <summary>
    /// Nincs megadva
    /// </summary>
    NotSet = 0, 
    /// <summary>
    /// Elokalkulált beszerzési ár
    /// </summary>
    EstimatedPurchase = 1,
    /// <summary>
    /// Nagyker ár
    /// </summary>
    Wholesale = 2,
    /// <summary>
    /// Kisker ár
    /// </summary>
    Retail = 3,
    /// <summary>
    /// Outlet ár
    /// </summary>
    Outlet = 4,
    /// <summary>
    /// Disztribútor ár
    /// </summary>
    Distributor = 5,
    /// <summary>
    /// Webshop ár
    /// </summary>
    Webshop = 6,
    /// <summary>
    /// Bizományos ár
    /// </summary>
    Commission = 7,
    /// <summary>
    /// Másodosztályú ár
    /// </summary>
    SecondClass = 8
}
