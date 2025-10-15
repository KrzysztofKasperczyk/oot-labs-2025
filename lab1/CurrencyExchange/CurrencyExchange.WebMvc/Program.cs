using CurrencyExchange.Domain.Rates;
using CurrencyExchange.Domain.Services;
using CurrencyExchange.Infrastructure.Cache;
using CurrencyExchange.Infrastructure.Rates;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var tableAUrl = builder.Configuration["Nbp:TableAUrl"]
                ?? throw new Exception("Missing Nbp:TableAUrl in appsettings.json");


builder.Services.AddSingleton<INbpApiClient>(sp => new NbpApiClient(tableAUrl));
builder.Services.AddSingleton<INbpXmlParser, NbpXmlParser>();
builder.Services.AddSingleton<RateCache>(_ => RateCache.Instance());
builder.Services.AddSingleton<IExchangeRateProvider, NbpExchangeRateProvider>();
builder.Services.AddSingleton<CurrencyConverter>();

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute("default", "{controller=Currency}/{action=Index}/{id?}");
app.Run();
