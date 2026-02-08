using Microsoft.EntityFrameworkCore;
using PetFamily.SharedKernel.Infrastructure.Configurations;
using VolunteerManagement.Infrastructure.Common.Configurations;

namespace VolunteerManagement.Infrastructure.Common.Contexts;

/// <summary>
/// Сборщик для контекста <see cref="VolunteerManagementDbContext"/>.
/// </summary>
internal sealed class CustomModelBuilder
{
    /// <summary>
    /// Собирает контекст конфигуратора EF для <see cref="VolunteerManagementDbContext"/>.
    /// </summary>
    /// <param name="modelBuilder">Конфигуратор модели.</param>
    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PetConfiguration());
        modelBuilder.ApplyConfiguration(new VolunteerConfiguration());
        modelBuilder.ApplyConfiguration(new SpeciesConfiguration());
        modelBuilder.ApplyConfiguration(new BreedConfiguration());

        modelBuilder.SetDefaultDateTimeKind(DateTimeKind.Utc);
    }
}