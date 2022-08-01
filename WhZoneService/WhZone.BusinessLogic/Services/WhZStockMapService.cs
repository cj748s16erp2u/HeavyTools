using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Containers.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Containers;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Base;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhZone.DataAccess.Repositories.Interfaces;
using FluentValidation;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Validators.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Interfaces;
using System.Linq.Expressions;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Enums;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Helpers;
using System.Runtime.CompilerServices;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Services;

[RegisterDI(Interface = typeof(IWhZStockMapService))]
public class WhZStockMapService : LogicServiceBase<OlcWhzstockmap>, IWhZStockMapService
{
    private readonly IWhZStockService stockService;

    public WhZStockMapService(
        IOlcWhzstockmapValidator validator,
        IRepository<OlcWhzstockmap> repository,
        IWhZStockService whZStockService,
        IUnitOfWork unitOfWork,
        IEnvironmentService environmentService) : base(validator, repository, unitOfWork, environmentService)
    {
        this.stockService = whZStockService ?? throw new ArgumentNullException(nameof(whZStockService));
    }

    /// <summary>
    /// Feldolgozási folyamat kontextus létrehozása
    /// </summary>
    /// <returns>A létrehozott kontextus</returns>
    public IWhZStockMapContext CreateContext()
    {
        var stockContext = this.stockService.CreateContext();

        return new WhZStockMapContext
        {
            StockContext = stockContext
        };
    }

    /// <summary>
    /// 1 db készlet bejegyzés betöltése a megadott kulcs alapján.
    /// Több találat esetén kivétel kerül kiváltásra.
    /// </summary>
    /// <param name="key">Betöltendő kulcs</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A betöltött bejegyzés vagy null</returns>
    /// <exception cref="InvalidOperationException"><paramref name="source" /> contains more than one element.</exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async ValueTask<OlcWhzstockmap?> GetAsync(IWhZStockMapKey key, CancellationToken cancellationToken = default)
    {
        this.ValidateKeyParameters(key, nameof(key));

        return await this.GetAsync(Find(key), cancellationToken);
    }

    /// <summary>
    /// Új készlet beérkezés rögzítése
    /// </summary>
    /// <param name="context">Feldolgozási folyamat kontextus</param>
    /// <param name="request">Feldolgozandó kérés</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A feldolgozott kérés adattartalma</returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async ValueTask<IWhZStockMapData> AddReceivingAsync(IWhZStockMapContext context, WhZStockMapDto request, CancellationToken cancellationToken = default)
    {
        this.ValidateAddParameters(context, request);

        var ctx = (WhZStockMapContext)context;

        var stockRequest = this.CreateStockRequest(request);
        var stockData = await this.stockService.AddReceivingAsync(ctx.StockContext, stockRequest, cancellationToken);

        var result = new WhZStockMapData
        {
            //StockData = stockData,
            Key = request.CreateKey(),
            Movement = WhZStockMovement.AddReceving,
            Qty = request.Qty.GetValueOrDefault(),
        };

        ctx.AddMovement(result);

        return result;
    }

    /// <summary>
    /// Meglévő készlet beérkezés törlése
    /// </summary>
    /// <param name="context">Feldolgozási folyamat kontextus</param>
    /// <param name="request">Feldolgozandó kérés</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A feldolgozott kérés adattartalma</returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async ValueTask<IWhZStockMapData> RemoveReceivingAsync(IWhZStockMapContext context, WhZStockMapDto request, CancellationToken cancellationToken = default)
    {
        this.ValidateRemoveParameters(context, request);

        var ctx = (WhZStockMapContext)context;

        var recQty = ctx.MovementList
            .Where(i => WhZStockKey.Comparer.Equals(request, i.Key))
            // a recQty-t akarjuk csokkenteni, igy a tobbi mennyiseg nem erdekes
            .Where(i => i.Movement == WhZStockMovement.AddReceving || i.Movement == WhZStockMovement.RemoveReceiving || i.Movement == WhZStockMovement.CommitReceving)
            .Select(i => i.Movement == WhZStockMovement.AddReceving ? i.Qty : -i.Qty)
            .Sum();

        var entity = await this.GetAsync(request, cancellationToken);
        if (entity is not null)
        {
            recQty += entity.Recqty;
        }

        if (recQty < request.Qty)
        {
            this.ThrowException(WhZStockExceptionType.RemoveReceivingQty, "Not enough receiving quantity to fulfill the remove request", entity);
        }

        var stockRequest = this.CreateStockRequest(request);
        var stockData = await this.stockService.RemoveReceivingAsync(ctx.StockContext, stockRequest, cancellationToken);

        var result = new WhZStockMapData
        {
            //StockData = stockData,
            Key = request.CreateKey(),
            Movement = WhZStockMovement.RemoveReceiving,
            Qty = request.Qty.GetValueOrDefault()
        };

        return ctx.AddMovement(result);
    }

