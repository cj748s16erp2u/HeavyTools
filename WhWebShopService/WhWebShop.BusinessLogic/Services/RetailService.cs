using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Options;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;



[RegisterDI(Interface = typeof(IRetailService))]
public class RetailService : IRetailService
{
    private readonly IOptions<RetailOptions> retailOption;
    private readonly IRepository<RetailOrderTmp> repository;
    private readonly IOlsRecidService olsRecidService;
    private readonly IOlsSorddocCacheService olsSorddocCacheService;
    private readonly IUnitOfWork unitOfWork;
    private readonly IOlsSordheadService olsSordheadService;
    private readonly IOlcSordheadService olcSordheadService;
    private readonly IOrderService orderService;

    public RetailService(
        IOptions<Options.RetailOptions> retailOption,
        IRepository<RetailOrderTmp> repository,
        IOlsRecidService olsRecidService,
        IOlsSorddocCacheService olsSorddocCacheService,
        IUnitOfWork unitOfWork,
        IOlsSordheadService olsSordheadService,
        IOlcSordheadService olcSordheadService,
        IOrderService orderService)
    {
        this.retailOption = retailOption ?? throw new ArgumentNullException(nameof(retailOption));
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        this.olsRecidService = olsRecidService ?? throw new ArgumentNullException(nameof(olsRecidService));
        this.olsSorddocCacheService = olsSorddocCacheService ?? throw new ArgumentNullException(nameof(olsSorddocCacheService));
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.olsSordheadService = olsSordheadService ?? throw new ArgumentNullException(nameof(olsSordheadService));
        this.olcSordheadService = olcSordheadService ?? throw new ArgumentNullException(nameof(olcSordheadService));
        this.orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
    }

    async Task<RetailOrderResultDto> IRetailService.CreateOrderAsync(JObject value, CancellationToken cancellationToken)
    {
        var retailOrderParam = JsonParser.ParseObject<RetailOrderParamDto>(value);

        FormattableString sql = $@"
select ROW_NUMBER() OVER (ORDER BY CAST(GETDATE() AS TIMESTAMP)) id, wh.cmpid, wh.whid, wh.partnid, wh.addrid, i.itemid, i.minqty-isnull(s.actqty,0)-isnull(ordqty,0) ordqty, adddate, addusrid
  from (
	select distinct whid, w.partnid, w.addrid, c.cmpid
	  from ols_warehouse w
	  join fin_pcash c on c.addrid=w.addrid
  ) wh
  join ols_itemwh i on i.whid=wh.whid
  left join ols_stock s on s.whid=wh.whid and s.itemid=i.itemid
 outer apply (
	select sum(l.ordqty-isnull(l.movqty,0)) ordqty
	  from ols_sordhead h
	  join ols_sordline l on l.sordid=h.sordid
	 where h.sorddocid={retailOption.Value.OrderSordDocId} and l.sordlinestat<100 and h.sordstat<100 and l.itemid=i.itemid
  ) sord
  where i.minqty>0
    and i.minqty-isnull(s.actqty,0)-isnull(ordqty,0)>0
  order by wh.whid
";

        var sordheadcount = 0;

        using (var tran = await unitOfWork.BeginTransactionAsync(cancellationToken))
        {
            var srrts = await  repository.ExecuteSql<RetailOrderTmp>(sql).ToListAsync(cancellationToken);

            OlsSordhead sordhead = null!; 
            OlcSordhead olcSordhead = null!;

            var sordlinecount = 0;

            foreach (var item in srrts)
            {
                if (sordhead == null)
                {
                    sordhead = await CreateSordHead(item, retailOrderParam, cancellationToken);
                    olcSordhead = await CreateOlcSordHead(sordhead, retailOrderParam, cancellationToken);
                    sordheadcount++;
                } else
                {
                    if (sordhead.Whid != item.Whid)
                    {
                        if (sordhead != null)
                        {
                            sordhead.Lastlinenum = sordlinecount;
                        }
                        sordhead = await CreateSordHead(item, retailOrderParam, cancellationToken);
                        olcSordhead = await CreateOlcSordHead(sordhead, retailOrderParam, cancellationToken);
                        sordlinecount = 0;
                        sordheadcount++;
                    }

                }
                sordlinecount++;

                var sl =  await CreateSordHead(item, retailOrderParam, cancellationToken);





            }

            //tran.Commit();
        }
        return new RetailOrderResultDto() { RetaulCount = sordheadcount };
    }

    private async Task<OlcSordhead> CreateOlcSordHead(OlsSordhead sh, RetailOrderParamDto retailOrderParam, CancellationToken cancellationToken)
    {
        var csh = new OlcSordhead
        {
            Sordid = sh.Sordid!,
            Addusrid = sh.Addusrid,
            Adddate = sh.Adddate
        };
        await olcSordheadService.AddAsync(csh, cancellationToken);
        return csh;
    }

    private async Task<OlsSordhead> CreateSordHead(RetailOrderTmp item, RetailOrderParamDto retailOrderParam, CancellationToken cancellationToken)
    { 
        var sordid = await olsRecidService.GetNewIdAsync("SordHead.SordID", cancellationToken);
        if (sordid == null)
        {
            throw new Exception("Cannot generate new id");
        }

        var sh = new OlsSordhead
        {
            Sordid = sordid.Lastid,
            Partnid = 1,

            Sorddate = DateTime.Today,
            Curid = retailOption.Value.Curid!,
            Paymid = retailOption.Value.Paymid!,
            Paycid = null,
            Sordstat = 10,
            Note = "",
            Langid = "hu-HU",
            Lastlinenum = 0,
        
            Gen = this.retailOption.Value.GenId,
            Sorddocid = this.retailOption.Value.OrderSordDocId!,
            Cmpid = item.Cmpid!.Value
        };
        sh.Docnum = await orderService.GetNewSordnum(sh, cancellationToken);


        var sd = await olsSorddocCacheService.GetSorddocAsync(sh.Sorddocid, cancellationToken);

        sh.Sordtype = sd.Type;
        sh.Partnid = item.Partnid!.Value;
        sh.Addrid = item.Addrid!.Value;
        sh.Addusrid = retailOrderParam.Addusrid!;
        sh.Adddate = DateTime.Now;

        await olsSordheadService.AddAsync(sh, cancellationToken);

        return sh;
    }
}