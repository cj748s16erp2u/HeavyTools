using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;

[RegisterDI(Interface = typeof(IPriceCalcValueCalculatorService))]
public class PricseCalcValueCalculatorService : IPriceCalcValueCalculatorService
{
    private readonly IOSSService oSSService;
    private readonly IOlsTaxService olsTaxService;

    public PricseCalcValueCalculatorService(IOSSService oSSService, IOlsTaxService olsTaxService)
    {
        this.oSSService = oSSService ?? throw new ArgumentNullException(nameof(oSSService));
        this.olsTaxService = olsTaxService ?? throw new ArgumentNullException(nameof(olsTaxService));
    }

    public async Task CalculateCartAsync(CalcJsonResultDto res, CancellationToken cancellationtoken = default)
    {
        OSSResultDto? iss = await oSSService.GetOss(new OSSParamsDto() { CoundtyId = res.CountryId }, cancellationtoken);

        if (iss == null)
        {
            throw new ArgumentNullException(nameof(iss));
        }
        if (!iss.Success!.Value)
        {
            throw new ArgumentNullException(iss.ErrorMessage);
        }

        var tax = await olsTaxService.GetAsync(p => p.Taxid == iss.Taxid, cancellationtoken);
        if (tax == null)
        {
            throw new ArgumentNullException(nameof(tax));
        }
        foreach (var item in res.Items)
        {
            item.RawOrigSelPrc = Round(res, item.RawOrigSelPrc);
            item.RawSelPrc = Round(res, item.RawSelPrc);
            item.RawSelVal = Round(res, item.RawSelVal);


            if (res.IsNet)
            {
                // Nettó ár az ártáblában
                item.SelPrc = item.RawSelPrc;
                item.NetVal = item.RawSelVal;

                item.TotVal = Round(res, item.NetVal / 100 * (100 + tax!.Percnt));
                item.TaxVal = Round(res, item.TotVal - item.NetVal);

                item.GrossPrc = Round(res, item.SelPrc / 100 * (100 + tax!.Percnt));


                item.OrignalSelPrc = item.RawOrigSelPrc;
                item.OrignalGrossPrc = Round(res, item.RawOrigSelPrc / 100 * (100 + tax!.Percnt));
                item.OrignalTotVal = 1;
            }
            else
            {
                // Bruttó ár az ártáblában
                item.GrossPrc = Round(res, item.RawSelPrc);

                item.TotVal = item.RawSelVal;
                item.SelPrc = Round(res, (item.TotVal / (100 + tax!.Percnt) * 100) / item.Quantity);
                item.NetVal = item.SelPrc * item.Quantity;
                item.TaxVal = Round(res, item.TotVal - item.NetVal);


                item.OrignalSelPrc = Round(res, (item.RawOrigSelPrc / (100 + tax!.Percnt) * 100) );
                item.OrignalGrossPrc = item.RawOrigSelPrc;
                item.OrignalTotVal = Round(res, item.RawOrigSelPrc * item.Quantity);
            }
        }
    }
    private decimal Round(CalcJsonResultDto res, decimal? value)
    {
        return Math.Round(value!.Value, res.Round!.Value);
    }
}
