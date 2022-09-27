using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;

public interface ILoyaltycardService
{
    Task<decimal> GetTotalPurchaseAmount(string loyaltyCardNo, string curid, CancellationToken cancellationToken);
}
