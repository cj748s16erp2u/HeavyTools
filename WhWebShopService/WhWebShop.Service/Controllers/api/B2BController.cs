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
public class B2BController : BaseController
{
    private readonly IB2BService service;

    public B2BController(IOlcApiloggerService apiloggerservice, IB2BService service) : base(apiloggerservice)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
    }


    [HttpPost("getpartners")]
    public async Task<IActionResult> GetPartnersAsync([FromBody] Newtonsoft.Json.Linq.JObject value)
    {
        try
        {
            var res = this.service.GetPartners();
            return this.Ok(res);
        }
        catch (Exception ex)
        {
            await ERP4U.Log.LoggerManager.Instance.LogErrorAsync<B2BController>(ex);
            return this.BadRequest(ex.Message);
        }
    }
}
