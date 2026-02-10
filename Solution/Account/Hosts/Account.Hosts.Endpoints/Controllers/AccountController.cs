using Account.Handlers.Commands.UpdatePhoto;
using Account.Handlers.Commands.UpdateProfile;
using Account.Handlers.Queries.GetByUserId;
using Account.Hosts.Endpoints.Requests;
using Account.Services.Dtos;
using FileStorage.Contracts.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.SharedKernel.WebApi.Services;
using Wolverine;

namespace Account.Hosts.Endpoints.Controllers;

/// <summary>
/// Контроллер для работы с профилями пользователей.
/// </summary>
/// <param name="bus">Шина сообщений Wolverine.</param>
/// <param name="currentUser">Сервис для получения информации о текущем пользователе.</param>
/// <param name="fileStorageClient">Клиент для загрузки файлов в FileStorage.</param>
[ApiController]
[Route("api/accounts")]
public class AccountController(
    IMessageBus bus,
    ICurrentUser currentUser,
    IFileStorageClient fileStorageClient) : ControllerBase
{
    /// <summary>
    /// Получить профиль пользователя по идентификатору.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUserId(
        [FromRoute] Guid userId,
        CancellationToken ct)
    {
        var query = new GetAccountByUserIdQuery(userId);
        var result = await bus.InvokeAsync<AccountDto>(query, ct);

        return Ok(result);
    }

    /// <summary>
    /// Обновить профильные данные текущего пользователя.
    /// </summary>
    /// <param name="request">Запрос на обновление профиля.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpPut("profile")]
    [Authorize(Policy = "UserPolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateProfileRequest request,
        CancellationToken ct)
    {
        var command = new UpdateProfileCommand
        {
            UserId = currentUser.UserId,
            PhoneNumber = request.PhoneNumber,
            AgeExperience = request.AgeExperience,
            Description = request.Description,
        };

        await bus.InvokeAsync(command, ct);

        return NoContent();
    }

    /// <summary>
    /// Загрузить и обновить фотографию профиля текущего пользователя.
    /// </summary>
    /// <param name="file">Файл фотографии.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpPut("photo")]
    [Authorize(Policy = "UserPolicy")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePhoto(
        IFormFile file,
        CancellationToken ct)
    {
        await using var stream = file.OpenReadStream();
        var uploadResponse = await fileStorageClient.UploadAsync(
            stream, file.FileName, file.ContentType, ct);

        var command = new UpdatePhotoCommand
        {
            UserId = currentUser.UserId,
            PhotoId = uploadResponse.FileId,
        };

        await bus.InvokeAsync(command, ct);

        return Ok(new { PhotoId = uploadResponse.FileId, uploadResponse.PreSignedUrl });
    }

    /// <summary>
    /// Удалить фотографию профиля текущего пользователя.
    /// </summary>
    /// <param name="ct">Токен отмены.</param>
    [HttpDelete("photo")]
    [Authorize(Policy = "UserPolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePhoto(CancellationToken ct)
    {
        var command = new UpdatePhotoCommand
        {
            UserId = currentUser.UserId,
            PhotoId = null,
        };

        await bus.InvokeAsync(command, ct);

        return NoContent();
    }
}
