using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Base;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Validators.Interfaces;
using eLog.HeavyTools.Services.WhZone.DataAccess.Repositories.Interfaces;
using FluentValidation;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Services;

[RegisterDI(Interface = typeof(IWhZTranLocService))]
public class WhZTranLocService : LogicServiceBase<OlcWhztranloc>, IWhZTranLocService
{
    private readonly IRepository<OlcWhztranhead> tranHeadRepository;
    private readonly IRepository<OlcWhztranline> tranLineRepository;
    private readonly IRepository<OlcWhlocation> locationRepository;
    private readonly IMapper mapper;

    public WhZTranLocService(
        IOlcWhztranlocValidator validator,
        IRepository<OlcWhztranloc> repository,
        IUnitOfWork unitOfWork,
        IEnvironmentService environmentService,
        IRepository<OlcWhztranhead> tranHeadRepository,
        IRepository<OlcWhztranline> tranLineRepository,
        IRepository<OlcWhlocation> locationRepository,
        IMapper mapper) : base(validator, repository, unitOfWork, environmentService)
    {
        this.tranHeadRepository = tranHeadRepository ?? throw new ArgumentNullException(nameof(tranHeadRepository));
        this.tranLineRepository = tranLineRepository ?? throw new ArgumentNullException(nameof(tranLineRepository));
        this.locationRepository = locationRepository ?? throw new ArgumentNullException(nameof(locationRepository));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    protected override async ValueTask<ValidationContext<OlcWhztranloc>> CreateValidationContextAsync(OlcWhztranloc entity, OlcWhztranloc? originalEntity, string[] ruleSets, CancellationToken cancellationToken = default)
    {
        var context = await base.CreateValidationContextAsync(entity, originalEntity, ruleSets, cancellationToken);

        var tranHead = await this.tranHeadRepository.Entities
            .Where(h => h.Whztid == entity.Whztid)
            .Include(h => h.Cmpid)
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
