using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;

public interface IOlcCartCacheService
{
    internal bool TryGet(string hash, out CalcJsonResultDto cjrd);

    internal void Add(string hash, CalcJsonResultDto res);

    internal void RemoveCartByAction(int aid);
    void Reset();
    string GenerateHash(CalcJsonParamsDto cart);
}
