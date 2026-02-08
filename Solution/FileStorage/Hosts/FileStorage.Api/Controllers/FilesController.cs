using FileStorage.Application.Services;
using FileStorage.Contracts.Dtos;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.SharedKernel.Contracts.Events.FileStorage;

namespace FileStorage.Api.Controllers;

/// <summary>
/// Контроллер для работы с файлами
/// </summary>
[ApiController]
[Route("api/files")]
[Authorize]
public class FilesController(
    IFileStorageService fileStorageService,
    IValidator<IFormFile> fileValidator,
    IPublishEndpoint publishEndpoint,
    ILogger<FilesController> logger) : ControllerBase
{
    /// <summary>
    /// Загружает файл и возвращает pre-signed URL
    /// </summary>
    [HttpPost("upload")]
    [ProducesResponseType(typeof(FileUploadResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<FileUploadResponse>> Upload(
        [FromBody] IFormFile file,
        CancellationToken ct)
    {
        var validationResult = await fileValidator.ValidateAsync(file, ct);
        if (!validationResult.IsValid)
        {
            return BadRequest(new
            {
                Errors = validationResult.Errors.Select(e => e.ErrorMessage)
            });
        }

        await using var stream = file.OpenReadStream();

        var response = await fileStorageService.UploadAsync(
            stream,
            file.FileName,
            file.ContentType,
            ct);

        logger.LogInformation(
            "Файл {FileId} загружен пользователем: {FileName} ({Size} bytes)",
            response.FileId,
            response.FileName,
            response.FileSize);

        return Ok(response);
    }

    /// <summary>
    /// Получает pre-signed URL для скачивания файла
    /// </summary>
    [HttpGet("{fileId:guid}/download")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<object>> GetDownloadUrl(
        [FromRoute] Guid fileId,
        [FromQuery] int expirySeconds = 3600,
        CancellationToken ct = default)
    {
        try
        {
            var url = await fileStorageService.GetPreSignedUrlAsync(fileId, expirySeconds, ct);

            return Ok(new
            {
                FileId = fileId,
                Url = url,
                ExpiresAt = DateTime.UtcNow.AddSeconds(expirySeconds)
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при получении URL для файла {FileId}", fileId);
            return NotFound(new { Error = "Файл не найден" });
        }
    }

    /// <summary>
    /// Получает метаданные файла
    /// </summary>
    [HttpGet("{fileId:guid}")]
    [ProducesResponseType(typeof(FileInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<FileInfoDto>> GetFileInfo(
        [FromRoute] Guid fileId,
        CancellationToken ct)
    {
        var fileInfo = await fileStorageService.GetFileInfoAsync(fileId, ct);

        if (fileInfo == null)
        {
            return NotFound(new { Error = "Файл не найден" });
        }

        return Ok(fileInfo);
    }

    /// <summary>
    /// Инициирует удаление файла через событие
    /// </summary>
    [HttpDelete("{fileId:guid}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid fileId,
        CancellationToken ct)
    {
        await publishEndpoint.Publish(new FileDeleteRequestedEvent(fileId, "petfamily-files"), ct);

        logger.LogInformation("Запрос на удаление файла {FileId} опубликован", fileId);

        return Accepted(new { Message = "Запрос на удаление файла принят" });
    }
}