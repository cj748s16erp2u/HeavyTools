using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Enums;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Enums;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Base;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Validators.Interfaces;
using eLog.HeavyTools.Services.WhZone.DataAccess.Repositories.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Services;

[RegisterDI(Interface = typeof(IWhZTranLineService))]
public class WhZTranLineService : LogicServiceBase<OlcWhztranline>, IWhZTranLineService
{
#pragma warning disable CA1822 // Mark members as static
    private readonly IRepository<OlcWhztranhead> tranHeadRepository;
    private readonly IRepository<OlsStline> stLineRepository;
    private readonly IRepository<OlsItem> itemRepository;
    private readonly IRepository<OlsUnit> unitRepository;
    private readonly IMapper mapper;

    public WhZTranLineService(
        IOlcWhztranlineValidator validator,
        IRepository<OlcWhztranline> repository,
        IUnitOfWork unitOfWork,
        IEnvironmentService environmentService,
        IRepository<OlcWhztranhead> tranHeadRepository,
        IRepository<OlsStline> stLineRepository,
        IRepository<OlsItem> itemRepository,
        IRepository<OlsUnit> unitRepository,
        IMapper mapper) : base(validator, repository, unitOfWork, environmentService)
    {
        this.tranHeadRepository = tranHeadRepository ?? throw new ArgumentNullException(nameof(tranHeadRepository));
        this.stLineRepository = stLineRepository ?? throw new ArgumentNullException(nameof(stLineRepository));
        this.itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        this.unitRepository = unitRepository ?? throw new ArgumentNullException(nameof(unitRepository));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Bevételezés típusú tranzakció tételek lekérdezése
    /// </summary>
    /// <param name="predicate">Szűrési feltétel</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Tranzakció tételek listája</returns>
    public async Task<IEnumerable<WhZReceivingTranLineDto>> QueryReceivingAsync(WhZTranLineQueryDto query, CancellationToken cancellationToken = default)
    {
        Expression<Func<OlcWhztranline, bool>> predicate = null!;
        if (query is not null)
        {
            predicate = this.CreatePredicate(query);
        }

        var q = this.Query(predicate);
        q = q.Where(l => l.Whzt.Whzttype == (int)WhZTranHead_Whzttype.Receiving);

        var list = await q.ToListAsync(cancellationToken);
        return this.mapper.Map<IEnumerable<WhZReceivingTranLineDto>>(list);
    }

    /// <summary>
    /// Bevételezés típusú tranzakció tétel rögzítése
    /// </summary>
    /// <param name="request">Tranzakció tétel adatok</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Rögzített tranzakció tétel</returns>
    public async Task<WhZReceivingTranLineDto> AddReceivingAsync(WhZReceivingTranLineDto request, CancellationToken cancellationToken = default)
    {
        this.ValidateAddReceivingParameters(request);

        var tranHead = request.Whztid != null && request.Whztid != 0
            ? await this.tranHeadRepository.FindAsync(new object[] { request.Whztid }, cancellationToken)
            : null;
        var entity = await this.MapAddDtoToEntityAsync(request, tranHead, cancellationToken);

        this.EnvironmentService.CustomData.TryAdd("AuthUser", request.AuthUser);
        using var tran = await this.UnitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            entity = await this.AddAsync(entity, cancellationToken);

            tran.Commit();

            return this.mapper.Map<WhZReceivingTranLineDto>(entity);
        }
        catch (Exception ex)
        {
            await ERP2U.Log.LoggerManager.Instance.LogErrorAsync<WhZTranLineService>(ex);
            throw;
        }
        finally
        {
            if (tran.HasTransaction())
            {
                tran.Rollback();
            }

            this.EnvironmentService.CustomData.TryRemove("AuthUser", out _);
        }
    }

