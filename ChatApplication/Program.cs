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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using NToastNotify;
using System.ComponentModel.Design;

//var logger = CreateNlogLogger();
//Logger CreateNlogLogger()

//{
//    //LogManager.Configuration = new NLogLoggingConfiguration(configuration.GetSection("Nlog"));
//    var logger = LogManager.Setup()
//        .LoadConfigurationFromAppSettings()
//        .GetCurrentClassLogger();
//    logger.Info("nlog initiation started");
//    try
//    {

//        logger.Debug("init main");
//        logger.Debug("Environment:");
//        //	logger.Debug(enviroment);
//    }
//    catch (Exception exception)
//    {
//        logger.Error(exception, "Stopped program of exception");
//        throw;
//    }
//    finally
//    {
//        NLog.LogManager.Shutdown();
//    }
//    return logger;
//}

var builder = WebApplication.CreateBuilder(args);
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
app.UseHttpsRedirection();
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

    DBUtility.InsertPageList(PageName, fullUrl, UserID, ipAddress);
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


    //string strSQL = "insert into PageList(PageName) values ('" + PageName.Replace("'","''")  + "')";

    //DBUtility.ExecuteSQL(strSQL);

    DBUtility.InsertPageList(PageName, fullUrl, UserID, ipAddress);

    string path = context.Request.Path;



    if ((path != "/" && path != "/Login" && path != "/PersonalDetails" && path != "/RegistrationPage" && path != "/ForgotPassword" && path != "/OTPVerify" && path != "/CreateNewPass" && path != "/Error") && (path != "/api/Order/InsertOrderDetails") && (path != "/VadhuVarMelava") && (path != "/Terms&Conditions") && (path != "/Privacy&Policy") && (path != "/api/Order/InsertGatheringDetails") && (path != "/api/Resend/InsertOtp"))

    {
        if (context.Session.GetString("LoginType") == null)
        {
            context.Response.Redirect("/Login");
            return;
        }
    }


    string[] adminPagesArray = new[]
    {
        "lookupmst",
        "admindashboard",
        "locationlist",
        "pagerole",
        "admin/customerlist",
        "customerimgsadmin",
        "customerimagesall",
         "admin/custsubscriptionlist",
         "admin/GatheringList",
          "admin/ConfigurationList",
          "admin/AddList",
          "admin/Member_Ship_Mst",
          "admin/OfflineMember_Ship",
          "admin/ChatHistoryList"

    };



    string strPageNameToCheck = PageName.ToLower().Trim();

    var IsSiteIsDown = SettingsConfigHelper.IsSiteIsDown().ToLower().Trim();


    if (strPageNameToCheck == "login" && IsSiteIsDown == "true")
    {
        context.Response.Redirect("/index");
        return;
    }

    if (strPageNameToCheck != "login")
    {

        foreach (string singlePage in adminPagesArray)
        {
            if (strPageNameToCheck.Contains(singlePage))
            {
                if (context.Session.GetString("LoginType") != "superadmin")
                {
                    context.Response.Redirect("/Login");
                    return;
                }
            }
        }



        string[] customerPagesArray = new[]
        {
            "advancesearch",
            "api/interested/insertinteresteddetails",
            "api/search/customerapprovedimgs",
            "api/search/customerimgs",
            "api/search/searchdetails",
            "api/v1/mymatches/getmydashboarddetails",
            "biodata",
            "changepassword",
            "customerimageuploadgallary",
            "dashboard",
            "disableaccount",
            "imageeditingtool",
            "knowmore",
            "membership",
            "mymatches",
            "myprofile",
            "preference",
            "regularsearch",
            "youandme"
        };


        foreach (string singlePage in customerPagesArray)
        {
            if (strPageNameToCheck.Contains(singlePage)
            && !strPageNameToCheck.Contains("admindashboard")
            && !strPageNameToCheck.Contains("customerimgsadmin")
            )
            {
                if (string.IsNullOrEmpty(context.Session.GetString("CustomerID")))
                {
                    context.Response.Redirect("/Login");
                    return;
                }

                if (context.Session.GetString("IsCompleted") != "1")
                {
                    context.Response.Redirect("/Login");
                    return;
                }
            }
        }

        // check registraction page
        if (strPageNameToCheck == "PersonalDetails".ToLower().Trim())
        {
            if (context.Session.GetString("IsCompleted") == "1")
            {
                context.Response.Redirect("/Login");
                return;
            }
        }
    }


    await next();

});

app.UseAuthorization();



app.MapRazorPages();
app.UseNToastNotify();
app.UseNotyf();
app.UseAuthentication();

//app.Use(async (context, next) =>
//{
//    string path = context.Request.Path;

//    if (path != null &&
//     (path.EndsWith(".css") || path.EndsWith(".js") || path.EndsWith(".png") ||
//      path.EndsWith(".jpg") || path.EndsWith(".jpeg") || path.EndsWith(".gif") ||
//      path.EndsWith(".svg") || path.EndsWith(".woff") || path.EndsWith(".woff2") ||
//      path.EndsWith(".ttf") || path.EndsWith(".ico")))
//    {
//        await next();
//        return;
//    }

//    if ((path != "/" && path != "/Index" && path != "/RegistrationPage" && path != "/ForgotPassword" && path != "/OTPVerify" && path != "/CreateNewPass" && path != "/Error") && context.Session.GetString("CustomerID") == null)
//    {
//        context.Response.Redirect("/Index");
//        return;
//    }

//    await next();
//    return;

//});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
});
//app.UseEndpoints(endpoints => { endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}"); });
SettingsConfigHelper.Appsetingconfig(app.Services.GetRequiredService<IConfiguration>());
//using (var scope = app.Services.CreateScope())
//{
//    var lookupService = scope.ServiceProvider.GetRequiredService<ILookUpMst>();
//    var coutryStateCityService = scope.ServiceProvider.GetRequiredService<ICountryStateCityService>();
//    await lookupService.GetAllLookupAsync();
//    await coutryStateCityService.GetAllCSCAsync();
//}


app.MapControllers();
//app.MapStaticAssets();
//app.MapRazorPages()
//   .WithStaticAssets();
//app.MapHub<ChatHub>("/chatHub");

app.Run();
