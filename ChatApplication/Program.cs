//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddRazorPages();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.MapRazorPages();

//app.Run();
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using ChatApplication.Extension;
using ChatApplication.Handler;
using ChatApplication.Interface;
using ChatApplication.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using NToastNotify;
using System.ComponentModel.Design;

var logger = CreateNlogLogger();
Logger CreateNlogLogger()

{
    //LogManager.Configuration = new NLogLoggingConfiguration(configuration.GetSection("Nlog"));
    var logger = LogManager.Setup()
        .LoadConfigurationFromAppSettings()
        .GetCurrentClassLogger();
    logger.Info("nlog initiation started");
    try
    {

        logger.Debug("init main");
        logger.Debug("Environment:");
        //	logger.Debug(enviroment);
    }
    catch (Exception exception)
    {
        logger.Error(exception, "Stopped program of exception");
        throw;
    }
    finally
    {
        NLog.LogManager.Shutdown();
    }
    return logger;
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICustomerInfo, CustomerInfoService>();



builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();

builder.Services.AddRazorPages().AddNToastNotifyNoty(new NotyOptions
{
    ProgressBar = true,
    Timeout = 5000
});

builder.Services.AddAntiforgery(o => o.HeaderName = "AntiForgeryHeaderName");

builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
});

#region claim identity code
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Set session timeout
        options.SlidingExpiration = true; // Enable sliding expiration
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        };
    });
#endregion

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Logging.ClearProviders();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddMvc();
builder.Services.AddControllers();
builder.Services.AddTransient<CommonUtility>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
//app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRouting();
app.UseSession();

app.Use(static async (context, next) =>
{
    string PageName = context.Request.Path;
    string PageName2 = context.Request.Path;
    string fullUrl = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";

    PageName = CommonUtility.RemoveFirstChar(PageName);

    string? UserID = string.IsNullOrWhiteSpace(context.Session.GetString("CustomerID")) ? "0" : context.Session.GetString("CustomerID");
    string ipAddress = context.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? context.Connection.RemoteIpAddress?.ToString() ?? "0";

    if (ipAddress == "::1")
    {
        ipAddress = "127.0.0.1";
    }

    //DBUtility.InsertPageList(PageName, fullUrl, UserID, ipAddress);
    string[] blockExtensionsList = new[] { ".txt", ".exe", ".xml", ".dat", ".bat" };




    string ext = System.IO.Path.GetExtension(PageName).ToLower();


    if (blockExtensionsList.Contains(ext))
    {
        context.Response.Redirect("/Error");
        return;
    }

    if (context.Request.Path == "/")
    {
        await next();
        return;
    }

    if (PageName.ToLower().Trim().Contains("css")
    || PageName.ToLower().Trim().Contains("index")
    || PageName.ToLower().Trim().Contains("registerinfo.aspx")
    || PageName.ToLower().Trim().Contains("registerinfo")
    || PageName.ToLower().Trim().Contains("obcidentity")
    || PageName.ToLower().Trim().Contains("CancellationPayment")
    || PageName.ToLower().Trim().Contains("refundpolicy")
    || PageName.ToLower().Trim().Contains("privacypolicy")
    || PageName.ToLower().Trim().Contains("termsandcondition")
    || PageName.ToLower().Trim().Contains("errorpage")
    || PageName.ToLower().Trim().Contains("error")
    || PageName.ToLower().Trim().Contains("razorpay")
    || PageName.ToLower().Trim().Contains("webhook")
    || PageName.ToLower().Trim().Contains("chat")


    )
    {
        await next();
        return;
    }

    //DBUtility.InsertPageList(PageName, fullUrl, UserID, ipAddress);

    string path = context.Request.Path;



    if ((path != "/" && path != "/Login" && path != "/Registration"))

    {
        if (context.Session.GetString("LoginType") == null)
        {
            context.Response.Redirect("/Login");
            return;
        }
    }

    string strPageNameToCheck = PageName.ToLower().Trim();

    var IsSiteIsDown = SettingsConfigHelper.IsSiteIsDown().ToLower().Trim();


   

    await next();

});
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.UseNToastNotify();
app.UseNotyf();




app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
});

SettingsConfigHelper.Appsetingconfig(app.Services.GetRequiredService<IConfiguration>());


app.MapControllers();
app.Run();
