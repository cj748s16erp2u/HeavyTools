using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace eLog.HeavyTools.Services.WhWebShop.Service.Middlewares
{
    [System.Diagnostics.DebuggerStepThrough]
    public class ErrorMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorMiddleware(RequestDelegate next)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                context.Request.EnableBuffering();

                await this.next(context);
            }
            catch (Exception ex)
            {
                await ERP4U.Log.LoggerManager.Instance.LogErrorAsync<ErrorMiddleware>(ex);
            }
        }
    }
}

namespace Microsoft.Extensions.DependencyInjection
{
    [System.Diagnostics.DebuggerStepThrough]
    public static class ErrorMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<eLog.HeavyTools.Services.WhWebShop.Service.Middlewares.ErrorMiddleware>();
        }
    }
}