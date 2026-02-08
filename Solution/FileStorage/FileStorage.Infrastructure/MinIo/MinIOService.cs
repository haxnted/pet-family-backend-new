using FileStorage.Application.Services;
using FileStorage.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace FileStorage.Infrastructure.MinIO;

/// <summary>
/// Сервис для работы с MinIO как с S3-совместимым файловым хранилищем
/// </summary>
public class MinIoService : IMinIoService
{
    private readonly IMinioClient _minioClient;
    private readonly MinIoSettings _settings;
    private readonly ILogger<MinIoService> _logger;

    /// <summary>
    /// Инициализировать сервис для работы с MinIO
    /// </summary>
    /// <param name="settings">Настройки подключения к MinIO</param>
    /// <param name="logger">Логгер</param>
    public MinIoService(
        IOptions<MinIoSettings> settings,
        ILogger<MinIoService> logger)
    {
        _settings = settings.Value;
        _logger = logger;

        _minioClient = new MinioClient()
            .WithEndpoint(_settings.Endpoint)
            .WithCredentials(_settings.AccessKey, _settings.SecretKey)
            .WithSSL(_settings.UseSSL)
            .Build();
    }

    /// <inheritdoc/>
    public async Task EnsureBucketExistsAsync(CancellationToken ct)
    {
        try
        {
            var bucketExists = await _minioClient.BucketExistsAsync(new BucketExistsArgs()
                    .WithBucket(_settings.BucketName),
                ct);

            if (!bucketExists)
            {
                await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_settings.BucketName), ct);

                _logger.LogInformation("Создан bucket: {BucketName}", _settings.BucketName);
            }
            else
            {
                _logger.LogInformation("Bucket {BucketName} уже существует", _settings.BucketName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при проверке или создании bucket {BucketName}", _settings.BucketName);

            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<string> UploadFileAsync(
        Guid fileId,
        Stream stream,
        string fileName,
        string contentType,
        CancellationToken ct)
    {
        var objectName = GetObjectName(fileId);

        var metadata = new Dictionary<string, string>
        {
            { "original-filename", fileName },
            { "uploaded-at", DateTime.UtcNow.ToString("O") }
        };

        try
        {
            await _minioClient.PutObjectAsync(
                new PutObjectArgs()
                    .WithBucket(_settings.BucketName)
                    .WithObject(objectName)
                    .WithStreamData(stream)
                    .WithObjectSize(stream.Length)
                    .WithContentType(contentType)
                    .WithHeaders(metadata),
                ct);

            _logger.LogInformation("Файл {FileId} загружен в MinIO как объект {ObjectName}", fileId, objectName);

            return objectName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при загрузке файла {FileId} в MinIO", fileId);

            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<string> GetPreSignedUrlAsync(Guid fileId, int expirySeconds, CancellationToken ct)
    {
        var objectName = GetObjectName(fileId);

        try
        {
            var url = await _minioClient.PresignedGetObjectAsync(
                new PresignedGetObjectArgs()
                    .WithBucket(_settings.BucketName)
                    .WithObject(objectName)
                    .WithExpiry(expirySeconds));

            _logger.LogDebug(
                "Сгенерирован pre-signed URL для файла {FileId} со сроком действия {ExpirySeconds} секунд",
                fileId,
                expirySeconds);

            return url;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при генерации pre-signed URL для файла {FileId}", fileId);

            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<ObjectStat?> GetObjectStatAsync(Guid fileId, CancellationToken ct)
    {
        var objectName = GetObjectName(fileId);

        try
        {
            var stat = await _minioClient.StatObjectAsync(
                new StatObjectArgs()
                    .WithBucket(_settings.BucketName)
                    .WithObject(objectName),
                ct);

            return new ObjectStat
            {
                Size = stat.Size,
                ContentType = stat.ContentType,
                MetaData = stat.MetaData
            };
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Файл {FileId} не найден в MinIO", fileId);

            return null;
        }
    }

    /// <inheritdoc/>
    public async Task DeleteFileAsync(Guid fileId, CancellationToken ct)
    {
        var objectName = GetObjectName(fileId);

        try
        {
            await _minioClient.RemoveObjectAsync(
                new RemoveObjectArgs()
                    .WithBucket(_settings.BucketName)
                    .WithObject(objectName),
                ct);

            _logger.LogInformation("Файл {FileId} удалён из MinIO", fileId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении файла {FileId} из MinIO", fileId);

            throw;
        }
    }

    /// <summary>
    /// Сформировать имя объекта в хранилище по идентификатору файла
    /// </summary>
    /// <param name="fileId">Идентификатор файла</param>
    /// <returns>Имя объекта в MinIO</returns>
    private string GetObjectName(Guid fileId)
    {
        return $"{fileId}.file";
    }
}