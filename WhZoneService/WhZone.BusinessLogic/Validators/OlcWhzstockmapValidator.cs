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
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Validators;

[RegisterDI(Interface = typeof(IOlcWhzstockmapValidator))]
internal class OlcWhzstockmapValidator : EntityValidator<OlcWhzstockmap>, IOlcWhzstockmapValidator
{
    private readonly IServiceProvider serviceProvider;
    private readonly IWhZStockService stockService;

    public OlcWhzstockmapValidator(
        WhZoneDbContext dbContext,
        IServiceProvider serviceProvider,
        IWhZStockService stockService) : base(dbContext)
    {
        this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        this.stockService = stockService ?? throw new ArgumentNullException(nameof(stockService));
    }

    protected override void AddDefaultRules()
    {
        base.AddDefaultRules();

        this.RuleFor(stockMap => stockMap.Itemid).NotEmpty();
        this.RuleFor(stockMap => stockMap.Whid).NotEmpty();
        this.RuleFor(stockMap => stockMap.Whlocid).NotEmpty();

        this.RuleFor(stockMap => stockMap.Whzstockmapid).CustomAsync(async (newValue, context, cancellationToken) =>
        {
            var stockMap = context.InstanceToValidate;
            if (stockMap!.Actqty < 0)
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzstockid), $"The actual value is less than 0 (actQty: {stockMap.Actqty})"));
            }

            if (stockMap.Recqty < 0)
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzstockid), $"The receiving value is less than 0 (recQty: {stockMap.Recqty})"));
            }

            if (stockMap.Resqty < 0)
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzstockid), $"The reserved value is less than 0 (resQty: {stockMap.Resqty})"));
            }

            var provQty = stockMap.Actqty + stockMap.Recqty - stockMap.Resqty;
            if (provQty < 0)
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzstockid), $"The provisioned value is less than 0 (actQty: {stockMap.Actqty}, recQty: {stockMap.Recqty}, resQty: {stockMap.Resqty})"));
            }

            var stock = await this.stockService.GetAsync(stockMap, cancellationToken);
            if (stock is null)
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzstockid), $"The stock entry is not found for key: [itemId: {stockMap.Itemid}, whId: {stockMap.Whid}, zoneId: {stockMap.Whzoneid}]"));
            }
            else
            {
                var stockMapService = this.serviceProvider.GetRequiredService<IWhZStockMapService>();
                var (sumRecQty, sumActQty, sumResQty, sumProvQty) = await stockMapService.SumStockMapQtyAsync(stockMap, stockMap.Whlocid, cancellationToken);
                if (stock!.Actqty < sumActQty + stockMap.Actqty)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzstockid), $"The stock actual value is less than sum of stock map value (stockActQty: {stock.Actqty}, otherLocActQty: {sumActQty}, actQty: {stockMap.Actqty})"));
                }

                if (stock!.Recqty < sumRecQty + stockMap.Recqty)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzstockid), $"The stock receiving value is less than sum of stock map value (stockRecQty: {stock.Recqty}, otherLocRecQty: {sumRecQty}, recQty: {stockMap.Recqty})"));
                }

                if (stock!.Resqty < sumResQty + stockMap.Resqty)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzstockid), $"The stock reserved value is less than sum of stock map value (stockResQty: {stock.Resqty}, otherLocResQty: {sumResQty}, resQty: {stockMap.Resqty})"));
                }

                // stock.Provqty < sumProvQty + provQty
                // nem vizsgálható, mert a nem helykódos készlet tranzakciók eredményezhetnek kevesebb mennyiséget
            }
        });
    }

    protected override void AddAddRules()
    {
        base.AddAddRules();

        this.RuleFor(stockMap => stockMap.Itemid).Custom((newValue, context) =>
        {
            var exists = this.dbContext.OlsItems
                .Where(i => i.Itemid == newValue)
                .Any();
            if (!exists)
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzoneid), $"The item doesn't exists (item: {newValue})"));
            }
        });

        this.RuleFor(stockMap => stockMap.Whid).Custom((newValue, context) =>
        {
            if (!string.IsNullOrWhiteSpace(newValue))
            {
                var exists = this.dbContext.OlsWarehouses
                    .Where(i => i.Whid == newValue)
                    .Any();
                if (!exists)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzoneid), $"The warehouse doesn't exists (warehouse: {newValue})"));
                }
            }
        });

        this.RuleFor(stockMap => stockMap.Whzoneid).Custom((newValue, context) =>
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

        this.RuleFor(stockMap => stockMap.Whlocid).Custom((newValue, context) =>
        {
            var stockMap = context.InstanceToValidate;
            if (!string.IsNullOrWhiteSpace(stockMap.Whid))
            {
                var existsInWh = this.dbContext.OlcWhlocations
                    .Where(i => i.Whid == stockMap.Whid)
                    .Where(i => i.Whzoneid == stockMap.Whzoneid)
                    .Where(i => i.Whlocid == newValue)
                    .Any();
                if (!existsInWh)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzoneid), $"The location doesn't exists in the given warehouse/zone: '{stockMap.Whid}'/{stockMap.Whzoneid} (location: {newValue})"));
                }
            }
        });
    }

    protected override void AddUpdateRules()
    {
        base.AddUpdateRules();

        this.RuleFor(stockMap => stockMap.Itemid).Custom((newValue, context) =>
        {
            var origStockMap = context.GetOriginalEntity();
            if (origStockMap?.Itemid != newValue)
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzoneid), $"Unable to change the item"));
            }
        });

        this.RuleFor(stockMap => stockMap.Whid).Custom((newValue, context) =>
        {
            var origStockMap = context.GetOriginalEntity();
            if (!string.Equals(origStockMap?.Whid, newValue, StringComparison.InvariantCultureIgnoreCase))
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

        this.RuleFor(stockMap => stockMap.Whzoneid).Custom((newValue, context) =>
        {
            var origStockMap = context.GetOriginalEntity();
            if (origStockMap?.Whzoneid != newValue)
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzoneid), $"Unable to change the zone"));
            }
        });

        this.RuleFor(stockMap => stockMap.Whlocid).Custom((newValue, context) =>
        {
            var origStockMap = context.GetOriginalEntity();
            if (origStockMap?.Whlocid != newValue)
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstock.Whzoneid), $"Unable to change the location"));
            }
        });
    }

    protected override void AddDeleteRules()
    {
        base.AddDeleteRules();

        this.RuleFor(stockMap => stockMap.Whzstockmapid).Custom((newValue, context) =>
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
