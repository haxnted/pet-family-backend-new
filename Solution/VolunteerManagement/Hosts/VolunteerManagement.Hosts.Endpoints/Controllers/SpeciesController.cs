using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolunteerManagement.Handlers.AnimalKinds.Commands.Add;
using VolunteerManagement.Handlers.AnimalKinds.Commands.AddBreed;
using VolunteerManagement.Handlers.AnimalKinds.Commands.DeleteBreed;
using VolunteerManagement.Handlers.AnimalKinds.Commands.DeleteSpecies;
using VolunteerManagement.Handlers.AnimalKinds.Commands.RestoreBreed;
using VolunteerManagement.Handlers.AnimalKinds.Commands.RestoreSpecies;
using VolunteerManagement.Handlers.AnimalKinds.Queries.GetAllSpecies;
using VolunteerManagement.Handlers.AnimalKinds.Queries.GetSpeciesById;
using VolunteerManagement.Hosts.Endpoints.Requests;
using VolunteerManagement.Services.AnimalKinds.Dtos;
using Wolverine;

namespace VolunteerManagement.Hosts.Endpoints.Controllers;

/// <summary>
/// Контроллер для работы с видами и породами животных.
/// </summary>
/// <param name="bus">Шина сообщений Wolverine.</param>
[ApiController]
[Route("api/species")]
public class SpeciesController(IMessageBus bus) : ControllerBase
{
    /// <summary>
    /// Добавить новый вид животного.
    /// </summary>
    /// <param name="request">Запрос на добавление вида.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Идентификатор созданного вида.</returns>
    [HttpPost]
    [AllowAnonymous]
    [Authorize(Policy = "AdminPolicy")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddSpecies(
        [FromBody] AddSpeciesRequest request,
        CancellationToken ct)
    {
        var command = new AddSpeciesCommand
        {
            AnimalKind = request.AnimalKind
        };

        var speciesId = await bus.InvokeAsync<Guid>(command, ct);

        return Ok(speciesId);
    }

    /// <summary>
    /// Добавить породу к виду животного.
    /// </summary>
    /// <param name="speciesId">Идентификатор вида.</param>
    /// <param name="request">Запрос на добавление породы.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Идентификатор созданной породы.</returns>
    [HttpPost("{speciesId:guid}/breeds")]
    [Authorize(Policy = "AdminPolicy")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddBreed(
        [FromRoute] Guid speciesId,
        [FromBody] AddBreedRequest request,
        CancellationToken ct)
    {
        var command = new AddBreedCommand
        {
            SpeciesId = speciesId,
            BreedName = request.BreedName
        };

        var breedId = await bus.InvokeAsync<Guid>(command, ct);

        return Ok(breedId);
    }

    /// <summary>
    /// Удалить вид животного (soft delete).
    /// </summary>
    /// <param name="speciesId">Идентификатор вида.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Результат операции.</returns>
    [HttpDelete("{speciesId:guid}")]
    [Authorize(Policy = "AdminPolicy")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSpecies(
        [FromRoute] Guid speciesId,
        CancellationToken ct)
    {
        var command = new DeleteSpeciesCommand
        {
            SpeciesId = speciesId
        };

        await bus.InvokeAsync(command, ct);

        return NoContent();
    }

    /// <summary>
    /// Удалить породу (soft delete).
    /// </summary>
    /// <param name="speciesId">Идентификатор вида.</param>
    /// <param name="breedId">Идентификатор породы.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Результат операции.</returns>
    [HttpDelete("{speciesId:guid}/breeds/{breedId:guid}")]
    [Authorize(Policy = "AdminPolicy")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBreed(
        [FromRoute] Guid speciesId,
        [FromRoute] Guid breedId,
        CancellationToken ct)
    {
        var command = new DeleteBreedCommand
        {
            SpeciesId = speciesId,
            BreedId = breedId
        };

        await bus.InvokeAsync(command, ct);

        return NoContent();
    }

    /// <summary>
    /// Получить все виды животных с их породами.
    /// </summary>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Список всех видов животных.</returns>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<SpeciesDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllSpecies(CancellationToken ct)
    {
        var query = new GetAllSpeciesQuery();

        var species = await bus.InvokeAsync<IEnumerable<SpeciesDto>>(query, ct);

        return Ok(species);
    }

    /// <summary>
    /// Получить вид животного по идентификатору.
    /// </summary>
    /// <param name="speciesId">Идентификатор вида.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Вид животного с породами.</returns>
    [HttpGet("{speciesId:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(SpeciesDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSpeciesById(
        [FromRoute] Guid speciesId,
        CancellationToken ct)
    {
        var query = new GetSpeciesByIdQuery(speciesId);

        var species = await bus.InvokeAsync<SpeciesDto>(query, ct);

        return Ok(species);
    }

    /// <summary>
    /// Восстановить вид животного.
    /// </summary>
    /// <param name="speciesId">Идентификатор вида.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Результат операции.</returns>
    [HttpPatch("{speciesId:guid}/restore")]
    [Authorize(Policy = "AdminPolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RestoreSpecies(
        [FromRoute] Guid speciesId,
        CancellationToken ct)
    {
        var command = new RestoreSpeciesCommand { SpeciesId = speciesId };
        await bus.InvokeAsync(command, ct);

        return NoContent();
    }

    /// <summary>
    /// Восстановить породу.
    /// </summary>
    /// <param name="speciesId">Идентификатор вида.</param>
    /// <param name="breedId">Идентификатор породы.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Результат операции.</returns>
    [HttpPatch("{speciesId:guid}/breeds/{breedId:guid}/restore")]
    [Authorize(Policy = "AdminPolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RestoreBreed(
        [FromRoute] Guid speciesId,
        [FromRoute] Guid breedId,
        CancellationToken ct)
    {
        var command = new RestoreBreedCommand
        {
            SpeciesId = speciesId,
            BreedId = breedId
        };
        await bus.InvokeAsync(command, ct);

        return NoContent();
    }
}