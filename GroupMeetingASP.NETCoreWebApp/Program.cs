using AspNetCoreHero.ToastNotification;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
	.ReadFrom.Configuration(builder.Configuration)
	.Enrich.FromLogContext()
	.WriteTo.Console()
	.WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
	.CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Simple session storage setup
 builder.Services.AddDistributedMemoryCache();

 builder.Services.AddSession(options =>
 {
     options.IdleTimeout = TimeSpan.FromMinutes(30);
     options.Cookie.HttpOnly = true;
     options.Cookie.IsEssential = true;
 });

builder.Services.AddNotyf(config =>
{
	config.DurationInSeconds = 10;
	config.IsDismissable = true;
	config.Position = NotyfPosition.TopRight;
});

//var redis = ConnectionMultiplexer
//	.Connect(Environment.GetEnvironmentVariable("Redis"));

//builder.Services
//	.AddDataProtection()
//	.PersistKeysToStackExchangeRedis(redis, "DataProtectionKeys");

//builder.Services.AddStackExchangeRedisCache(redisOptions =>
//{
//	//option.Configuration = Environment
//	//	.GetEnvironmentVariable("Redis");
//	//option.InstanceName = "RedisInstance";

//	string connection = builder.Configuration.GetConnectionString("Redis");

//	redisOptions.Configuration = connection;

//});

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=RegisterUser}/{action=Signup}/{id?}");

app.Run();
