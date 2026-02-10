using Microsoft.AspNetCore.Mvc;
using VolunteerManagement.Handlers.Volunteers.Pets.Queries.SearchPets;
using VolunteerManagement.Services.Volunteers.Dtos;
using Wolverine;

namespace VolunteerManagement.Hosts.Endpoints.Controllers;

/// <summary>
/// Контроллер для глобального поиска животных.
/// </summary>
/// <param name="bus">Шина сообщений Wolverine.</param>
[ApiController]
[Route("api/pets")]
public class PetController(IMessageBus bus) : ControllerBase
{
    /// <summary>
    /// Поиск животных по фильтрам с сортировкой и пагинацией.
    /// </summary>
    /// <param name="nickName">Кличка (частичное совпадение).</param>
    /// <param name="speciesId">Идентификатор вида.</param>
    /// <param name="breedId">Идентификатор породы.</param>
    /// <param name="helpStatus">Статус помощи.</param>
    /// <param name="city">Город.</param>
    /// <param name="shelterId">Идентификатор приюта.</param>
    /// <param name="birthDateFrom">Дата рождения от.</param>
    /// <param name="birthDateTo">Дата рождения до.</param>
    /// <param name="isCastrated">Кастрирован.</param>
    /// <param name="isVaccinated">Вакцинирован.</param>
    /// <param name="weightFrom">Вес от.</param>
    /// <param name="weightTo">Вес до.</param>
    /// <param name="heightFrom">Рост от.</param>
    /// <param name="heightTo">Рост до.</param>
    /// <param name="sortBy">Поле сортировки (dateCreated, nickName, birthDate, weight, height).</param>
    /// <param name="sortDirection">Направление сортировки (asc/desc).</param>
    /// <param name="page">Номер страницы.</param>
    /// <param name="count">Количество на странице.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PetDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Search(
        [FromQuery] string? nickName,
        [FromQuery] Guid? speciesId,
        [FromQuery] Guid? breedId,
        [FromQuery] int? helpStatus,
        [FromQuery] string? city,
        [FromQuery] Guid? shelterId,
        [FromQuery] DateTime? birthDateFrom,
        [FromQuery] DateTime? birthDateTo,
        [FromQuery] bool? isCastrated,
        [FromQuery] bool? isVaccinated,
        [FromQuery] double? weightFrom,
        [FromQuery] double? weightTo,
        [FromQuery] double? heightFrom,
        [FromQuery] double? heightTo,
        [FromQuery] string? sortBy,
        [FromQuery] string? sortDirection,
        [FromQuery] int page = 1,
        [FromQuery] int count = 10,
        CancellationToken ct = default)
    {
        var query = new SearchPetsQuery(
            nickName,
            speciesId,
            breedId,
            helpStatus,
            city,
            shelterId,
            birthDateFrom,
            birthDateTo,
            isCastrated,
            isVaccinated,
            weightFrom,
            weightTo,
            heightFrom,
            heightTo,
            sortBy,
            sortDirection,
            page,
            count);

        var result = await bus.InvokeAsync<IEnumerable<PetDto>>(query, ct);

        return Ok(result);
    }
}
