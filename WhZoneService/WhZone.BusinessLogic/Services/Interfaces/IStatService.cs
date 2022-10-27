using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;

public interface IStatService
{
    Task<OlsStatline?> GetStatusValueAsync(string statKey, int value, CancellationToken cancellationToken = default);
}
