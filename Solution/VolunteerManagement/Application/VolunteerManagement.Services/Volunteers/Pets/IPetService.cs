using VolunteerManagement.Domain.Aggregates.Volunteers.Entities;
using VolunteerManagement.Services.Volunteers.Dtos;

namespace VolunteerManagement.Services.Volunteers.Pets;

/// <summary>
/// Интерфейс-сервис для работы с животными.
/// </summary>
public interface IPetService
{
    /// <summary>
    /// Добавить животное волонтёру.
    /// </summary>
    Task<Guid> AddPet(
        Guid volunteerId,
        string nickName,
        string description,
        string healthInformation,
        Guid breedId,
        Guid speciesId,
        double weight,
        double height,
        DateTime birthDate,
        bool isCastrated,
        bool isVaccinated,
        int helpStatus,
        IEnumerable<RequisiteDto> requisites,
        CancellationToken ct);

    /// <summary>
    /// Обновить информацию о животном.
    /// </summary>
    Task UpdatePet(
        Guid volunteerId,
        Guid petId,
        string description,
        string healthInformation,
        double weight,
        double height,
        bool isCastrated,
        bool isVaccinated,
        int helpStatus,
        IEnumerable<RequisiteDto> requisites,
        CancellationToken ct);

    /// <summary>
    /// Удалить животное.
    /// </summary>
    Task DeletePet(Guid volunteerId, Guid petId, CancellationToken ct);

    /// <summary>
    /// Получить животное по идентификатору.
    /// </summary>
    Task<Pet> GetPetById(Guid volunteerId, Guid petId, CancellationToken ct);

    /// <summary>
    /// Получить всех животных волонтёра.
    /// </summary>
    Task<IReadOnlyList<Pet>> GetPetsByVolunteerId(Guid volunteerId, CancellationToken ct);

    /// <summary>
    /// Мягкое удаление питомца.
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="petId">Идентификатор питомца.</param>
    /// <param name="ct">Токен отмены.</param>
    Task SoftDeletePetAsync(Guid volunteerId, Guid petId, CancellationToken ct);

    /// <summary>
    /// Восстановить питомца.
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="petId">Идентификатор питомца.</param>
    /// <param name="ct">Токен отмены.</param>
    Task RestorePetAsync(Guid volunteerId, Guid petId, CancellationToken ct);

    /// <summary>
    /// Переместить питомца на новую позицию.
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="petId">Идентификатор питомца.</param>
    /// <param name="newPosition">Новая позиция.</param>
    /// <param name="ct">Токен отмены.</param>
    Task MovePetAsync(Guid volunteerId, Guid petId, int newPosition, CancellationToken ct);
}
