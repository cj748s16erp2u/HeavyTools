using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;

public interface IPartnVatTypCacheService
{
    Task<OlsPartnvattyp> GetAsync(string? ptvattypid, CancellationToken cancellationToken = default);
}
