using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Text;

namespace eLog.HeavyTools.Services.WhWebShop.Service.Helper
{
    abstract public class BaseController : Controller
    {
        private IOlcApiloggerService apiloggerservice;

        public bool ApiLoggerEnabled = true;
        public OlcApilogger? apilogger;

        public BaseController(IOlcApiloggerService apiloggerservice)
        {
            this.apiloggerservice = apiloggerservice ?? throw new ArgumentNullException(nameof(apiloggerservice));
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
             
            if (ApiLoggerEnabled && apilogger != null)
            {
                apilogger.Response = JsonConvert.SerializeObject(context.Result);
                apiloggerservice.UpdateAsync(apilogger).GetAwaiter().GetResult();
            }
        }

        private string GetRequest(HttpRequest? req)
        {
            using (StreamReader reader
                      = new StreamReader(req!.Body, Encoding.UTF8, true, 1024, true))
            {
                req.Body.Position = 0;
                var s=reader.ReadToEnd(); 
                req.Body.Position = 0;
                return s;
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (!ApiLoggerEnabled)
            {
                return;
            }
            var request = GetRequest(context.HttpContext.Request);
            var lg = new OlcApilogger
            {
                Command = (string)context.RouteData.Values["Controller"]!,
                Request = request,
                Response = JsonConvert.SerializeObject(context.Result)
            };

            apilogger = apiloggerservice.AddAsync(lg).GetAwaiter().GetResult(); 
        }
         
    }
}
