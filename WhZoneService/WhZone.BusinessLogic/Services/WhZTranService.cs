using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
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
using NLog.Targets;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Services;

[RegisterDI(Interface = typeof(IWhZTranService))]
public class WhZTranService : LogicServiceBase<OlcWhztranhead>, IWhZTranService
{
    private readonly IWhZoneService whZoneService;
    private readonly IRepository<OlsSthead> stHeadRepository;
    private readonly IRepository<OlsCompany> companyRepository;
    private readonly IMapper mapper;

    public WhZTranService(
        IOlcWhztranheadValidator validator,
        IRepository<OlcWhztranhead> repository,
        IUnitOfWork unitOfWork,
        IEnvironmentService environmentService,
        IWhZoneService whZoneService,
        IRepository<OlsSthead> stHeadRepository,
        IRepository<OlsCompany> companyRepository,
        IMapper mapper) : base(validator, repository, unitOfWork, environmentService)
    {
        this.whZoneService = whZoneService ?? throw new ArgumentNullException(nameof(whZoneService));
        this.stHeadRepository = stHeadRepository ?? throw new ArgumentNullException(nameof(stHeadRepository));
        this.companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Bevételezés típusú tranzakciók lekérdezése
    /// </summary>
    /// <param name="predicate">Szűrési feltétel</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Tranzakciók listája</returns>
    public async Task<IEnumerable<WhZReceivingTranHeadDto>> QueryReceivingAsync(WhZTranHeadQueryDto query = null!, CancellationToken cancellationToken = default)
    {
        Expression<Func<OlcWhztranhead, bool>> predicate = null!;
        if (query is not null)
        {
            predicate = this.CreatePredicate(query);
        }

        var q = this.Query(predicate);
        q = q.Where(h => h.Whzttype == (int)WhZTranHead_Whzttype.Receiving);

        var list = await q.ToListAsync(cancellationToken);
        return this.mapper.Map<IEnumerable<WhZReceivingTranHeadDto>>(list);
    }

    /// <summary>
    /// Bevételezés típusú tranzakció rögzítése
    /// </summary>
    /// <param name="request">Tranzakció adatok</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Rögzített tranzakció</returns>
    public async Task<WhZReceivingTranHeadDto> AddReceivingAsync(WhZReceivingTranHeadDto request, CancellationToken cancellationToken = default)
    {
        this.ValidateAddReceivingParameters(request);

        var entity = this.MapAddDtoToEntity(request);

        this.EnvironmentService.CustomData.TryAdd("AuthUser", request.AuthUser);
        using var tran = await this.UnitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            entity = await this.AddAsync(entity, cancellationToken);

            tran.Commit();

            return this.mapper.Map<WhZReceivingTranHeadDto>(entity);
        }
        catch (Exception ex)
        {
            await ERP2U.Log.LoggerManager.Instance.LogErrorAsync<WhZTranService>(ex);
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
    /// Bevételezés típusú tranzakció módosítása
    /// </summary>
    /// <param name="request">Tranzakció adatok</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Módosított tranzakció</returns>
    public async Task<WhZReceivingTranHeadDto> UpdateReceivingAsync(WhZReceivingTranHeadDto request, CancellationToken cancellationToken = default)
    {
        this.ValidateUpdateReceivingParameters(request);

        var originalEntity = await this.LoadEntityAsync(request, cancellationToken);
        if (originalEntity is null)
        {
            this.ThrowException(WhZTranExceptionType.EntryNotFound, $"The referenced transaction is not found (whztid: {request.Whztid}, stid: {request.Stid})");
        }

        var entity = this.MapUpdateDtoToEntity(request, originalEntity!);

        this.EnvironmentService.CustomData.TryAdd("AuthUser", request.AuthUser);
        using var tran = await this.UnitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            entity = await this.UpdateAsync(entity, cancellationToken);

            tran.Commit();

            return this.mapper.Map<WhZReceivingTranHeadDto>(entity);
        }
        catch (Exception ex)
        {
            await ERP2U.Log.LoggerManager.Instance.LogErrorAsync<WhZTranService>(ex);
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

    public async Task<WhZTranHeadStatChangeResultDto> StatChangeAsync(WhZTranHeadStatChangeDto request, CancellationToken cancellationToken = default)
    {
        return new WhZTranHeadStatChangeResultDto
        {
            Result = -1,
            Message = new NotImplementedException().Message
        };
    }

    public async Task<WhZTranHeadCloseResultDto> CloseAsync(WhZTranHeadCloseDto request, CancellationToken cancellationToken = default)
    {
        return new WhZTranHeadCloseResultDto
        {
            Result = -1,
            Message = new NotImplementedException().Message
        };
    }

    /// <summary>
    /// Bejegyzés betöltése azonosító vagy raktári tranzakció azonosító alapján
    /// </summary>
    /// <param name="request">Tranzakció adatok</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Betöltött bejegyzés, ha nincs találat, akkor null</returns>
    private async Task<OlcWhztranhead?> LoadEntityAsync(WhZReceivingTranHeadDto request, CancellationToken cancellationToken)
    {
        return request.Whztid is not null && request.Whztid != 0
            ? await this.GetByIdAsync(new object[] { request.Whztid }, cancellationToken)
            : request.Stid is not null && request.Stid != 0
                ? await this.GetAsync(e => e.Stid == request.Stid, cancellationToken)
                : null;
    }

    /// <summary>
    /// <see cref="WhZReceivingTranHeadDto"/> request-ből <see cref="OlcWhztranhead"/> entity létrehozása
    /// </summary>
    /// <param name="request">A rögzítendő tranzakció adatok</param>
    /// <returns>Az rögzítendő entity</returns>
    private OlcWhztranhead MapAddDtoToEntity(WhZReceivingTranHeadDto request)
    {
        var entity = this.mapper.Map<OlcWhztranhead>(request);
        entity.Whztstat = (int)WhZTranHead_Whztstat.Created;
        entity.Gen = (int)OlcWhZTranHead_Gen.Normal;

        entity.Whztdate = entity.Whztdate.Date;

        return entity;
    }

    /// <summary>
    /// <see cref="WhZReceivingTranHeadDto"/> request-ből <see cref="OlcWhztranhead"/> entity létrehozása
    /// </summary>
    /// <param name="request">A módosítandó tranzakció adatok</param>
    /// <param name="originalEntity">A tárolt tranzakció</param>
    /// <returns>Az módosítandó entity</returns>
    private OlcWhztranhead MapUpdateDtoToEntity(WhZReceivingTranHeadDto request, OlcWhztranhead originalEntity)
    {
        var entity = this.mapper.Map<OlcWhztranhead>(request);
        entity.Gen = originalEntity.Gen;
        entity.Addusrid = originalEntity.Addusrid;
        entity.Adddate = originalEntity.Adddate;

        entity.Whztdate = entity.Whztdate.Date;

        if (request.Whztid is null || request.Whztid == 0)
        {
            entity.Whztid = originalEntity.Whztid;
        }

        if (request.Whztstat is null || request.Whztstat == 0)
        {
            entity.Whztstat = originalEntity.Whztstat;
        }

        return entity;
    }

    /// <summary>
    /// Bevételezés típusú tranzakció adatok validálása
    /// </summary>
    /// <param name="request">Tranzakció adatok</param>
    private void ValidateAddReceivingParameters(WhZReceivingTranHeadDto request)
    {
        this.ValidateReceivingBaseParameters(request);
    }

    /// <summary>
    /// Bevételezés típusú tranzakció adatok validálása
    /// </summary>
    /// <param name="request">Tranzakció adatok</param>
    private void ValidateUpdateReceivingParameters(WhZReceivingTranHeadDto request)
    {
        this.ValidateReceivingBaseParameters(request);

        // Nem lehet validálni, mert az ERP nem minden esetben fogja tudni adni ezt az adatot
        //if (request.Whztid is null)
        //{
        //    this.ThrowException(WhZTranExceptionType.InvalidWhztid, "The transaction identifier is not set", fieldName: nameof(WhZReceivingTranHeadDto.Whztid));
        //}
    }

    /// <summary>
    /// Alap bevételezés request validálások
    /// </summary>
    /// <param name="request">Tranzakció adatok</param>
    private void ValidateReceivingBaseParameters(WhZReceivingTranHeadDto request)
    {
        this.ValidateBaseParameters(request);

        if (request.Whzttype != WhZTranHead_Whzttype.Receiving)
        {
            this.ThrowException(WhZTranExceptionType.InvalidWhzttype, "The request's transaction type is invalid", fieldName: nameof(WhZReceivingTranHeadDto.Whzttype));
        }

        if (request.Stid is null)
        {
            this.ThrowException(WhZTranExceptionType.InvalidStid, "The source stock tran identifier is not set", fieldName: nameof(WhZReceivingTranHeadDto.Stid));
        }

        if (request.Towhzid is null)
        {
            this.ThrowException(WhZTranExceptionType.InvalidTowhzid, "The destination zone identifier is not set", fieldName: nameof(WhZReceivingTranHeadDto.Towhzid));
        }
    }

    /// <summary>
    /// Alap request validalasok
    /// </summary>
    /// <param name="request">Tranzakció adatok</param>
    /// <exception cref="ArgumentNullException"><paramref name="request"/> értéke nincs megadva</exception>
    private void ValidateBaseParameters(WhZReceivingTranHeadDto request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.Cmpid == 0)
        {
            this.ThrowException(WhZTranExceptionType.InvalidCmpid, "The company identifier is not valid", fieldName: nameof(WhZReceivingTranHeadDto.Cmpid));
        }

        if (string.IsNullOrWhiteSpace(request.AuthUser))
        {
            this.ThrowException(WhZTranExceptionType.InvalidAuthUser, "The 'AuthUser' must be set", fieldName: nameof(WhZReceivingTranHeadDto.AuthUser));
        }
    }

    /// <summary>
    /// Új felvitel esetén, a <see cref="BusinessEntities.Model.Base.BusinessEntity.Addusrid"/> meghatározása a request-ben kapott <see cref="WhZTranHeadDto.AuthUser"/> alapján
    /// </summary>
    /// <param name="entity">Mentendő bejegyzés</param>
    protected override async Task FillSystemFieldsOnAddAsync(OlcWhztranhead entity, CancellationToken cancellationToken = default)
    {
        await base.FillSystemFieldsOnAddAsync(entity, cancellationToken);

        if (this.EnvironmentService.CustomData.TryGetValue("AuthUser", out var s) && s is string authUser)
        {
            entity.Addusrid = authUser;
        }
    }

    /// <summary>
    /// Exception kiváltása
    /// </summary>
    /// <param name="type">Hiba típusa</param>
    /// <param name="message">Hibaüzenet</param>
    /// <param name="entity">A feldolgozott entity</param>
    /// <param name="fieldName">A hiba mely mezőn érintett</param>
    /// <exception cref="WhZTranServiceException">A kiváltott Exception</exception>
    private void ThrowException(WhZTranExceptionType type, string message, OlcWhztranhead? entity = null, string? fieldName = null)
    {
        throw new WhZTranServiceException(type, message, entity, fieldName);
    }

    /// <summary>
    /// Validálásnál használt adattároló feltöltése
    /// új adat: vállalat, forrás zóna, cél zóna, raktári tranzakció, forrás raktár, cél raktár
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="originalEntity"></param>
    /// <param name="ruleSets"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected override async ValueTask<ValidationContext<OlcWhztranhead>> CreateValidationContextAsync(OlcWhztranhead entity, OlcWhztranhead? originalEntity, string[] ruleSets, CancellationToken cancellationToken = default)
    {
        var context = await base.CreateValidationContextAsync(entity, originalEntity, ruleSets, cancellationToken);

        var company = await this.companyRepository.FindAsync(new object[] { entity.Cmpid }, cancellationToken);
        if (company is not null && !context.TryAddEntity(company))
        {
            this.ThrowException("Unable to add company to the validation context", entity);
        }

        if (entity.Fromwhzid is not null)
        {
            var zone = await this.whZoneService.GetByIdAsync(new object[] { entity.Fromwhzid }, cancellationToken);
            if (zone is not null && !context.TryAddEntity(zone, "fromwhzone"))
            {
                this.ThrowException("Unable to add source zone to the validation context", entity);
            }
        }

        if (entity.Towhzid is not null)
        {
            var zone = await this.whZoneService.GetByIdAsync(new object[] { entity.Towhzid }, cancellationToken);
            if (zone is not null && !context.TryAddEntity(zone, "towhzone"))
            {
                this.ThrowException("Unable to add destination zone to the validation context", entity);
            }
        }

        if (entity.Stid is not null)
        {
            var stHead = await this.stHeadRepository.Entities
                .Where(h => h.Stid == entity.Stid)
                .Include(h => h.Fromwh)
                .Include(h => h.Towh)
                .FirstOrDefaultAsync(cancellationToken);

            if (stHead is not null)
            {
                if (!context.TryAddEntity(stHead))
                {
                    this.ThrowException("Unable to add stock transaction to the validation context", entity);
                }

                if (stHead.Fromwh is not null && !context.TryAddEntity(stHead.Fromwh, "fromwh"))
                {
                    this.ThrowException("Unable to add source warehouse to the validation context", entity);
                }

                if (stHead.Towh is not null && !context.TryAddEntity(stHead.Towh, "towh"))
                {
                    this.ThrowException("Unable to add destination warehouse to the validation context", entity);
                }
            }
        }

        return context;
    }
}
