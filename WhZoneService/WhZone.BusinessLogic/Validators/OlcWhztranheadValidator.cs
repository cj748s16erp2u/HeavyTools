using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Enums;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Validators.Base;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Validators.Interfaces;
using eLog.HeavyTools.Services.WhZone.DataAccess.Context;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Validators;

[RegisterDI(Interface = typeof(IOlcWhztranheadValidator))]
public class OlcWhztranheadValidator : EntityValidator<OlcWhztranhead>, IOlcWhztranheadValidator
{
    public OlcWhztranheadValidator(WhZoneDbContext dbContext) : base(dbContext)
    {
    }

    protected override void AddDefaultRules()
    {
        base.AddDefaultRules();

        this.RuleFor(head => head.Cmpid).NotEmpty();

        this.RuleFor(head => head.Whzttype).NotEmpty();
        this.RuleFor(head => head.Whztdate).NotEmpty();

        this.RuleFor(head => head.Fromwhzid).Empty()
            .When(head => head.Whzttype == (int)WhZTranHead_Whzttype.Receiving);
        this.RuleFor(head => head.Fromwhzid).NotEmpty()
            .When(head => head.Whzttype == (int)WhZTranHead_Whzttype.Issuing || head.Whzttype == (int)WhZTranHead_Whzttype.Transfering);
        this.RuleFor(head => head.Towhzid).Empty()
            .When(head => head.Whzttype == (int)WhZTranHead_Whzttype.Issuing);
        this.RuleFor(head => head.Towhzid).NotEmpty()
            .When(head => head.Whzttype == (int)WhZTranHead_Whzttype.Receiving || head.Whzttype == (int)WhZTranHead_Whzttype.Transfering);

        this.RuleFor(head => head.Closeusrid).Empty()
            .When(head => head.Whztstat < (int)WhZTranHead_Whztstat.Closed);
        this.RuleFor(head => head.Closeusrid).NotEmpty()
            .When(head => head.Whztstat >= (int)WhZTranHead_Whztstat.Closed);

        this.RuleFor(head => head.Closedate).Empty()
            .When(head => head.Whztstat < (int)WhZTranHead_Whztstat.Closed);
        this.RuleFor(head => head.Closedate).NotEmpty()
            .When(head => head.Whztstat >= (int)WhZTranHead_Whztstat.Closed);

        this.RuleFor(head => head.Whztstat).NotEmpty();

        this.RuleFor(head => head.Stid).NotEmpty()
            .When(head => head.Whzttype == (int)WhZTranHead_Whzttype.Receiving);
        this.RuleFor(head => head.Stid).Empty()
            .When(head => head.Whzttype == (int)WhZTranHead_Whzttype.Issuing);

        this.RuleFor(head => head.Sordid).Empty()
            .When(head => head.Whzttype == (int)WhZTranHead_Whzttype.Receiving);
        this.RuleFor(head => head.Pordid).Empty()
            .When(head => head.Whzttype == (int)WhZTranHead_Whzttype.Receiving);
        this.RuleFor(head => head.Taskid).Empty()
            .When(head => head.Whzttype == (int)WhZTranHead_Whzttype.Receiving);

        this.RuleFor(head => head.Gen).NotEmpty();
    }

