using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.Service.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using System.Text.Json;

namespace eLog.HeavyTools.Services.WhWebShop.Service.Controllers.api;

[Produces("application/json")]
[Route("api/[controller]")]
[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
[Authorize]
public class PriceCalcController : Controller
{
    private readonly IPriceCalcService service;

    public PriceCalcController(IPriceCalcService service) 
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
    }


    [HttpPost("calc")]
    public async Task<IActionResult> CalcAsync([FromBody] Newtonsoft.Json.Linq.JObject value)
    {
        try
        {
            var res = await this.service.CalcJsonAsync(value);
            return this.Ok(res);
        }
        catch (Exception ex)
        {
            await ERP4U.Log.LoggerManager.Instance.LogErrorAsync<PriceCalcController>(ex);
            return this.BadRequest(ex.Message);
        }
    }
    [HttpPost("reset")]
    public async Task<IActionResult> ResetAsync([FromBody] Newtonsoft.Json.Linq.JObject value)
    {
        try
        {
            var res = await this.service.ResetJsonAsync(value);
            return this.Ok(res);
        }
        catch (Exception ex)
        {
            await ERP4U.Log.LoggerManager.Instance.LogErrorAsync<PriceCalcController>(ex);
            return this.BadRequest(ex.Message);
        }
    }
    /*
    public override JsonResult Json(object? data, object? serializerSettings)
    {
        serializerSettings = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        return base.Json(data, serializerSettings);
    }*/
}
