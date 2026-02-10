using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolunteerManagement.Handlers.Shelters.Commands.Add;
using VolunteerManagement.Handlers.Shelters.Commands.AssignVolunteer;
using VolunteerManagement.Handlers.Shelters.Commands.ChangeStatus;
using VolunteerManagement.Handlers.Shelters.Commands.HardRemove;
using VolunteerManagement.Handlers.Shelters.Commands.RemoveVolunteer;
using VolunteerManagement.Handlers.Shelters.Commands.SoftRemove;
using VolunteerManagement.Handlers.Shelters.Commands.Update;
using VolunteerManagement.Handlers.Shelters.Commands.UpdateAddress;
using VolunteerManagement.Handlers.Shelters.Queries.GetAssignment;
using VolunteerManagement.Handlers.Shelters.Queries.GetShelterById;
using VolunteerManagement.Handlers.Shelters.Queries.GetSheltersWithPagination;
using VolunteerManagement.Hosts.Endpoints.Requests;
using VolunteerManagement.Services.Shelters.Dtos;
using Wolverine;

namespace VolunteerManagement.Hosts.Endpoints.Controllers;

/// <summary>
/// Контроллер для работы с приютами.
/// </summary>
/// <param name="bus">Шина сообщений Wolverine.</param>
[ApiController]
[Route("api/shelters")]
public class ShelterController(IMessageBus bus) : ControllerBase
{
    /// <summary>
    /// Добавить новый приют.
    /// </summary>
    /// <param name="request">Запрос на добавление приюта.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpPost]
    [Authorize(Policy = "AdminPolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Add(
        [FromBody] AddShelterRequest request,
        CancellationToken ct)
    {
        var command = new AddShelterCommand
        {
            Name = request.Name,
            Street = request.Street,
            City = request.City,
            State = request.State,
            ZipCode = request.ZipCode,
            PhoneNumber = request.PhoneNumber,
            Description = request.Description,
            OpenTime = request.OpenTime,
            CloseTime = request.CloseTime,
            Capacity = request.Capacity
        };

        await bus.InvokeAsync(command, ct);

        return NoContent();
    }

    /// <summary>
    /// Обновить основную информацию о приюте.
    /// </summary>
    /// <param name="shelterId">Идентификатор приюта.</param>
    /// <param name="request">Запрос на обновление.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpPatch("{shelterId:guid}/general")]
    [Authorize(Policy = "AdminPolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid shelterId,
        [FromBody] UpdateShelterRequest request,
        CancellationToken ct)
    {
        var command = new UpdateShelterCommand
        {
            ShelterId = shelterId,
            Name = request.Name,
            PhoneNumber = request.PhoneNumber,
            Description = request.Description,
            OpenTime = request.OpenTime,
            CloseTime = request.CloseTime,
            Capacity = request.Capacity
        };

        await bus.InvokeAsync(command, ct);

        return NoContent();
    }

    /// <summary>
    /// Обновить адрес приюта.
    /// </summary>
    /// <param name="shelterId">Идентификатор приюта.</param>
    /// <param name="request">Запрос на обновление адреса.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpPatch("{shelterId:guid}/address")]
    [Authorize(Policy = "AdminPolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAddress(
        [FromRoute] Guid shelterId,
        [FromBody] UpdateShelterAddressRequest request,
        CancellationToken ct)
    {
        var command = new UpdateShelterAddressCommand
        {
            ShelterId = shelterId,
            Street = request.Street,
            City = request.City,
            State = request.State,
            ZipCode = request.ZipCode
        };

        await bus.InvokeAsync(command, ct);

        return NoContent();
    }

    /// <summary>
    /// Изменить статус приюта.
    /// </summary>
    /// <param name="shelterId">Идентификатор приюта.</param>
    /// <param name="request">Запрос на изменение статуса.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpPatch("{shelterId:guid}/status")]
    [Authorize(Policy = "AdminPolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeStatus(
        [FromRoute] Guid shelterId,
        [FromBody] ChangeShelterStatusRequest request,
        CancellationToken ct)
    {
        var command = new ChangeShelterStatusCommand
        {
            ShelterId = shelterId,
            NewStatus = request.NewStatus
        };

        await bus.InvokeAsync(command, ct);

        return NoContent();
    }

    /// <summary>
    /// Назначить волонтёра в приют.
    /// </summary>
    /// <param name="shelterId">Идентификатор приюта.</param>
    /// <param name="request">Запрос на назначение волонтёра.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpPost("{shelterId:guid}/assignments")]
    [Authorize(Policy = "AdminPolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignVolunteer(
        [FromRoute] Guid shelterId,
        [FromBody] AssignVolunteerRequest request,
        CancellationToken ct)
    {
        var command = new AssignVolunteerCommand
        {
            ShelterId = shelterId,
            VolunteerId = request.VolunteerId,
            Role = request.Role
        };

        await bus.InvokeAsync(command, ct);

        return NoContent();
    }

    /// <summary>
    /// Убрать волонтёра из приюта.
    /// </summary>
    /// <param name="shelterId">Идентификатор приюта.</param>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpDelete("{shelterId:guid}/assignments/{volunteerId:guid}")]
    [Authorize(Policy = "AdminPolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveVolunteer(
        [FromRoute] Guid shelterId,
        [FromRoute] Guid volunteerId,
        CancellationToken ct)
    {
        var command = new RemoveVolunteerFromShelterCommand
        {
            ShelterId = shelterId,
            VolunteerId = volunteerId
        };

        await bus.InvokeAsync(command, ct);

        return NoContent();
    }

    /// <summary>
    /// Получить назначение волонтёра по идентификатору.
    /// </summary>
    /// <param name="shelterId">Идентификатор приюта.</param>
    /// <param name="assignmentId">Идентификатор назначения.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpGet("{shelterId:guid}/assignments/{assignmentId:guid}")]
    [ProducesResponseType(typeof(VolunteerAssignmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAssignment(
        [FromRoute] Guid shelterId,
        [FromRoute] Guid assignmentId,
        CancellationToken ct)
    {
        var query = new GetAssignmentQuery(shelterId, assignmentId);
        var result = await bus.InvokeAsync<VolunteerAssignmentDto>(query, ct);

        return Ok(result);
    }

    /// <summary>
    /// Получить приют по идентификатору.
    /// </summary>
    /// <param name="shelterId">Идентификатор приюта.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpGet("{shelterId:guid}")]
    [ProducesResponseType(typeof(ShelterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromRoute] Guid shelterId,
        CancellationToken ct)
    {
        var query = new GetShelterByIdQuery(shelterId);
        var result = await bus.InvokeAsync<ShelterDto>(query, ct);

        return Ok(result);
    }

    /// <summary>
    /// Получить приюты с пагинацией.
    /// </summary>
    /// <param name="page">Номер страницы.</param>
    /// <param name="count">Количество на странице.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpGet("pagination")]
    [ProducesResponseType(typeof(IEnumerable<ShelterDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetWithPagination(
        [FromQuery] int page = 1,
        [FromQuery] int count = 10,
        CancellationToken ct = default)
    {
        var query = new GetSheltersWithPaginationQuery(page, count);
        var result = await bus.InvokeAsync<IEnumerable<ShelterDto>>(query, ct);

        return Ok(result);
    }

    /// <summary>
    /// Жёстко удалить приют.
    /// </summary>
    /// <param name="shelterId">Идентификатор приюта.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpDelete("{shelterId:guid}/hard")]
    [Authorize(Policy = "AdminPolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> HardRemove(
        [FromRoute] Guid shelterId,
        CancellationToken ct)
    {
        var command = new HardRemoveShelterCommand { ShelterId = shelterId };
        await bus.InvokeAsync(command, ct);

        return NoContent();
    }

    /// <summary>
    /// Мягко удалить приют.
    /// </summary>
    /// <param name="shelterId">Идентификатор приюта.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpDelete("{shelterId:guid}/soft")]
    [Authorize(Policy = "AdminPolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SoftRemove(
        [FromRoute] Guid shelterId,
        CancellationToken ct)
    {
        var command = new SoftRemoveShelterCommand { ShelterId = shelterId };
        await bus.InvokeAsync(command, ct);

        return NoContent();
    }
}