    /// <summary>
    /// Készlet beérkezés véglegesítése, tényleges készlet növelése
    /// </summary>
    /// <param name="context">Feldolgozási folyamat kontextus</param>
    /// <param name="request">Feldolgozandó kérés</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A feldolgozott kérés adattartalma</returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async ValueTask<IWhZStockMapData> CommitReceivingAsync(IWhZStockMapContext context, WhZStockMapDto request, CancellationToken cancellationToken = default)
    {
        this.ValidateCommitParameters(context, request);

        var ctx = (WhZStockMapContext)context;

        var recQty = ctx.MovementList
            .Where(i => WhZStockMapKey.Comparer.Equals(request, i.Key))
            // a recQty-t akarjuk csokkenteni, igy a tobbi mennyiseg nem erdekes
            .Where(i => i.Movement == WhZStockMovement.AddReceving || i.Movement == WhZStockMovement.RemoveReceiving || i.Movement == WhZStockMovement.CommitReceving)
            .Select(i => i.Movement == WhZStockMovement.AddReceving ? i.Qty : -i.Qty)
            .Sum();

        var entity = await this.GetAsync(request, cancellationToken);
        if (entity is not null)
        {
            recQty += entity.Recqty;
        }

        if (recQty < request.Qty)
        {
            this.ThrowException(WhZStockExceptionType.CommitReceivingQty, "Not enough receiving quantity to fulfill the commit request", entity);
        }

        var stockRequest = this.CreateStockRequest(request);
        var stockData = await this.stockService.CommitReceivingAsync(ctx.StockContext, stockRequest, cancellationToken);

        var result = new WhZStockMapData
        {
            //StockData = stockData,
            Key = request.CreateKey(),
            Movement = WhZStockMovement.CommitReceving,
            Qty = request.Qty.GetValueOrDefault()
        };

        return ctx.AddMovement(result);
    }

    /// <summary>
    /// Új kiadás foglalás rögzítése
    /// </summary>
    /// <param name="context">Feldolgozási folyamat kontextus</param>
    /// <param name="request">Feldolgozandó kérés</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A feldolgozott kérés adattartalma</returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async ValueTask<IWhZStockMapData> AddReservedAsync(IWhZStockMapContext context, WhZStockMapDto request, CancellationToken cancellationToken = default)
    {
        this.ValidateAddParameters(context, request);

        var ctx = (WhZStockMapContext)context;

        var processedQty = ctx.MovementList
            .Where(i => WhZStockMapKey.Comparer.Equals(request, i.Key))
            .Select(CalculateProcessedQty)
            .Sum();

        var actQty = 0M;
        var entity = await this.GetAsync(request, cancellationToken);
        if (entity is not null)
        {
            actQty = entity!.Actqty + entity.Recqty - entity.Resqty;
        }

        var proposedQty = actQty + processedQty;
        if (proposedQty < request.Qty)
        {
            this.ThrowException(WhZStockExceptionType.AddReservedQty, "Not enough stock to fulfill the reserve request", entity);
        }

        var stockRequest = this.CreateStockRequest(request);
        var stockData = await this.stockService.AddReservedAsync(ctx.StockContext, stockRequest, cancellationToken);

        var result = new WhZStockMapData
        {
            //StockData = stockData,
            Key = request.CreateKey(),
            Movement = WhZStockMovement.AddReserved,
            Qty = request.Qty.GetValueOrDefault()
        };

        return ctx.AddMovement(result);
    }

