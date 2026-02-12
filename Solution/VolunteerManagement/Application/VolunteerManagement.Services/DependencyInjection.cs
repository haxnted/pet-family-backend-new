using Microsoft.Extensions.DependencyInjection;
using VolunteerManagement.Services.AnimalKinds;
using VolunteerManagement.Services.Shelters;
using VolunteerManagement.Services.Volunteers;
using VolunteerManagement.Services.Volunteers.Adoption;
using VolunteerManagement.Services.Volunteers.Pets;

namespace VolunteerManagement.Services;

/// <summary>
/// Класс для настройки зависимостей Application слоя.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавляет сервисы Application слоя в контейнер зависимостей.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IVolunteerService, VolunteerService>();
        services.AddScoped<IPetService, PetService>();
        services.AddScoped<ISpeciesService, SpeciesService>();
        services.AddScoped<IShelterService, ShelterService>();
        services.AddScoped<IPetSearchService, PetSearchService>();
        services.AddScoped<IPetAdoptionService, PetAdoptionService>();

        return services;
    }
}