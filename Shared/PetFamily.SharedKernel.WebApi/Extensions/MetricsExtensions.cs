using Microsoft.AspNetCore.Builder;
using Prometheus;

namespace PetFamily.SharedKernel.WebApi.Extensions;

/// <summary>
/// Extension методы для настройки метрик Prometheus.
/// </summary>
public static class MetricsExtensions
{
    /// <summary>
    /// Настраивает middleware для сбора метрик HTTP запросов и экспорта в Prometheus.
    /// </summary>
    /// <param name="app">WebApplication.</param>
    /// <returns>WebApplication для цепочки вызовов.</returns>
    public static WebApplication UsePrometheusMetrics(this WebApplication app)
    {
        app.UseHttpMetrics();

        app.MapMetrics();

        return app;
    }
}
