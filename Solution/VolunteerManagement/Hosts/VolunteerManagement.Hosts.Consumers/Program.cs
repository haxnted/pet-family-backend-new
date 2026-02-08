using Serilog;
using Serilog.Events;
using VolunteerManagement.Hosts.Consumers;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("MassTransit", LogEventLevel.Information)
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.Seq(
        Environment.GetEnvironmentVariable("Seq__Url") ?? "http://localhost:5341")
    .Enrich.WithProperty("Application", "VolunteerManagement.Consumers")
    .CreateLogger();

try
{
    var builder = Host.CreateApplicationBuilder(args);

    builder.Services.AddSerilog();
    builder.Services.AddProgramDependencies(builder.Configuration);

    var app = builder.Build();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "VolunteerManagement Consumers Worker Service terminated unexpectedly");
    throw;
}
finally
{
    await Log.CloseAndFlushAsync();
}