﻿using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Enums;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Base;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;


[RegisterDI(Interface = typeof(IPriceCalcOriginalService))]
public class PriceCalcOriginalService : LogicServiceBase<OlcPrctable>, IPriceCalcOriginalService
{
    private string PrcSql = @"select top 1 p.*
  from olc_prctable p
  join olc_itemmodelseason ms on ms.imid=p.imid
  join olc_item c on c.imsid=ms.imsid
  join ols_item i on i.itemid=c.itemid
  outer apply (
	select dbo.fn_olc_prctable_order(p.partnid, p.addrid, p.isid, p.icid, p.itemid, p.prctype) ordernum
  ) x 
 where GETDATE() between p.startdate and p.enddate
   and wid='{0}'
   and i.itemcode='{1}'
   and prctype=1
 order by ordernum";

    private IOlcPrctypeService olcPrctypeService;
    private readonly IOlsCurrencyCacheService olsCurrencyCacheService;
    private readonly IItemCache itemCache;

    public PriceCalcOriginalService(IValidator<OlcPrctable> validator,
                                    IRepository<OlcPrctable> repository,
                                    IUnitOfWork unitOfWork,
                                    IEnvironmentService environmentService,
                                    IOlcPrctypeService olcPrctypeService,
                                    IOlsCurrencyCacheService olsCurrencyCacheService, 
                                    IItemCache itemCache) : base(validator, repository, unitOfWork, environmentService)
    {
        this.olcPrctypeService = olcPrctypeService ?? throw new ArgumentNullException(nameof(olcPrctypeService));
        this.olsCurrencyCacheService = olsCurrencyCacheService ?? throw new ArgumentNullException(nameof(olsCurrencyCacheService));
        this.itemCache = itemCache ?? throw new ArgumentNullException(nameof(itemCache));
    }


    public async Task<CalcJsonResultDto> GetOriginalPrice(CalcJsonParamsDto? cart, CancellationToken cancellationToken = default)
    {
        if (cart == null)
        {
            throw new ArgumentNullException(nameof(cart));
        }

        if (cart.Curid == null)
        {
            throw new ArgumentNullException(nameof(cart.Curid));
        }
        if (string.IsNullOrEmpty(cart.CountryId))
        {
            throw new ArgumentNullException(nameof(cart.CountryId));
        }

        var o = new CalcJsonResultDto
        {
            Curid = cart.Curid,
            Round = await olsCurrencyCacheService.GetRound(cart.Curid),
            CountryId = cart.CountryId
        };

        var l = new List<CalcItemJsonResultDto>();

        foreach (var cartitem in cart.Items)
        {
            if (cartitem.Itemid.HasValue)
            {
                cartitem.ItemCode = (await this.itemCache.GetAsync(cartitem.Itemid, cancellationToken))!;
            }
            var origprc = await GetCartFullPrice(cart, cartitem, cancellationToken);
            if (origprc != null)
            {
                if (origprc.Curid != cart.Curid)
                {
                    throw new Exception($"Curid not equal {origprc.Curid} != {cart.Curid}");
                }
                for (int i = 0; i < cartitem.Quantity; i++)
                {
                    var ncijrd = new CalcItemJsonResultDto
                    {
                        ItemCode = cartitem.ItemCode,
                        Itemid = cartitem.Itemid,
                        Quantity = 1,
                        RawOrigSelPrc = origprc!.Prc,
                        RawSelPrc = origprc!.Prc,
                        RawSelVal = origprc!.Prc
                    };

                    if (i == 0)
                    {
                        ncijrd.CartId = cartitem.CartId;
                    }

                    l.Add(ncijrd);
                }
            }
        }
        o.Items = l.ToArray();
        o.Success = true;
        return o;
    }
     

    private async Task<PriceCalcOriginalResultDto?> GetCartFullPrice(CalcJsonParamsDto cart, CartItemJson cartitem, CancellationToken cancellationToken = default)
    {
        OlcPrctable? p = null;
        if (!string.IsNullOrEmpty(cartitem.ItemCode))
        {
            p = await Repository.FromSql(String.Format(PrcSql, cart.Wid, cartitem.ItemCode))!.FirstOrDefaultAsync(cancellationToken);
        } else
        {
            p = await Repository.FromSql(String.Format(PrcSql, cart.Wid, cartitem.Itemid))!.FirstOrDefaultAsync(cancellationToken);
        }

        if (p == null)
        {
            return null;
        }
        var pt = await olcPrctypeService.GetAsync(pp=> pp.Ptid == p.Ptid, cancellationToken);
         
        return new PriceCalcOriginalResultDto()
        {
            Prc = p.Prc,
            Prctype = (PrcType)p.Prctype,
            IsNet = pt!.Isnet == 1,
            Curid = p.Curid
        };
    }
}
