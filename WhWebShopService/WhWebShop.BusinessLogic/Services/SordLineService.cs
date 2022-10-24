using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Other;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Base;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Context;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Base;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Interfaces;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;


[RegisterDI(Interface = typeof(ISordLineService))]
internal class SordLineService : LogicServiceBase<OlsSordline>, ISordLineService
{
    private readonly IReserveService reserveService;
    private readonly IOlsSordlineService olsSordlineService; 
    private readonly IOlcSordlineService olcSordlineService;
    private readonly IOlsSordheadService olsSordheadService;
    private readonly IOlcTmpSordsordService olcTmpSordsordService;
    private readonly IOlsTmpSordstService olsTmpSordstService;
    private readonly IOlcSordlineResService olcSordlineResService;

    public SordLineService(IValidator<OlsSordline> validator,
                           IRepository<OlsSordline> repository,
                           IUnitOfWork unitOfWork,
                           IEnvironmentService environmentService,
                           IReserveService reserveService,
                           IOlsSordlineService olsSordlineService,
                           IOlcSordlineService olcSordlineService,
                           IOlsSordheadService olsSordheadService,
                           IOlcTmpSordsordService olcTmpSordsordService,
                           IOlsTmpSordstService olsTmpSordstService,
                           IOlcSordlineResService olcSordlineResService) : base(validator, repository, unitOfWork, environmentService)
    {
        
        this.reserveService = reserveService ?? throw new ArgumentNullException(nameof(reserveService));
        this.olsSordlineService = olsSordlineService ?? throw new ArgumentNullException(nameof(olsSordlineService)); 
        this.olcSordlineService = olcSordlineService ?? throw new ArgumentNullException(nameof(olcSordlineService));
        this.olsSordheadService = olsSordheadService ?? throw new ArgumentNullException(nameof(olsSordheadService));
        this.olcTmpSordsordService = olcTmpSordsordService ?? throw new ArgumentNullException(nameof(olcTmpSordsordService));
        this.olsTmpSordstService = olsTmpSordstService ?? throw new ArgumentNullException(nameof(olsTmpSordstService));
        this.olcSordlineResService = olcSordlineResService ?? throw new ArgumentNullException(nameof(olcSordlineResService));
    }

    public async Task<SordLineDto> SordLineDeleteAsync(JObject value, CancellationToken cancellationToken = default)
    {
        var slp = JsonParser.ParseObject<SordLineParamDto>(value);

        try {
            var sl = await olsSordlineService.GetByIdAsync(slp.Sordlineid!, cancellationToken);

            if (sl == null)
            {
                throw new MessageException("$sordlinenotfound", "eLog.HeavyTools.Sales.Sord");
            }

            var preordcsl = await olcSordlineService.Query(p => p.Preordersordlineid == sl.Sordlineid).
                FirstOrDefaultAsync(cancellationToken);

            if (preordcsl != null)
            {
                throw new MessageException("$preordersordlineidfound", "eLog.HeavyTools.Sales.Sord");
            }

            await SordLinePreDelete(sl, cancellationToken);

            using (var tran = await this.UnitOfWork.BeginTransactionAsync(cancellationToken))
            {
                if (sl!.Resid.HasValue)
                {
                    var resid = sl.Resid.Value; 
                    await reserveService.ReserveDeleteAsync(resid, cancellationToken);
                }

                foreach (var slr in await olcSordlineResService.QueryAsync(p => p.Sordlineid == sl.Sordlineid, cancellationToken))
                {
                    await reserveService.ReserveDeleteAsync(slr.Resid, cancellationToken);
                }

                var csl = await olcSordlineService.GetByIdAsync(sl.Sordlineid, cancellationToken); 
                if (csl != null)
                {
                    await olcSordlineService.DeleteAsync(csl, cancellationToken);
                }

                var ttd = await olcTmpSordsordService.QueryAsync(p => p.Sordlineid == sl.Sordlineid, cancellationToken);
                foreach (var tt in ttd)
                {
                    await olcTmpSordsordService.DeleteAsync(tt, cancellationToken);
                }

                var tss = await olsTmpSordstService.QueryAsync(p => p.Sordlineid == sl.Sordlineid, cancellationToken);
                foreach (var tt in tss)
                {
                    await olsTmpSordstService.DeleteAsync(tt, cancellationToken);
                }

                await olsSordlineService.DeleteAsync(sl!, cancellationToken);
                tran.Commit();
            }

            return new SordLineDto()
            {
                Sordlineid = slp.Sordlineid
            };
        }
        catch (MessageException e)
        { 
            return new SordLineDto(e.GetDto());
        }
        catch (Exception e)
        { 
            return new SordLineDto(e.Message);
        }

    }

    private async Task SordLinePreDelete(OlsSordline sl, CancellationToken cancellationToken)
    {
        var sh = await olsSordheadService.GetByIdAsync(sl.Sordid, cancellationToken);
        if (sh == null)
        { 
            throw new MessageException("$sordheadnotfound", "eLog.HeavyTools.Sales.Sord");
        }
        if (sl.Resid.HasValue && sh.Sordtype == 2)
        {
            FormattableString sql = $@"
  select r.resid, lll.ordqty-isnull(anotherresqty,0) newresqty, ll.adddate, ll.addusrid
    from ols_sordline ll
    join olc_sordline cc on cc.sordlineid=ll.sordlineid
    join ols_sordline lll on lll.sordlineid=cc.preordersordlineid
    left join ols_reserve r on r.resid=lll.resid
    outer apply (
	select sum( isnull(dispqty,0)+isnull(resqty,0)) anotherresqty
	    from olc_sordline c
	    join ols_sordline l on l.sordlineid=c.sordlineid
		left join olc_sordline_res rr on rr.sordlineid=l.sordlineid
	    left join ols_reserve r on r.resid in (rr.resid, l.resid)
	    outer apply (
		select ordqty, dispqty
		    from ols_stline st
		    where st.sordlineid=l.sordlineid
	    ) x
	    where c.preordersordlineid=lll.sordlineid
	    and c.sordlineid<>ll.sordlineid
    ) x
  where ll.sordlineid={sl.Sordlineid}";

            var srrts = Repository.ExecuteSql<SordReserveRecalcTmp>(sql);

            int? resid = null;
            decimal? newresqty = null;


            foreach (var item in srrts)
            {
                if (item.Resid.HasValue && item.Newresqty.HasValue)
                {
                    resid = item.Resid;
                    newresqty = item.Newresqty;
                }
                break;
            }
            if (resid.HasValue && newresqty.HasValue)
            {
                await reserveService.ReserveSetQtyAsync(sl.Resid.Value, 0, cancellationToken);
                await reserveService.ReserveSetQtyAsync(resid.Value, newresqty.Value, cancellationToken);
            }
        }
    }
}
