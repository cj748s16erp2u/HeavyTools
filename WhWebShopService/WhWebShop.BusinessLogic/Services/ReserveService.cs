using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Other;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Options;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Base;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Context;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Base;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Interfaces;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;

[RegisterDI(Interface = typeof(IReserveService))]
internal class ReserveService : LogicServiceBase<OlsReserve>, IReserveService
{
    private readonly IOlsReserveService olsReserveService;
    private readonly IOlsRecidService olsRecidService;
    private readonly IOlcSpOlsReserveReservestockService olcSpOlsReserveReservestockService;
    private readonly IOptions<ReserveOptions> reserveOption;
    private readonly IOlsStockService olsStockService;
    private readonly IOlcSordlineResService olcSordlineResService;
    private readonly IRepository<TmpPresorder> tmpPresorderService;

    public ReserveService(IValidator<OlsReserve> validator, 
        IRepository<OlsReserve> repository, 
        IUnitOfWork unitOfWork, 
        IEnvironmentService environmentService,
        IOlsReserveService olsReserveService,
        IOlsRecidService olsRecidService,
        IOlcSpOlsReserveReservestockService olcSpOlsReserveReservestockService,
        IOptions<Options.ReserveOptions> reserveOption,
        IOlsStockService olsStockService,
        IOlcSordlineResService olcSordlineResService,
        IRepository<TmpPresorder> tmpPresorderService) : base(validator, repository, unitOfWork, environmentService)
    {
        this.olsReserveService = olsReserveService ?? throw new ArgumentNullException(nameof(olsReserveService));
        this.olsRecidService = olsRecidService ?? throw new ArgumentNullException(nameof(olsRecidService));
        this.olcSpOlsReserveReservestockService = olcSpOlsReserveReservestockService ?? throw new ArgumentNullException(nameof(olcSpOlsReserveReservestockService));
        this.reserveOption = reserveOption ?? throw new ArgumentNullException(nameof(reserveOption));
        this.olsStockService = olsStockService ?? throw new ArgumentNullException(nameof(olsStockService));
        this.olcSordlineResService = olcSordlineResService ?? throw new ArgumentNullException(nameof(olcSordlineResService));
        this.tmpPresorderService = tmpPresorderService ?? throw new ArgumentNullException(nameof(tmpPresorderService)); 
    }

    public async Task<ReserveDto> ReserveAsync(JObject value, CancellationToken cancellationToken = default)
    {
        var reserve = JsonParser.ParseObject<ReserveParamsDto>(value);
        return await ReserveInternalAsync(reserve, cancellationToken);
    }

    public async Task<ReserveDto> ReserveInternalAsync(ReserveParamsDto reserve, CancellationToken cancellationToken = default)
    {
        decimal originalResQty = 0;
        bool isnew=false;
        int? resid = null;
        try
        {
            var r = reserve.ToOlsReserve(out isnew);
            
            if (isnew)
            {
                var id = await olsRecidService.GetNewIdAsync("Reserve.ResID", cancellationToken);
                r.Resid = id!.Lastid;

                await olsReserveService.AddAsync(r, cancellationToken);

                reserve.Resid = r.Resid;
                resid = reserve.Resid;
            }
            else
            {
                var er = await Repository.FindAsync(r.Resid);
                if (er == null)
                {
                    throw new MissingMemberException();
                }
                originalResQty = er.Resqty;
                resid = er.Resid;
                Fill(er, r);
                await olsReserveService.UpdateAsync(er!, cancellationToken);

            }
             
            #region PostSave 
            await ReserveStock(r, originalResQty, cancellationToken); 
            #endregion 
        }
        catch (MessageException e)
        {
            try
            {
                await RevertReserve(originalResQty, isnew, resid, cancellationToken);
            }
            catch (Exception e2)
            {
                return new ReserveDto("Revert error" + e2.Message + Environment.NewLine + 
                                        "InnerException: " + Environment.NewLine + e.Message);
            }
            return new ReserveDto(e.GetDto());
        }
        catch (Exception e)
        {
            try
            {
                await RevertReserve(originalResQty, isnew, resid, cancellationToken);
            }
            catch (Exception e2)
            {
                return new ReserveDto("Revert error" + e2.Message + Environment.NewLine + "InnerException: " + Environment.NewLine + e.Message);
            }
            return new ReserveDto(e.Message);
        }

        var res = new ReserveDto(reserve);

        return res;
    }

