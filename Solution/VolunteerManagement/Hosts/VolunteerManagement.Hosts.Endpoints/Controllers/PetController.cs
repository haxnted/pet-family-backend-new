using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.SharedKernel.WebApi.Services;
using VolunteerManagement.Handlers.Volunteers.Pets.Commands.ConfirmAdoption;
using VolunteerManagement.Handlers.Volunteers.Pets.Commands.InitiateAdoption;
using VolunteerManagement.Handlers.Volunteers.Pets.Commands.RejectAdoption;
using VolunteerManagement.Handlers.Volunteers.Pets.Queries.GetAdoptionStatus;
using VolunteerManagement.Handlers.Volunteers.Pets.Queries.SearchPets;
using VolunteerManagement.Hosts.Endpoints.Requests;
using VolunteerManagement.Services.Volunteers.Dtos;
using Wolverine;

namespace VolunteerManagement.Hosts.Endpoints.Controllers;

/// <summary>
/// Контроллер для глобального поиска животных и процесса усыновления.
/// </summary>
/// <param name="bus">Шина сообщений Wolverine.</param>
/// <param name="currentUser">Текущий пользователь.</param>
[ApiController]
[Route("api/pets")]
public class PetController(
    IMessageBus bus,
    ICurrentUser currentUser) : ControllerBase
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

    /// <summary>
    /// Инициировать процесс усыновления питомца (запуск саги).
    /// Имя усыновителя и кличка питомца определяются автоматически.
    /// </summary>
    /// <param name="petId">Идентификатор питомца.</param>
    /// <param name="request">Данные запроса.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpPost("{petId:guid}/adopt")]
    [Authorize(Policy = "UserPolicy")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> InitiateAdoption(
        Guid petId,
        [FromBody] InitiateAdoptionRequest request,
        CancellationToken ct)
    {
        var command = new InitiateAdoptionCommand(
            request.VolunteerId,
            petId,
            currentUser.UserId,
            currentUser.UserName ?? "Unknown");

        var sagaId = await bus.InvokeAsync<Guid>(command, ct);

        return Accepted(new { SagaId = sagaId, Message = "Процесс усыновления инициирован" });
    }

    /// <summary>
    /// Получить статус процесса усыновления.
    /// </summary>
    /// <param name="sagaId">Идентификатор саги (CorrelationId).</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpGet("adoption-status/{sagaId:guid}")]
    [Authorize(Policy = "UserPolicy")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAdoptionStatus(Guid sagaId, CancellationToken ct)
    {
        var query = new GetAdoptionStatusQuery(sagaId);

        var result = await bus.InvokeAsync<AdoptionStatusDto?>(query, ct);

        if (result == null)
            return NotFound(new { Message = "Сага усыновления не найдена" });

        return Ok(result);
    }

    /// <summary>
    /// Подтвердить усыновление питомца (волонтёр отдаёт питомца).
    /// </summary>
    /// <param name="sagaId">Идентификатор саги (CorrelationId).</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpPost("adoption/{sagaId:guid}/confirm")]
    [Authorize(Policy = "VolunteerPolicy")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmAdoption(Guid sagaId, CancellationToken ct)
    {
        var command = new ConfirmAdoptionCommand(sagaId, currentUser.UserId);

        await bus.InvokeAsync(command, ct);

        return Ok(new { Message = "Подтверждение усыновления отправлено" });
    }

    /// <summary>
    /// Отклонить усыновление питомца (волонтёр отказывает).
    /// </summary>
    /// <param name="sagaId">Идентификатор саги (CorrelationId).</param>
    /// <param name="request">Данные запроса с причиной отказа.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpPost("adoption/{sagaId:guid}/reject")]
    [Authorize(Policy = "VolunteerPolicy")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RejectAdoption(
        Guid sagaId,
        [FromBody] RejectAdoptionRequest request,
        CancellationToken ct)
    {
        var command = new RejectAdoptionCommand(sagaId, currentUser.UserId, request.Reason);

        await bus.InvokeAsync(command, ct);

        return Ok(new { Message = "Отклонение усыновления отправлено" });
    }
}
