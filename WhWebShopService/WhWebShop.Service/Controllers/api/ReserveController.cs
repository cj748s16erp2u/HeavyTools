using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 

namespace eLog.HeavyTools.Services.WhWebShop.Service.Controllers.api;

[Produces("application/json")]
[Route("api/[controller]")]
[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
[Authorize]
public class ReserveController : Controller
{

    private readonly IReserveService service;

    public ReserveController(IReserveService service)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpPost("reserve")]
    public async Task<IActionResult> DoReserveAsync([FromBody] Newtonsoft.Json.Linq.JObject value)
    {
        try
        {
            var res = await this.service.ReserveAsync(value);
            return this.Ok(res);
        }
        catch (Exception ex)
        {
            await ERP4U.Log.LoggerManager.Instance.LogErrorAsync<ReserveController>(ex);
            return this.BadRequest(ex.Message);
        }
    }
    [HttpPost("reservedelete")]
    public async Task<IActionResult> DoReserveDeleteAsync([FromBody] Newtonsoft.Json.Linq.JObject value)
    {
        try
        {
            var res = await this.service.ReserveDeleteAsync(value);
            return this.Ok(res);
        }
        catch (Exception ex)
        {
            await ERP4U.Log.LoggerManager.Instance.LogErrorAsync<ReserveController>(ex);
            return this.BadRequest(ex.Message);
        }
    }
}
