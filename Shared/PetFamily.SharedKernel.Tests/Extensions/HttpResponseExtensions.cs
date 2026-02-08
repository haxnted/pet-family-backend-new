using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;

namespace PetFamily.SharedKernel.Tests.Extensions;

/// <summary>
/// Расширения для работы с HttpResponseMessage в тестах.
/// </summary>
public static class HttpResponseExtensions
{
    private static readonly JsonSerializerOptions DefaultJsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <summary>
    /// Проверяет успешный статус код ответа и десериализует тело ответа.
    /// </summary>
    /// <typeparam name="T">Тип результата десериализации.</typeparam>
    /// <param name="response">HTTP ответ для проверки.</param>
    /// <param name="options">Опции сериализации JSON. Если null, используются настройки по умолчанию.</param>
    /// <returns>Десериализованный результат типа T.</returns>
    public static async Task<T> EnsureSuccessAndReadAsync<T>(
        this HttpResponseMessage response,
        JsonSerializerOptions? options = null)
    {
        response.IsSuccessStatusCode.Should().BeTrue(
            $"Expected success status code but got {response.StatusCode}. " +
            $"Content: {await response.Content.ReadAsStringAsync()}");

        var result = await response.Content.ReadFromJsonAsync<T>(options ?? DefaultJsonOptions);
        result.Should().NotBeNull();
        return result!;
    }

    /// <summary>
    /// Проверяет ожидаемый статус код ответа и десериализует тело ответа.
    /// </summary>
    /// <typeparam name="T">Тип результата десериализации.</typeparam>
    /// <param name="response">HTTP ответ для проверки.</param>
    /// <param name="expectedStatusCode">Ожидаемый HTTP статус код.</param>
    /// <param name="options">Опции сериализации JSON. Если null, используются настройки по умолчанию.</param>
    /// <returns>Десериализованный результат типа T.</returns>
    public static async Task<T> EnsureStatusCodeAndReadAsync<T>(
        this HttpResponseMessage response,
        HttpStatusCode expectedStatusCode,
        JsonSerializerOptions? options = null)
    {
        response.StatusCode.Should().Be(expectedStatusCode,
            $"Content: {await response.Content.ReadAsStringAsync()}");

        var result = await response.Content.ReadFromJsonAsync<T>(options ?? DefaultJsonOptions);
        result.Should().NotBeNull();
        return result!;
    }

    /// <summary>
    /// Проверяет, что HTTP ответ имеет ожидаемый статус код.
    /// </summary>
    /// <param name="response">HTTP ответ для проверки.</param>
    /// <param name="expectedStatusCode">Ожидаемый HTTP статус код.</param>
    public static async Task EnsureStatusCodeAsync(
        this HttpResponseMessage response,
        HttpStatusCode expectedStatusCode)
    {
        response.StatusCode.Should().Be(expectedStatusCode,
            $"Content: {await response.Content.ReadAsStringAsync()}");
    }

    /// <summary>
    /// Проверяет, что HTTP ответ имеет успешный статус код (2xx).
    /// </summary>
    /// <param name="response">HTTP ответ для проверки.</param>
    public static async Task EnsureSuccessAsync(this HttpResponseMessage response)
    {
        response.IsSuccessStatusCode.Should().BeTrue(
            $"Expected success but got {response.StatusCode}. " +
            $"Content: {await response.Content.ReadAsStringAsync()}");
    }

    /// <summary>
    /// Проверяет, что HTTP ответ имеет статус код 400 (Bad Request).
    /// </summary>
    /// <param name="response">HTTP ответ для проверки.</param>
    public static async Task EnsureBadRequestAsync(this HttpResponseMessage response)
        => await response.EnsureStatusCodeAsync(HttpStatusCode.BadRequest);

    /// <summary>
    /// Проверяет, что HTTP ответ имеет статус код 404 (Not Found).
    /// </summary>
    /// <param name="response">HTTP ответ для проверки.</param>
    public static async Task EnsureNotFoundAsync(this HttpResponseMessage response)
        => await response.EnsureStatusCodeAsync(HttpStatusCode.NotFound);

    /// <summary>
    /// Проверяет, что HTTP ответ имеет статус код 401 (Unauthorized).
    /// </summary>
    /// <param name="response">HTTP ответ для проверки.</param>
    public static async Task EnsureUnauthorizedAsync(this HttpResponseMessage response)
        => await response.EnsureStatusCodeAsync(HttpStatusCode.Unauthorized);

    /// <summary>
    /// Проверяет, что HTTP ответ имеет статус код 403 (Forbidden).
    /// </summary>
    /// <param name="response">HTTP ответ для проверки.</param>
    public static async Task EnsureForbiddenAsync(this HttpResponseMessage response)
        => await response.EnsureStatusCodeAsync(HttpStatusCode.Forbidden);
}
