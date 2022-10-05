using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eLog.HeavyTools.Services.WhZone.Service.Controllers.api;

[Produces("application/json")]
[Route("api/[controller]")]
[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
[Authorize]
public class WhZTranController : Controller
{
    private readonly IWhZTranService tranService;

    public WhZTranController(
        IWhZTranService tranService)
    {
        this.tranService = tranService ?? throw new ArgumentNullException(nameof(tranService));
    }

    /// <summary>
    /// Zóna készlet tranzakciók lekérdezése
    /// </summary>
    /// <param name="query">Keresési paraméterek</param>
    /// <returns>Lekérdezés eredménye</returns>
    [HttpPost("receiving/query")]
    public async Task<ActionResult<IEnumerable<WhZReceivingTranHeadDto>>> QueryReceivingAsync([FromBody] WhZTranHeadQueryDto query = null!)
    {
        try
        {
            return this.Ok(await this.tranService.QueryReceivingAsync(query));
        }
        catch (Exception ex)
        {
            await ERP2U.Log.LoggerManager.Instance.LogErrorAsync<WhZTranController>(ex);
            return this.BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Új zóna készlet tranzakció rögzítése
    /// </summary>
    /// <param name="request">Tranzakció információk</param>
    /// <returns>Létrehozott tranzakció</returns>
    [HttpPost("receiving/add")]
    public async Task<ActionResult<WhZReceivingTranHeadDto>> AddReceivingAsync([FromBody] WhZReceivingTranHeadDto request)
    {
        if (request is null)
        {
            return this.BadRequest("'request' must be set");
        }

        try
        {
            return this.Ok(await this.tranService.AddReceivingAsync(request));
        }
        catch (Exception ex)
        {
            await ERP2U.Log.LoggerManager.Instance.LogErrorAsync<WhZTranController>(ex);
            return this.BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Meglévő zóna készlet tranzakció módosítása
    /// </summary>
    /// <param name="request">Tranzakció információk</param>
    /// <returns>Módosított tranzakció</returns>
    [HttpPost("receiving/update")]
    public async Task<ActionResult<WhZReceivingTranHeadDto>> UpdateReceivingAsync([FromBody] WhZReceivingTranHeadDto request)
    {
        if (request is null)
        {
            return this.BadRequest("'request' must be set");
        }

        try
        {
            return this.Ok(await this.tranService.UpdateReceivingAsync(request));
        }
        catch (Exception ex)
        {
            await ERP2U.Log.LoggerManager.Instance.LogErrorAsync<WhZTranController>(ex);
            return this.BadRequest(ex.Message);
        }
    }
}
