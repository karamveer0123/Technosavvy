global using Microsoft.EntityFrameworkCore;
global using NavExM.Int.Maintenance.APIs.Data;
global using NavExM.Int.Maintenance.APIs.Data.Entity;
global using NavExM.Int.Maintenance.APIs.Manager;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using NavExM.Int.Maintenance.APIs.Extension;
global using NavExM.Int.Maintenance.APIs.Model;
//global using NavExM.Int.Maintenance.APIs.Manager;
global using NavExM.Int.Maintenance.APIs.Static;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using NavExM.Int.Maintenance.APIs.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(option =>
{
    option.AddPolicy("cors", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMvc();
ConfigExtention.Initialize(builder.Configuration);
ConfigEx.Initialize(builder.Configuration);
builder.Services.AddDbContext<ApiAppContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("ApiDBContext")));
builder.Services.AddDbContext<CareerAppContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("CareerAppContext")));
builder.Services.AddDbContext<ContentAppContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("ContentAppContext")));
//if (ConfigEx.VersionType == versionType.PreBeta)
builder.Services.AddDbContext<PreBetaDBContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("PreBetaDBContext")));
builder.Services.AddDbContext<EventAppContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("EventDBContext")));
builder.Services.AddDbContext<RewardAppContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("RewardAppContext")));
builder.Services.Configure<SmtpConfig>(builder.Configuration.GetSection(SmtpConfig.Smtp));
builder.Services.AddHostedService<MaintBGService>();


builder.Services.AddControllers(o =>
{
    o.Filters.Add(new ProducesAttribute("application/json"));
    o.RespectBrowserAcceptHeader = true;
}).AddNewtonsoftJson(o => o.SerializerSettings.ContractResolver = new DefaultContractResolver());




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var smtp = services.GetRequiredService<IOptions<SmtpConfig>>();
    var context = services.GetRequiredService<ApiAppContext>();
    context.Database.EnsureCreated();
    APIDbInitializer.Initialize(context);

    var ctxContent = services.GetRequiredService<ContentAppContext>();
    ctxContent.Database.EnsureCreated();
    ContentDbInitializer.Initialize(ctxContent);
    if (ConfigEx.VersionType == versionType.PreBeta)
    {
        var pbctxContent = services.GetRequiredService<PreBetaDBContext>();
        pbctxContent.Database.EnsureCreated();
        PBDbInitializer.Initialize(pbctxContent);
    }
    //if (ConfigEx.VersionType != ConfigEx.versionType.PreBeta)
    {
        AppWorkerFactory.AddWorker(new SrvOnDemandFundChecker());
        AppWorkerFactory.AddWorker(new SrvFundReceiver(smtp.Value));
        AppWorkerFactory.AddWorker(new SrvEthNetworkWalletWorker());
        AppWorkerFactory.AddWorker(new SrvStakingReFunds());
        AppWorkerFactory.AddWorker(new SrvStakingRenewal());
        AppWorkerFactory.AddWorker(new SrvCurrencyWatch());
        AppWorkerFactory.AddWorker(new Srv24HrsChangeWatch());
        AppWorkerFactory.AddWorker(new SrvCoinWatch());
        AppWorkerFactory.AddWorker(new SrvPlugIn());
        AppWorkerFactory.AddWorker(new SrvMarketProxy());
        AppWorkerFactory.AddWorker(new SrvNavCCashbackPool());
    }
}



app.UseAuthorization();

app.MapControllers();
app.MapHub<APIHub>("/MAPIStream");

app.Run();
