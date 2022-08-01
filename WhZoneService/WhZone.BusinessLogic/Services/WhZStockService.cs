using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Enums;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Containers;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Base;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Validators.Interfaces;
using eLog.HeavyTools.Services.WhZone.DataAccess.Repositories.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Containers.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Interfaces;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Services;

[RegisterDI(Interface = typeof(IWhZStockService))]
public class WhZStockService : LogicServiceBase<OlcWhzstock>, IWhZStockService
{
    public WhZStockService(
        IOlcWhzstockValidator validator,
        IRepository<OlcWhzstock> repository,
        IUnitOfWork unitOfWork,
        IEnvironmentService environmentService) : base(validator, repository, unitOfWork, environmentService)
    {
    }

    /// <summary>
    /// Feldolgozási folyamat kontextus létrehozása
    /// </summary>
    /// <returns>A létrehozott kontextus</returns>
    public IWhZStockContext CreateContext() => new WhZStockContext();

    /// <summary>
    /// 1 db készlet bejegyzés betöltése a megadott kulcs alapján.
    /// Több találat esetén kivétel kerül kiváltásra.
    /// </summary>
    /// <param name="key">Betöltendő kulcs</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A betöltött bejegyzés vagy null</returns>
    /// <exception cref="InvalidOperationException"><paramref name="source" /> contains more than one element.</exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async ValueTask<OlcWhzstock?> GetAsync(IWhZStockKey key, CancellationToken cancellationToken = default)
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
    public ValueTask<IWhZStockData> AddReceivingAsync(IWhZStockContext context, WhZStockDto request, CancellationToken cancellationToken = default)
    {
        this.ValidateAddParameters(context, request);

        var ctx = (WhZStockContext)context;

        var result = new WhZStockData
        {
            Key = request.CreateKey(),
            Movement = WhZStockMovement.AddReceving,
            Qty = request.Qty.GetValueOrDefault(),
        };

        ctx.AddMovement(result);

        return ValueTask.FromResult<IWhZStockData>(result);
    }

