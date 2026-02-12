using FileStorage.Application.Services;
using FileStorage.Consumers;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("MassTransit", LogEventLevel.Information)
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.Seq(
        Environment.GetEnvironmentVariable("Seq__Url") ?? "http://localhost:5341")
    .Enrich.WithProperty("Application", "FileStorage.Consumers")
    .CreateLogger();

try
{
    var builder = Host.CreateApplicationBuilder(args);

    builder.Services.AddSerilog();
    builder.Services.AddProgramDependencies(builder.Configuration);

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var minioService = scope.ServiceProvider.GetRequiredService<IMinIoService>();
        await minioService.EnsureBucketExistsAsync(CancellationToken.None);
        Log.Information("MinIO bucket initialized successfully");
    }

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "FileStorage Consumers Worker Service terminated unexpectedly");
    throw;
}
finally
{
    await Log.CloseAndFlushAsync();
}