using System.Net.Http.Headers;
using System.Net.Http.Json;
using FileStorage.Contracts.Dtos;

namespace FileStorage.Contracts.Client;

/// <summary>
/// HTTP-клиент для взаимодействия с FileStorage микросервисом.
/// </summary>
public sealed class FileStorageHttpClient(HttpClient httpClient) : IFileStorageClient
{
    /// <inheritdoc />
    public async Task<FileUploadResponse> UploadAsync(
        Stream stream,
        string fileName,
        string contentType,
        CancellationToken ct)
    {
        using var content = new MultipartFormDataContent();

        var streamContent = new StreamContent(stream);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

        content.Add(streamContent, "file", fileName);

        var response = await httpClient.PostAsync("api/files/upload", content, ct);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<FileUploadResponse>(ct)
               ?? throw new InvalidOperationException("Не удалось десериализовать ответ FileStorage.");
    }
}
