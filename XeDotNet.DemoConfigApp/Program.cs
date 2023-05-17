using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using XeDotNet.DemoConfigApp;
using XeDotNet.DemoConfigApp.Data;

var builder = WebApplication.CreateBuilder();

//builder.Configuration.Sources.Clear();

// Retrieve the connection strings
string? appConfigCnnString = builder.Configuration.GetConnectionString("AppConfig");
string? sqlServerCnnString = builder.Configuration.GetConnectionString("SqlServer");

if (sqlServerCnnString is not null)
    builder.Configuration.AddSqlServer(sqlServerCnnString, "Table", "Key", "Value");

if (appConfigCnnString is not null)
{
    // Load configuration from Azure App Configuration
    builder.Configuration.AddAzureAppConfiguration(appConfigCnnString);

    //builder.Configuration.AddAzureAppConfiguration(options =>
    //{
    //    options.Connect(appConfigCnnString)
    //    .ConfigureKeyVault(kv =>
    //    {
    //        kv.SetCredential(new DefaultAzureCredential());
    //    })
    //            .UseFeatureFlags(options => options.CacheExpirationInterval = TimeSpan.FromSeconds(10))
    //           // Load all keys that start with `TestApp:` and have no label
    //           .Select("SmtpServer:*", LabelFilter.Null)
    //           // Configure to reload configuration if the registered sentinel key is modified
    //           .ConfigureRefresh(refreshOptions =>
    //                refreshOptions.Register("SmtpServer:Sentinel", refreshAll: true));
    //});

    builder.Services.AddAzureAppConfiguration();
}

builder.Services.AddFeatureManagement()
    .AddFeatureFilter<PercentageFilter>()
    .AddFeatureFilter<TimeWindowFilter>()
    .AddFeatureFilter<ClaimsFeatureFilter>();

    //.AddFeatureFilter<TargetingFilter>()
    //TargetingFilter view https://github.com/microsoft/FeatureManagement-Dotnet


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddOptions<SmtpServerOptions>().Bind(builder.Configuration.GetSection(SmtpServerOptions.SmtpServer));

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (appConfigCnnString is not null) app.UseAzureAppConfiguration();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();