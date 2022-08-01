using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Validators.Base;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Validators.Interfaces;
using eLog.HeavyTools.Services.WhZone.DataAccess.Context;
using eLog.HeavyTools.Services.WhZone.DataAccess.Repositories.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Validators;

[RegisterDI(Interface = typeof(IOlcWhzstockValidator))]
internal class OlcWhzstockValidator : EntityValidator<OlcWhzstock>, IOlcWhzstockValidator
{
    private readonly IWarehouseService warehouseService;

    //private readonly IServiceProvider serviceProvider;

    public OlcWhzstockValidator(
        WhZoneDbContext dbContext,
        IWarehouseService warehouseService
        //IServiceProvider serviceProvider
        ) : base(dbContext)
    {
        this.warehouseService = warehouseService ?? throw new ArgumentNullException(nameof(warehouseService));
        //this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    protected override void AddDefaultRules()
    {
        base.AddDefaultRules();

        this.RuleFor(stock => stock.Itemid).NotEmpty();
        this.RuleFor(stock => stock.Whid).NotEmpty();

        this.RuleFor(stock => stock.Whzstockid).CustomAsync(async (newValue, context, cancellationToken) =>
        {
            var stock = context.InstanceToValidate;
            if (stock!.Actqty < 0)
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzstockid), $"The actual value is less than 0 (actQty: {stock.Actqty})"));
            }

            if (stock.Recqty < 0)
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzstockid), $"The receiving value is less than 0 (recQty: {stock.Recqty})"));
            }

            if (stock.Resqty < 0)
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzstockid), $"The reserved value is less than 0 (resQty: {stock.Resqty})"));
            }

            var provQty = stock.Actqty + stock.Recqty - stock.Resqty;
            if (provQty < 0)
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzstockid), $"The provisioned value is less than 0 (actQty: {stock.Actqty}, recQty: {stock.Recqty}, resQty: {stock.Resqty})"));
            }

            //var stockMapService = this.serviceProvider.GetRequiredService<IWhZStockMapService>();
            //var (sumRecQty, sumActQty, sumResQty, sumProvQty) = await stockMapService.SumStockMapQtyAsync(stock, null, cancellationToken);
        });
    }

    protected override void AddAddRules()
    {
        base.AddAddRules();

        this.RuleFor(stock => stock.Itemid).Custom((newValue, context) =>
        {
            var exists = this.dbContext.OlsItems
                .Where(i => i.Itemid == newValue)
                .Any();
            if (!exists)
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzoneid), $"The item doesn't exists (item: {newValue})"));
            }
        });

        this.RuleFor(stock => stock.Whid).CustomAsync(async (newValue, context, cancellationToken) =>
        {
            if (!string.IsNullOrWhiteSpace(newValue))
            {
                var warehouse = await this.warehouseService.GetByIdAsync(newValue, cancellationToken);
                if (warehouse is null)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzoneid), $"The warehouse doesn't exists (warehouse: {newValue})"));
                }
                else
                {
                    var stockMap = this.GetEntity<OlcWhzstockmap>(context);
                    var hasLocationHandling = this.warehouseService.HasLocationHandling(warehouse);
                    if (hasLocationHandling && stockMap is null)
                    {
                        context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzoneid), $"The warehouse is marked as location handling, but the stockMap entry is not found (warehouse: {newValue})"));
                    }
                    else if (!hasLocationHandling && stockMap is not null)
                    {
                        context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzoneid), $"The warehouse is marked as no location handling, but the stockMap entry is found (warehouse: {newValue})"));
                    }
                }
            }
        });

        this.RuleFor(stock => stock.Whzoneid).Custom((newValue, context) =>
        {
            var stock = context.InstanceToValidate;
            if (newValue != null && !string.IsNullOrWhiteSpace(stock.Whid))
            {
                var existsInWh = this.dbContext.OlcWhzones
                    .Where(i => i.Whid == stock.Whid)
                    .Where(i => i.Whzoneid == newValue)
                    .Any();
                if (!existsInWh)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzoneid), $"The zone doesn't exists in the given warehouse: '{stock.Whid}' (zone: {newValue})"));
                }
            }
        });
    }

    protected override void AddUpdateRules()
    {
        base.AddUpdateRules();

        this.RuleFor(stock => stock.Itemid).Custom((newValue, context) =>
        {
            var origStock = context.GetOriginalEntity();
            if (origStock?.Itemid != newValue)
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzoneid), $"Unable to change the item"));
            }
        });

        this.RuleFor(stock => stock.Whid).Custom((newValue, context) =>
        {
            var origStock = context.GetOriginalEntity();
            if (!string.Equals(origStock?.Whid, newValue, StringComparison.InvariantCultureIgnoreCase))
            {
                var exists = this.dbContext.OlsWarehouses
                    .Where(i => i.Whid == newValue)
                    .Any();
                if (!exists)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzoneid), $"Unable to change the warehouse"));
                }
            }
        });

        this.RuleFor(stock => stock.Whzoneid).Custom((newValue, context) =>
        {
            var origStock = context.GetOriginalEntity();
            if (origStock?.Whzoneid != newValue)
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzoneid), $"Unable to change the zone"));
            }
        });
    }

    protected override void AddDeleteRules()
    {
        base.AddDeleteRules();

        this.RuleFor(stock => stock.Whzstockid).Custom((newValue, context) =>
        {
            var stock = context.InstanceToValidate;
            if (stock!.Actqty != 0)
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzstockid), $"Unable to delete stock if actual value is not 0 (actQty: {stock.Actqty})"));
            }

            if (stock.Recqty != 0)
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzstockid), $"Unable to delete stock if receiving value is not 0 (recQty: {stock.Recqty})"));
            }

            if (stock.Resqty < 0)
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzstockid), $"Unable to delete stock if reserved value is not 0 (resQty: {stock.Resqty})"));
            }
        });
    }
}
