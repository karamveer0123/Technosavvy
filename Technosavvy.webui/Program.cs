global using System.Linq;
global using Microsoft.AspNetCore.Mvc.ViewFeatures;
global using Microsoft.AspNetCore.Mvc.Rendering;
global using TechnoApp.Ext.Web.UI.ViewModels;
global using TechnoApp.Ext.Web.UI.Models;
global using TechnoApp.Ext.Web.UI.Extentions;
global using TechnoApp.Ext.Web.UI.Manager;
global using Microsoft.Extensions.Options;
global using Microsoft.AspNetCore.DataProtection;
global using Microsoft.EntityFrameworkCore;
global using TechnoApp.Ext.Web.UI.Static;
using TechnoApp.Ext.Web.UI.Service;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AuthUser", policy => policy.RequireClaim("AuthCode"));
});
builder.Services.Configure<SmtpConfig>(builder.Configuration.GetSection(SmtpConfig.Smtp));
builder.Services.AddDataProtection();
ConfigExtention.Initialize(builder.Configuration);
AppWorkerFactory.AddWorker(new SrvCurrencyPriceHUB());
AppWorkerFactory.AddWorker(new SrvPageEvents());
AppWorkerFactory.AddWorker(new SrvCoinPriceHUB());
AppWorkerFactory.AddWorker(new SrvPrebeta());
AppWorkerFactory.AddWorker(new SrvBroadcast());
builder.Services.AddHostedService<WebUIBGService>();
//builder.Services.AddAuthentication()
//        .AddCookie(options =>
//        {
//            options.LoginPath = "/Login/Login";
//            options.AccessDeniedPath = "/Login/Forbidden";
//        });
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);
  

//.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
    //    options => builder.Configuration.Bind("CookieSettings", options));
//builder.Services.AddAuthentication().AddGoogle(googleOptions => { googleOptions.ClientId = builder.Configuration.GetSection("GoogleOuth.ClientId").Value; googleOptions.ClientSecret = builder.Configuration.GetSection("GoogleOuth.ClientSecrit").Value; });

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
ConfigEx.Initialize(builder.Configuration);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<APIHub>("/Stream");

app.Run();
