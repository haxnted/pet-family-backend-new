using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.SharedKernel.WebApi.Services;
using VolunteerManagement.Handlers.Volunteers.Commands.ActivateAccount;
using VolunteerManagement.Handlers.Volunteers.Commands.Add;
using VolunteerManagement.Handlers.Volunteers.Commands.HardRemove;
using VolunteerManagement.Handlers.Volunteers.Commands.HardRemoveAllPets;
using VolunteerManagement.Handlers.Volunteers.Commands.SoftRemove;
using VolunteerManagement.Handlers.Volunteers.Pets.Commands.Add;
using VolunteerManagement.Handlers.Volunteers.Pets.Commands.AddPhotos;
using VolunteerManagement.Handlers.Volunteers.Pets.Commands.RemovePhoto;
using VolunteerManagement.Handlers.Volunteers.Pets.Commands.UpdatePhotos;
using VolunteerManagement.Handlers.Volunteers.Pets.Commands.Delete;
using VolunteerManagement.Handlers.Volunteers.Pets.Commands.Move;
using VolunteerManagement.Handlers.Volunteers.Pets.Commands.Restore;
using VolunteerManagement.Handlers.Volunteers.Pets.Commands.SoftDelete;
using VolunteerManagement.Handlers.Volunteers.Pets.Commands.Update;
using VolunteerManagement.Handlers.Volunteers.Pets.Queries.GetPetById;
using VolunteerManagement.Handlers.Volunteers.Pets.Queries.GetPetsByVolunteerId;
using VolunteerManagement.Handlers.Volunteers.Queries.GetVolunteerById;
using VolunteerManagement.Handlers.Volunteers.Queries.GetVolunteersWithPagination;
using VolunteerManagement.Hosts.Endpoints.Requests;
using VolunteerManagement.Services.Volunteers.Dtos;
using FileStorage.Contracts.Client;
using Wolverine;

namespace VolunteerManagement.Hosts.Endpoints.Controllers;

