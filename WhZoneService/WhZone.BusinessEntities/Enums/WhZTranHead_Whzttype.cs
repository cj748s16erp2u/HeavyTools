using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Enums;

/// <summary>
/// Zóna tranzakció: tranzakció típus
/// </summary>
public enum WhZTranHead_Whzttype
{
    /// <summary>
    /// Bevét
    /// </summary>
    Receiving = 1,
    /// <summary>
    /// Kivét
    /// </summary>
    Issuing = 2,
    /// <summary>
    /// Zónaközi
    /// </summary>
    Transfering = 3,
}
