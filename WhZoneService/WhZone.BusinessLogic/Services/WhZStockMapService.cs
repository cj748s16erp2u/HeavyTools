using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Containers.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Containers;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Base;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhZone.DataAccess.Repositories.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Validators.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Enums;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Helpers;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Services;

[RegisterDI(Interface = typeof(IWhZStockMapService))]
public class WhZStockMapService : LogicServiceBase<OlcWhzstockmap>, IWhZStockMapService
{
#pragma warning disable CA1822 // Mark members as static
    private readonly IWarehouseService warehouseService;
    private readonly IRepository<OlsCompany> companyRepository;
    private readonly IMapper mapper;

    public WhZStockMapService(
        IOlcWhzstockmapValidator validator,
        IRepository<OlcWhzstockmap> repository,
        IUnitOfWork unitOfWork,
        IEnvironmentService environmentService,
        IWarehouseService warehouseService,
        IRepository<OlsCompany> companyRepository,
        IMapper mapper) : base(validator, repository, unitOfWork, environmentService)
    {
        this.warehouseService = warehouseService ?? throw new ArgumentNullException(nameof(warehouseService));
        this.companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Feldolgozási folyamat kontextus létrehozása
    /// </summary>
    /// <returns>A létrehozott kontextus</returns>
    public IWhZStockMapContext CreateContext()
    {
        return new WhZStockMapContext();
    }

    /// <summary>
    /// Helykód készlet lekérdezése
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>Készlet lista</returns>
    public async Task<IEnumerable<WhZStockMapQDto>> QueryStockMapAsync(WhZStockMapQueryDto query = null!, CancellationToken cancellationToken = default)
    {
        Expression<Func<OlcWhzstockmap, bool>> predicate = null!;
        if (query is not null)
        {
            predicate = this.CreatePredicate(query);
        }

        var q = this.Query(predicate);

        q = q
            .Include(s => s.Item)
            .Include(s => s.Wh)
            .Include(s => s.Whzone)
            .Include(s => s.Whloc);

        if (query?.Cmpid is not null)
        { 
            q = q
                .Where(s =>
                    (s.Item.Cmpcodes == CompanyUtils.CMPCODEALL || this.companyRepository.Entities
                        .Any(c => EF.Functions.Like(s.Item.Cmpcodes, "%" + c.Cmpcode + "%") && query.Cmpid.Contains(c.Cmpid))) &&
                    (s.Wh.Cmpcodes == CompanyUtils.CMPCODEALL || this.companyRepository.Entities
                        .Any(c => EF.Functions.Like(s.Wh.Cmpcodes, "%" + c.Cmpcode + "%") && query.Cmpid.Contains(c.Cmpid))));
        }

        if (query?.Nonzerostock == true)
        {
            q = q
                .Where(s => s.Recqty != 0 || s.Actqty != 0 || s.Resqty != 0);
        }

        var list = await q.ToListAsync(cancellationToken);
        return this.mapper.Map<IEnumerable<WhZStockMapQDto>>(list);
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
    public ValueTask<IWhZStockMapData> AddReceivingAsync(IWhZStockMapContext context, WhZStockMapDto request, CancellationToken cancellationToken = default)
    {
        this.ValidateAddParameters(context, request);

        var ctx = (WhZStockMapContext)context;

        var result = new WhZStockMapData
        {
            Key = request.CreateKey(),
            Movement = WhZStockMovement.AddReceving,
            Qty = request.Qty.GetValueOrDefault(),
        };

        ctx.AddMovement(result);

        return ValueTask.FromResult<IWhZStockMapData>(result);
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
            ThrowException(WhZStockExceptionType.RemoveReceivingQty, "Not enough receiving quantity to fulfill the remove request", entity);
        }

        var result = new WhZStockMapData
        {
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
            ThrowException(WhZStockExceptionType.CommitReceivingQty, "Not enough receiving quantity to fulfill the commit request", entity);
        }

        var result = new WhZStockMapData
        {
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
            ThrowException(WhZStockExceptionType.AddReservedQty, "Not enough stock to fulfill the reserve request", entity);
        }

        var result = new WhZStockMapData
        {
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
            ThrowException(WhZStockExceptionType.RemoveReservedQty, "Not enough reserved quantity to fulfill the remove request", entity);
        }

        var result = new WhZStockMapData
        {
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
            ThrowException(WhZStockExceptionType.CommitReservedQty, "Not enough reserved quantity to fulfill the commit request", entity);
        }

        var result = new WhZStockMapData
        {
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
            ThrowException(WhZStockExceptionType.DeleteNotEnoughQty, "Unable to remove this request, cause the further requests uses its quantity");
        }

        if (!ctx.TryRemoveMovement(request))
        {
            ThrowException(WhZStockExceptionType.DeleteNotEnoughQty, "Unable to remove this request, cause the further requests uses its quantity");
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
            foreach (var movements in ctx.MovementList.GroupBy(m => m.Key, WhZStockMapKey.Comparer))
            {
                await this.StoreAsync(movements.Key, movements, cancellationToken);
            }

            tran.Commit();

            ctx.Clear();
        }
        catch (Exception ex)
        {
            await ERP2U.Log.LoggerManager.Instance.LogErrorAsync<WhZStockMapService>(ex);
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
            catch (WhZStockMapServiceException ex)
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

        await this.ValidateAndThrowAsync(entityToSave, ruleSets: AddRuleSets, cancellationToken: cancellationToken);

        var insertSql = this.CreateInsertSql(entityToSave);
        var affectedRows = await this.Repository.ExecuteSqlCommandAsync(insertSql, cancellationToken);

        if (affectedRows != 1)
        {
            ThrowException(WhZStockExceptionType.AlreadyExists, $"Adding the current stock map was failed, cause it is already exists [Itemid: {entityToSave.Itemid}, Whid: {entityToSave.Whid}, Whzoneid: {entityToSave.Whzoneid}, Whlocid: {entityToSave.Whlocid}]");
        }

        entity = (await this.GetAsync(entityToSave, cancellationToken))!;
        if (entity is null)
        {
            ThrowException(WhZStockExceptionType.InsertFailed, $"Adding the current stock map was failed [Itemid: {entityToSave.Itemid}, Whid: {entityToSave.Whid}, Whzoneid: {entityToSave.Whzoneid}, Whlocid: {entityToSave.Whlocid}]");
        }

        var primaryKey = this.Repository.GetPrimaryKey(entity!);

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

        await this.ValidateAndThrowAsync(entityToSave, knownEntity, UpdateRuleSets, cancellationToken);

        var updateSql = this.CreateUpdateSql(entityToSave, knownEntity!);
        var affectedRows = await this.Repository.ExecuteSqlCommandAsync(updateSql, cancellationToken);

        if (affectedRows != 1)
        {
            ThrowException(WhZStockExceptionType.AlreadyModified, $"Updating the current stock map was failed, cause it was modified [Itemid: {entityToSave.Itemid}, Whid: {entityToSave.Whid}, Whzoneid: {entityToSave.Whzoneid}, Whlocid: {entityToSave.Whlocid}]");
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

        //var sql = new StringBuilder($"if (select count(0) from {this.Repository.GetTableName()} [t] (nolock) where {Utils.ToSql(entityToSave, t => new { t.Itemid, t.Whid, t.Whzoneid, t.Whlocid })}) = 0{Environment.NewLine}");
        //sql.AppendLine($"  insert into {this.Repository.GetTableName()} ([{nameof(OlcWhzstockmap.Itemid)}], [{nameof(OlcWhzstockmap.Whid)}], [{nameof(OlcWhzstockmap.Whzoneid)}], [{nameof(OlcWhzstockmap.Whlocid)}], [{nameof(OlcWhzstockmap.Recqty)}], [{nameof(OlcWhzstockmap.Reqqty)}], [{nameof(OlcWhzstockmap.Actqty)}], [{nameof(OlcWhzstockmap.Resqty)}])");
        //sql.Append($"  values (");
        //sql.Append($"{Utils.ToSqlValue(entityToSave.Itemid)}, ");
        //sql.Append($"{Utils.ToSqlValue(entityToSave.Whid)}, ");
        //sql.Append($"{Utils.ToSqlValue(entityToSave.Whzoneid)}, ");
        //sql.Append($"{Utils.ToSqlValue(entityToSave.Whlocid)}, ");
        //sql.Append($"{Utils.ToSqlValue(entityToSave.Recqty)}, ");
        //sql.Append($"{Utils.ToSqlValue(entityToSave.Reqqty)}, ");
        //sql.Append($"{Utils.ToSqlValue(entityToSave.Actqty)}, ");
        //sql.Append($"{Utils.ToSqlValue(entityToSave.Resqty)})");

        var (whereSql, whereParameters) = Utils.ToSqlWhereParameter(entityToSave, t => new { t.Itemid, t.Whid, t.Whzoneid, t.Whlocid }, "key");
        var (fieldsSql, valuesSql, insertParameters) = Utils.ToSqlInsertParameter(entityToSave, t => new { t.Itemid, t.Whid, t.Whzoneid, t.Whlocid, t.Recqty, t.Reqqty, t.Actqty, t.Resqty });

        var sqlBldr = new StringBuilder($"if (select count(0) from {this.Repository.GetTableName()} [t] (nolock) where {whereSql}) = 0{Environment.NewLine}");
        sqlBldr.AppendLine($"  insert into {this.Repository.GetTableName()} ({fieldsSql})");
        sqlBldr.Append($"  values ({valuesSql})");
        var sql = sqlBldr.ToString();

        //var i = 0;
        var list = new List<object>();
        foreach (var p in whereParameters.Concat(insertParameters))
        {
            list.Add(new Microsoft.Data.SqlClient.SqlParameter(p.Key, p.Value));

            //sql = sql.Replace(p.Key, $"{{{i++}}}");
            //list.Add(p.Value!);
        }

        return FormattableStringFactory.Create(sql, list.ToArray());
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

        var (whereSql, whereParameters) = Utils.ToSqlWhereParameter(entityToSave, t => new { t.Itemid, t.Whid, t.Whzoneid, t.Whlocid }, "key");
        var (knownSql, knownParameters) = Utils.ToSqlWhereParameter(knownEntity, t => new { t.Recqty, t.Reqqty, t.Actqty, t.Resqty }, "known");
        var (updateSql, updateParameters) = Utils.ToSqlUpdateParameter(entityToSave, t => new { t.Recqty, t.Reqqty, t.Actqty, t.Resqty });

        var sqlBldr = new StringBuilder($"update {this.Repository.GetTableName()} set{Environment.NewLine}");
        sqlBldr.AppendLine(updateSql);
        sqlBldr.AppendLine($"where {whereSql}");
        sqlBldr.AppendLine($"  and {knownSql}");
        var sql = sqlBldr.ToString();

        //var i = 0;
        var list = new List<object>();
        foreach (var p in whereParameters.Concat(knownParameters).Concat(updateParameters))
        {
            list.Add(new Microsoft.Data.SqlClient.SqlParameter(p.Key, p.Value));

            //sql = sql.Replace(p.Key, $"{{{i++}}}");
            //list.Add(p.Value);
        }

        return FormattableStringFactory.Create(sql, list.ToArray());
    }

    /// <summary>
    /// Készlet bejegyzést kereső kifejezés létrehozása (<see cref="Expression{Func{OlcWhzstock, bool}}"/> predicate)
    /// </summary>
    /// <param name="key">A keresett bejegyzés kulcsa</param>
    /// <returns>A kereső kifejezés (<see cref="Expression{Func{OlcWhzstock, bool}}"/> predicate)</returns>
    private static Expression<Func<OlcWhzstockmap, bool>> Find(IWhZStockMapKey key)
    {
        if (key is null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        return entity => entity.Itemid == key.Itemid && entity.Whid == key.Whid && entity.Whzoneid == key.Whzoneid && entity.Whlocid == key.Whlocid;
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
            ThrowException(WhZStockExceptionType.CantHandleNegativStock, $"Unable to store movements, cause not enough stock is available (current: {actQty} + recQty: {recQty} < {resQty}) [key: {key.KeyString()}]");
        }

        if (provQty < 0)
        {
            var key = movements.First().Key;
            ThrowException(WhZStockExceptionType.CantHandleNegativStock, $"Unable to store movements, cause not enough stock is available (current: {actQty} + recQty: {recQty} < {resQty}) [key: {key.KeyString()}]");
        }
    }

    /// <summary>
    /// Paraméterként érkezett készlet bejegyzés kulcs validálása
    /// </summary>
    /// <param name="key">A validálandó kulcs</param>
    /// <param name="name">A hibaüzenetben megjelenítendő paraméter neve</param>
    private void ValidateKeyParameters(IWhZStockMapKey key, string name)
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
            ThrowException(WhZStockExceptionType.InvalidContext, "The context is not set");
        }

        if (context is not WhZStockMapContext)
        {
            ThrowException(WhZStockExceptionType.InvalidContext, $"The given context is not a {typeof(WhZStockMapContext).FullName}");
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
            ThrowException(WhZStockExceptionType.InvalidRequestQty, "The add qty is not set", fieldName: nameof(WhZStockMapDto.Qty));
        }

        if (request.Qty <= 0M)
        {
            ThrowException(WhZStockExceptionType.InvalidRequestQty, "The add qty cannot be less or equal to 0", fieldName: nameof(WhZStockMapDto.Qty));
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
            ThrowException(WhZStockExceptionType.InvalidRequestQty, "The remove qty is not set", fieldName: nameof(WhZStockMapDto.Qty));
        }

        if (request.Qty <= 0M)
        {
            ThrowException(WhZStockExceptionType.InvalidRequestQty, "The remove qty cannot be less or equal to 0", fieldName: nameof(WhZStockMapDto.Qty));
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
            ThrowException(WhZStockExceptionType.InvalidRequestData, "The data is not set");
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
            ThrowException(WhZStockExceptionType.InvalidRequestQty, "The commit qty is not set", fieldName: nameof(WhZStockMapDto.Qty));
        }

        if (request.Qty <= 0M)
        {
            ThrowException(WhZStockExceptionType.InvalidRequestQty, "The commit qty cannot be less or equal to 0", fieldName: nameof(WhZStockMapDto.Qty));
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
    private static void ThrowException(WhZStockExceptionType type, string message, OlcWhzstockmap? entity = null, string? fieldName = null)
    {
        throw new WhZStockMapServiceException(type, message, entity, fieldName);
    }

    /// <summary>
    /// Validálásnál használt adattároló feltöltése
    /// új adat: raktár, zóna
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="originalEntity"></param>
    /// <param name="ruleSets"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected override async ValueTask<ValidationContext<OlcWhzstockmap>> CreateValidationContextAsync(OlcWhzstockmap entity, OlcWhzstockmap? originalEntity, string[] ruleSets, CancellationToken cancellationToken = default)
    {
        var context = await base.CreateValidationContextAsync(entity, originalEntity, ruleSets, cancellationToken);

        if (!string.IsNullOrWhiteSpace(entity.Whid))
        {
            var warehouse = await this.warehouseService.GetByIdAsync(new object[] { entity.Whid }, cancellationToken);
            if (warehouse is not null)
            {
                if (!context.TryAddEntity(warehouse))
                {
                    this.ThrowException("Unable to add warehouse to the validation context", entity);
                }
            }
        }

        return context;
    }
#pragma warning restore CA1822 // Mark members as static
}