/// <summary>
/// Контроллер для работы с волонтерами и их питомцами.
/// </summary>
/// <param name="bus">Шина сообщений Wolverine.</param>
/// <param name="currentUser">Сервис для получения информации о текущем пользователе.</param>
/// <param name="fileStorageClient">Клиент для загрузки файлов в FileStorage.</param>
[ApiController]
[Route("api/volunteers")]
public class VolunteerController(
    IMessageBus bus,
    ICurrentUser currentUser,
    IFileStorageClient fileStorageClient) : ControllerBase
{
    /// <summary>
    /// Добавить нового волонтера.
    /// </summary>
    /// <param name="request">Запрос на добавление волонтера.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpPost]
    [Authorize(Policy = "UserPolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddVolunteer(
        [FromBody] AddVolunteerRequest request,
        CancellationToken ct)
    {
        var command = new AddVolunteerCommand
        {
            Name = request.FullName.Name,
            Surname = request.FullName.Surname,
            Patronymic = request.FullName.Patronymic,
            UserId = currentUser.UserId,
        };

        await bus.InvokeAsync(command, ct);

        return NoContent();
    }

    /// <summary>
    /// Активировать профиль волонтёра (восстановить после мягкого удаления).
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpPatch("{volunteerId:guid}/activate")]
    [Authorize(Policy = "VolunteerPolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ActivateAccountVolunteer(
        [FromRoute] Guid volunteerId,
        CancellationToken ct)
    {
        var userId = currentUser.UserId;
        if (userId != volunteerId)
        {
            return Forbid();
        }

        var command = new ActivateAccountVolunteerCommand { VolunteerId = volunteerId };
        await bus.InvokeAsync(command, ct);

        return NoContent();
    }

    /// <summary>
    /// Жёстко удалить волонтёра и всех его животных из базы данных.
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpDelete("{volunteerId:guid}/hard")]
    [Authorize(Policy = "AdminPolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> HardRemove(
        [FromRoute] Guid volunteerId,
        CancellationToken ct)
    {
        var command = new HardRemoveVolunteerCommand { VolunteerId = volunteerId };
        await bus.InvokeAsync(command, ct);

        return NoContent();
    }

    /// <summary>
    /// Мягко удалить волонтёра (пометить как удалённого).
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpDelete("{volunteerId:guid}/soft")]
    [Authorize(Policy = "VolunteerPolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> SoftRemove(
        [FromRoute] Guid volunteerId,
        CancellationToken ct)
    {
        var userId = currentUser.UserId;
        if (userId != volunteerId)
        {
            return Forbid();
        }

        var command = new SoftRemoveVolunteerCommand { VolunteerId = volunteerId };
        await bus.InvokeAsync(command, ct);

        return NoContent();
    }

    /// <summary>
    /// Получить информацию о волонтёре по идентификатору.
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpGet("{volunteerId:guid}/general-info")]
    [ProducesResponseType(typeof(VolunteerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromRoute] Guid volunteerId,
        CancellationToken ct)
    {
        var query = new GetVolunteerByIdQuery(volunteerId);
        var result = await bus.InvokeAsync<VolunteerDto>(query, ct);

        return Ok(result);
    }

    /// <summary>
    /// Получить всех питомцев волонтёра.
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpGet("{volunteerId:guid}/pets")]
    [ProducesResponseType(typeof(List<PetDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPets(
        [FromRoute] Guid volunteerId,
        CancellationToken ct)
    {
        var query = new GetPetsByVolunteerIdQuery(volunteerId);
        var result = await bus.InvokeAsync<List<PetDto>>(query, ct);

        return Ok(result);
    }

    /// <summary>
    /// Получить информацию о питомце по идентификатору.
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="petId">Идентификатор питомца.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Информация о питомце.</returns>
    [HttpGet("{volunteerId:guid}/pets/{petId:guid}")]
    [ProducesResponseType(typeof(PetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPetById(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        CancellationToken ct)
    {
        var query = new GetPetByIdQuery(volunteerId, petId);
        var result = await bus.InvokeAsync<PetDto>(query, ct);

        return Ok(result);
    }

    /// <summary>
    /// Получить информацию о волонтёре по идентификатору.
    /// </summary>
    /// <param name="page">Текущая страница.</param>
    /// <param name="count">Количество страниц.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Информация о волонтёре.</returns>
    [HttpGet("pagination")]
    [ProducesResponseType(typeof(VolunteerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVolunteersWithPagination(
        [FromQuery] int page,
        [FromQuery] int count,
        CancellationToken ct)
    {
        var query = new GetVolunteersWithPaginationQuery(page, count);
        var result = await bus.InvokeAsync<IEnumerable<VolunteerDto>>(query, ct);

        return Ok(result);
    }

    /// <summary>
    /// Добавить питомца волонтёру.
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="request">Запрос на добавление питомца.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Идентификатор созданного питомца.</returns>
    [HttpPost("{volunteerId:guid}/pets")]
    [Authorize(Policy = "VolunteerPolicy")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddPet(
        [FromRoute] Guid volunteerId,
        [FromBody] AddPetRequest request,
        CancellationToken ct)
    {
        var userId = currentUser.UserId;
        if (userId != volunteerId)
        {
            return Forbid();
        }

        var command = new AddPetCommand
        {
            VolunteerId = volunteerId,
            NickName = request.NickName,
            GeneralDescription = request.GeneralDescription,
            HealthInformation = request.HealthInformation,
            BreedId = request.BreedId,
            SpeciesId = request.SpeciesId,
            Address = request.Address,
            Weight = request.Weight,
            Height = request.Height,
            PhoneNumber = request.PhoneNumber,
            BirthDate = request.BirthDate,
            IsCastrated = request.IsCastrated,
            IsVaccinated = request.IsVaccinated,
            HelpStatus = request.HelpStatus,
            Requisites = request.Requisites
        };

        var petId = await bus.InvokeAsync<Guid>(command, ct);

        return Ok(petId);
    }

    /// <summary>
    /// Обновить информацию о питомце.
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="request">Запрос на обновление питомца.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpPut("{volunteerId:guid}/pets")]
    [Authorize(Policy = "VolunteerPolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePet(
        [FromRoute] Guid volunteerId,
        [FromBody] UpdatePetRequest request,
        CancellationToken ct)
    {
        var userId = currentUser.UserId;
        if (userId != volunteerId)
        {
            return Forbid();
        }

        var command = new UpdatePetCommand
        {
            VolunteerId = volunteerId,
            PetId = request.PetId,
            GeneralDescription = request.GeneralDescription,
            HealthInformation = request.HealthInformation,
            Address = request.Address,
            Weight = request.Weight,
            Height = request.Height,
            PhoneNumber = request.PhoneNumber,
            IsCastrated = request.IsCastrated,
            IsVaccinated = request.IsVaccinated,
            HelpStatus = request.HelpStatus,
            Requisites = request.Requisites
        };

        await bus.InvokeAsync(command, ct);

        return NoContent();
    }

    /// <summary>
    /// Удалить питомца.
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра-владельца.</param>
    /// <param name="petId">Идентификатор животного.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpDelete("{volunteerId:guid}/pets/{petId:guid}")]
    [Authorize(Policy = "VolunteerPolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        CancellationToken ct)
    {
        var userId = currentUser.UserId;
        if (userId != volunteerId)
        {
            return Forbid();
        }

        var command = new DeletePetCommand
        {
            VolunteerId = volunteerId,
            PetId = petId
        };

        await bus.InvokeAsync(command, ct);

        return NoContent();
    }

    /// <summary>
    /// Жёстко удалить всех питомцев волонтёра.
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpDelete("{volunteerId:guid}/pets")]
    [Authorize(Policy = "VolunteerPolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> HardRemoveAllPets(
        [FromRoute] Guid volunteerId,
        CancellationToken ct)
    {
        var userId = currentUser.UserId;
        if (userId != volunteerId)
        {
            return Forbid();
        }

        var command = new HardRemoveAllPetsCommand { VolunteerId = volunteerId };
        await bus.InvokeAsync(command, ct);

        return NoContent();
    }

    /// <summary>
    /// Мягкое удаление питомца.
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="petId">Идентификатор питомца.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpDelete("{volunteerId:guid}/pets/{petId:guid}/soft")]
    // [Authorize(Policy = "VolunteerPolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SoftDeletePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        CancellationToken ct)
    {
        var userId = currentUser.UserId;
        if (userId != volunteerId)
        {
            return Forbid();
        }

        var command = new SoftDeletePetCommand
        {
            VolunteerId = volunteerId,
            PetId = petId
        };
        await bus.InvokeAsync(command, ct);

        return NoContent();
    }

    /// <summary>
    /// Восстановить питомца.
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="petId">Идентификатор питомца.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpPatch("{volunteerId:guid}/pets/{petId:guid}/restore")]
    [Authorize(Policy = "VolunteerPolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RestorePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        CancellationToken ct)
    {
        var userId = currentUser.UserId;
        if (userId != volunteerId)
        {
            return Forbid();
        }

        var command = new RestorePetCommand
        {
            VolunteerId = volunteerId,
            PetId = petId
        };
        await bus.InvokeAsync(command, ct);

        return NoContent();
    }

    /// <summary>
    /// Переместить питомца на новую позицию.
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="petId">Идентификатор питомца.</param>
    /// <param name="request">Запрос с новой позицией.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpPatch("{volunteerId:guid}/pets/{petId:guid}/position")]
    [Authorize(Policy = "VolunteerPolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MovePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] MovePetRequest request,
        CancellationToken ct)
    {
        var userId = currentUser.UserId;
        if (userId != volunteerId)
        {
            return Forbid();
        }

        var command = new MovePetCommand
        {
            VolunteerId = volunteerId,
            PetId = petId,
            NewPosition = request.NewPosition
        };
        await bus.InvokeAsync(command, ct);

        return NoContent();
    }

    /// <summary>
    /// Добавить фотографии питомцу.
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="petId">Идентификатор питомца.</param>
    /// <param name="files">Список файлов-фотографий.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpPost("{volunteerId:guid}/pets/{petId:guid}/photos")]
    [Authorize(Policy = "VolunteerPolicy")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AddPetPhotos(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        List<IFormFile> files,
        CancellationToken ct)
    {
        var userId = currentUser.UserId;
        if (userId != volunteerId)
        {
            return Forbid();
        }

        var photoIds = new List<Guid>(files.Count);
        foreach (var file in files)
        {
            await using var stream = file.OpenReadStream();
            var uploadResponse = await fileStorageClient.UploadAsync(
                stream, file.FileName, file.ContentType, ct);
            photoIds.Add(uploadResponse.FileId);
        }

        var command = new AddPetPhotosCommand
        {
            VolunteerId = volunteerId,
            PetId = petId,
            PhotoIds = photoIds
        };

        await bus.InvokeAsync(command, ct);

        return Ok(photoIds);
    }

    /// <summary>
    /// Обновить фотографии питомца.
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="petId">Идентификатор питомца.</param>
    /// <param name="files">Список файлов-фотографий.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpPut("{volunteerId:guid}/pets/{petId:guid}/photos")]
    [Authorize(Policy = "VolunteerPolicy")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdatePetPhotos(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        List<IFormFile> files,
        CancellationToken ct)
    {
        var userId = currentUser.UserId;
        if (userId != volunteerId)
        {
            return Forbid();
        }

        var photoIds = new List<Guid>(files.Count);
        foreach (var file in files)
        {
            await using var stream = file.OpenReadStream();
            var uploadResponse = await fileStorageClient.UploadAsync(
                stream, file.FileName, file.ContentType, ct);
            photoIds.Add(uploadResponse.FileId);
        }

        var command = new UpdatePetPhotosCommand
        {
            VolunteerId = volunteerId,
            PetId = petId,
            PhotoIds = photoIds
        };

        await bus.InvokeAsync(command, ct);

        return Ok(photoIds);
    }

    /// <summary>
    /// Удалить фотографию питомца.
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="petId">Идентификатор питомца.</param>
    /// <param name="photoId">Идентификатор фотографии.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpDelete("{volunteerId:guid}/pets/{petId:guid}/photos")]
    [Authorize(Policy = "VolunteerPolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RemovePetPhoto(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromQuery] Guid photoId,
        CancellationToken ct)
    {
        var userId = currentUser.UserId;
        if (userId != volunteerId)
        {
            return Forbid();
        }

        var command = new RemovePetPhotoCommand
        {
            VolunteerId = volunteerId,
            PetId = petId,
            PhotoId = photoId
        };

        await bus.InvokeAsync(command, ct);

        return NoContent();
    }
}