using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;


[RegisterDI(Interface = typeof(IPriceCalcCuponUsageService))]
public class PriceCalcCuponUsageService : IPriceCalcCuponUsageService
{
    private readonly IOlcActionCacheService olcActionCacheService; 

    public PriceCalcCuponUsageService(IOlcActionCacheService olcActionCacheService)
    {
        this.olcActionCacheService = olcActionCacheService ?? throw new ArgumentNullException(nameof(olcActionCacheService));
    }
     

    public async Task UseCuponAsync(string[] cuponNumbers, CancellationToken cancellationToken = default)
    {
        if (cuponNumbers != null)
        {
            foreach (var cupon in cuponNumbers)
            {
                var ca = await olcActionCacheService.GetCuponByIdAsync(cupon, cancellationToken);

             
                if (ca != null)
                {
                    if (ca.Couponunlimiteduse == 0)
                    {
                        ca.Delstat = 1;
                        ca.Isactive = 0;

                        await olcActionCacheService.UpdateAsync(ca, cancellationToken);
                        continue;
                    }
                }

                var an=await olcActionCacheService.GetCoupoNumberByIdAsync(cupon, cancellationToken);
                 
                if (an != null)
                {
                    an.Used = 1;
                    await olcActionCacheService.UpdateAsync(an, cancellationToken);
                    continue;
                } 
            }
        }
    }
}
