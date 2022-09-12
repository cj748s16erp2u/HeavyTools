using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces
{
    public interface IItemCache
    {
        Task<int?> GetAsync(string? itemcode, CancellationToken cancellationToken = default);
        Task<string?> GetAsync(int? itemid, CancellationToken cancellationToken = default);
    }
}
