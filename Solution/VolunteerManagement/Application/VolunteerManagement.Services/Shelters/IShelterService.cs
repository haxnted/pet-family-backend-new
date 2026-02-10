using VolunteerManagement.Domain.Aggregates.Shelters;

namespace VolunteerManagement.Services.Shelters;

/// <summary>
/// Интерфейс-сервис для работы с приютами.
/// </summary>
public interface IShelterService
{
    /// <summary>
    /// Добавить новый приют.
    /// </summary>
    /// <param name="name">Название.</param>
    /// <param name="street">Улица.</param>
    /// <param name="city">Город.</param>
    /// <param name="state">Регион.</param>
    /// <param name="zipCode">Почтовый индекс.</param>
    /// <param name="phoneNumber">Номер телефона.</param>
    /// <param name="description">Описание.</param>
    /// <param name="openTime">Время открытия.</param>
    /// <param name="closeTime">Время закрытия.</param>
    /// <param name="capacity">Вместимость.</param>
    /// <param name="ct">Токен отмены.</param>
    Task AddAsync(
        string name,
        string street,
        string city,
        string state,
        string zipCode,
        string phoneNumber,
        string description,
        TimeOnly openTime,
        TimeOnly closeTime,
        int capacity,
        CancellationToken ct);

    /// <summary>
    /// Обновить основные данные о приюте.
    /// </summary>
    /// <param name="shelterId">Идентификатор приюта.</param>
    /// <param name="name">Название.</param>
    /// <param name="phoneNumber">Номер телефона.</param>
    /// <param name="description">Описание.</param>
    /// <param name="openTime">Время открытия.</param>
    /// <param name="closeTime">Время закрытия.</param>
    /// <param name="capacity">Вместимость.</param>
    /// <param name="ct">Токен отмены.</param>
    Task UpdateAsync(
        Guid shelterId,
        string name,
        string phoneNumber,
        string description,
        TimeOnly openTime,
        TimeOnly closeTime,
        int capacity,
        CancellationToken ct);

    /// <summary>
    /// Полностью удалить приют.
    /// </summary>
    /// <param name="shelterId">Идентификатор приюта.</param>
    /// <param name="ct">Токен отмены.</param>
    Task HardRemoveAsync(Guid shelterId, CancellationToken ct);

    /// <summary>
    /// Мягко удалить приют.
    /// </summary>
    /// <param name="shelterId">Идентификатор приюта.</param>
    /// <param name="ct">Токен отмены.</param>
    Task SoftRemoveAsync(Guid shelterId, CancellationToken ct);

    /// <summary>
    /// Получить приют по идентификатору.
    /// </summary>
    /// <param name="shelterId">Идентификатор приюта.</param>
    /// <param name="ct">Токен отмены.</param>
    Task<Shelter> GetAsync(Guid shelterId, CancellationToken ct);

    /// <summary>
    /// Получить приюты с пагинацией.
    /// </summary>
    /// <param name="page">Номер страницы.</param>
    /// <param name="count">Количество на странице.</param>
    /// <param name="ct">Токен отмены.</param>
    Task<IReadOnlyList<Shelter>> GetWithPaginationAsync(int page, int count, CancellationToken ct);

    /// <summary>
    /// Обновить адрес приюта.
    /// </summary>
    /// <param name="shelterId">Идентификатор приюта.</param>
    /// <param name="street">Улица.</param>
    /// <param name="city">Город.</param>
    /// <param name="state">Регион.</param>
    /// <param name="zipCode">Почтовый индекс.</param>
    /// <param name="ct">Токен отмены.</param>
    Task UpdateAddressAsync(
        Guid shelterId,
        string street,
        string city,
        string state,
        string zipCode,
        CancellationToken ct);

    /// <summary>
    /// Изменить статус приюта.
    /// </summary>
    /// <param name="shelterId">Идентификатор приюта.</param>
    /// <param name="newStatus">Новый статус.</param>
    /// <param name="ct">Токен отмены.</param>
    Task ChangeStatusAsync(Guid shelterId, int newStatus, CancellationToken ct);

    /// <summary>
    /// Назначить волонтёра в приют.
    /// </summary>
    /// <param name="shelterId">Идентификатор приюта.</param>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="role">Роль волонтёра в приюте.</param>
    /// <param name="ct">Токен отмены.</param>
    Task AssignVolunteerAsync(Guid shelterId, Guid volunteerId, int role, CancellationToken ct);

    /// <summary>
    /// Убрать волонтёра из приюта.
    /// </summary>
    /// <param name="shelterId">Идентификатор приюта.</param>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="ct">Токен отмены.</param>
    Task RemoveVolunteerAsync(Guid shelterId, Guid volunteerId, CancellationToken ct);

    /// <summary>
    /// Получить назначение волонтёра по идентификатору.
    /// </summary>
    /// <param name="shelterId">Идентификатор приюта.</param>
    /// <param name="assignmentId">Идентификатор назначения.</param>
    /// <param name="ct">Токен отмены.</param>
    Task<Shelter> GetWithAssignmentAsync(Guid shelterId, Guid assignmentId, CancellationToken ct);
}