    /// <summary>
    /// Meglévő kiadás foglalás törlése
    /// </summary>
    /// <param name="context">Feldolgozási folyamat kontextus</param>
    /// <param name="request">Feldolgozandó kérés</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A feldolgozott kérés adattartalma</returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async ValueTask<IWhZStockMapData> RemoveReservedAsync(IWhZStockMapContext context, WhZStockMapDto request, CancellationToken cancellationToken = default)
    {
        this.ValidateRemoveParameters(context, request);

        var ctx = (WhZStockMapContext)context;

        var resQty = ctx.MovementList
            .Where(i => WhZStockMapKey.Comparer.Equals(request, i.Key))
            // a resQty-t akarjuk csokkenteni, igy a tobbi mennyiseg nem erdekes
            .Where(i => i.Movement == WhZStockMovement.AddReserved || i.Movement == WhZStockMovement.RemoveReserved)
            .Select(i => i.Movement == WhZStockMovement.AddReserved ? i.Qty : -i.Qty)
            .Sum();

        var entity = await this.GetAsync(request, cancellationToken);
        if (entity is not null)
        {
            resQty += entity.Resqty;
        }

        if (resQty < request.Qty)
        {
            this.ThrowException(WhZStockExceptionType.RemoveReservedQty, "Not enough reserved quantity to fulfill the remove request", entity);
        }

        var stockRequest = this.CreateStockRequest(request);
        var stockData = await this.stockService.RemoveReservedAsync(ctx.StockContext, stockRequest, cancellationToken);

        var result = new WhZStockMapData
        {
            //StockData = stockData,
            Key = request.CreateKey(),
            Movement = WhZStockMovement.RemoveReserved,
            Qty = request.Qty.GetValueOrDefault()
        };

        return ctx.AddMovement(result);
    }

    /// <summary>
    /// Kiadás foglalás véglegesítése, tényleges készlet csökkentése
    /// </summary>
    /// <param name="context">Feldolgozási folyamat kontextus</param>
    /// <param name="request">Feldolgozandó kérés</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A feldolgozott kérés adattartalma</returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async ValueTask<IWhZStockMapData> CommitReservedAsync(IWhZStockMapContext context, WhZStockMapDto request, CancellationToken cancellationToken = default)
    {
        this.ValidateCommitParameters(context, request);

        var ctx = (WhZStockMapContext)context;

        var resQty = ctx.MovementList
            .Where(i => WhZStockMapKey.Comparer.Equals(request, i.Key))
            // a resQty-t akarjuk csokkenteni, igy a tobbi mennyiseg nem erdekes
            .Where(i => i.Movement == WhZStockMovement.AddReserved || i.Movement == WhZStockMovement.RemoveReserved || i.Movement == WhZStockMovement.CommitReserved)
            .Select(i => i.Movement == WhZStockMovement.AddReserved ? i.Qty : -i.Qty)
            .Sum();

        var entity = await this.GetAsync(request, cancellationToken);
        if (entity is not null)
        {
            resQty += entity.Resqty;
        }

        if (resQty < request.Qty)
        {
            this.ThrowException(WhZStockExceptionType.CommitReservedQty, "Not enough reserved quantity to fulfill the commit request", entity);
        }

        var stockRequest = this.CreateStockRequest(request);
        var stockData = await this.stockService.CommitReservedAsync(ctx.StockContext, stockRequest, cancellationToken);

        var result = new WhZStockMapData
        {
            //StockData = stockData,
            Key = request.CreateKey(),
            Movement = WhZStockMovement.CommitReserved,
            Qty = request.Qty.GetValueOrDefault()
        };

        return ctx.AddMovement(result);
    }

    /// <summary>
    /// Egy feldolgozott kérés kivétele a feldolgozási folyamatból
    /// </summary>
    /// <param name="context">Feldolgozási folyamat kontextus</param>
    /// <param name="request">Feldolgozott kérés</param>
    public void Delete(IWhZStockMapContext context, IWhZStockMapData request)
    {
        this.ValidateDeleteParameters(context, request);

        var ctx = (WhZStockMapContext)context;

        if (!ctx.ProbeRemoveMovement(request))
        {
            this.ThrowException(WhZStockExceptionType.DeleteNotEnoughQty, "Unable to remove this request, cause the further requests uses its quantity");
        }

        //this.stockService.Delete(ctx.StockContext, request.StockData);
        throw new NotImplementedException();

        if (!ctx.TryRemoveMovement(request))
        {
            this.ThrowException(WhZStockExceptionType.DeleteNotEnoughQty, "Unable to remove this request, cause the further requests uses its quantity");
        }
    }

