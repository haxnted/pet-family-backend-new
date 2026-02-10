namespace VolunteerManagement.Handlers.Volunteers.Pets.Queries.SearchPets;

/// <summary>
/// Запрос на поиск животных по фильтрам.
/// </summary>
/// <param name="NickName">Кличка (частичное совпадение).</param>
/// <param name="SpeciesId">Идентификатор вида.</param>
/// <param name="BreedId">Идентификатор породы.</param>
/// <param name="HelpStatus">Статус помощи.</param>
/// <param name="City">Город.</param>
/// <param name="ShelterId">Идентификатор приюта.</param>
/// <param name="BirthDateFrom">Дата рождения от.</param>
/// <param name="BirthDateTo">Дата рождения до.</param>
/// <param name="IsCastrated">Кастрирован.</param>
/// <param name="IsVaccinated">Вакцинирован.</param>
/// <param name="WeightFrom">Вес от.</param>
/// <param name="WeightTo">Вес до.</param>
/// <param name="HeightFrom">Рост от.</param>
/// <param name="HeightTo">Рост до.</param>
/// <param name="SortBy">Поле сортировки (dateCreated, nickName, birthDate, weight, height).</param>
/// <param name="SortDirection">Направление сортировки (asc/desc).</param>
/// <param name="Page">Номер страницы.</param>
/// <param name="Count">Количество на странице.</param>
public sealed record SearchPetsQuery(
    string? NickName,
    Guid? SpeciesId,
    Guid? BreedId,
    int? HelpStatus,
    string? City,
    Guid? ShelterId,
    DateTime? BirthDateFrom,
    DateTime? BirthDateTo,
    bool? IsCastrated,
    bool? IsVaccinated,
    double? WeightFrom,
    double? WeightTo,
    double? HeightFrom,
    double? HeightTo,
    string? SortBy,
    string? SortDirection,
    int Page,
    int Count);
