using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Enums;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Enums;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Validators.Base;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Validators.Interfaces;
using eLog.HeavyTools.Services.WhZone.DataAccess.Context;
using FluentValidation;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Validators;

[RegisterDI(Interface = typeof(IOlcWhztranlocValidator))]
public class OlcWhzstocklocValidator : EntityValidator<OlcWhztranloc>, IOlcWhztranlocValidator
{
    public OlcWhzstocklocValidator(WhZoneDbContext dbContext) : base(dbContext)
    {
    }

    protected override void AddDefaultRules()
    {
        base.AddDefaultRules();

        this.RuleFor(loc => loc.Whztid).NotEmpty();
        this.RuleFor(loc => loc.Whztlineid).NotEmpty();
        this.RuleFor(loc => loc.Whid).NotEmpty();
        this.RuleFor(loc => loc.Whzoneid).NotEmpty();
        this.RuleFor(loc => loc.Whlocid).NotEmpty();
        this.RuleFor(loc => loc.Whztltype).NotEmpty();

        this.RuleFor(loc => loc.Ordqty).NotNull();
        this.RuleFor(loc => loc.Dispqty).NotNull();
        this.RuleFor(loc => loc.Movqty).NotNull();
    }

    protected override void AddAddRules()
    {
        base.AddAddRules();

        this.RuleFor(loc => loc.Whztid)
            .Custom((newValue, context) =>
            {
                var tranHead = context.TryGetEntity<OlcWhztranloc, OlcWhztranhead>();
                if (tranHead is null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranloc.Whztid), $"The transaction doesn't exists (transaction: {newValue})"));
                }
                else if (tranHead.Whztstat >= (int)WhZTranHead_Whztstat.Closed)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranloc.Whztid), $"Unable to add a location to a closed transaction (transaction: {newValue})"));
                }
            });

        this.RuleFor(loc => loc.Whztlineid)
            .Custom((newValue, context) =>
            {
                var tranHead = context.TryGetEntity<OlcWhztranloc, OlcWhztranhead>();
                var tranLine = context.TryGetEntity<OlcWhztranloc, OlcWhztranline>();
                if (tranLine is null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranloc.Whztlineid), $"The transaction line doesn't exists (transaction line: {newValue})"));
                }
                else if (tranLine.Whztid != tranHead?.Whztid)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranloc.Whztlineid), $"The transaction line doesn't belongs to the transaction (transaction line: {newValue}, transaction: {tranHead?.Whztid})"));
                }
            });

        this.RuleFor(loc => loc.Whid)
            .Custom((newValue, context) =>
            {
                var loc = context.InstanceToValidate;
                var tranHead = context.TryGetEntity<OlcWhztranloc, OlcWhztranhead>();
                var stHead = context.TryGetEntity<OlcWhztranloc, OlsSthead>();
                if (tranHead is null)
                {
                    context.AddFailure(x => x.Whid, $"The transaction doesn't exists (transaction: {newValue})");
                }
                else if (tranHead.Whzttype == (int)WhZTranHead_Whzttype.Receiving)
                {
                    if (stHead is null && tranHead.Stid is not null)
                    {
                        context.AddFailure(x => x.Whid, $"The stock transaction doesn't exists (stock transaction: {tranHead.Stid})");
                    }
                    else if (stHead is not null && !string.Equals(stHead.Towhid, newValue, StringComparison.InvariantCultureIgnoreCase))
                    {
                        context.AddFailure(x => x.Whid, $"The warehouse doesn't same as the stock tran destination warehouse (destination warehouse: {stHead.Towhid}, warehouse: {newValue})");
                    }
                }
                else
                {
                    context.AddFailure(x => x.Whid, "not implemented");
                }
            });

        this.RuleFor(loc => loc.Whzoneid)
            .Custom((newValue, context) =>
            {
                var loc = context.InstanceToValidate;
                var tranHead = context.TryGetEntity<OlcWhztranloc, OlcWhztranhead>();
                if (tranHead is null)
                {
                    context.AddFailure(x => x.Whid, $"The transaction doesn't exists (transaction: {newValue})");
                }
                else if (tranHead.Whzttype == (int)WhZTranHead_Whzttype.Receiving && tranHead.Towhzid != newValue)
                {
                    context.AddFailure(x => x.Whid, $"The zone doesn't same as the tranzaction destination zone (destination zone: {tranHead.Towhzid}, zone: {newValue})");
                }
                else if (tranHead.Whzttype != (int)WhZTranHead_Whzttype.Receiving)
                {
                    context.AddFailure(x => x.Whid, "not implemented");
                }
            });

        this.RuleFor(loc => loc.Whlocid)
            .Custom((newValue, context) =>
            {
                var location = context.TryGetEntity<OlcWhztranloc, OlcWhlocation>();
                if (location is null)
                {
                    context.AddFailure(x => x.Whlocid, $"The location doesn't exists (location: {newValue})");
                }
                else
                {
                    var loc = context.InstanceToValidate;
                    var tranHead = context.TryGetEntity<OlcWhztranloc, OlcWhztranhead>();
                    var stHead = context.TryGetEntity<OlcWhztranloc, OlsSthead>();
                    if (tranHead is null)
                    {
                        context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranloc.Whlocid), $"The transaction doesn't exists (transaction: {loc.Whztid})"));
                    }
                    else if (tranHead.Whzttype == (int)WhZTranHead_Whzttype.Receiving)
                    {
                        if (stHead is null && tranHead.Stid is not null)
                        {
                            context.AddFailure(x => x.Whlocid, $"The stock transaction doesn't exists (stock transaction: {tranHead.Stid})");
                        }
                        else if (stHead is not null && !string.Equals(stHead.Towhid, location?.Whid, StringComparison.InvariantCultureIgnoreCase))
                        {
                            context.AddFailure(x => x.Whlocid, $"The location doesn't belongs to the stock tran destination warehouse (destination warehouse: {stHead.Towhid}, location: {location?.Whid})");
                        }

                        if (tranHead.Towhzid != location?.Whzoneid)
                        {
                            context.AddFailure(x => x.Whlocid, $"The location doesn't belongs to the transaction destination zone (destination zone: {tranHead.Towhzid}, location: {location?.Whzoneid})");
                        }
                    }
                    else
                    {
                        context.AddFailure(x => x.Whlocid, "not implemented");
                    }
                }
            });
    }
}
