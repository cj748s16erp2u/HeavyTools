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
public class OSSController : BaseController
{
    private readonly IOSSService service;

    public OSSController(IOlcApiloggerService apiloggerservice, IOSSService service) : base (apiloggerservice)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
    }


    [HttpPost("get")]
    public async Task<IActionResult> Get([FromBody] OSSParamsDto parms)
    {
        try
        {
            var res = await this.service.GetOss(parms, default);
            return this.Ok(res);
        }
        catch (Exception ex)
        {
            await ERP4U.Log.LoggerManager.Instance.LogErrorAsync<OSSController>(ex);
            return this.BadRequest(ex.Message);
        }
    }
}
