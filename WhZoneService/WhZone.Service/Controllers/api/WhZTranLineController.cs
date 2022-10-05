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

    /// <summary>
    /// Zóna készlet tranzakció tételek lekérdezése
    /// </summary>
    /// <param name="query">Keresési feltételek</param>
    /// <returns>Lekérdezés eredménye</returns>
    [HttpPost("receiving/query")]
    public async Task<ActionResult<IEnumerable<WhZReceivingTranLineDto>>> QueryReceivingAsync([FromBody] WhZTranLineQueryDto query = null)
    {
        try
        {
            return this.Ok(await this.tranLineService.QueryReceivingAsync(query));
        }
        catch (Exception ex)
        {
            await ERP2U.Log.LoggerManager.Instance.LogErrorAsync<WhZTranLineController>(ex);
            return this.BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Új zóna készlet tranzakció tétel rögzítése
    /// </summary>
    /// <param name="request">Tétel információk</param>
    /// <returns>Létrehozott tétel</returns>
    [HttpPost("receiving/add")]
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

    /// <summary>
    /// Meglévő zóna készlet tranzakció tétel módosítása
    /// </summary>
    /// <param name="request">Tétel információk</param>
    /// <returns>Módosított tétel</returns>
    [HttpPost("receiving/update")]
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
