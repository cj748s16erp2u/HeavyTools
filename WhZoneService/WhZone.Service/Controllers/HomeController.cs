using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
//using eLog.HeavyTools.Services.WhZone.Service.ActionFilters;
using Microsoft.AspNetCore.Mvc;

namespace eLog.HeavyTools.Services.WhZone.Service.Controllers;

[Produces("application/json")]
[Route("[controller]")]
[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
[Authorize]
public class HomeController : Controller
{
    private readonly ILogicService<OlsCompany> logicService;
    private readonly IEnvironmentService environmentService;

    public HomeController(
        ILogicService<OlsCompany> logicService,
        IEnvironmentService environmentService)
    {
        this.logicService = logicService ?? throw new ArgumentNullException(nameof(logicService));
        this.environmentService = environmentService ?? throw new ArgumentNullException(nameof(environmentService));
    }

    [HttpGet]
    public async Task<IActionResult> IndexAsync()
    {
        var user = this.environmentService.CurrentUserId;
        var companies = await this.logicService.QueryAsync();

        return this.Ok(new { user, companies });
    }
}
