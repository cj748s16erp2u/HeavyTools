﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Base;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces; 
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Validators.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;

[RegisterDI(Interface = typeof(IPriceCalcService))]
internal class PriceCalcService : LogicServiceBase<OlcPriceCalcResult>, IPriceCalcService
{
    private readonly IRepository<CfwUser> userRepository;
    private readonly IOlcApiloggerService olcApiloggerService;
    private readonly IPriceCalcOriginalService priceCalcOriginalService;

    public PriceCalcService(
        IOlcPriceCalcResultValidator validator,
        IRepository<OlcPriceCalcResult> repository,
        IUnitOfWork unitOfWork,
        IEnvironmentService environmentService,
        IRepository<CfwUser> userRepository,
        IOlcApiloggerService olcApiloggerService,
        IPriceCalcOriginalService priceCalcOriginalService) : base(validator, repository, unitOfWork, environmentService)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.olcApiloggerService = olcApiloggerService ?? throw new ArgumentNullException(nameof(olcApiloggerService));
        this.priceCalcOriginalService = priceCalcOriginalService ?? throw new ArgumentNullException(nameof(priceCalcOriginalService));
    }

    public int Calc(int? x, int? y)
    {
        return x.GetValueOrDefault() * y.GetValueOrDefault() * 5;
    }

    public async Task<CalcResultDto> CalcAsync(CalcParamsDto parms, CancellationToken cancellationToken = default)
    {
        this.ValidateCalcParams(parms);

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        Task.Run(() => ERP4U.Log.LoggerManager.Instance.LogDebugAsync<PriceCalcService>($"Func: {CalcAsync}, Params: [{Newtonsoft.Json.JsonConvert.SerializeObject(parms)}]"));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

        //var devUser = await this.userRepository.QueryAsync(u => u.Usrid == "dev");
        //var devUser = await this.userRepository.FindAsync(new[] { "dev" }, cancellationToken);
        var userId = this.EnvironmentService.CurrentUserId!;
        var user = await this.userRepository.FindAsync(new[] { userId }, cancellationToken);

        var result = this.Calc(parms.X, parms.Y);

        var entResult = new OlcPriceCalcResult
        {
            Value = result            
        };

        var entity = await this.AddAsync(entResult, cancellationToken);

        return new CalcResultDto
        {
            //RequestToken = "...",
            Id = entity?.Id,
            Result = result,
            UserId = user?.Usrid,
            UserName = user?.Name
        };
    }

    public static string jsonRoot = "Cart";

    private void ValidateCalcParams(CalcParamsDto parms)
    {
        if (parms is null)
        {
            throw new ArgumentNullException(nameof(parms));
        }

        if (parms.Y < parms.X)
        {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
            throw new ArgumentOutOfRangeException("y less than x");
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
        }
    }

    public async Task<CalcJsonResultDto> CalcJsonAsync(Newtonsoft.Json.Linq.JObject parms, CancellationToken cancellationToken = default)
    {
        var calcitem = JsonParser.ParseObject<CalcJsonParamsDto>(parms);
        CalcJsonResultDto res = await priceCalcOriginalService.GetOriginalPrice(calcitem, cancellationToken);



        var a = new OlcApilogger
        {
            Command = "PriceCalcService",
            Request = parms.ToString(),
            Response = JsonConvert.SerializeObject(res)
        };

        await olcApiloggerService.AddAsync(a);

        return res;
    }
}