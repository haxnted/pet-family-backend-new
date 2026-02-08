namespace VolunteerManagement.Handlers.Volunteers.Queries.GetVolunteersWithPagination;

/// <summary>
/// Получение волонтёров с пагинацией.
/// </summary>
/// <param name="Page">Текущая страница.</param>
/// <param name="Count">Количество страниц.</param>
public record GetVolunteersWithPaginationQuery(int Page, int Count);