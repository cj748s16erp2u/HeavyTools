using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
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
   and i.itemcode='{2}'
   and prctype=1
 order by ordernum";

    public PriceCalcOriginalService(IValidator<OlcPrctable> validator, IRepository<OlcPrctable> repository, IUnitOfWork unitOfWork, IEnvironmentService environmentService) : base(validator, repository, unitOfWork, environmentService)
    {
    }

    public async Task<CalcJsonResultDto> GetOriginalPrice(CalcJsonParamsDto? cart, CancellationToken cancellationToken = default)
    {
        var o = new CalcJsonResultDto
        {
            Curid = "HUF"
        };
        var l = new List<CalcItemJsonResultDto>();

        foreach (var cartitem in cart!.Items)
        {
            l.Add(new CalcItemJsonResultDto
            {
                ItemCode = cartitem.ItemCode,
                Quantity = cartitem.Quantity,
                OrigSelVal = await GetCartFullPrice(cart, cartitem, cancellationToken)
            });
        }
        o.Items = l.ToArray();
        o.Success = true;
        return o;
    }
     

    private async Task<decimal?> GetCartFullPrice(CalcJsonParamsDto cart, CartItemJson cartitem, CancellationToken cancellationToken = default)
    {
        var p = await Repository.FromSql(String.Format(PrcSql, cart.Wid, cartitem.ItemCode))!.FirstOrDefaultAsync(cancellationToken);

        if (p == null)
        {
            throw new ArgumentException("NO price" + cartitem.ItemCode);
        }
        return p.Prc;
    }
}
