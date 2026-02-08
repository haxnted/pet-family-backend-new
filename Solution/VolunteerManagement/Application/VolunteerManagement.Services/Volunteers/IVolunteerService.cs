using VolunteerManagement.Domain.Aggregates.Volunteers;

namespace VolunteerManagement.Services.Volunteers;

/// <summary>
/// Интерфейс-сервис для работы с волонтерами.
/// </summary>
public interface IVolunteerService
{
    /// <summary>
    /// Добавить нового волонтёра.
    /// </summary>
    /// <param name="name">Имя.</param>
    /// <param name="surname">Фамилия.</param>
    /// <param name="patronymic">Отчество.</param>
    /// <param name="userId">Идентификатор волонтёра.</param>
    /// <param name="generalDescription">Общее описание.</param>
    /// <param name="ct">Токен отмены.</param>
    Task AddAsync(
        string name,
        string surname,
        string? patronymic,
        Guid userId,
        string generalDescription,
        CancellationToken ct);

    /// <summary>
    /// Обновить основные данные об волонтере.
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="generalDescription">Общее описание.</param>
    /// <param name="ageExperience">Опыт работы в годах.</param>
    /// <param name="phoneNumber">Номер телефона.</param>
    /// <param name="ct">Токен отмены.</param>
    Task UpdateAsync(
        Guid volunteerId,
        string generalDescription,
        int? ageExperience,
        string? phoneNumber,
        CancellationToken ct);

    /// <summary>
    /// Полностью удаляет волонтёра из системы.
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="ct">Токен отмены.</param>
    Task HardRemoveAsync(Guid volunteerId, CancellationToken ct);

    /// <summary>
    /// Скрывает волонтёра.
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <remarks>
    /// Переключает IsDeleted у сущности в состояние true, и при следующих запросах
    /// этот волонтёр не будет попадать в выборку.
    /// </remarks>
    Task SoftRemoveAsync(Guid volunteerId, CancellationToken ct);

    /// <summary>
    /// Получить волонтёра.
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="ct">Токен отмены.</param>
    Task<Volunteer> GetAsync(Guid volunteerId, CancellationToken ct);

    /// <summary>
    /// Получить волонтёров с пагинацией.
    /// </summary>
    /// <param name="page">Порядковый номер страницы.</param>
    /// <param name="count">Количество элементов отображаемых на странице.</param>
    /// <param name="ct">Токен отмены.</param>
    Task<IReadOnlyList<Volunteer>> GetWithPaginationAsync(int page, int count, CancellationToken ct);

    /// <summary>
    /// Активировать аккаунт волонтёра (восстановить после мягкого удаления).
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="ct">Токен отмены.</param>
    Task ActivateAccountVolunteerRequest(Guid volunteerId, CancellationToken ct);

    /// <summary>
    /// Жёстко удалить всех питомцев волонтёра.
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="ct">Токен отмены.</param>
    Task HardRemoveAllPetsAsync(Guid volunteerId, CancellationToken ct);
}