    /// <summary>
    /// Bevételezés típusú tranzakció tétel módosítása
    /// </summary>
    /// <param name="request">Tranzakció tétel adatok</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Módosított tranzakció tétel</returns>
    public async Task<WhZReceivingTranLineDto> UpdateReceivingAsync(WhZReceivingTranLineDto request, CancellationToken cancellationToken = default)
    {
        this.ValidateUpdateReceivingParameters(request);

        var originalEntity = await this.LoadEntityAsync(request, cancellationToken);
        if (originalEntity is null)
        {
            ThrowException(WhZTranLineExceptionType.EntryNotFound, $"The referenced transaction line is not found (whztlineid: {request.Whztlineid}, stlineid: {request.Stlineid})");
        }

        var tranHead = await this.tranHeadRepository.FindAsync(new object[] { originalEntity!.Whztid }, cancellationToken);

        var entity = await this.MapUpdateDtoToEntityAsync(request, originalEntity, tranHead, cancellationToken);

        this.EnvironmentService.CustomData.TryAdd("AuthUser", request.AuthUser);
        using var tran = await this.UnitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            entity = await this.UpdateAsync(entity, cancellationToken);

            tran.Commit();

            return this.mapper.Map<WhZReceivingTranLineDto>(entity);
        }
        catch (Exception ex)
        {
            await ERP2U.Log.LoggerManager.Instance.LogErrorAsync<WhZTranLineService>(ex);
            throw;
        }
        finally
        {
            if (tran.HasTransaction())
            {
                tran.Rollback();
            }

            this.EnvironmentService.CustomData.TryRemove("AuthUser", out _);
        }
    }

    /// <summary>
    /// Bejegyzés betöltése azonosító vagy raktári tranzakció tétel azonosító alapján
    /// </summary>
    /// <param name="request">Tranzakció tétel adatok</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Betöltött bejegyzés, ha nincs találat, akkor null</returns>
    private async Task<OlcWhztranline?> LoadEntityAsync(WhZReceivingTranLineDto request, CancellationToken cancellationToken)
    {
        return request.Whztlineid is not null && request.Whztlineid != 0
            ? await this.GetByIdAsync(new object[] { request.Whztlineid }, cancellationToken)
            : request.Stlineid is not null && request.Stlineid != 0
                ? await this.GetAsync(e => e.Stlineid == request.Stlineid, cancellationToken)
                : null;
    }

    /// <summary>
    /// <see cref="WhZReceivingTranLineDto"/> request-ből <see cref="OlcWhztranline"/> entity létrehozása
    /// </summary>
    /// <param name="request">A rögzítendő tranzakció tétel adatok</param>
    /// <returns>Az rögzítendő entity</returns>
    private async Task<OlcWhztranline> MapAddDtoToEntityAsync(WhZReceivingTranLineDto request, OlcWhztranhead? tranHead, CancellationToken cancellationToken = default)
    {
        var entity = this.mapper.Map<OlcWhztranline>(request);

        if (entity.Stlineid is not null)
        {
            var stLine = await this.stLineRepository.FindAsync(new object[] { entity.Stlineid }, cancellationToken);
            if (stLine is not null)
            {
                this.CopyDataFromStLine(request, stLine, entity);
            }
            else if ((tranHead?.Whztstat).GetValueOrDefault((int)WhZTranHead_Whztstat.Closed) < (int)WhZTranHead_Whztstat.Closed)
            {
                this.DetermineData(request, entity);
            }
        }
        else if ((tranHead?.Whztstat).GetValueOrDefault((int)WhZTranHead_Whztstat.Closed) < (int)WhZTranHead_Whztstat.Closed)
        {
            this.DetermineData(request, entity);
        }

        entity.Gen = (int)OlcWhZTranLine_Gen.Normal;

        return entity;
    }

    /// <summary>
    /// <see cref="WhZReceivingTranLineDto"/> request-ből <see cref="OlcWhztranline"/> entity létrehozása
    /// </summary>
    /// <param name="request">A módosítandó tranzakció tétel adatok</param>
    /// <returns>Az módosítandó entity</returns>
    private async Task<OlcWhztranline> MapUpdateDtoToEntityAsync(WhZReceivingTranLineDto request, OlcWhztranline originalEntity, OlcWhztranhead? tranHead, CancellationToken cancellationToken)
    {
        var entity = this.mapper.Map<OlcWhztranline>(request);

        if (entity.Stlineid is not null)
        {
            var stLine = await this.stLineRepository.FindAsync(new object[] { entity.Stlineid }, cancellationToken);
            if (stLine is not null)
            {
                this.CopyDataFromStLine(request, stLine, entity);
            }
            else if ((tranHead?.Whztstat).GetValueOrDefault((int)WhZTranHead_Whztstat.Closed) < (int)WhZTranHead_Whztstat.Closed)
            {
                this.DetermineData(request, entity);
            }
        }
        else if ((tranHead?.Whztstat).GetValueOrDefault((int)WhZTranHead_Whztstat.Closed) < (int)WhZTranHead_Whztstat.Closed)
        {
            this.DetermineData(request, entity);
        }

        entity.Whztid = originalEntity.Whztid;
        if (request.Whztlineid is null || request.Whztlineid == 0)
        {
            entity.Whztlineid = originalEntity.Whztlineid;
        }

        entity.Gen = originalEntity.Gen;
        entity.Addusrid = originalEntity.Addusrid;
        entity.Adddate = originalEntity.Adddate;

        return entity;
    }

    /// <summary>
    /// Készlet tétel adatok másolása
    /// </summary>
    /// <param name="request">Tranzakció tétel adatok</param>
    /// <param name="entity">Létrehozandó tétel</param>
    /// <param name="stLine">Készlet tétel bejegyzés</param>
    private void CopyDataFromStLine(WhZReceivingTranLineDto request, OlsStline stLine, OlcWhztranline entity)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        if (stLine is null)
        {
            throw new ArgumentNullException(nameof(stLine));
        }

        entity.Linenum = request.Linenum ?? stLine.Linenum;
        entity.Itemid = request.Itemid ?? stLine.Itemid;
        // ezek az adatok már meghatározásra kerültek, így átvesszük őket
        entity.Ordqty = stLine.Ordqty;
        entity.Dispqty = stLine.Dispqty;
        entity.Movqty = stLine.Movqty;
        entity.Inqty = stLine.Inqty;
        entity.Outqty = stLine.Outqty;
        entity.Ordqty2 = request.Ordqty2 ?? stLine.Ordqty2;
        entity.Unitid2 = request.Unitid2 ?? stLine.Unitid2;
        entity.Change = request.Change ?? stLine.Change;
        entity.Dispqty2 = request.Dispqty2 ?? stLine.Dispqty2;
        entity.Movqty2 = request.Movqty2 ?? stLine.Movqty2;
    }

    /// <summary>
    /// Készlet tétel adatok másolása
    /// </summary>
    /// <param name="request">Tranzakció tétel adatok</param>
    /// <param name="entity">Létrehozandó tétel</param>
    /// <param name="stLine">Készlet tétel bejegyzés</param>