    /// <summary>
    /// Feldolgozási folyamat véglegesítése
    /// Korábban kalkulált változások mentése az adatbázisba
    /// </summary>
    /// <param name="context">Feldolgozási folyamat kontextus</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    public async Task StoreAsync(IWhZStockMapContext context, CancellationToken cancellationToken = default)
    {
        this.ValidateStoreParameters(context);

        var ctx = (WhZStockMapContext)context;

        using var tran = await this.UnitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await this.stockService.StoreAsync(ctx.StockContext, cancellationToken);

            foreach (var movements in ctx.MovementList.GroupBy(m => m.Key, WhZStockMapKey.Comparer))
            {
                await this.StoreAsync(movements.Key, movements, cancellationToken);
            }

            tran.Commit();

            ctx.Clear();
        }
        catch (Exception ex)
        {
            await ERP4U.Log.LoggerManager.Instance.LogErrorAsync<WhZStockService>(ex);
            throw;
        }
        finally
        {
            if (tran.HasTransaction())
            {
                tran.Rollback();
            }
        }
    }

    /// <summary>
    /// Egy [cikk, raktár, helykód] bejegyzéshez tartozó összes helykód készlet összegzése
    /// </summary>
    /// <param name="key">Betöltendő kulcs</param>
    /// <param name="excludedLocId">Kizárt helykód (opcionális)</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>(beérkező mennyiség, aktuális mennyiség, foglalt mennyiség, előjelzett mennyiség)</returns>
    public async ValueTask<(decimal recQty, decimal actQty, decimal resQty, decimal provQty)> SumStockMapQtyAsync(IWhZStockKey key, int? excludedLocId = null, CancellationToken cancellationToken = default)
    {
        this.ValidateKeyParameters(key, nameof(key));

        var query = this.Query(Find(key));
        if (excludedLocId is not null)
        {
            query = query.Where(i => i.Whlocid != excludedLocId);
        }

        var result = await query
            .GroupBy(i => new { i.Itemid, i.Whid, i.Whzoneid })
            .Select(i => new { recQty = i.Sum(e => e.Recqty), actQty = i.Sum(e => e.Actqty), resQty = i.Sum(e => e.Resqty), provQty = i.Sum(e => e.Provqty) })
            .SingleOrDefaultAsync(cancellationToken);

        return (result?.recQty ?? 0, result?.actQty ?? 0, result?.resQty ?? 0, result?.provQty ?? 0);
    }

    /// <summary>
    /// Tranzakció mozgás típus alapján a feldolgozandó értékek előjel helyes meghatározása
    /// </summary>
    private static decimal CalculateProcessedQty(IWhZStockMapData data)
    {
        // új bevét vagy visszavont kivét foglalás esetén a mennyiség növekedik
        if (data.Movement == WhZStockMovement.AddReceving || data.Movement == WhZStockMovement.RemoveReserved)
        {
            return data.Qty;
        }
        // visszavont bevét vagy új kivét foglalás esetén a mennyiség csökken
        else if (data.Movement == WhZStockMovement.AddReserved || data.Movement == WhZStockMovement.RemoveReceiving)
        {
            return -data.Qty;
        }

        // bevét vagy kivét foglalás véglegesítése esetén a mennyiség nem változik
        return 0;
    }

    /// <summary>
    /// Egy készlet bejegyzés mentése az adatbázisba
    /// </summary>
    /// <param name="key">Készlet bejegyzés azonosító</param>
    /// <param name="movements">Bejegyzést érintő változások listája</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    private async Task StoreAsync(IWhZStockMapKey key, IEnumerable<IWhZStockMapData> movements, CancellationToken cancellationToken = default)
    {
        var tryCount = 5;
        var needTryAgain = true;

        // ha menet közben változotak az állapotok, akkor többször megpróbálja menteni az adatokat az új állapot meghatározása után
        while (tryCount-- > 0 && needTryAgain)
        {
            try
            {
                var current = await this.GetAsync(key, cancellationToken);
                current ??= new OlcWhzstockmap
                {
                    Itemid = key.Itemid,
                    Whid = key.Whid,
                    Whzoneid = key.Whzoneid,
                    Whlocid = key.Whlocid,
                };

                this.ValidateMovementBeforeStore(current, movements);

                current.Recqty +=
                    // bevét előjelzés növelése
                    movements.Where(m => m.Movement == WhZStockMovement.AddReceving).Sum(m => m.Qty) -
                    // bevét előjelzés csökkentése
                    movements.Where(m => m.Movement == WhZStockMovement.RemoveReceiving).Sum(m => m.Qty) -
                    // bevét véglegesítése
                    movements.Where(m => m.Movement == WhZStockMovement.CommitReceving).Sum(m => m.Qty);
                current.Resqty +=
                    // kivét foglalás növelése
                    movements.Where(m => m.Movement == WhZStockMovement.AddReserved).Sum(m => m.Qty) -
                    // kivét foglalás csökkentése
                    movements.Where(m => m.Movement == WhZStockMovement.RemoveReserved).Sum(m => m.Qty) -
                    // kivét véglegesítése
                    movements.Where(m => m.Movement == WhZStockMovement.CommitReserved).Sum(m => m.Qty);
                current.Actqty +=
                    // bevét véglegesítése
                    movements.Where(m => m.Movement == WhZStockMovement.CommitReceving).Sum(m => m.Qty) -
                    // kivét véglegesítése
                    movements.Where(m => m.Movement == WhZStockMovement.CommitReserved).Sum(m => m.Qty);

                if (current.Whzstockmapid == 0)
                {
                    await this.AddAsync(current, cancellationToken);
                }
                else
                {
                    await this.UpdateAsync(current, cancellationToken);
                }

                needTryAgain = false;
            }
            catch (WhZStockServiceException ex)
            {
                needTryAgain = false;
                if (ex.Type == WhZStockExceptionType.AlreadyExists || ex.Type == WhZStockExceptionType.AlreadyModified)
                {
                    needTryAgain = true;
                }
            }
        }
    }

    /// <summary>
    /// Új készlet bejegyzés egyedi létrehozó eljárás
    /// </summary>
    /// <param name="entity">Létrehozandó bejegyzés</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A létrehozott készlet bejegyzés</returns>
    protected override async Task<OlcWhzstockmap?> AddIntlAsync(OlcWhzstockmap entity, CancellationToken cancellationToken = default)
    {
        this.ValidateAddOrUpdateIntlParameters(entity);

        var entityToSave = entity.Clone<OlcWhzstockmap>();

        await this.FillSystemFieldsOnAddAsync(entityToSave, cancellationToken);

        await this.ValidateAndThrowAsync(entityToSave, ruleSets: AddRuleSets);

        var insertSql = this.CreateInsertSql(entityToSave);
        var affectedRows = await this.Repository.ExecuteSqlCommandAsync(insertSql, cancellationToken);

        if (affectedRows != 1)
        {
            this.ThrowException(WhZStockExceptionType.AlreadyExists, $"Adding the current stock map was failed, cause it is already exists [Itemid: {entityToSave.Itemid}, Whid: {entityToSave.Whid}, Whzoneid: {entityToSave.Whzoneid}, Whlocid: {entityToSave.Whlocid}]");
        }

        var primaryKey = this.Repository.GetPrimaryKey(entity);

        return await this.GetByIdAsync(primaryKey!.Values.ToArray(), cancellationToken);
    }

    /// <summary>
    /// Meglévő készlet bejegyzés egyedi módosító eljárás
    /// </summary>
    /// <param name="entity">Módosítandó bejegyzés</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A módosított készlet bejegyzés</returns>
    protected override async Task<OlcWhzstockmap?> UpdateIntlAsync(OlcWhzstockmap entity, CancellationToken cancellationToken = default)
    {
        this.ValidateAddOrUpdateIntlParameters(entity);

        var primaryKey = this.Repository.GetPrimaryKey(entity);
        OlcWhzstockmap? knownEntity = null;
        if (primaryKey is not null)
        {
            knownEntity = await this.GetByIdAsync(primaryKey.Values.ToArray(), cancellationToken);
        }

        if (knownEntity is null)
        {
            this.ThrowKeyNotFoundException(primaryKey);
        }

        this.Repository.Detach(knownEntity!);

        var entityToSave = this.MergeOriginal(entity, knownEntity!);

        await this.FillSystemFieldsOnUpdateAsync(entityToSave, cancellationToken);

        await this.ValidateAndThrowAsync(entityToSave, knownEntity, UpdateRuleSets);

        var updateSql = this.CreateUpdateSql(entityToSave, knownEntity!);
        var affectedRows = await this.Repository.ExecuteSqlCommandAsync(updateSql, cancellationToken);

        if (affectedRows != 1)
        {
            this.ThrowException(WhZStockExceptionType.AlreadyModified, $"Updating the current stock map was failed, cause it was modified [Itemid: {entityToSave.Itemid}, Whid: {entityToSave.Whid}, Whzoneid: {entityToSave.Whzoneid}, Whlocid: {entityToSave.Whlocid}]");
        }

        return await this.GetByIdAsync(primaryKey!.Values.ToArray(), cancellationToken);
    }

    /// <summary>
    /// Új készlet bejegyzést létrehozó egyedi SQL előállítása
    /// </summary>
    /// <param name="entityToSave">Létrehozandó bejegyzés</param>
    /// <returns>A létrehozó SQL</returns>
    private FormattableString CreateInsertSql(OlcWhzstockmap entityToSave)
    {
        if (entityToSave is null)
        {
            throw new ArgumentNullException(nameof(entityToSave));
        }

        var sql = new StringBuilder($"if (select count(0) from {this.Repository.GetTableName()} [t] (nolock) where {Utils.ToSql(entityToSave, t => new { t.Itemid, t.Whid, t.Whzoneid, t.Whlocid })}) = 0{Environment.NewLine}");
        sql.AppendLine($"  insert into {this.Repository.GetTableName()} ([{nameof(OlcWhzstockmap.Itemid)}], [{nameof(OlcWhzstockmap.Whid)}], [{nameof(OlcWhzstockmap.Whzoneid)}], [{nameof(OlcWhzstockmap.Whlocid)}], [{nameof(OlcWhzstockmap.Recqty)}], [{nameof(OlcWhzstockmap.Actqty)}], [{nameof(OlcWhzstockmap.Resqty)}])");
        sql.Append($"  values (");
        sql.Append($"{Utils.ToSqlValue(entityToSave.Itemid)}, ");
        sql.Append($"{Utils.ToSqlValue(entityToSave.Whid)}, ");
        sql.Append($"{Utils.ToSqlValue(entityToSave.Whzoneid)}, ");
        sql.Append($"{Utils.ToSqlValue(entityToSave.Whlocid)}, ");
        sql.Append($"{Utils.ToSqlValue(entityToSave.Recqty)}, ");
        sql.Append($"{Utils.ToSqlValue(entityToSave.Actqty)}, ");
        sql.Append($"{Utils.ToSqlValue(entityToSave.Resqty)})");

        return FormattableStringFactory.Create(sql.ToString());
    }

    /// <summary>
    /// Meglévő készlet bejegyzést módosító egyedi SQL előállítása
    /// </summary>
    /// <param name="entityToSave">Módosítandó bejegyzés</param>
    /// <returns>A módosító SQL</returns>
    private FormattableString CreateUpdateSql(OlcWhzstockmap entityToSave, OlcWhzstockmap knownEntity)
    {
        if (entityToSave is null)
        {
            throw new ArgumentNullException(nameof(entityToSave));
        }

        if (knownEntity is null)
        {
            throw new ArgumentNullException(nameof(knownEntity));
        }

        var sql = new StringBuilder($"update {this.Repository.GetTableName()} set{Environment.NewLine}");
        sql.AppendLine($"  {Utils.ToSql(entityToSave, t => t.Recqty)},");
        sql.AppendLine($"  {Utils.ToSql(entityToSave, t => t.Actqty)},");
        sql.AppendLine($"  {Utils.ToSql(entityToSave, t => t.Resqty)}");
        sql.AppendLine($"where {Utils.ToSql(entityToSave, t => new { t.Itemid, t.Whid, t.Whzoneid, t.Whlocid })}");
        sql.AppendLine($"  and {Utils.ToSql(knownEntity, t => new { t.Recqty, t.Actqty, t.Resqty })}");

        return FormattableStringFactory.Create(sql.ToString());
    }

    /// <summary>
    /// Készlet bejegyzést kereső kifejezés létrehozása (<see cref="Expression{Func{OlcWhzstock, bool}}"/> predicate)
    /// </summary>
    /// <param name="key">A keresett bejegyzés kulcsa</param>
    /// <returns>A kereső kifejezés (<see cref="Expression{Func{OlcWhzstock, bool}}"/> predicate)</returns>
    private static Expression<Func<OlcWhzstockmap, bool>> Find(IWhZStockKey key)
    {
        return entity => entity.Itemid == key.Itemid && entity.Whid == key.Whid && entity.Whzoneid == key.Whzoneid;
    }

    /// <summary>
    /// Készlet bejegyzést kereső kifejezés létrehozása (<see cref="Expression{Func{OlcWhzstock, bool}}"/> predicate)
    /// </summary>
    /// <param name="key">A keresett bejegyzés kulcsa</param>
    /// <returns>A kereső kifejezés (<see cref="Expression{Func{OlcWhzstock, bool}}"/> predicate)</returns>
    private static Expression<Func<OlcWhzstockmap, bool>> Find(IWhZStockMapKey key)
    {
        return entity => entity.Itemid == key.Itemid && entity.Whid == key.Whid && entity.Whzoneid == key.Whzoneid && entity.Whlocid == key.Whlocid;
    }

    /// <summary>
    /// A helykódos készlet mozgás kérésből készlet mozgás kérés létrehozása
    /// </summary>
    /// <param name="request">Helykódos készlet mozgás kérés</param>
    /// <returns>A létrehozott készlet mozgás kérés</returns>
    private WhZStockDto CreateStockRequest(WhZStockMapDto request)
    {
        return new WhZStockDto
        {
            Itemid = request.Itemid,
            Whid = request.Whid,
            Whzoneid = request.Whzoneid,
            Qty = request.Qty
        };
    }

    /// <summary>
    /// Egy készlet bejegyzés mentés előtti módosítások validálása
    /// </summary>
    /// <param name="current">Aktuális készlet bejegyzés</param>
    /// <param name="movements">Módosító mozgások</param>
    private void ValidateMovementBeforeStore(OlcWhzstockmap current, IEnumerable<IWhZStockMapData> movements)
    {
        if (current is null)
        {
            throw new ArgumentNullException(nameof(current));
        }

        if (movements is null)
        {
            throw new ArgumentNullException(nameof(movements));
        }

        var receivingMovements = movements.Where(m => m.Movement == WhZStockMovement.AddReceving || m.Movement == WhZStockMovement.RemoveReceiving || m.Movement == WhZStockMovement.CommitReceving);
        var reservedMovements = movements.Where(m => m.Movement == WhZStockMovement.AddReserved || m.Movement == WhZStockMovement.RemoveReserved || m.Movement == WhZStockMovement.CommitReserved);
        var commitMovements = movements.Where(m => m.Movement == WhZStockMovement.CommitReceving || m.Movement == WhZStockMovement.CommitReserved);

        var actQty = current.Actqty + commitMovements.Sum(m => m.Movement == WhZStockMovement.CommitReceving ? m.Qty : -m.Qty);
        var recQty = current.Recqty + receivingMovements.Sum(m => m.Movement == WhZStockMovement.AddReceving ? m.Qty : -m.Qty);
        var resQty = current.Resqty + reservedMovements.Sum(m => m.Movement == WhZStockMovement.AddReserved ? m.Qty : -m.Qty);
        var provQty = actQty + recQty - resQty;

        if (actQty < 0)
        {
            var key = movements.First().Key;
            this.ThrowException(WhZStockExceptionType.CantHandleNegativStock, $"Unable to store movements, cause not enough stock is available (current: {actQty} + recQty: {recQty} < {resQty}) [key: {key.KeyString()}]");
        }

        if (provQty < 0)
        {
            var key = movements.First().Key;
            this.ThrowException(WhZStockExceptionType.CantHandleNegativStock, $"Unable to store movements, cause not enough stock is available (current: {actQty} + recQty: {recQty} < {resQty}) [key: {key.KeyString()}]");
        }
    }

    /// <summary>
    /// Paraméterként érkezett készlet bejegyzés kulcs validálása
    /// </summary>
    /// <param name="key">A validálandó kulcs</param>
    /// <param name="name">A hibaüzenetben megjelenítendő paraméter neve</param>
    private void ValidateKeyParameters(IWhZStockKey key, string name)
    {
        if (key is null)
        {
            throw new ArgumentNullException(name);
        }
    }

    /// <summary>
    /// Paraméterként érkezett feldolgozási folyamat kontextus validálása
    /// </summary>
    /// <param name="context">A validálandó kontextus</param>
    private void ValidateContextParameters(IWhZStockMapContext context)
    {
        if (context is null)
        {
            this.ThrowException(WhZStockExceptionType.InvalidContext, "The context is not set");
        }

        if (context is not WhZStockMapContext)
        {
            this.ThrowException(WhZStockExceptionType.InvalidContext, $"The given context is not a {typeof(WhZStockMapContext).FullName}");
        }
    }

    /// <summary>
    /// Mozgás hozzáadása folyamat során paraméterként érkezett adatok validálása
    /// </summary>
    /// <param name="context">Feldolgozási folyamat kontextus</param>
    /// <param name="request">Feldolgozott kérés</param>
    private void ValidateAddParameters(IWhZStockMapContext context, WhZStockMapDto request)
    {
        this.ValidateKeyParameters(request, nameof(request));
        this.ValidateContextParameters(context);

        if (request.Qty is null)
        {
            this.ThrowException(WhZStockExceptionType.InvalidRequestQty, "The add qty is not set", fieldName: nameof(WhZStockMapDto.Qty));
        }

        if (request.Qty <= 0M)
        {
            this.ThrowException(WhZStockExceptionType.InvalidRequestQty, "The add qty cannot be less or equal to 0", fieldName: nameof(WhZStockMapDto.Qty));
        }
    }

    /// <summary>
    /// Mozgás visszavonása folyamat során paraméterként érkezett adatok validálása
    /// </summary>
    /// <param name="context">Feldolgozási folyamat kontextus</param>
    /// <param name="request">Feldolgozott kérés</param>
    private void ValidateRemoveParameters(IWhZStockMapContext context, WhZStockMapDto request)
    {
        this.ValidateKeyParameters(request, nameof(request));
        this.ValidateContextParameters(context);

        if (request.Qty is null)
        {
            this.ThrowException(WhZStockExceptionType.InvalidRequestQty, "The remove qty is not set", fieldName: nameof(WhZStockMapDto.Qty));
        }

        if (request.Qty <= 0M)
        {
            this.ThrowException(WhZStockExceptionType.InvalidRequestQty, "The remove qty cannot be less or equal to 0", fieldName: nameof(WhZStockMapDto.Qty));
        }
    }

    /// <summary>
    /// Mozgás törlése folyamat során paraméterként érkezett adatok validálása
    /// </summary>
    /// <param name="context">Feldolgozási folyamat kontextus</param>
    /// <param name="data">Feldolgozott kérés</param>
    private void ValidateDeleteParameters(IWhZStockMapContext context, IWhZStockMapData data)
    {
        this.ValidateContextParameters(context);

        if (data is null)
        {
            this.ThrowException(WhZStockExceptionType.InvalidRequestData, "The data is not set");
        }
    }

    /// <summary>
    /// Mozgás véglegesítése folyamat során paraméterként érkezett adatok validálása
    /// </summary>
    /// <param name="context">Feldolgozási folyamat kontextus</param>
    /// <param name="request">Feldolgozott kérés</param>
    private void ValidateCommitParameters(IWhZStockMapContext context, WhZStockMapDto request)
    {
        this.ValidateKeyParameters(request, nameof(request));
        this.ValidateContextParameters(context);

        if (request.Qty is null)
        {
            this.ThrowException(WhZStockExceptionType.InvalidRequestQty, "The commit qty is not set", fieldName: nameof(WhZStockMapDto.Qty));
        }

        if (request.Qty <= 0M)
        {
            this.ThrowException(WhZStockExceptionType.InvalidRequestQty, "The commit qty cannot be less or equal to 0", fieldName: nameof(WhZStockMapDto.Qty));
        }
    }

    /// <summary>
    /// Feldolgozási folyamat mentése során paraméterként érkezett adatok validálása
    /// </summary>
    /// <param name="context">Feldolgozási folyamat kontextus</param>
    private void ValidateStoreParameters(IWhZStockMapContext context)
    {
        this.ValidateContextParameters(context);
    }

    /// <summary>
    /// Hibaüzenet kiváltása
    /// </summary>
    /// <param name="type">Hibakód</param>
    /// <param name="message">Üzenet</param>
    /// <param name="entity">Érintett készlet bejegyzés példány</param>
    /// <param name="fieldName">Érintett mező neve</param>
    private void ThrowException(WhZStockExceptionType type, string message, OlcWhzstockmap? entity = null, string? fieldName = null)
    {
        throw new WhZStockMapServiceException(type, message, entity, fieldName);
    }
}
