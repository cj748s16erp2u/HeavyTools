using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eLog.HeavyTools.Services.WhZone.Service.Controllers.api;

[Produces("application/json")]
[Route("api/[controller]")]
[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
[Authorize]
public class WhZTranLocController : Controller
{
    private readonly IWhZTranLocService tranLocService;

    public WhZTranLocController(
        IWhZTranLocService tranLocService)
    {
        this.tranLocService = tranLocService ?? throw new ArgumentNullException(nameof(tranLocService));
    }

    /// <summary>
    /// Zóna készlet tranzakció helykód lekérdezése
    /// </summary>
    /// <param name="query">Keresési feltételek</param>
    /// <returns>Lekérdezés eredménye</returns>
    [HttpPost("query")]
    public async Task<ActionResult<IEnumerable<WhZTranLocDto>>> QueryAsync([FromBody] WhZTranLocQueryDto query = null!)
    {
        try
        {
            return this.Ok(await this.tranLocService.QueryAsync(query));
        }
        catch (Exception ex)
        {
            await ERP2U.Log.LoggerManager.Instance.LogErrorAsync<WhZTranLocController>(ex);
            return this.BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Zóna készlet tranzakció helykód lekérdezése (ERP)
    /// </summary>
    /// <param name="query">Keresési feltételek</param>
    /// <returns>Lekérdezés eredménye</returns>
    [HttpPost("query_erp")]
    public async Task<ActionResult<string>> QueryERPAsync([FromBody] WhZTranLocQueryDto query = null!)
    {
        try
        {
            var list = await this.tranLocService.QueryAsync(query);
            return this.Ok(Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }
        catch (Exception ex)
        {
            await ERP2U.Log.LoggerManager.Instance.LogErrorAsync<WhZTranLocController>(ex);
            return this.BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Zóna készlet tranzakció helykód lekérés
    /// </summary>
    /// <param name="whztlocid">Helykód azonosító</param>
    /// <returns>Helykód bejegyzés</returns>
    [HttpGet("get/{whztlocid}")]
    public async Task<ActionResult<WhZTranLocDto>> GetAsync(int whztlocid)
    {
        try
        {
            return this.Ok(await this.tranLocService.GetAsync(whztlocid));
        }
        catch (Exception ex)
        {
            await ERP2U.Log.LoggerManager.Instance.LogErrorAsync<WhZTranLocController>(ex);
            return this.BadRequest(ex.Message);
        }
    }
}
