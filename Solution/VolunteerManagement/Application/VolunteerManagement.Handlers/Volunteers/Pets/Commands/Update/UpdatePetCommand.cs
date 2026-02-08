using PetFamily.SharedKernel.Domain.Primitives;
using VolunteerManagement.Services.Volunteers.Dtos;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Commands.Update;

/// <summary>
/// Команда на обновление животного.
/// </summary>
public sealed class UpdatePetCommand : Command
{
    /// <summary>
    /// Идентификатор волонтёра.
    /// </summary>
    public Guid VolunteerId { get; init; }

    /// <summary>
    /// Идентификатор животного.
    /// </summary>
    public Guid PetId { get; init; }

    /// <summary>
    /// Общее описание животного.
    /// </summary>
    public string GeneralDescription { get; init; } = string.Empty;

    /// <summary>
    /// Информация о здоровье животного.
    /// </summary>
    public string HealthInformation { get; init; } = string.Empty;

    /// <summary>
    /// Адрес местонахождения животного.
    /// </summary>
    public AddressDto Address { get; init; } = null!;

    /// <summary>
    /// Вес животного в кг.
    /// </summary>
    public double Weight { get; init; }

    /// <summary>
    /// Рост животного в см.
    /// </summary>
    public double Height { get; init; }

    /// <summary>
    /// Контактный номер телефона.
    /// </summary>
    public string PhoneNumber { get; init; } = string.Empty;

    /// <summary>
    /// Признак кастрации.
    /// </summary>
    public bool IsCastrated { get; init; }

    /// <summary>
    /// Признак вакцинации.
    /// </summary>
    public bool IsVaccinated { get; init; }

    /// <summary>
    /// Статус помощи (0 - ищет дом, 1 - ищет помощь, 2 - нашёл дом).
    /// </summary>
    public int HelpStatus { get; init; }

    /// <summary>
    /// Коллекция реквизитов для пожертвований.
    /// </summary>
    public IEnumerable<RequisiteDto> Requisites { get; init; } = [];
}
