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

public class RetailController : Controller
{
    private readonly IRetailService service;

    public RetailController(IRetailService service)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
    }



    [HttpPost("createorder")]
    public async Task<IActionResult> CreateOrder([FromBody] Newtonsoft.Json.Linq.JObject value)
    {
        try
        {
            var r = await service.CreateOrderAsync(value);
            return this.Ok(r);
        }
        catch (Exception ex)
        {
            await ERP4U.Log.LoggerManager.Instance.LogErrorAsync<PriceCalcController>(ex);
            return this.BadRequest(ex.Message);
        }
    }
}
