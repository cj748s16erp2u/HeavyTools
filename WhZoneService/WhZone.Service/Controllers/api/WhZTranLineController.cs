using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eLog.HeavyTools.Services.WhZone.Service.Controllers.api;

[Produces("application/json")]
[Route("api/[controller]")]
[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
[Authorize]
public class WhZTranLineController : Controller
{
    private readonly IWhZTranLineService tranLineService;

    public WhZTranLineController(
        IWhZTranLineService tranLineService)
    {
        this.tranLineService = tranLineService ?? throw new ArgumentNullException(nameof(tranLineService));
    }

    //[HttpPost("receiving/line/query")]
    //public async Task<ActionResult<IEnumerable<WhZReceivingTranLineDto>>> QueryReceivingAsync([FromBody] WhZTranLineQueryDto query = null)
    //{
    //}

    [HttpPost("receiving/line/add")]
    public async Task<ActionResult<WhZReceivingTranLineDto>> AddReceivingAsync([FromBody] WhZReceivingTranLineDto request)
    {
        if (request is null)
        {
            return this.BadRequest("'request' must be set");
        }

        try
        {
            return this.Ok(await this.tranLineService.AddReceivingAsync(request));
        }
        catch (Exception ex)
        {
            await ERP2U.Log.LoggerManager.Instance.LogErrorAsync<WhZTranLineController>(ex);
            return this.BadRequest(ex.Message);
        }
    }

    [HttpPost("receiving/line/update")]
    public async Task<ActionResult<WhZReceivingTranLineDto>> UpdateReceivingAsync([FromBody] WhZReceivingTranLineDto request)
    {
        if (request is null)
        {
            return this.BadRequest("'request' must be set");
        }

        try
        {
            return this.Ok(await this.tranLineService.UpdateReceivingAsync(request));
        }
        catch (Exception ex)
        {
            await ERP2U.Log.LoggerManager.Instance.LogErrorAsync<WhZTranLineController>(ex);
            return this.BadRequest(ex.Message);
        }
    }
}