    protected override void AddAddRules()
    {
        base.AddAddRules();

        this.RuleFor(head => head.Cmpid)
            .Custom((newValue, context) =>
            {
                var company = context.TryGetEntity<OlcWhztranhead, OlsCompany>();
                if (company is null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranhead.Cmpid), $"The company doesn't exists (company: {newValue})"));
                }
                else if (company.Delstat != 0)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranhead.Cmpid), $"The company is hidden (company: {newValue})"));
                }
            });

        this.RuleFor(head => head.Fromwhzid)
            .Custom((newValue, context) =>
            {
                var zone = context.TryGetEntity<OlcWhztranhead, OlcWhzone>("fromwhzone");
                if (zone is null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranhead.Fromwhzid), $"The source zone doesn't exists (zone: {newValue})"));
                }
                else if (zone.Delstat != 0)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranhead.Fromwhzid), $"The source zone is hidden (zone: {newValue})"));
                }

                var warehouse = context.TryGetEntity<OlcWhztranhead, OlsWarehouse>("fromwh");
                if (warehouse is null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranhead.Fromwhzid), $"The source warehouse doesn't exists (zone: {newValue})"));
                }
                else if (warehouse.Delstat != 0)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranhead.Fromwhzid), $"The source warehouse is hidden (zone: {newValue})"));
                }
                else
                {
                    var company = context.TryGetEntity<OlcWhztranhead, OlsCompany>();
                    if (!CompanyUtils.CmpCodesContains(warehouse.Cmpcodes, company?.Cmpcode))
                    {
                        context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranhead.Fromwhzid), $"The source warehouse doesn't valid in the given company (zone: {newValue}, company: {company?.Cmpid})"));
                    }
                }
            })
            .When(head => head.Whzttype == (int)WhZTranHead_Whzttype.Issuing || head.Whzttype == (int)WhZTranHead_Whzttype.Transfering);

        this.RuleFor(head => head.Towhzid)
            .Custom((newValue, context) =>
            {
                var zone = context.TryGetEntity<OlcWhztranhead, OlcWhzone>("towhzone");
                if (zone is null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranhead.Towhzid), $"The destination zone doesn't exists (zone: {newValue})"));
                }
                else if (zone.Delstat != 0)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranhead.Towhzid), $"The destination zone is hidden (zone: {newValue})"));
                }

                var warehouse = context.TryGetEntity<OlcWhztranhead, OlsWarehouse>("towh");
                if (warehouse is null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranhead.Towhzid), $"The destination warehouse doesn't exists (zone: {newValue})"));
                }
                else if (warehouse.Delstat != 0)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranhead.Towhzid), $"The destination warehouse is hidden (zone: {newValue})"));
                }
                else
                {
                    var company = context.TryGetEntity<OlcWhztranhead, OlsCompany>();
                    if (!CompanyUtils.CmpCodesContains(warehouse.Cmpcodes, company?.Cmpcode))
                    {
                        context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranhead.Towhzid), $"The destination warehouse doesn't valid in the given company (zone: {newValue}, company: {company?.Cmpid})"));
                    }
                }
            })
            .When(head => head.Whzttype == (int)WhZTranHead_Whzttype.Receiving || head.Whzttype == (int)WhZTranHead_Whzttype.Transfering);

        this.RuleFor(head => head.Stid)
            .CustomAsync(async (newValue, context, cancellationToken) =>
            {
                if (newValue is not null)
                {
                    var head = context.InstanceToValidate;
                    var stHead = context.TryGetEntity<OlcWhztranhead, OlsSthead>();
                    if (stHead is null)
                    {
                        context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranhead.Stid), $"The stock tran doesn't exists (stock tran: {newValue})"));
                    }
                    else
                    {
                        var exists = await this.dbContext.OlcWhztranheads
                            .Where(h => h.Stid == newValue)
                            .AnyAsync(cancellationToken);
                        if (exists)
                        {
                            context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranhead.Stid), $"A transaction is already exists for the given stock tran (stock tran: {newValue})"));
                        }

                        if (head.Cmpid != stHead.Cmpid)
                        {
                            context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranhead.Cmpid), $"The company must be same as stock tran company (stock tran: {stHead?.Cmpid}, transaction: {head.Cmpid})"));
                        }

                        var zone = context.TryGetEntity<OlcWhztranhead, OlcWhzone>("towhzone");
                        if (!string.Equals(stHead!.Towhid, zone?.Whid, StringComparison.InvariantCultureIgnoreCase))
                        {
                            context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranhead.Towhzid), $"The destination zone's warehouse must be same as stock tran destination warehouse (stock tran: {stHead.Towhid}, zone: {zone?.Whid})"));
                        }

                        if (head.Whztdate != stHead.Stdate)
                        {
                            context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranhead.Whztdate), $"The transaction date must be the same as stock tran date (stock tran: {stHead.Stdate:yyyy.MM.dd}, transaction: {head.Whztdate:yyyy.MM.dd})"));
                        }
                    }
                }
            })
            .When(head => head.Whzttype == (int)WhZTranHead_Whzttype.Receiving || head.Whzttype == (int)WhZTranHead_Whzttype.Transfering);
    }

    protected override void AddUpdateRules()
    {
        base.AddUpdateRules();

        this.RuleFor(head => head.Cmpid).ReadOnly(head => head.Cmpid);
        this.RuleFor(head => head.Fromwhzid).ReadOnly(head => head.Fromwhzid);

        //this.RuleFor(head => head.Towhzid).ReadOnly(head => head.Towhzid);

        this.RuleFor(head => head.Stid).ReadOnly(head => head.Stid);
        this.RuleFor(head => head.Sordid).ReadOnly(head => head.Sordid);
        this.RuleFor(head => head.Pordid).ReadOnly(head => head.Pordid);
        this.RuleFor(head => head.Whzttype).ReadOnly(head => head.Whzttype);

        this.RuleFor(head => head.Whztdate).ReadOnly(head => head.Whztdate)
            .When(head => head.Whztstat >= (int)WhZTranHead_Whztstat.Closed);

        this.RuleFor(head => head.Whztdate)
            .Custom((newValue, context) =>
            {
                var head = context.InstanceToValidate;
                if (head.Stid is not null)
                {
                    var stHead = context.TryGetEntity<OlcWhztranhead, OlsSthead>();
                    if (stHead?.Stdate != newValue)
                    {
                        context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranhead.Whztdate), $"The transaction date must be the same as stock tran date (stock tran: {stHead?.Stdate:yyyy.MM.dd}, transaction: {newValue:yyyy.MM.dd})"));
                    }
                }
            })
            .When(head => head.Whzttype == (int)WhZTranHead_Whzttype.Receiving);
    }

    protected override void AddDeleteRules()
    {
        base.AddDeleteRules();

        this.RuleFor(head => head.Whztstat)
            .Must(newValue => newValue < (int)WhZTranHead_Whztstat.Closed)
            .WithMessage(e => $"Unable to delete a closed transaction (transaction: {e.Whztid})");
    }
}
