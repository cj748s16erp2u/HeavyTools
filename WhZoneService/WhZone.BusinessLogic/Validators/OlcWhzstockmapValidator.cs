using System;
using System.Collections;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Validators;

[RegisterDI(Interface = typeof(IOlcWhzstockmapValidator))]
internal class OlcWhzstockmapValidator : EntityValidator<OlcWhzstockmap>, IOlcWhzstockmapValidator
{
    private readonly IWarehouseService warehouseService;

    public OlcWhzstockmapValidator(
        WhZoneDbContext dbContext,
        IWarehouseService warehouseService) : base(dbContext)
    {
        this.warehouseService = warehouseService ?? throw new ArgumentNullException(nameof(warehouseService));
    }

    protected override void AddDefaultRules()
    {
        base.AddDefaultRules();

        this.RuleFor(stockMap => stockMap.Itemid).NotEmpty();
        this.RuleFor(stockMap => stockMap.Whid).NotEmpty();

        this.RuleFor(stockMap => stockMap.Actqty).GreaterThanOrEqualTo(0)
            .WithMessage(stockMap => $"The actual value is less than 0 (actQty: {stockMap.Actqty})");
        this.RuleFor(stockMap => stockMap.Recqty).GreaterThanOrEqualTo(0)
            .WithMessage(stockMap => $"The receiving value is less than 0 (recQty: {stockMap.Recqty})");
        this.RuleFor(stockMap => stockMap.Resqty).GreaterThanOrEqualTo(0)
            .WithMessage(stockMap => $"The reserved value is less than 0 (resQty: {stockMap.Resqty})");

        this.RuleFor(stockMap => stockMap.Provqty).Custom((newValue, context) =>
        {
            var stockMap = context.InstanceToValidate;
            //if (stockMap!.Actqty < 0)
            //{
            //    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstockmap.Actqty), $"The actual value is less than 0 (actQty: {stockMap.Actqty})"));
            //}

            //if (stockMap.Recqty < 0)
            //{
            //    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstockmap.Recqty), $"The receiving value is less than 0 (recQty: {stockMap.Recqty})"));
            //}

            //if (stockMap.Resqty < 0)
            //{
            //    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstockmap.Resqty), $"The reserved value is less than 0 (resQty: {stockMap.Resqty})"));
            //}

            var provQty = stockMap.Actqty + stockMap.Recqty - stockMap.Resqty;
            if (provQty < 0)
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstockmap.Provqty), $"The provisioned value is less than 0 (actQty: {stockMap.Actqty}, recQty: {stockMap.Recqty}, resQty: {stockMap.Resqty})"));
            }
        });
    }

    protected override void AddAddRules()
    {
        base.AddAddRules();

        this.RuleFor(stockMap => stockMap.Itemid).CustomAsync(async (newValue, context, cancellationToken) =>
        {
            var exists = await this.dbContext.OlsItems
                .Where(i => i.Itemid == newValue)
                .AnyAsync(cancellationToken);
            if (!exists)
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstockmap.Whzoneid), $"The item doesn't exists (item: {newValue})"));
            }
        });

        this.RuleFor(stockMap => stockMap.Whid).CustomAsync(async (newValue, context, cancellationToken) =>
        {
            if (!string.IsNullOrWhiteSpace(newValue))
            {
                var warehouse = context.TryGetEntity<OlcWhzstockmap, OlsWarehouse>();
                if (warehouse is null)
                {
                    var exists = await this.dbContext.OlsWarehouses
                        .Where(i => i.Whid == newValue)
                        .AnyAsync(cancellationToken);
                    if (!exists)
                    {
                        context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstockmap.Whzoneid), $"The warehouse doesn't exists (warehouse: {newValue})"));
                    }
                }
            }
        });

        this.RuleFor(stockMap => stockMap.Whzoneid).CustomAsync(async (newValue, context, cancellationToken) =>
        {
            var stockMap = context.InstanceToValidate;
            if (newValue is not null)
            {
                var existsInWh = await this.dbContext.OlcWhzones
                    .Where(i => i.Whid == stockMap.Whid)
                    .Where(i => i.Whzoneid == newValue)
                    .AnyAsync(cancellationToken);
                if (!existsInWh)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstockmap.Whzoneid), $"The zone doesn't exists in the given warehouse: '{stockMap.Whid}' (zone: {newValue})"));
                }
            }
        });

        this.RuleFor(stockMap => stockMap.Whlocid).CustomAsync(async (newValue, context, cancellationToken) =>
        {
            var stockMap = context.InstanceToValidate;
            if (newValue is not null)
            {
                var warehouse = context.TryGetEntity<OlcWhzstockmap, OlsWarehouse>();
                if (warehouse is not null && !this.warehouseService.HasLocationHandling(warehouse))
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstockmap.Whzoneid), $"The warehouse doesn't allow to handle location (warehouse: {stockMap.Whid})"));
                }

                var existsInWh = await this.dbContext.OlcWhlocations
                    .Where(i => i.Whid == stockMap.Whid)
                    .Where(i => i.Whzoneid == stockMap.Whzoneid)
                    .Where(i => i.Whlocid == newValue)
                    .AnyAsync(cancellationToken);
                if (!existsInWh)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstockmap.Whzoneid), $"The location doesn't exists in the given warehouse/zone: '{stockMap.Whid}'/{stockMap.Whzoneid} (location: {newValue})"));
                }
            }
            else
            {
                var warehouse = context.TryGetEntity<OlcWhzstockmap, OlsWarehouse>();
                if (warehouse is not null && this.warehouseService.HasLocationHandling(warehouse))
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstockmap.Whzoneid), $"The warehouse requires to handle location (warehouse: {stockMap.Whid})"));
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
                context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstockmap.Whzoneid), $"Unable to change the item"));
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
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstockmap.Whzoneid), $"Unable to change the warehouse"));
                }
            }
        });

        this.RuleFor(stockMap => stockMap.Whzoneid).Custom((newValue, context) =>
        {
            var origStockMap = context.GetOriginalEntity();
            if (origStockMap?.Whzoneid != newValue)
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstockmap.Whzoneid), $"Unable to change the zone"));
            }
        });

        this.RuleFor(stockMap => stockMap.Whlocid).Custom((newValue, context) =>
        {
            var origStockMap = context.GetOriginalEntity();
            if (origStockMap?.Whlocid != newValue)
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstockmap.Whzoneid), $"Unable to change the location"));
            }
        });
    }

    protected override void AddDeleteRules()
    {
        base.AddDeleteRules();

        this.RuleFor(stockMap => stockMap.Actqty).Equal(0)
            .WithMessage(stockMap => $"Unable to delete stock if actual value is not 0 (actQty: {stockMap.Actqty})");
        this.RuleFor(stockMap => stockMap.Recqty).Equal(0)
            .WithMessage(stockMap => $"Unable to delete stock if receiving value is not 0 (recQty: {stockMap.Recqty})");
        this.RuleFor(stockMap => stockMap.Resqty).Equal(0)
            .WithMessage(stockMap => $"Unable to delete stock if reserved value is not 0 (resQty: {stockMap.Resqty})");

        //this.RuleFor(stockMap => stockMap.Whzstockmapid).Custom((newValue, context) =>
        //{
        //    var stockMap = context.InstanceToValidate;
        //    if (stockMap!.Actqty != 0)
        //    {
        //        context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstockmap.Actqty), $"Unable to delete stock if actual value is not 0 (actQty: {stockMap.Actqty})"));
        //    }

        //    if (stockMap.Recqty != 0)
        //    {
        //        context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstockmap.Recqty), $"Unable to delete stock if receiving value is not 0 (recQty: {stockMap.Recqty})"));
        //    }

        //    if (stockMap.Resqty < 0)
        //    {
        //        context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(OlcWhzstockmap.Resqty), $"Unable to delete stock if reserved value is not 0 (resQty: {stockMap.Resqty})"));
        //    }
        //});
    }
}
