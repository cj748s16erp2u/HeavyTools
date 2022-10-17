using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eLog.HeavyTools.Services.WhWebShop.Service.Controllers.api;

[Produces("application/json")]
[Route("api/[controller]")]
[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
[Authorize]
public class SordController : Controller
{
    private readonly ISordLineService service;

    public SordController(ISordLineService service)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpPost("sordlinedelete")]
    public async Task<IActionResult> DoSordlineDeleteAsync([FromBody] Newtonsoft.Json.Linq.JObject value)
    {
        try
        {
            var res = await this.service.SordLineDeleteAsync(value);
            return this.Ok(res);
        }
        catch (Exception ex)
        {
            await ERP4U.Log.LoggerManager.Instance.LogErrorAsync<SordController>(ex);
            return this.BadRequest(ex.Message);
        }
    }
}
