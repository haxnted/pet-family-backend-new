using FileStorage.Api;
using FileStorage.Application.Services;
using PetFamily.SharedKernel.WebApi.Diagnostics;
using PetFamily.SharedKernel.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogWithElk("filestorage-api");

builder.Services.AddProgramDependencies(builder.Configuration);

builder.Services.AddOpenTelemetryTracing(
    builder.Configuration,
    serviceName: DiagnosticNames.FileStorage,
    serviceVersion: "1.0.0");

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseGlobalErrorHandling();

await using (var scope = app.Services.CreateAsyncScope())
{
    var minioService = scope.ServiceProvider.GetRequiredService<IMinIoService>();
    await minioService.EnsureBucketExistsAsync(CancellationToken.None);
}

// Enable Swagger for non-production environments
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FileStorage API v1");
        c.RoutePrefix = string.Empty;
        c.DisplayRequestDuration();
    });
}

app.UsePrometheusMetrics();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthCheckEndpoints();

app.MapControllers();

app.Run();