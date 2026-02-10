using Account.Hosts.Endpoints;
using PetFamily.SharedKernel.Infrastructure;
using PetFamily.SharedKernel.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogWithElk("account-api");

builder.Services.AddProgramDependencies(builder.Configuration);

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var migrator = scope.ServiceProvider.GetRequiredService<IMigrator>();
    await migrator.Migrate();
}

app.UseSerilogRequestLogging();

app.UseGlobalErrorHandling();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Account API v1");
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
