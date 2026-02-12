using ApiGateway;
using Ocelot.Middleware;
using PetFamily.SharedKernel.WebApi.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("Configuration/ocelot.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"Configuration/ocelot.{builder.Environment.EnvironmentName}.json",
        optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddProgramDependencies(builder.Configuration);

var app = builder.Build();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthCheckEndpoints();

await app.UseOcelot();

app.Run();