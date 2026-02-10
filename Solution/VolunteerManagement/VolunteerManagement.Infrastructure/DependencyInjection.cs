using Microsoft.Extensions.DependencyInjection;
using PetFamily.SharedKernel.Infrastructure;
using PetFamily.SharedKernel.Infrastructure.Abstractions;
using VolunteerManagement.Domain.Aggregates.AnimalKinds;
using VolunteerManagement.Domain.Aggregates.Shelters;
using VolunteerManagement.Domain.Aggregates.Volunteers;
using VolunteerManagement.Domain.Aggregates.Volunteers.Entities;
using VolunteerManagement.Infrastructure.Common;
using VolunteerManagement.Infrastructure.Common.Contexts;

namespace VolunteerManagement.Infrastructure;

/// <summary>
/// Класс для настройки инфраструктуры зависимостей.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Настройка инфраструктуры зависимостей.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddDatabase<VolunteerManagementDbContext, VolunteerDbContextConfigurator>();

        services.AddScoped<IMigrator, VolunteerManagementMigrator>();

        services.AddScoped<IRepository<Volunteer>, EntityFrameworkRepository<Volunteer>>();
        services.AddScoped<IRepository<Species>, EntityFrameworkRepository<Species>>();
        services.AddScoped<IRepository<Shelter>, EntityFrameworkRepository<Shelter>>();
        services.AddScoped<IRepository<Pet>, EntityFrameworkRepository<Pet>>();
    }
}