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
public class PartnerController : BaseController
{
    private readonly IPartnerService service;

    public PartnerController(IOlcApiloggerService apiloggerservice, IPartnerService service) : base(apiloggerservice)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
    }


    [HttpPost("create")]
    public async Task<IActionResult> GetPartnersAsync([FromBody] Newtonsoft.Json.Linq.JObject value)
    {
        try
        {
            var res = await this.service.CreatePartnersAsync(value, apilogger);
            return this.Ok(res);
        }
        catch (Exception ex)
        {
            await ERP4U.Log.LoggerManager.Instance.LogErrorAsync<B2BController>(ex);
            return this.BadRequest(ex.Message);
        }
    }
}
