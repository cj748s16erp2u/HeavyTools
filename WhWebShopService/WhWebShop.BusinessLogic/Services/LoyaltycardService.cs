using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Other;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;


[RegisterDI(Interface = typeof(ILoyaltycardService))]
public class LoyaltycardService : ILoyaltycardService
{ 
    public static ConcurrentDictionary<string, LoyaltycardDataItem> cache =
        new ConcurrentDictionary<string, LoyaltycardDataItem>();
  
    private readonly IOlcPartnerService olcPartnerService;
    private readonly IOlsSinvheadService olsSinvheadService;

    public LoyaltycardService(IOlcPartnerService olcPartnerService, IOlsSinvheadService olsSinvheadService)
    {
        this.olcPartnerService = olcPartnerService ?? throw new ArgumentNullException(nameof(olcPartnerService));
        this.olsSinvheadService = olsSinvheadService ?? throw new ArgumentNullException(nameof(olsSinvheadService));
    }

    public async Task<decimal> GetTotalPurchaseAmount(string loyaltyCardNo, string curid, CancellationToken cancellationToken)
    {
        if (cache.TryGetValue(loyaltyCardNo, out var lsdi))
        {
            if (lsdi.ValidTo< DateTime.Now)
            {
                return lsdi.TotalPurchaseAmount;
            }
            cache.TryRemove(loyaltyCardNo, out var lsdi2);
        }

        var cps = await olcPartnerService.QueryAsync(p => p.Loyaltycardno == loyaltyCardNo, cancellationToken);
        if (cps != null)
        {
            var cp = cps.FirstOrDefault();
            if (cp != null)
            {
                if (cp.Loyaltyturnover.HasValue)
                {
                    var tpa = cp.Loyaltyturnover.Value;
                    if (tpa < 0)
                    {
                        tpa = 0;
                    }
                    var shs = await olsSinvheadService.
                        QueryAsync(p => p.Partnid == cp.Partnid && p.Curid == curid, cancellationToken);

                    foreach (var sh in shs)
                    {
                        tpa += sh.Totval;
                    }

                    var ldi = new LoyaltycardDataItem
                    {
                        TotalPurchaseAmount = tpa
                    };
                    cache.TryAdd(loyaltyCardNo, ldi);

                    return tpa;
                }
            }
        }
        return 0;
    }
}