    private async Task RevertReserve(decimal originalResQty, bool isnew, int? resid, CancellationToken cancellationToken)
    {
        if (isnew)
        {
            if (resid.HasValue)
            {
                var er = await Repository.FindAsync(resid);
                if (er != null)
                {
                    await olsReserveService.DeleteAsync(er);
                }
            }
        }
        else
        {
            if (resid.HasValue)
            {
                var er = await Repository.FindAsync(resid);
                if (er != null)
                {
                    er.Resqty = originalResQty;
                    await olsReserveService.UpdateAsync(er!, cancellationToken);
                }
            }
        }
    }

    protected async Task ReserveStock(OlsReserve reserve, decimal valueOrDefault2, CancellationToken cancellationToken)
    {
        decimal deltaQty = reserve.Resqty - valueOrDefault2;
        await ReserveStock2(reserve, deltaQty, cancellationToken);
    }
    protected async Task ReserveStock2(OlsReserve reserve, decimal deltaQty, CancellationToken cancellationToken)
    {
        if (deltaQty == 0m)
        {
            return;
        }

        string msg = await ReserveStock3(reserve!.Resid, deltaQty, reserve.Addusrid, cancellationToken);
        if (!string.IsNullOrEmpty(msg))
        {
            throw new MessageException(msg);
        }

        if (reserve.Lotid.HasValue)
        {
            throw new NotImplementedException();
        }
    }

    public async Task<string> ReserveStock3(int resid, decimal qty, string usrid, CancellationToken cancellationToken)
    {
        if (qty == 0m)
        {
            return string.Empty;
        }
        var storeId = await olsRecidService.GetNewIdAsync("sp_ols_reserve_reservestock", cancellationToken);
     
        var sps = new List<SqlParameter>
        {
            new SqlParameter("storeId", storeId!.Lastid),
            new SqlParameter("resid", resid),
            new SqlParameter("qty", qty),
            new SqlParameter("userid", usrid)
        };
        
        await Repository.ExecuteStoredProcedure("sp_olc_sp_ols_reserve_reservestock", sps);


        var s = await olcSpOlsReserveReservestockService.Query(p => p.Id == storeId!.Lastid).FirstOrDefaultAsync(cancellationToken);
         
        if (s == null)
        {
            throw new Exception("Missing olcSpOlsReserveReservestock record");
        }
        if (s.Result.HasValue && s.Result.Value!=0 && s.Errkey!=null)
        {
            return s.Errkey;
        }
        return String.Empty; 
    } 
    private void Fill(OlsReserve n, OlsReserve r)
    {
        n.Cmpid = r.Cmpid;
        n.Partnid = r.Partnid;
        n.Addrid = r.Addrid;
        n.Whid = r.Whid;
        n.Itemid = r.Itemid;
        n.Lotid = r.Lotid;
        n.Restype = r.Restype;
        n.Resqty = r.Resqty;
        n.Resdate = r.Resdate;
        n.Freedate = r.Freedate;
        n.Note = r.Note;
        n.Addusrid = r.Addusrid;
        n.Adddate = r.Adddate;
    }

    
   

    private async Task DeleteSordReserveReference(int resid, CancellationToken cancellationToken)
    {
        string sql =
              $"update ols_sordline " +
              "set resid = null " +
              "where resid = " + resid;
        await ExecuteNonQuery(sql, cancellationToken);
    } 
    
    private async Task ExecuteNonQuery(string sql, CancellationToken cancellationToken)
    {
        await Repository.ExecuteSql($"{sql}", cancellationToken);
    }

