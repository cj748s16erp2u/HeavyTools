using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eLog.HeavyTools.Services.WhWebShop.Service.Controllers.api;


[Produces("application/json")]
[Route("api/[controller]")]
[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
[Authorize]
public class OrderController : Controller
{
    private readonly IOrderService service;

    public OrderController(IOrderService service)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
    }


    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromBody] OrderParamsDto parms)
    {
        try
        {
            var res = await this.service.CreateAsync(parms);
            return this.Ok(res);
        }
        catch (Exception ex)
        {
            await ERP4U.Log.LoggerManager.Instance.LogErrorAsync<OrderController>(ex);
            return this.BadRequest(ex.Message);
        }
    }

    [HttpPost("calcprice")]
    public Task<IActionResult> CalcPriceAsync([FromBody] OrderParamsDto parms)
    {
        throw new NotImplementedException();
    }
}
