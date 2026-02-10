namespace VolunteerManagement.Handlers.Shelters.Queries.GetSheltersWithPagination;

/// <summary>
/// Запрос на получение приютов с пагинацией.
/// </summary>
/// <param name="Page">Номер страницы.</param>
/// <param name="Count">Количество на странице.</param>
public sealed record GetSheltersWithPaginationQuery(int Page, int Count);
