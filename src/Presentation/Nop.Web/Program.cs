using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Configuration;
using Nop.Web.Framework.Infrastructure.Extensions;
using Nop.Web.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Configuration.AddJsonFile(NopConfigurationDefaults.AppSettingsFilePath, true, true);
if (!string.IsNullOrEmpty(builder.Environment?.EnvironmentName))
{
    var path = string.Format(NopConfigurationDefaults.AppSettingsEnvironmentFilePath, builder.Environment.EnvironmentName);
    builder.Configuration.AddJsonFile(path, true, true);
}
builder.Configuration.AddEnvironmentVariables();

//Add services to the application and configure service provider
builder.Services.ConfigureApplicationServices(builder);

builder.Services.AddSignalR();

var app = builder.Build();


app.MapHub<SignalREventHub>("/eventHub");

//Configure the application HTTP request pipeline
app.ConfigureRequestPipeline();
app.StartEngine();

app.Run();
