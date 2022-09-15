using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Enums;

public enum Discountcalculationtype
{
    /// <summary>
    /// Szétosztás
    /// </summary>
    Division = 0,
    /// <summary>
    /// Egy termék
    /// </summary>
    OneProduct = 1,


    /// <summary>
    /// Csak kedvezmény sor
    /// </summary>
    ExtDiscountOnly = 2,

    /// <summary>
    /// Csak feltétel sor
    /// </summary>
    ExtConditionOnly = 3,

}
