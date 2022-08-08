using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces
{
    public interface IRecIdService
    {
        public Task<OlsRecid?> GetNewIdAsync(string riid, CancellationToken cancellationtoken);
        public Task<OlsRecid?> GetCurrentAsync(string riid, CancellationToken cancellationtoken);
    }
}
