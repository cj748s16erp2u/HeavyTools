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
using Microsoft.Extensions.Logging;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Validators;

[RegisterDI(Interface = typeof(IOlcWhztranlineValidator))]
public class OlcWhztranlineValidator : EntityValidator<OlcWhztranline>, IOlcWhztranlineValidator
{
    public OlcWhztranlineValidator(WhZoneDbContext dbContext) : base(dbContext)
    {
    }

    protected override void AddDefaultRules()
    {
        base.AddDefaultRules();

        this.RuleFor(line => line.Whztid).NotEmpty();

        this.RuleFor(line => line.Linenum).NotEmpty();
        this.RuleFor(line => line.Itemid).NotEmpty();

        this.RuleFor(line => line.Ordqty).NotNull();
        this.RuleFor(line => line.Dispqty).NotNull();
        this.RuleFor(line => line.Movqty).NotNull();
        this.RuleFor(line => line.Inqty).NotNull();
        this.RuleFor(line => line.Outqty).NotNull();

        this.RuleFor(line => line.Unitid2).NotEmpty();
        this.RuleFor(line => line.Change).NotNull();
        this.RuleFor(line => line.Ordqty2).NotNull();
        this.RuleFor(line => line.Dispqty2).NotNull();
        this.RuleFor(line => line.Movqty2).NotNull();

        this.RuleFor(line => line.Gen).NotEmpty();
    }