    public async Task<ReserveDto> ReserveDeleteAsync(int resid, CancellationToken cancellationToken)
    {
        var r = await olsReserveService.Query(p => p.Resid == resid).FirstOrDefaultAsync(cancellationToken);

        var reserve = new ReserveParamsDto()
        {
            Resid = r.Resid,
            Cmpid = r.Cmpid!,
            Partnid = r.Partnid,
            Addrid = r.Addrid,
            Whid = r.Whid,
            Itemid = r.Itemid,
            Lotid = r.Lotid,
            ResType = r.Restype,
            ResQty = r.Resqty,
            ResDate = r.Resdate,
            FreeDate = r.Freedate,
            Note = r.Note!,
            Addusrid = r.Addusrid,
            Adddate = r.Adddate
        };

        return await ReserveDeleteInternalAsync(reserve, cancellationToken);
    }

    public async Task<ReserveDto> ReserveDeleteAsync(JObject value, CancellationToken cancellationToken = default)
    {
        var reserve = JsonParser.ParseObject<ReserveParamsDto>(value);

        return await ReserveDeleteInternalAsync(reserve, cancellationToken);
    }

    internal async Task<ReserveDto> ReserveDeleteInternalAsync(ReserveParamsDto reserve, CancellationToken cancellationToken = default) { 
        decimal originalResQty = 0;
        bool isnew = false;
        int? resid = null;

        try
        {
            var r = reserve.ToOlsReserve(out isnew);

            if (!isnew)
            {
                var er = await Repository.FindAsync(r.Resid);
                if (er == null)
                {
                    throw new MissingMemberException();
                }
                originalResQty = er.Resqty;
                resid = er.Resid;

                #region PreDelete
                await ReserveStock2(r, -r.Resqty, cancellationToken);
                await DeleteSordReserveReference(resid.Value, cancellationToken);
                #endregion

                foreach (var slr in await olcSordlineResService.QueryAsync(p => p.Resid == r.Resid, cancellationToken))
                {
                    await olcSordlineResService.DeleteAsync(slr, cancellationToken);
                }

                await olsReserveService.DeleteAsync(er!, cancellationToken);
            }

        }
        catch (MessageException e)
        {
            try
            {
                await RevertReserve(originalResQty, isnew, resid, cancellationToken);
            }
            catch (Exception e2)
            {
                return new ReserveDto("Revert error" + e2.Message + Environment.NewLine +
                                        "InnerException: " + Environment.NewLine + e.Message);
            }
            return new ReserveDto(e.GetDto());
        }
        catch (Exception e)
        {
            try
            {
                await RevertReserve(originalResQty, isnew, resid, cancellationToken);
            }
            catch (Exception e2)
            {
                return new ReserveDto("Revert error" + e2.Message + Environment.NewLine + "InnerException: " + Environment.NewLine + e.Message);
            }
            return new ReserveDto(e.Message);
        }

        var res = new ReserveDto(reserve);

        return res;
    }

    public async Task<ReserveDto> ReserveSetQtyAsync(int resid, decimal qty, CancellationToken cancellationToken)
    {
        var r = await olsReserveService.Query(p => p.Resid == resid).FirstOrDefaultAsync(cancellationToken);

        var reserve = new ReserveParamsDto()
        {
            Resid = r.Resid,
            Cmpid = r.Cmpid!,
            Partnid = r.Partnid,
            Addrid = r.Addrid,
            Whid = r.Whid,
            Itemid = r.Itemid,
            Lotid = r.Lotid,
            ResType = r.Restype,
            ResQty = r.Resqty,
            ResDate = r.Resdate,
            FreeDate = r.Freedate,
            Note = r.Note!,
            Addusrid = r.Addusrid,
            Adddate = r.Adddate
        };
        reserve.ResQty = qty;

        return await ReserveInternalAsync(reserve, cancellationToken);
    }
      