    /// <summary>
    /// Meglévő készlet beérkezés törlése
    /// </summary>
    /// <param name="context">Feldolgozási folyamat kontextus</param>
    /// <param name="request">Feldolgozandó kérés</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A feldolgozott kérés adattartalma</returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async ValueTask<IWhZStockData> RemoveReceivingAsync(IWhZStockContext context, WhZStockDto request, CancellationToken cancellationToken = default)
    {
        this.ValidateRemoveParameters(context, request);

        var ctx = (WhZStockContext)context;

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

        var result = new WhZStockData
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
    public async ValueTask<IWhZStockData> CommitReceivingAsync(IWhZStockContext context, WhZStockDto request, CancellationToken cancellationToken = default)
    {
        this.ValidateCommitParameters(context, request);

        var ctx = (WhZStockContext)context;

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
            this.ThrowException(WhZStockExceptionType.CommitReceivingQty, "Not enough receiving quantity to fulfill the commit request", entity);
        }

        var result = new WhZStockData
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
    public async ValueTask<IWhZStockData> AddReservedAsync(IWhZStockContext context, WhZStockDto request, CancellationToken cancellationToken = default)
    {
        this.ValidateAddParameters(context, request);

        var ctx = (WhZStockContext)context;

        var processedQty = ctx.MovementList
            .Where(i => WhZStockKey.Comparer.Equals(request, i.Key))
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

        var result = new WhZStockData
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
    public async ValueTask<IWhZStockData> RemoveReservedAsync(IWhZStockContext context, WhZStockDto request, CancellationToken cancellationToken = default)
    {
        this.ValidateRemoveParameters(context, request);

        var ctx = (WhZStockContext)context;

        var resQty = ctx.MovementList
            .Where(i => WhZStockKey.Comparer.Equals(request, i.Key))
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

        var result = new WhZStockData
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
    public async ValueTask<IWhZStockData> CommitReservedAsync(IWhZStockContext context, WhZStockDto request, CancellationToken cancellationToken = default)
    {
        this.ValidateCommitParameters(context, request);

        var ctx = (WhZStockContext)context;

        var resQty = ctx.MovementList
            .Where(i => WhZStockKey.Comparer.Equals(request, i.Key))
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

        var result = new WhZStockData
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
    public void Delete(IWhZStockContext context, IWhZStockData request)
    {
        this.ValidateDeleteParameters(context, request);

        var ctx = (WhZStockContext)context;

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
    public async Task StoreAsync(IWhZStockContext context, CancellationToken cancellationToken = default)
    {
        this.ValidateStoreParameters(context);

        var ctx = (WhZStockContext)context;

        using var tran = await this.UnitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            foreach (var movements in ctx.MovementList.GroupBy(m => m.Key, WhZStockKey.Comparer))
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
    /// Tranzakció mozgás típus alapján a feldolgozandó értékek előjel helyes meghatározása
    /// </summary>
    private static decimal CalculateProcessedQty(IWhZStockData data)
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
    private async Task StoreAsync(IWhZStockKey key, IEnumerable<IWhZStockData> movements, CancellationToken cancellationToken = default)
    {
        var tryCount = 5;
        var needTryAgain = true;

        // ha menet közben változotak az állapotok, akkor többször megpróbálja menteni az adatokat az új állapot meghatározása után
        while (tryCount-- > 0 && needTryAgain)
        {
            try
            {
                var current = await this.GetAsync(key, cancellationToken);
                current ??= new OlcWhzstock
                {
                    Itemid = key.Itemid,
                    Whid = key.Whid,
                    Whzoneid = key.Whzoneid,
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

                var stockMaps = movements.Select(m => m.StockMapData).ToList();
                if (current.Whzstockid == 0)
                {
                    await this.AddIntlAsync(current, stockMaps, cancellationToken);
                }
                else
                {
                    await this.UpdateIntlAsync(current, stockMaps, cancellationToken);
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
    protected override Task<OlcWhzstock?> AddIntlAsync(OlcWhzstock entity, CancellationToken cancellationToken = default)
    {
        return this.AddIntlAsync(entity, null, cancellationToken);
    }

    /// <summary>
    /// Új készlet bejegyzés egyedi létrehozó eljárás
    /// </summary>
    /// <param name="entity">Létrehozandó bejegyzés</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A létrehozott készlet bejegyzés</returns>
    protected virtual async Task<OlcWhzstock?> AddIntlAsync(OlcWhzstock entity, IEnumerable<IWhZStockMapData>? stockMaps, CancellationToken cancellationToken = default)
    {
        this.ValidateAddOrUpdateIntlParameters(entity);

        var entityToSave = entity.Clone<OlcWhzstock>();

        await this.FillSystemFieldsOnAddAsync(entityToSave, cancellationToken);

        await this.ValidateAndThrowAsync(entityToSave, stockMaps: stockMaps, ruleSets: AddRuleSets);

        var insertSql = this.CreateInsertSql(entityToSave);
        var affectedRows = await this.Repository.ExecuteSqlCommandAsync(insertSql, cancellationToken);

        if (affectedRows != 1)
        {
            this.ThrowException(WhZStockExceptionType.AlreadyExists, $"Adding the current stock was failed, cause it is already exists [Itemid: {entityToSave.Itemid}, Whid: {entityToSave.Whid}, Whzoneid: {entityToSave.Whzoneid}]");
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
    protected override Task<OlcWhzstock?> UpdateIntlAsync(OlcWhzstock entity, CancellationToken cancellationToken = default)
    {
        return this.UpdateIntlAsync(entity, null, cancellationToken);
    }

    /// <summary>
    /// Meglévő készlet bejegyzés egyedi módosító eljárás
    /// </summary>
    /// <param name="entity">Módosítandó bejegyzés</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A módosított készlet bejegyzés</returns>
    protected virtual async Task<OlcWhzstock?> UpdateIntlAsync(OlcWhzstock entity, IEnumerable<IWhZStockMapData>? stockMaps, CancellationToken cancellationToken = default)
    {
        this.ValidateAddOrUpdateIntlParameters(entity);

        var primaryKey = this.Repository.GetPrimaryKey(entity);
        OlcWhzstock? knownEntity = null;
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

        await this.ValidateAndThrowAsync(entityToSave, knownEntity, stockMaps, UpdateRuleSets);

        var updateSql = this.CreateUpdateSql(entityToSave, knownEntity!);
        var affectedRows = await this.Repository.ExecuteSqlCommandAsync(updateSql, cancellationToken);

        if (affectedRows != 1)
        {
            this.ThrowException(WhZStockExceptionType.AlreadyModified, $"Updating the current stock was failed, cause it was modified [Itemid: {entityToSave.Itemid}, Whid: {entityToSave.Whid}, Whzoneid: {entityToSave.Whzoneid}]");
        }

        return await this.GetByIdAsync(primaryKey!.Values.ToArray(), cancellationToken);
    }

    public override Task ValidateAndThrowAsync(OlcWhzstock entity, OlcWhzstock? originalEntity = null, params string[] ruleSets)
    {
        return this.ValidateAndThrowAsync(entity, originalEntity, null, ruleSets);
    }

    public virtual async Task ValidateAndThrowAsync(OlcWhzstock entity, OlcWhzstock? originalEntity = null, IEnumerable<IWhZStockMapData>? stockMaps = null, params string[] ruleSets)
    {
        if (this.Validator is not null)
        {
            if (ruleSets?.Any() != true)
            {
                ruleSets = DefaultRuleSets;
            }
            else
            {
                ruleSets = DefaultRuleSets.Concat(ruleSets).Distinct().ToArray();
            }

            var context = this.CreateValidationContext(entity, originalEntity, ruleSets);
            if (stockMaps?.Any() == true)
            {
                context.RootContextData.TryAdd(nameof(OlcWhzstockmap), stockMaps);
            }

            var validationResult = await this.Validator.ValidateAsync(context);
            if (!validationResult.IsValid)
            {
                this.ThrowException(validationResult.Errors, entity, originalEntity);
            }
        }
    }

    /// <summary>
    /// Új készlet bejegyzést létrehozó egyedi SQL előállítása
    /// </summary>
    /// <param name="entityToSave">Létrehozandó bejegyzés</param>
    /// <returns>A létrehozó SQL</returns>
    private FormattableString CreateInsertSql(OlcWhzstock entityToSave)
    {
        if (entityToSave is null)
        {
            throw new ArgumentNullException(nameof(entityToSave));
        }

        var sql = new StringBuilder($"if (select count(0) from {this.Repository.GetTableName()} [t] (nolock) where {Utils.ToSql(entityToSave, t => new { t.Itemid, t.Whid, t.Whzoneid })}) = 0{Environment.NewLine}");
        sql.AppendLine($"  insert into {this.Repository.GetTableName()} ([{nameof(OlcWhzstock.Itemid)}], [{nameof(OlcWhzstock.Whid)}], [{nameof(OlcWhzstock.Whzoneid)}], [{nameof(OlcWhzstock.Recqty)}], [{nameof(OlcWhzstock.Actqty)}], [{nameof(OlcWhzstock.Resqty)}])");
        sql.Append($"  values (");
        sql.Append($"{Utils.ToSqlValue(entityToSave.Itemid)}, ");
        sql.Append($"{Utils.ToSqlValue(entityToSave.Whid)}, ");
        sql.Append($"{Utils.ToSqlValue(entityToSave.Whzoneid)}, ");
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
    private FormattableString CreateUpdateSql(OlcWhzstock entityToSave, OlcWhzstock knownEntity)
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
        sql.AppendLine($"where {Utils.ToSql(entityToSave, t => new { t.Itemid, t.Whid, t.Whzoneid })}");
        sql.AppendLine($"  and {Utils.ToSql(knownEntity, t => new { t.Recqty, t.Actqty, t.Resqty })}");

        return FormattableStringFactory.Create(sql.ToString());
    }

    /// <summary>
    /// Készlet bejegyzést kereső kifejezés létrehozása (<see cref="Expression{Func{OlcWhzstock, bool}}"/> predicate)
    /// </summary>
    /// <param name="key">A keresett bejegyzés kulcsa</param>
    /// <returns>A kereső kifejezés (<see cref="Expression{Func{OlcWhzstock, bool}}"/> predicate)</returns>
    private static Expression<Func<OlcWhzstock, bool>> Find(IWhZStockKey key)
    {
        return entity => entity.Itemid == key.Itemid && entity.Whid == key.Whid && entity.Whzoneid == key.Whzoneid;
    }

    /// <summary>
    /// Egy készlet bejegyzés mentés előtti módosítások validálása
    /// </summary>
    /// <param name="current">Aktuális készlet bejegyzés</param>
    /// <param name="movements">Módosító mozgások</param>
    private void ValidateMovementBeforeStore(OlcWhzstock current, IEnumerable<IWhZStockData> movements)
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
    private void ValidateContextParameters(IWhZStockContext context)
    {
        if (context is null)
        {
            this.ThrowException(WhZStockExceptionType.InvalidContext, "The context is not set");
        }

        if (context is not WhZStockContext)
        {
            this.ThrowException(WhZStockExceptionType.InvalidContext, $"The given context is not a {typeof(WhZStockContext).FullName}");
        }
    }

    /// <summary>
    /// Mozgás hozzáadása folyamat során paraméterként érkezett adatok validálása
    /// </summary>
    /// <param name="context">Feldolgozási folyamat kontextus</param>
    /// <param name="request">Feldolgozott kérés</param>
    private void ValidateAddParameters(IWhZStockContext context, WhZStockDto request)
    {
        this.ValidateKeyParameters(request, nameof(request));
        this.ValidateContextParameters(context);

        if (request.Qty is null)
        {
            this.ThrowException(WhZStockExceptionType.InvalidRequestQty, "The add qty is not set", fieldName: nameof(WhZStockDto.Qty));
        }

        if (request.Qty <= 0M)
        {
            this.ThrowException(WhZStockExceptionType.InvalidRequestQty, "The add qty cannot be less or equal to 0", fieldName: nameof(WhZStockDto.Qty));
        }
    }

    /// <summary>
    /// Mozgás visszavonása folyamat során paraméterként érkezett adatok validálása
    /// </summary>
    /// <param name="context">Feldolgozási folyamat kontextus</param>
    /// <param name="request">Feldolgozott kérés</param>
    private void ValidateRemoveParameters(IWhZStockContext context, WhZStockDto request)
    {
        this.ValidateKeyParameters(request, nameof(request));
        this.ValidateContextParameters(context);

        if (request.Qty is null)
        {
            this.ThrowException(WhZStockExceptionType.InvalidRequestQty, "The remove qty is not set", fieldName: nameof(WhZStockDto.Qty));
        }

        if (request.Qty <= 0M)
        {
            this.ThrowException(WhZStockExceptionType.InvalidRequestQty, "The remove qty cannot be less or equal to 0", fieldName: nameof(WhZStockDto.Qty));
        }
    }

    /// <summary>
    /// Mozgás törlése folyamat során paraméterként érkezett adatok validálása
    /// </summary>
    /// <param name="context">Feldolgozási folyamat kontextus</param>
    /// <param name="data">Feldolgozott kérés</param>
    private void ValidateDeleteParameters(IWhZStockContext context, IWhZStockData data)
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
    private void ValidateCommitParameters(IWhZStockContext context, WhZStockDto request)
    {
        this.ValidateKeyParameters(request, nameof(request));
        this.ValidateContextParameters(context);

        if (request.Qty is null)
        {
            this.ThrowException(WhZStockExceptionType.InvalidRequestQty, "The commit qty is not set", fieldName: nameof(WhZStockDto.Qty));
        }

        if (request.Qty <= 0M)
        {
            this.ThrowException(WhZStockExceptionType.InvalidRequestQty, "The commit qty cannot be less or equal to 0", fieldName: nameof(WhZStockDto.Qty));
        }
    }

    /// <summary>
    /// Feldolgozási folyamat mentése során paraméterként érkezett adatok validálása
    /// </summary>
    /// <param name="context">Feldolgozási folyamat kontextus</param>
    private void ValidateStoreParameters(IWhZStockContext context)
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
    private void ThrowException(WhZStockExceptionType type, string message, OlcWhzstock? entity = null, string? fieldName = null)
    {
        throw new WhZStockServiceException(type, message, entity, fieldName);
    }
}
