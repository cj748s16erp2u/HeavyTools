using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eLog.HeavyTools.Services.WhZone.Service.Controllers.api;

[Produces("application/json")]
[Route("api/[controller]")]
[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
[Authorize]
public class WhZStockMapController : Controller
{
    private readonly IWhZStockMapService stockMapService;

    public WhZStockMapController(
        IWhZStockMapService stockMapService)
    {
        this.stockMapService = stockMapService ?? throw new ArgumentNullException(nameof(stockMapService));
    }

    /// <summary>
    /// Zóna helykód készlet lekérdezése
    /// </summary>
    /// <returns>Lekérdezés eredménye</returns>
    [HttpPost("query")]
    public async Task<ActionResult<IEnumerable<WhZStockMapQDto>>> QueryStockMapAsync()
    {
        try
        {
            return this.Ok(await this.stockMapService.QueryStockMapAsync());
        }
        catch (Exception ex)
        {
            await ERP2U.Log.LoggerManager.Instance.LogErrorAsync<WhZTranController>(ex);
            return this.BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Zóna helykód készlet lekérdezése (ERP)
    /// </summary>
    /// <returns>Lekérdezés eredménye</returns>
    [HttpPost("query_erp")]
    public async Task<ActionResult<string>> QueryStockMapERPAsync()
    {
        try
        {
            return this.Ok(await this.stockMapService.QueryStockMapAsync());
        }
        catch (Exception ex)
        {
            await ERP2U.Log.LoggerManager.Instance.LogErrorAsync<WhZTranController>(ex);
            return this.BadRequest(ex.Message);
        }
    }
}
