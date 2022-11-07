using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Enums;

public enum WhZStockMovement
{
    /// <summary>
    /// Increase the value of <see cref="OlcWhzstock.Recqty"/>
    /// </summary>
    AddReceving,
    /// <summary>
    /// Increase the value of <see cref="OlcWhzstock.Resqty"/>
    /// </summary>
    AddReserved,
    /// <summary>
    /// Decrease the value of <see cref="OlcWhzstock.Recqty"/>
    /// </summary>
    RemoveReceiving,
    /// <summary>
    /// Decrease the value of <see cref="OlcWhzstock.Resqty"/>
    /// </summary>
    RemoveReserved,
    /// <summary>
    /// Commit the some value of <see cref="OlcWhzstock.Recqty"/> to <see cref="OlcWhzstock.Actqty"/>
    /// </summary>
    CommitReceving,
    /// <summary>
    /// Commit the some value of <see cref="OlcWhzstock.Resqty"/> to <see cref="OlcWhzstock.Actqty"/>
    /// </summary>
    CommitReserved,
}