    protected override void AddAddRules()
    {
        base.AddAddRules();

        this.RuleFor(line => line.Whztid)
            .Custom((newValue, context) =>
            {
                var tranHead = context.TryGetEntity<OlcWhztranline, OlcWhztranhead>();
                if (tranHead is null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Whztid), $"The transaction doesn't exists (transaction: {newValue})"));
                }
                else if (tranHead.Whztstat >= (int)WhZTranHead_Whztstat.Closed)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Whztid), $"Unable to add a line to a closed transaction (transaction: {newValue})"));
                }
            });

        this.RuleFor(line => line.Itemid)
            .Custom((newValue, context) =>
            {
                var item = context.TryGetEntity<OlcWhztranline, OlsItem>();
                if (item is null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Itemid), $"The item doesn't exists (item: {newValue})"));
                }
                else if (item.Delstat != 0)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Itemid), $"The item is hidden (item: {newValue})"));
                }
                else
                {
                    var company = context.TryGetEntity<OlcWhztranline, OlsCompany>();
                    if (!CompanyUtils.CmpCodesContains(item.Cmpcodes, company?.Cmpcode))
                    {
                        context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Itemid), $"The item doesn't valid in the given company (item: {newValue}, company: {company?.Cmpid})"));
                    }
                }
            });

        this.RuleFor(line => line.Stlineid)
            .CustomAsync(async (newValue, context, cancellationToken) =>
            {
                var tranHead = context.TryGetEntity<OlcWhztranline, OlcWhztranhead>();
                if (tranHead is null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Whztid), $"The zone tran can't be found"));
                }
                else if (tranHead!.Whzttype == (int)WhZTranHead_Whzttype.Receiving)
                {
                    if (newValue == null)
                    {
                        context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Stlineid), $"'Stlineid' must not be empty."));
                    }
                    else
                    {
                        var exists = await this.dbContext.OlcWhztranlines
                            .Where(l => l.Stlineid == newValue)
                            .AnyAsync(cancellationToken);
                        if (exists)
                        {
                            context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Stlineid), $"A transaction line is already exists for the given stock tran line (stock tran line: {newValue})"));
                        }

                        var stLineStId = await this.dbContext.OlsStlines
                            .Where(stLine => stLine.Stlineid == newValue)
                            .Select(stLine => (int?)stLine.Stid)
                            .FirstOrDefaultAsync(cancellationToken);
                        if (stLineStId is null)
                        {
                            context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Stlineid), $"The stock tran line doesn't exists (stock tran line: {newValue})"));
                        }
                        else if (stLineStId != tranHead.Stid)
                        {
                            context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Stlineid), $"The stock tran line doesn't belongs to the references stock tran (stock tran: {tranHead.Stid}, stock tran line: {newValue})"));
                        }
                    }
                }
                else if (newValue != null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Stlineid), $"The stock tran line can't be set (stock tran line: {newValue})"));
                }
            });

        this.RuleFor(line => line.Sordlineid)
            .Custom((newValue, context) =>
            {
                var tranHead = context.TryGetEntity<OlcWhztranline, OlcWhztranhead>();
                if (tranHead is null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Whztid), $"The zone tran can't be found"));
                }
                else if (tranHead!.Whzttype == (int)WhZTranHead_Whzttype.Receiving && newValue is not null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Sordlineid), $"The sales order line can't be set (sales order line: {newValue})"));
                }
                else
                {
                    //context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Sordlineid), $"The sales order line can't be set (sales order line: {newValue})"));
                }
            });

        this.RuleFor(line => line.Pordlineid)
            .Custom((newValue, context) =>
            {
                var tranHead = context.TryGetEntity<OlcWhztranline, OlcWhztranhead>();
                if (tranHead is null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Whztid), $"The zone tran can't be found"));
                }
                else if (tranHead!.Whzttype == (int)WhZTranHead_Whzttype.Receiving && newValue is not null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Pordlineid), $"The purchase order line can't be set (purchase order line: {newValue})"));
                }
                else
                {
                    //context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Pordlineid), $"The purchase order line can't be set (purchase order line: {newValue})"));
                }
            });

        this.AddStLineEqualRules();
    }

    protected override void AddUpdateRules()
    {
        base.AddUpdateRules();

        this.RuleFor(line => line.Whztid).ReadOnly(line => line.Whztid);
        this.RuleFor(line => line.Itemid).ReadOnly(line => line.Itemid);
        this.RuleFor(line => line.Stlineid).ReadOnly(line => line.Stlineid);

        this.AddStLineEqualRules();
    }

    protected override void AddDeleteRules()
    {
        base.AddDeleteRules();

        this.RuleFor(line => line.Whztlineid)
            .Custom((newValue, context) =>
            {
                var tranHead = context.TryGetEntity<OlcWhztranline, OlcWhztranhead>();
                if (tranHead is null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Whztid), $"The zone tran can't be found"));
                }
                else if (tranHead!.Whztstat >= (int)WhZTranHead_Whztstat.Closed)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Whztlineid), $"Unable to delete a closed transaction's line (transaction: {tranHead.Whztid}, line: {newValue})"));
                }
            });
    }

    protected void AddStLineEqualRules()
    {
        this.RuleFor(line => line.Linenum)
            .Custom((newValue, context) =>
            {
                var line = context.InstanceToValidate;
                var stLine = context.TryGetEntity<OlcWhztranline, OlsStline>();
                if (stLine is null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Stlineid), $"The stock tran line can't be found"));
                }
                else if (stLine.Linenum != newValue)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Linenum), $"The transaction line's linenum must be the same as stock tran line's linenum (stock tran: {stLine.Linenum}, transaction: {newValue})"));
                }
            });

        this.RuleFor(line => line.Itemid)
            .Custom((newValue, context) =>
            {
                var line = context.InstanceToValidate;
                var stLine = context.TryGetEntity<OlcWhztranline, OlsStline>();
                if (stLine is null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Stlineid), $"The stock tran line can't be found"));
                }
                else if (stLine.Itemid != newValue)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Itemid), $"The transaction line's itemid must be the same as stock tran line's itemid (stock tran: {stLine.Itemid}, transaction: {newValue})"));
                }
            });

        this.RuleFor(line => line.Ordqty)
            .Custom((newValue, context) =>
            {
                var line = context.InstanceToValidate;
                var stLine = context.TryGetEntity<OlcWhztranline, OlsStline>();
                if (stLine is null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Stlineid), $"The stock tran line can't be found"));
                }
                else if (stLine.Ordqty != newValue)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Ordqty), $"The transaction line's ordqty must be the same as stock tran line's ordqty (stock tran: {stLine.Ordqty}, transaction: {newValue})"));
                }
            });

        this.RuleFor(line => line.Dispqty)
            .Custom((newValue, context) =>
            {
                var line = context.InstanceToValidate;
                var stLine = context.TryGetEntity<OlcWhztranline, OlsStline>();
                if (stLine is null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Stlineid), $"The stock tran line can't be found"));
                }
                else if (stLine.Dispqty != newValue)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Dispqty), $"The transaction line's dispqty must be the same as stock tran line's dispqty (stock tran: {stLine.Dispqty}, transaction: {newValue})"));
                }
            });

        this.RuleFor(line => line.Movqty)
            .Custom((newValue, context) =>
            {
                var line = context.InstanceToValidate;
                var stLine = context.TryGetEntity<OlcWhztranline, OlsStline>();
                if (stLine is null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Stlineid), $"The stock tran line can't be found"));
                }
                else if (stLine.Movqty != newValue)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Movqty), $"The transaction line's movqty must be the same as stock tran line's movqty (stock tran: {stLine.Movqty}, transaction: {newValue})"));
                }
            });

        this.RuleFor(line => line.Inqty)
            .Custom((newValue, context) =>
            {
                var line = context.InstanceToValidate;
                var stLine = context.TryGetEntity<OlcWhztranline, OlsStline>();
                if (stLine is null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Stlineid), $"The stock tran line can't be found"));
                }
                else if (stLine.Inqty != newValue)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Inqty), $"The transaction line's inqty must be the same as stock tran line's inqty (stock tran: {stLine.Inqty}, transaction: {newValue})"));
                }
            });

        this.RuleFor(line => line.Outqty)
            .Custom((newValue, context) =>
            {
                var line = context.InstanceToValidate;
                var stLine = context.TryGetEntity<OlcWhztranline, OlsStline>();
                if (stLine is null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Stlineid), $"The stock tran line can't be found"));
                }
                else if (stLine.Outqty != newValue)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Outqty), $"The transaction line's outqty must be the same as stock tran line's outqty (stock tran: {stLine.Outqty}, transaction: {newValue})"));
                }
            });

        this.RuleFor(line => line.Unitid2)
            .Custom((newValue, context) =>
            {
                var line = context.InstanceToValidate;
                var stLine = context.TryGetEntity<OlcWhztranline, OlsStline>();
                if (stLine is null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Stlineid), $"The stock tran line can't be found"));
                }
                else if (!string.Equals(stLine.Unitid2, newValue, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Unitid2), $"The transaction line's unitid2 must be the same as stock tran line's unitid2 (stock tran: {stLine.Unitid2}, transaction: {newValue})"));
                }
                else
                {
                    var unit = context.TryGetEntity<OlcWhztranline, OlsUnit>();
                    if (unit is null)
                    {
                        context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Unitid2), $"The unit can't be found (unit: {newValue})"));
                    }
                    else if (unit.Delstat != 0)
                    {
                        context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Unitid2), $"the unit is hidden (unit: {newValue})"));
                    }
                }
            });

        this.RuleFor(line => line.Change)
            .Custom((newValue, context) =>
            {
                var line = context.InstanceToValidate;
                var stLine = context.TryGetEntity<OlcWhztranline, OlsStline>();
                if (stLine is null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Stlineid), $"The stock tran line can't be found"));
                }
                else if (stLine.Change != newValue)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Change), $"The transaction line's change must be the same as stock tran line's change (stock tran: {stLine.Change}, transaction: {newValue})"));
                }
            });

        this.RuleFor(line => line.Ordqty2)
            .Custom((newValue, context) =>
            {
                var line = context.InstanceToValidate;
                var stLine = context.TryGetEntity<OlcWhztranline, OlsStline>();
                if (stLine is null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Stlineid), $"The stock tran line can't be found"));
                }
                else if (stLine.Ordqty2 != newValue)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Ordqty2), $"The transaction line's ordqty2 must be the same as stock tran line's ordqty2 (stock tran: {stLine.Ordqty2}, transaction: {newValue})"));
                }
            });

        this.RuleFor(line => line.Dispqty2)
            .Custom((newValue, context) =>
            {
                var line = context.InstanceToValidate;
                var stLine = context.TryGetEntity<OlcWhztranline, OlsStline>();
                if (stLine is null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Stlineid), $"The stock tran line can't be found"));
                }
                else if (stLine.Dispqty2 != newValue)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Dispqty2), $"The transaction line's dispqty2 must be the same as stock tran line's dispqty2 (stock tran: {stLine.Dispqty2}, transaction: {newValue})"));
                }
            });

        this.RuleFor(line => line.Movqty2)
            .Custom((newValue, context) =>
            {
                var line = context.InstanceToValidate;
                var stLine = context.TryGetEntity<OlcWhztranline, OlsStline>();
                if (stLine is null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Stlineid), $"The stock tran line can't be found"));
                }
                else if (stLine.Movqty2 != newValue)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhztranline.Movqty2), $"The transaction line's movqty2 must be the same as stock tran line's movqty2 (stock tran: {stLine.Movqty2}, transaction: {newValue})"));
                }
            });
    }
}
