using System;
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
    private readonly IPriceCalcActionService priceCalcActionService;
    private readonly IPriceCalcGroupService priceCalcGroupService;
    private readonly IPriceCalcValueCalculatorService pricseCalcValueCalculatorService;
    private readonly IOlcActionCacheService olcActionCacheService;
    private readonly IOlcCartCacheService olcCartCacheService;

    public PriceCalcService(
        IOlcPriceCalcResultValidator validator,
        IRepository<OlcPriceCalcResult> repository,
        IUnitOfWork unitOfWork,
        IEnvironmentService environmentService,
        IRepository<CfwUser> userRepository,
        IOlcApiloggerService olcApiloggerService,
        IPriceCalcOriginalService priceCalcOriginalService,
        IPriceCalcActionService priceCalcActionService,
        IPriceCalcGroupService priceCalcGroupService,
        IPriceCalcValueCalculatorService pricseCalcValueCalculatorService,
        IOlcActionCacheService olcActionCacheService, 
        IOlcCartCacheService olcCartCacheService) : base(validator, repository, unitOfWork, environmentService)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.olcApiloggerService = olcApiloggerService ?? throw new ArgumentNullException(nameof(olcApiloggerService));
        this.priceCalcOriginalService = priceCalcOriginalService ?? throw new ArgumentNullException(nameof(priceCalcOriginalService));
        this.priceCalcActionService = priceCalcActionService ?? throw new ArgumentNullException(nameof(priceCalcActionService));
        this.priceCalcGroupService = priceCalcGroupService ?? throw new ArgumentNullException(nameof(priceCalcGroupService));
        this.pricseCalcValueCalculatorService = pricseCalcValueCalculatorService ?? throw new ArgumentNullException(nameof(pricseCalcValueCalculatorService));
        this.olcActionCacheService = olcActionCacheService ?? throw new ArgumentNullException(nameof(olcActionCacheService));
        this.olcCartCacheService = olcCartCacheService ?? throw new ArgumentNullException(nameof(olcCartCacheService));
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
        var pcar = new PriceCalcActionResultDto();
        var cart = JsonParser.ParseObject<CalcJsonParamsDto>(parms);
        var res = await CalculatePrice(cart, pcar, cancellationToken);
        

        if (UnitTestDetector.IsRunningFromNUnit) {
            Debug(pcar);
            Debug(res);
        }

            /*
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            var a = new OlcApilogger
            {
                Command = "PriceCalcService",
                Request = parms.ToString(),
                Response = JsonConvert.SerializeObject(res, Formatting.None, jsSettings)
            };

            await olcApiloggerService.AddAsync(a);
            var a2 = new OlcApilogger
            {
                Command = "PriceCalcServiceActionResult",
                Request ="",
                Response = JsonConvert.SerializeObject(pcar, Formatting.None, jsSettings)
            };
            await olcApiloggerService.AddAsync(a2);
            */
            return res;
    }

    public static void Debug(object res)
    {
        var jsSettings = new JsonSerializerSettings();
        jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(res, Formatting.Indented, jsSettings));
    }

    public async Task<CalcJsonResultDto> CalculatePrice(CalcJsonParamsDto cart, PriceCalcActionResultDto pricecalcactionresult, CancellationToken cancellationToken = default)
    { 
        if (olcCartCacheService.TryGet(cart, out var cjrd))
        {
            return cjrd;
        } else {
            var res = await priceCalcOriginalService.GetOriginalPrice(cart, cancellationToken);
            await priceCalcActionService.CalculateActionPriceAsync(cart, res, pricecalcactionresult, cancellationToken);
            priceCalcGroupService.GroupCart(res);
            await pricseCalcValueCalculatorService.CalculateCartAsync(res, cancellationToken);
            olcCartCacheService.Add(cart, res);
            return res;
        }
    }

    public async Task<bool> ResetJsonAsync(JObject parms, CancellationToken cancellationToken = default)
    {
        var cartReset = JsonParser.ParseObject<CalcResetJsonParamsDto>(parms);

        return await olcActionCacheService.Reset(cartReset.Aid, cancellationToken);
    }

    public void ResetCartCacheAsync(JObject value)
    {
        olcCartCacheService.Reset();
    }
}
