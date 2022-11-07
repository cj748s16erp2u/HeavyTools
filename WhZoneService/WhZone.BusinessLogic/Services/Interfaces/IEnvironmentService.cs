using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;

public interface IEnvironmentService
{
    /// <summary>
    /// Gets the current user identifier.
    /// </summary>
    /// <value>
    /// The current user identifier.
    /// </value>
    string? CurrentUserId { get; }

    /// <summary>
    /// Gets a custom data container.
    /// </summary>
    /// <value>
    /// THe custom data container.
    /// </value>
    ConcurrentDictionary<string, object> CustomData { get; }
}
