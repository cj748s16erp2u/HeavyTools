using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.Service.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eLog.HeavyTools.Services.WhWebShop.Service.Controllers.api;


[Produces("application/json")]
[Route("api/[controller]")]
[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
[Authorize]
public class OrderController : BaseController
{
    private readonly IOrderService service;

    public OrderController(IOlcApiloggerService apiloggerservice, IOrderService service) :base(apiloggerservice)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
    }


    [HttpPost("createold")]
    public async Task<IActionResult> CreateOldAsync([FromBody] OrderParamsDto parms)
    {
        try
        {
            var res = await this.service.CreateOldAsync(parms, apilogger!);
            return this.Ok(res);
        }
        catch (Exception ex)
        {
            await ERP4U.Log.LoggerManager.Instance.LogErrorAsync<OrderController>(ex);
            return this.BadRequest(ex.Message);
        }
    }
    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromBody] Newtonsoft.Json.Linq.JObject value)
    { 
        try
        {
            var res = await this.service.CreateAsync(value, apilogger!);
            return this.Ok(res);
        }
        catch (Exception ex)
        {
            await ERP4U.Log.LoggerManager.Instance.LogErrorAsync<OrderController>(ex);
            return this.BadRequest(ex.Message);
        }
    }
}
