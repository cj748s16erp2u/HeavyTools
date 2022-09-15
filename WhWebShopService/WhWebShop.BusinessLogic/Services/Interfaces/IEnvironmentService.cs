using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;

public interface IEnvironmentService
{
    /// <summary>
    /// Gets the current user identifier.
    /// </summary>
    /// <value>
    /// The current user identifier.
    /// </value>
    string? CurrentUserId { get; }
}
