using VolunteerManagement.Domain.Aggregates.Volunteers.Entities;

namespace VolunteerManagement.Services.Volunteers.Pets;

/// <summary>
/// Интерфейс-сервис для поиска животных по фильтрам.
/// </summary>
public interface IPetSearchService
{
    /// <summary>
    /// Найти животных по фильтрам с сортировкой и пагинацией.
    /// </summary>
    /// <param name="nickName">Кличка (частичное совпадение).</param>
    /// <param name="speciesId">Идентификатор вида.</param>
    /// <param name="breedId">Идентификатор породы.</param>
    /// <param name="helpStatus">Статус помощи.</param>
    /// <param name="shelterId">Идентификатор приюта.</param>
    /// <param name="birthDateFrom">Дата рождения от.</param>
    /// <param name="birthDateTo">Дата рождения до.</param>
    /// <param name="isCastrated">Кастрирован.</param>
    /// <param name="isVaccinated">Вакцинирован.</param>
    /// <param name="weightFrom">Вес от.</param>
    /// <param name="weightTo">Вес до.</param>
    /// <param name="heightFrom">Рост от.</param>
    /// <param name="heightTo">Рост до.</param>
    /// <param name="sortBy">Поле сортировки.</param>
    /// <param name="sortDirection">Направление сортировки (asc/desc).</param>
    /// <param name="page">Номер страницы.</param>
    /// <param name="count">Количество элементов на странице.</param>
    /// <param name="ct">Токен отмены.</param>
    Task<IEnumerable<Pet>> SearchAsync(
        string? nickName,
        Guid? speciesId,
        Guid? breedId,
        int? helpStatus,
        Guid? shelterId,
        DateTime? birthDateFrom,
        DateTime? birthDateTo,
        bool? isCastrated,
        bool? isVaccinated,
        double? weightFrom,
        double? weightTo,
        double? heightFrom,
        double? heightTo,
        string? sortBy,
        string? sortDirection,
        int page,
        int count,
        CancellationToken ct);
}