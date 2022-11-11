using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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
using Microsoft.Extensions.Options;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Services;

[RegisterDI(Interface = typeof(IWhZTranLocService))]
public class WhZTranLocService : LogicServiceBase<OlcWhztranloc>, IWhZTranLocService
{
    private readonly IWhZStockMapService whZStockMapService;
    private readonly IRepository<OlcWhztranhead> tranHeadRepository;
    private readonly IRepository<OlcWhztranline> tranLineRepository;
    private readonly IRepository<OlcWhlocation> locationRepository;
    private readonly IRepository<OlsSthead> stHeadRepository;
    private readonly IOptions<Options.WhZTranOptions> whZTranOptions;
    private readonly IMapper mapper;

    public WhZTranLocService(
        IOlcWhztranlocValidator validator,
        IRepository<OlcWhztranloc> repository,
        IUnitOfWork unitOfWork,
        IEnvironmentService environmentService,
        IWhZStockMapService whZStockMapService,
        IRepository<OlcWhztranhead> tranHeadRepository,
        IRepository<OlcWhztranline> tranLineRepository,
        IRepository<OlcWhlocation> locationRepository,
        IRepository<OlsSthead> stHeadRepository,
        IOptions<Options.WhZTranOptions> whZTranOptions,
        IMapper mapper) : base(validator, repository, unitOfWork, environmentService)
    {
        this.whZStockMapService = whZStockMapService ?? throw new ArgumentNullException(nameof(whZStockMapService));
        this.tranHeadRepository = tranHeadRepository ?? throw new ArgumentNullException(nameof(tranHeadRepository));
        this.tranLineRepository = tranLineRepository ?? throw new ArgumentNullException(nameof(tranLineRepository));
        this.locationRepository = locationRepository ?? throw new ArgumentNullException(nameof(locationRepository));
        this.stHeadRepository = stHeadRepository ?? throw new ArgumentNullException(nameof(stHeadRepository));
        this.whZTranOptions = whZTranOptions ?? throw new ArgumentNullException(nameof(whZTranOptions));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    private string? DefaultReceivingLocCode => this.whZTranOptions.Value?.DefaultReceivingLocCode;

    /// <summary>
    /// Zóna készlet tranzakció helykód lekérdezése
    /// </summary>
    /// <param name="predicate">Szűrési feltétel</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Tranzakció tételek listája</returns>
    public async Task<IEnumerable<WhZTranLocDto>> QueryAsync(WhZTranLocQueryDto query = null!, CancellationToken cancellationToken = default)
    {
        Expression<Func<OlcWhztranloc, bool>> predicate = null!;
        if (query is not null)
        {
            predicate = this.CreatePredicate(query);
        }

        var q = this.Query(predicate);
        q = q
            .Include(l => l.Wh)
            .Include(l => l.Whzone)
            .Include(l => l.Whloc)
            .Include(l => l.Whztline)
            .ThenInclude(l => l.Item);

        var list = await q.ToListAsync(cancellationToken);
        return this.mapper.Map<IEnumerable<WhZTranLocDto>>(list);
    }

    /// <summary>
    /// Zóna készlet tranzakció helykód lekérés
    /// </summary>
    /// <param name="predicate">Szűrési feltétel</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Tranzakció tételek listája</returns>
    public async Task<WhZTranLocDto> GetAsync(int whztlocid, CancellationToken cancellationToken = default)
    {
        var q = this.Query(l => l.Whztlocid == whztlocid);
        q = q
            .Include(l => l.Wh)
            .Include(l => l.Whzone)
            .Include(l => l.Whloc)
            .Include(l => l.Whztline)
            .ThenInclude(l => l.Item);

        var list = await q.FirstOrDefaultAsync(cancellationToken);
        return this.mapper.Map<WhZTranLocDto>(list);
    }

    /// <summary>
    /// Bevételezés alapértelmezett helykód bejegyzés létrehozása
    /// </summary>
    /// <param name="whZTranHead">Bevételezés tranzakció adatok</param>
    /// <param name="whZTranLine">Tétel adatok</param>
    /// <param name="context">Készletmozgás csomag adatok</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Létrehozott helykód bejegyzés</returns>
    public async Task<OlcWhztranloc?> AddReceivingDefaultIfNotExistsAsync(OlcWhztranhead whZTranHead, OlcWhztranline whZTranLine, Containers.Interfaces.IWhZStockMapContext context, CancellationToken cancellationToken = default)
    {
        if (whZTranHead is null)
        {
            throw new ArgumentNullException(nameof(whZTranHead));
        }

        if (whZTranLine is null)
        {
            throw new ArgumentNullException(nameof(whZTranLine));
        }

        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var entity = await this.GetAsync(e => e.Whztid == whZTranHead.Whztid && e.Whztlineid == whZTranLine.Whztlineid, cancellationToken);
        if (entity is null)
        {
            var locCode = this.DefaultReceivingLocCode;
            if (string.IsNullOrWhiteSpace(locCode))
            {
                ThrowException(WhZTranLocExceptionType.DefaultReceivingLocCodeNotSet, "The default receiving location code is not setted up");
            }

            var location = await this.locationRepository.Entities.AsNoTracking().FirstOrDefaultAsync(e => e.Whloccode == locCode, cancellationToken);
            if (location is null)
            {
                ThrowException(WhZTranLocExceptionType.LocationNotFoundForCode, $"The location is not found for the given code: {locCode}");
            }

            OlsSthead? stHead = null;
            if (whZTranHead.Stid is not null)
            {
                stHead = await this.stHeadRepository.FindAsync(new object[] { whZTranHead.Stid }, cancellationToken: cancellationToken);
            }

            entity = new OlcWhztranloc
            {
                Whztid = whZTranHead.Whztid,
                Whztlineid = whZTranLine.Whztlineid,
                Whid = stHead?.Towhid!,
                Whzoneid = whZTranHead.Towhzid!.Value,
                Whlocid = location!.Whlocid,
                Whztltype = (int)WhZTranLoc_Whztltype.Receiving,
                Ordqty = whZTranLine.Ordqty,
                Dispqty = whZTranLine.Dispqty,
                Movqty = whZTranLine.Movqty,
            };

            entity = await this.AddAsync(entity, cancellationToken);

            var request = new BusinessEntities.Dto.WhZStockMapDto
            {
                Itemid = whZTranLine.Itemid,
                Whid = entity!.Whid,
                Whzoneid = entity!.Whzoneid,
                Whlocid = entity!.Whlocid,
                Qty = entity!.Movqty,
            };

            await this.whZStockMapService.AddReceivingAsync(context, request, cancellationToken);
        }

        return entity;
    }

    /// <summary>
    /// Bevételezés helykódok véglegesítése
    /// </summary>
    /// <param name="whZTranHead">Bevételezés tranzakció adatok</param>
    /// <param name="whZTranLine">Tétel adatok</param>
    /// <param name="context">Készletmozgás csomag adatok</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Véglegesített helykód bejegyzések</returns>
    public async Task<IEnumerable<OlcWhztranloc>> CommitReceivingAsync(OlcWhztranhead whZTranHead, OlcWhztranline whZTranLine, Containers.Interfaces.IWhZStockMapContext context, CancellationToken cancellationToken = default)
    {
        if (whZTranHead is null)
        {
            throw new ArgumentNullException(nameof(whZTranHead));
        }

        if (whZTranLine is null)
        {
            throw new ArgumentNullException(nameof(whZTranLine));
        }

        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var list = await this.QueryAsync(e => e.Whztid == whZTranHead.Whztid && e.Whztlineid == whZTranLine.Whztlineid, cancellationToken);
        if (list?.Any() != true)
        {
            ThrowException(WhZTranLocExceptionType.LocationsNotFoundForTranLine, $"Locations are not found for transaction line (whztlineid: {whZTranLine.Whztlineid})");
        }
        else
        {
            var ordQty = list.Sum(l => l.Ordqty);
            var dispQty = list.Sum(l => l.Dispqty);
            var movQty = list.Sum(l => l.Movqty);

            this.ValidateQuantity(whZTranLine.Ordqty, ordQty, WhZTranLocExceptionType.LocationOrdQtyValueMismatch, nameof(OlcWhztranloc.Ordqty));
            this.ValidateQuantity(whZTranLine.Dispqty, dispQty, WhZTranLocExceptionType.LocationDispQtyValueMismatch, nameof(OlcWhztranloc.Dispqty));
            this.ValidateQuantity(whZTranLine.Movqty, movQty, WhZTranLocExceptionType.LocationMovQtyValueMismatch, nameof(OlcWhztranloc.Movqty));

            OlsSthead? stHead = null;
            if (whZTranHead.Stid is not null)
            {
                stHead = await this.stHeadRepository.FindAsync(new object[] { whZTranHead.Stid }, cancellationToken: cancellationToken);
            }

            var groupped = list.GroupBy(l => l.Whlocid);
            foreach (var g in groupped)
            {
                BusinessEntities.Dto.WhZStockMapDto request = new BusinessEntities.Dto.WhZStockMapDto
                {
                    Itemid = whZTranLine.Itemid,
                    Whid = stHead?.Towhid!,
                    Whzoneid = whZTranHead.Towhzid!.Value,
                    Whlocid = g.Key,
                    Qty = g.Sum(l => l.Movqty),
                };

                await this.whZStockMapService.CommitReceivingAsync(context, request, cancellationToken);
            }

            return list.AsEnumerable();
        }

        return null!;
    }

    /// <summary>
    /// Lekérdezés, hogy az adott tétel azonosítóhoz tartozik-e helykód információ
    /// </summary>
    /// <param name="whztranlineid">Tétel azonosító</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Igen / Name</returns>
    public async Task<bool> AnyAsync(int whztranlineid, CancellationToken cancellationToken = default)
    {
        return await this.Repository.Entities.AnyAsync(l => l.Whztlineid == whztranlineid, cancellationToken);
    }

    /// <summary>
    /// Tételhez tartozó helykód információk törlése
    /// </summary>
    /// <param name="whztlineid">Tétel azonosító</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IEnumerable<WhZTranLocDto>> DeleteAllAsync(int whztlineid, CancellationToken cancellationToken = default)
    {
        using var tran = await this.UnitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var entries = await this.QueryAsync(l => l.Whztlineid == whztlineid, cancellationToken);

            var result = new List<WhZTranLocDto>();
            foreach (var entry in entries)
            {
                var entity = await this.DeleteAsync(entry, cancellationToken);
                result.Add(this.mapper.Map<WhZTranLocDto>(entity));
            }

            tran.Commit();

            return result;
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
        }
    }

