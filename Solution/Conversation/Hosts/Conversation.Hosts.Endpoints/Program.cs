using Conversation.Hosts.Endpoints;
using PetFamily.SharedKernel.Infrastructure;
using PetFamily.SharedKernel.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogWithElk("conversation-api");

builder.Services.AddProgramDependencies(builder.Configuration);

var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();

using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
var migrator = scope.ServiceProvider.GetRequiredService<IMigrator>();
await migrator.Migrate(cts.Token);

app.UseSerilogRequestLogging();

app.UseGlobalErrorHandling();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Conversation API v1");
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
