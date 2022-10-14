using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Other;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Base;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Base;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Interfaces;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;

[RegisterDI(Interface = typeof(IReserveService))]
internal class ReserveService : LogicServiceBase<OlsReserve>, IReserveService
{
    private readonly IOlsReserveService olsReserveService;
    private readonly IOlsRecidService olsRecidService;
    private readonly IOlcSpOlsReserveReservestockService olcSpOlsReserveReservestockService;

    public ReserveService(IValidator<OlsReserve> validator, 
        IRepository<OlsReserve> repository, 
        IUnitOfWork unitOfWork, 
        IEnvironmentService environmentService,
        IOlsReserveService olsReserveService,
        IOlsRecidService olsRecidService,
        IOlcSpOlsReserveReservestockService olcSpOlsReserveReservestockService) : base(validator, repository, unitOfWork, environmentService)
    {
        this.olsReserveService = olsReserveService ?? throw new ArgumentNullException(nameof(olsReserveService));
        this.olsRecidService = olsRecidService ?? throw new ArgumentNullException(nameof(olsRecidService));
        this.olcSpOlsReserveReservestockService = olcSpOlsReserveReservestockService ?? throw new ArgumentNullException(nameof(olcSpOlsReserveReservestockService));
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
}