#pragma warning disable IDE0060 // Remove unused parameter
    private void DetermineData(WhZReceivingTranLineDto request, OlcWhztranline entity)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        entity.Dispqty2 = 0M;

        entity.Ordqty = this.CalcOrdQty(entity);
        entity.Dispqty = this.CalcDispQty(entity);
        entity.Movqty = this.CalcMovQty(entity);
    }

    /// <summary>
    /// Rendelt mennyiség számolása a cikk mértékegységére
    /// </summary>
    /// <param name="entity">Tétel adatok</param>
    /// <returns>Számolt mennyiség</returns>
    private decimal CalcOrdQty(OlcWhztranline entity)
    {
        return this.CalcQty(entity.Ordqty2, entity.Change);
    }

    /// <summary>
    /// Diszponált mennyiség számolása a cikk mértékegységére
    /// </summary>
    /// <param name="entity">Tétel adatok</param>
    /// <returns>Számolt mennyiség</returns>
    private decimal CalcDispQty(OlcWhztranline entity)
    {
        return this.CalcQty(entity.Dispqty2, entity.Change);
    }

    /// <summary>
    /// Valós mennyiség számolása a cikk mértékegységére
    /// </summary>
    /// <param name="entity">Tétel adatok</param>
    /// <returns>Számolt mennyiség</returns>
    private decimal CalcMovQty(OlcWhztranline entity)
    {
        return this.CalcQty(entity.Movqty2, entity.Change);
    }

    /// <summary>
    /// Érték számolása váltószámmal
    /// </summary>
    /// <param name="qty">Forrás érték</param>
    /// <param name="change">Váltószám</param>
    /// <returns>Számolt érték</returns>
    private decimal CalcQty(decimal qty, decimal change)
    {
        return qty * change;
    }

    /// <summary>
    /// Bevételezés típusú tranzakció tétel adatok validálása
    /// </summary>
    /// <param name="request">Tranzakció tétel adatok</param>
    private void ValidateAddReceivingParameters(WhZReceivingTranLineDto request)
    {
        this.ValidateReceivingBaseParameters(request);
    }

    /// <summary>
    /// Bevételezés típusú tranzakció tétel adatok validálása
    /// </summary>
    /// <param name="request">Tranzakció tétel adatok</param>
    private void ValidateUpdateReceivingParameters(WhZReceivingTranLineDto request)
    {
        this.ValidateReceivingBaseParameters(request);
    }

    /// <summary>
    /// Alap bevételezés tétel request validálások
    /// </summary>
    /// <param name="request">Tranzakció tétel adatok</param>
    private void ValidateReceivingBaseParameters(WhZReceivingTranLineDto request)
    {
        this.ValidateBaseParameters(request);

        if (request.Stlineid is null)
        {
            ThrowException(WhZTranLineExceptionType.InvalidStlineid, "The source stock tran line identifier is not set", fieldName: nameof(WhZReceivingTranLineDto.Stlineid));
        }
    }

    /// <summary>
    /// Alap request validalasok
    /// </summary>
    /// <param name="request">Tranzakció tétel adatok</param>
    /// <exception cref="ArgumentNullException"><paramref name="request"/> értéke nincs megadva</exception>
    private void ValidateBaseParameters(WhZReceivingTranLineDto request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }
    }

    /// <summary>
    /// Új felvitel esetén, a <see cref="BusinessEntities.Model.Base.BusinessEntity.Addusrid"/> meghatározása a request-ben kapott <see cref="WhZTranLineDto.AuthUser"/> alapján
    /// </summary>
    /// <param name="entity">Mentendő bejegyzés</param>
    protected override async Task FillSystemFieldsOnAddAsync(OlcWhztranline entity, CancellationToken cancellationToken = default)
    {
        await base.FillSystemFieldsOnAddAsync(entity, cancellationToken);

        //if (this.EnvironmentService.CustomData.TryGetValue("AuthUser", out var s) && s is string authUser)
        //{
        //    entity.Addusrid = authUser;
        //}
    }

    private static void ThrowException(WhZTranLineExceptionType type, string message, OlcWhztranline? entity = null, string? fieldName = null)
    {
        throw new WhZTranLineServiceException(type, message, entity, fieldName);
    }

    /// <summary>
    /// Validálásnál használt adattároló feltöltése
    /// új adat: zóna tranzakció fej, cikk, raktári tranzakció tétel
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="originalEntity"></param>
    /// <param name="ruleSets"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected override async ValueTask<ValidationContext<OlcWhztranline>> CreateValidationContextAsync(OlcWhztranline entity, OlcWhztranline? originalEntity, string[] ruleSets, CancellationToken cancellationToken = default)
    {
        var context = await base.CreateValidationContextAsync(entity, originalEntity, ruleSets, cancellationToken);

        var tranHead = await this.tranHeadRepository.Entities
            .Where(h => h.Whztid == entity.Whztid)
            .Include(h => h.Cmp)
            .FirstOrDefaultAsync(cancellationToken);
        if (tranHead is not null)
        {
            if (!context.TryAddEntity(tranHead))
            {
                this.ThrowException("Unable to add transaction to the validation context", entity);
            }

            if (tranHead.Cmp is not null && !context.TryAddEntity(tranHead.Cmp))
            {
                this.ThrowException("Unable to add company to the validation context", entity);
            }
        }

        var item = await this.itemRepository.FindAsync(new object[] { entity.Itemid }, cancellationToken);
        if (item is not null && !context.TryAddEntity(item))
        {
            this.ThrowException("Unable to add item to the validation context", entity);
        }

        var unit = await this.unitRepository.FindAsync(new object[] { entity.Unitid2 }, cancellationToken);
        if (unit is not null && !context.TryAddEntity(unit))
        {
            this.ThrowException("Unable to add unit to the validation context", entity);
        }

        if (entity.Stlineid is not null)
        {
            var stLine = await this.stLineRepository.FindAsync(new object[] { entity.Stlineid }, cancellationToken);
            if (stLine is not null && !context.TryAddEntity(stLine))
            {
                this.ThrowException("Unable to add stock transaction line to the validation context", entity);
            }
        }

        return context;
    }
#pragma warning restore CA1822 // Mark members as static
}
