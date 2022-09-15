using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;

public interface IOlsCurrencyCacheService
{
    Task<int> GetRound(string curid, CancellationToken cancellationToken = default);
}
