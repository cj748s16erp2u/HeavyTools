using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eLog.HeavyTools.Services.WhWebShop.Service.Controllers;

[Produces("application/json")]
[Route("[controller]")]
[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
//[Authorize]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return this.Ok("This works.");
    }
}
