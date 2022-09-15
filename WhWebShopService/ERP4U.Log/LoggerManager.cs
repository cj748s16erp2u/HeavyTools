using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP4U.Log;

[System.Diagnostics.DebuggerStepThrough]
public sealed class LoggerManager
{
    private static LoggerManager? instance;

    private IHttpContextAccessor? httpContextAccessor;

    private readonly ILogger logger = LogManager.GetCurrentClassLogger();

    public static LoggerManager Instance
    {
        get
        {
            if (instance is null)
            {
                instance = new LoggerManager();
            }

            return instance;
        }
    }

    private LoggerManager() { }

    public static void Initialize(IServiceProvider serviceProvider)
    {
        instance = new LoggerManager
        {
            httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>()
        };
    }

    public async Task LogAsync<T>(LogEventInfo logEventInfo)
    {
        logEventInfo.LoggerName = typeof(T).ToString();

        this.SetRequestId(logEventInfo);
        this.SetRequestUrl(logEventInfo);
        await this.SetRequestBodyAsync(logEventInfo).ConfigureAwait(false);
        this.SetClientIpAddress(logEventInfo);

        this.logger.Log(logEventInfo);
    }

    public async Task LogDebugAsync<T>(string message, LogCategory category = LogCategory.General, params object[] args)
    {
        var logEventInfo = new LogEventInfo(LogLevel.Debug, this.logger.Name, args.Length > 0 ? string.Format(message, args) : message);
        this.SetCategory(category, logEventInfo);

        await this.LogAsync<T>(logEventInfo).ConfigureAwait(false);
    }

    public async Task LogErrorAsync<T>(string message, params object[] args)
    {
        var logEventInfo = new LogEventInfo(LogLevel.Error, this.logger.Name, args.Length > 0 ? string.Format(message, args) : message);

        await this.LogAsync<T>(logEventInfo).ConfigureAwait(false);
    }

    public async Task LogErrorAsync<T>(Exception exception)
    {
        var logEventInfo = new LogEventInfo(LogLevel.Error, this.logger.Name, exception.Message)
        {
            Exception = exception
        };

        await this.LogAsync<T>(logEventInfo).ConfigureAwait(false);
    }

    public async Task LogInformationAsync<T>(string message, LogCategory category = LogCategory.General, params object[] args)
    {
        var logEventInfo = new LogEventInfo(LogLevel.Info, logger.Name, args.Length > 0 ? string.Format(message, args) : message);
        this.SetCategory(category, logEventInfo);

        await this.LogAsync<T>(logEventInfo).ConfigureAwait(false);
    }

    private void SetCategory(LogCategory category, LogEventInfo logEventInfo)
    {
        logEventInfo.Properties.Add("Category", category);
    }

    private void SetRequestId(LogEventInfo logEventInfo)
    {
        if (!logEventInfo.Properties.ContainsKey("RequestId"))
        {
            logEventInfo.Properties.Add("RequestId", this.httpContextAccessor?.HttpContext?.TraceIdentifier);
        }
    }

    private void SetRequestUrl(LogEventInfo logEventInfo)
    {
        if (!logEventInfo.Properties.ContainsKey("RequestUrl"))
        {
            logEventInfo.Properties.Add("RequestUrl", this.httpContextAccessor?.HttpContext?.Request?.GetDisplayUrl());
        }
    }

    private async Task SetRequestBodyAsync(LogEventInfo logEventInfo)
    {
        if (this.httpContextAccessor is not null && !logEventInfo.Properties.ContainsKey("RequestBody"))
        {
            var bodyContent = string.Empty;
            var request = httpContextAccessor.HttpContext?.Request;
            if (httpContextAccessor.HttpContext?.Items.ContainsKey("BodyContent") == true)
            {
                bodyContent = httpContextAccessor.HttpContext.Items["BodyContent"]?.ToString();
            }
            else
            {
                if (request is not null && request.Body.CanRead && request.ContentLength > 0)
                {
                    //request.EnableRewind();
                    request.EnableBuffering();
                    var temp = request.Body;

                    var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                    request.Body.Seek(0, SeekOrigin.Begin);
                    await request.Body.ReadAsync(buffer).ConfigureAwait(false);
                    bodyContent = Encoding.UTF8.GetString(buffer);

                    if (!string.IsNullOrWhiteSpace(bodyContent))
                    {
                        try
                        {
                            var dictBodyContent = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(bodyContent);
                            if (dictBodyContent is not null)
                            {
                                var filteredKeyValues = dictBodyContent.Where(x => x.Key.Equals("Password", StringComparison.InvariantCultureIgnoreCase));
                                if (filteredKeyValues is not null)
                                {
                                    foreach (var kv in filteredKeyValues)
                                    {
                                        dictBodyContent[kv.Key] = "***";
                                    }
                                }

                                bodyContent = Newtonsoft.Json.JsonConvert.SerializeObject(dictBodyContent);
                            }
                        }
                        catch { }
                    }

                    if (httpContextAccessor.HttpContext is not null)
                    {
                        if (httpContextAccessor.HttpContext.Items.ContainsKey("BodyContent") == true)
                        {
                            httpContextAccessor.HttpContext.Items.Add("BodyContent", bodyContent);
                        }

                        httpContextAccessor.HttpContext.Request.Body = temp;
                    }
                }
            }

            logEventInfo.Properties.Add("RequestBody", bodyContent);
        }
    }

    private void SetClientIpAddress(LogEventInfo logEventInfo)
    {
        if (!logEventInfo.Properties.ContainsKey("ClientIpAddress"))
        {
            logEventInfo.Properties.Add("ClientIpAddress", this.httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress);
        }
    }
}
