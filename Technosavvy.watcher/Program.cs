global using NavExM.Int.Watcher.WatchDog.Model;
global using NavExM.Int.Watcher.WatchDog.Extention;
global using NavExM.Int.Watcher.WatchDog.Data;
global using NavExM.Int.Watcher.WatchDog.Service;
global using NavExM.Int.Watcher.WatchDog.WHub;
global using NavExM.Int.Watcher.WatchDog.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureServices(services => {
    services.Configure<HostOptions>(options => {
        options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
    });
});

// Add services to the container.
//builder.Services.AddCors(option => {
//    option.AddPolicy("navexmOnly", policy => {
//        policy.WithOrigins(
//            "http://navexm.com", 
//            "https://navexm.com", 
//            "http://navexm.com/", 
//            "https://navexm.com/").AllowAnyHeader().AllowAnyMethod();
//    });
//});
//ToDo: Naveen, Test Environment may need to be added here

builder.Services.AddCors(option => {
option.AddPolicy("cors", policy =>
{
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
});
});

builder.Services.AddSignalR();
builder.Services.AddHttpClient();
AppWorkerFactory.AddWorker(new SrvCompDataBroker()); 
AppWorkerFactory.AddWorker(new SrvCompIntMaintAPI());
AppWorkerFactory.AddWorker(new SrvCompWDDBLogger()); 
AppWorkerFactory.AddWorker(new SrvCompIntTradingAPI());
AppWorkerFactory.AddWorker(new SrvCompBroadcastWorker());
AppWorkerFactory.AddWorker(new SrvCompUMAPIs());
AppWorkerFactory.AddWorker(new SrvCompWatcherWallet());
AppWorkerFactory.AddWorker(new SrvCompWatcherPrice());
AppWorkerFactory.AddWorker(new SrvCompDataPersistence());
AppWorkerFactory.AddWorker(new SrvCompDataSummary());
AppWorkerFactory.AddWorker(new SrvCompTradingSettlement());
AppWorkerFactory.AddWorker(new SrvCompTradingArbitrage());
AppWorkerFactory.AddWorker(new SrvCompTradingCashback());
AppWorkerFactory.AddWorker(new SrvWalletWatcher());
AppWorkerFactory.AddWorker(new SrvCompNavCPrice());
AppWorkerFactory.AddWorker(new SrvCompDataBroadcast());
AppWorkerFactory.AddWorker(new SrvCompStaffAPIs());
AppWorkerFactory.AddWorker(new SrvCompStaffWeb());
AppWorkerFactory.AddWorker(new SrvCompDataAudit());


builder.Services.AddHostedService<LogBackgroundCaller>();
builder.Services.AddHostedService<WatchDogBGService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
//builder.Services.AddDbContext<ApiAppContext>(options =>
  //options.UseSqlServer(builder.Configuration.GetConnectionString("ApiDBContext")));

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;

//    var context = services.GetRequiredService<ApiAppContext>();
//    //context.Database.EnsureCreated();
//    //DbInitializer.Initialize(context);
//}


ConfigEx.Initialize(builder.Configuration);

//app.UseAuthorization();

app.MapControllers();
app.UseWebSockets();
app.MapHub<ErrorHub>("/broadcasterror");
app.MapHub<EventHub>("/broadcastevent");
app.MapHub<LogHub>("/broadcastlog");
app.MapHub<MyHub>("/my");

app.Run();