    /// <summary>
    /// Paraméterben átadott mennyiség eggyezőség vizsgálata
    /// </summary>
    /// <param name="lineQty">Tétel mennyiség</param>
    /// <param name="qty">Össz helykód mennyiség</param>
    /// <param name="type">Hibaüzenet típusa</param>
    /// <param name="fieldName">Érintett mező neve</param>
    private void ValidateQuantity(decimal lineQty, decimal qty, WhZTranLocExceptionType type, string fieldName)
    {
        if (lineQty != qty)
        {
            ThrowException(type, $"The line's {fieldName} value does not equal to locations' value", fieldName: fieldName);
        }
    }

    private static void ThrowException(WhZTranLocExceptionType type, string message, OlcWhztranloc? entity = null, string? fieldName = null)
    {
        throw new WhZTranLocServiceException(type, message, entity, fieldName);
    }

    protected override async ValueTask<ValidationContext<OlcWhztranloc>> CreateValidationContextAsync(OlcWhztranloc entity, OlcWhztranloc? originalEntity, string[] ruleSets, CancellationToken cancellationToken = default)
    {
        var context = await base.CreateValidationContextAsync(entity, originalEntity, ruleSets, cancellationToken);

        var tranHead = await this.tranHeadRepository.Entities
            .Where(h => h.Whztid == entity.Whztid)
            .Include(h => h.Cmp)
            .Include(h => h.St)
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

            if (tranHead.St is not null && !context.TryAddEntity(tranHead.St))
            {
                this.ThrowException("Unable to add stock tran head to the validation context", entity);
            }
        }

        var tranLine = await this.tranLineRepository.FindAsync(new object[] { entity.Whztlineid }, cancellationToken);
        if (tranLine is not null && !context.TryAddEntity(tranLine))
        {
            this.ThrowException("Unable to add transaction line to the validation context", entity);
        }

        var whLoc = await this.locationRepository.FindAsync(new object[] { entity.Whlocid }, cancellationToken);
        if (whLoc is not null && !context.TryAddEntity(whLoc))
        {
            this.ThrowException("Unable to add location to the validation context", entity);
        }

        return context;
    }
}
