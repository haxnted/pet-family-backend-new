using Microsoft.EntityFrameworkCore;

namespace PetFamily.SharedKernel.Infrastructure.Configurations;

/// <summary>
/// Конфигуратор контекста базы данных.
/// </summary>
public interface IDbContextOptionsConfigurator<TContext>
    where TContext : DbContext
{
    /// <summary>
    /// Настраивает контекст.
    /// </summary>
    /// <param name="options">Настройки.</param>
    void Configure(DbContextOptionsBuilder<TContext> options);
}