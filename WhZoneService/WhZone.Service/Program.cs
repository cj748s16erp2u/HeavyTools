var options = new WebApplicationOptions
{
    ContentRootPath = AppContext.BaseDirectory,
    Args = args
};

var builder = WebApplication.CreateBuilder(options);

builder.Host.UseWindowsService();

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    var env = hostingContext.HostingEnvironment;
    config.SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
        .AddEnvironmentVariables()
        .AddCommandLine(args);
});

builder.Host.ConfigureLogging((hostingContext, logging) =>
{
    logging.ClearProviders();
    logging.SetMinimumLevel(LogLevel.Information);

#if DEBUG
    logging.AddDebug();
    logging.AddConsole();
#endif
});

builder.Services.AddBusinessEntities();
builder.Services.AddDataAccess(builder.Configuration);
builder.Services.AddBusinessLogic(builder.Configuration);

builder.Services.AddAuthentication(options =>
{
    options.AddScheme<eLog.HeavyTools.Services.WhZone.Service.Handlers.BasicAuthenticationHandler>("BasicAuthentication", null);
    options.DefaultScheme = "BasicAuthentication";
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BasicAuthentication", new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder("BasicAuthentication").RequireAuthenticatedUser().Build());
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
#if DEBUG
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
#else
        .AllowAnyMethod()
        .AllowCredentials()
#endif
        );
});

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    //options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    //{
    //    Version = "v1",
    //    Title = "ToDo API",
    //});
});

var app = builder.Build();

ERP2U.Log.LoggerManager.Initialize(app.Services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseSwagger(options =>
    {
        //options.SerializeAsV2 = true;
    });
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseStaticFiles();

app.UseAuthentication();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