    public async Task<SordReserveResultDto> DoFrameOrderReserve(OlsSordhead sh, OlsSordline line, CancellationToken cancellationToken)
    {
        var ordqty = line.Ordqty;
        var whid = reserveOption.Value.CentralWarehouse;

        FormattableString sql = $@"
 select r.resid, l.sordlineid, r.addusrid, r.adddate
  from olc_sorddoc d
  join ols_sordhead h on h.sorddocid=d.frameordersorddocid
  join ols_sordline l on l.sordid=h.sordid
  join ols_reserve r on r.resid=l.resid
 where d.sorddocid={sh.Sorddocid}
   and l.itemid={line.Itemid}
   and resqty>0
   and r.whid={whid}";
 
        var rs = tmpPresorderService.ExecuteSql<TmpPresorder>(sql).ToList();

        foreach (var tmppresorder in rs)
        {
            var olsReserve = await olsReserveService.GetByIdAsync(tmppresorder.Resid, cancellationToken);

            var co = ordqty;
            if (olsReserve!.Resqty < co)
            {
                co = olsReserve.Resqty;
            }
            try
            {
                using (var tran = await this.UnitOfWork.BeginTransactionAsync(cancellationToken))
                {
                    await ReserveStock2(olsReserve, -co, cancellationToken); 
                    olsReserve.Resqty -= co;

                    await olsReserveService.UpdateAsync(olsReserve, cancellationToken); 
                    await CreateReserve(sh, line, whid, co, tmppresorder.Sordlineid, cancellationToken);
                    ordqty -= co;
                     
                    tran.Commit();
                }
                ordqty -= co;
            }
            catch (Exception ex)
            {
                await ERP4U.Log.LoggerManager.Instance.LogErrorAsync<Exception>(ex);
            }
           
            if (ordqty == 0)
            {
                break;
            }
        }

        return new SordReserveResultDto()
        {
            RemnantQty = ordqty
        };
    }

    public async Task<SordReserveResultDto> DoCentralWarehouseReserve(OlsSordhead sh, OlsSordline line, decimal remnantQty, CancellationToken cancellationToken)
    {
        var whid = reserveOption.Value.CentralWarehouse;
        var ordqty = remnantQty;

        var stock = await olsStockService.Query(p => p.Whid == whid && p.Itemid == line.Itemid).FirstOrDefaultAsync(cancellationToken);

        if (stock != null)
        {
            var co = ordqty;
            var max = stock.Actqty - stock.Resqty;
            if (co > max)
            {
                co = max;
            }
            if (co > 0)
            {
                try
                {
                    using (var tran = await this.UnitOfWork.BeginTransactionAsync(cancellationToken))
                    {
                        await CreateReserve(sh, line, whid, co,null, cancellationToken);
                        ordqty -= co;

                        tran.Commit();
                    }
                }
                catch (Exception ex)
                {
                    await ERP4U.Log.LoggerManager.Instance.LogErrorAsync<Exception>(ex);
                }
            }
        }

        return new SordReserveResultDto()
        {
            RemnantQty = ordqty
        };
    }

    private async Task CreateReserve(OlsSordhead sh, OlsSordline line, string? whid, decimal co, int? preordersordlineid, CancellationToken cancellationToken)
    {

        var nr = new OlsReserve
        {
            Resid = (await olsRecidService.GetNewIdAsync("Reserve.ResID", cancellationToken))!.Lastid,
            Cmpid = sh.Cmpid,
            Partnid = sh.Partnid,
            Addrid = sh.Addrid,
            Whid = whid!,
            Itemid = line.Itemid,
            Restype = 3,
            Resqty = co,
            Resdate = DateTime.Today,
            Addusrid = sh.Addusrid,
            Adddate = DateTime.Now
        };
         
        await olsReserveService.AddAsync(nr, cancellationToken);
        await ReserveStock2(nr, nr.Resqty, cancellationToken);
        await CreateOlcSordlineRes(line, nr.Resid, preordersordlineid, cancellationToken);
          
    }

    private async Task CreateOlcSordlineRes(OlsSordline line, int resid, int? preordersordlineid, CancellationToken cancellationToken)
    {
        var slr = new OlcSordlineRes();

        slr.Sordlineid = line.Sordlineid;
        slr.Resid = resid;
        slr.Preordersordlineid = preordersordlineid;
        slr.Addusrid = line.Addusrid;
        slr.Adddate = DateTime.Now;

        await olcSordlineResService.AddAsync(slr, cancellationToken);

    }
}
