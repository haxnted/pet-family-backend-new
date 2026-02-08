using PetFamily.SharedKernel.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseGlobalErrorHandling();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Volunteer API v1");
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