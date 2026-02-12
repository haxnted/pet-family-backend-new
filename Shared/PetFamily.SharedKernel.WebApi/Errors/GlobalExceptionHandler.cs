using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PetFamily.SharedKernel.WebApi.Errors;

/// <summary>
/// Глобальный обработчик исключений для ASP.NET Core 8+
/// Маппит исключения на HTTP статус коды в соответствии с RFC 7231
/// </summary>
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    /// <summary>
    /// Обрабатывает исключение и возвращает соответствующий HTTP ответ
    /// </summary>
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        logger.LogError(
            exception,
            "Exception occurred: {ExceptionType} - {Message}",
            exception.GetType().Name,
            exception.Message);

        var problemDetails = MapExceptionToProblemDetails(exception, httpContext);

        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, ct);

        return true;
    }

    /// <summary>
    /// Маппит исключение на ProblemDetails с соответствующим HTTP статус кодом
    /// </summary>
    private static ProblemDetails MapExceptionToProblemDetails(Exception exception, HttpContext httpContext)
    {
        return exception switch
        {
            _ when IsDomainException(exception) => CreateValidationProblemDetails(exception, httpContext),
            _ when IsEntityNotFoundException(exception) => CreateNotFoundProblemDetails(exception, httpContext),
            _ when IsForbiddenException(exception) => CreateForbiddenProblemDetails(exception, httpContext),
            _ when IsUseCaseException(exception) => CreateUseCaseProblemDetails(exception, httpContext),
            _ => CreateInternalServerErrorProblemDetails(exception, httpContext)
        };
    }

    /// <summary>
    /// Проверяет, является ли исключение доменным исключением из SharedKernel или любого модуля
    /// </summary>
    private static bool IsDomainException(Exception exception)
    {
        var exceptionType = exception.GetType();

        return exceptionType.Name == "DomainException" &&
               (exceptionType.Namespace == "PetFamily.SharedKernel.Domain.Exceptions" ||
                exceptionType.Namespace?.EndsWith(".Domain.Exceptions") == true);
    }

    /// <summary>
    /// Проверяет, является ли исключение EntityNotFoundException из SharedKernel или любого модуля
    /// </summary>
    private static bool IsEntityNotFoundException(Exception exception)
    {
        var exceptionType = exception.GetType();

        return exceptionType.Name.StartsWith("EntityNotFoundException") &&
               (exceptionType.Namespace == "PetFamily.SharedKernel.Application.Exceptions" ||
                exceptionType.Namespace?.Contains(".Application.Exceptions") == true);
    }

    /// <summary>
    /// Проверяет, является ли исключение ForbiddenException из SharedKernel или любого модуля.
    /// </summary>
    private static bool IsForbiddenException(Exception exception)
    {
        var exceptionType = exception.GetType();

        return exceptionType.Name == "ForbiddenException" &&
               (exceptionType.Namespace == "PetFamily.SharedKernel.Application.Exceptions" ||
                exceptionType.Namespace?.Contains(".Application.Exceptions") == true);
    }

    /// <summary>
    /// Создает ProblemDetails для ForbiddenException (403 Forbidden).
    /// </summary>
    private static ProblemDetails CreateForbiddenProblemDetails(Exception exception, HttpContext httpContext)
    {
        const int statusCode = StatusCodes.Status403Forbidden;

        return new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            Title = "Forbidden",
            Status = statusCode,
            Detail = exception.Message,
            Instance = httpContext.Request.Path,
            Extensions =
            {
                ["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier,
                ["exceptionType"] = exception.GetType().Name
            }
        };
    }

    /// <summary>
    /// Проверяет, является ли исключение UseCaseException из SharedKernel или любого модуля
    /// </summary>
    private static bool IsUseCaseException(Exception exception)
    {
        var exceptionType = exception.GetType();

        return exceptionType.Name == "UseCaseException" &&
               (exceptionType.Namespace == "PetFamily.SharedKernel.Application.Exceptions" ||
                exceptionType.Namespace?.EndsWith(".Application.Exceptions") == true);
    }

    /// <summary>
    /// Создает ProblemDetails для доменных исключений (400 Bad Request)
    /// </summary>
    private static ProblemDetails CreateValidationProblemDetails(Exception exception, HttpContext httpContext)
    {
        const int statusCode = StatusCodes.Status400BadRequest;

        return new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Validation Error",
            Status = statusCode,
            Detail = exception.Message,
            Instance = httpContext.Request.Path,
            Extensions =
            {
                ["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier,
                ["exceptionType"] = exception.GetType().Name
            }
        };
    }

    /// <summary>
    /// Создает ProblemDetails для EntityNotFoundException (404 Not Found)
    /// </summary>
    private static ProblemDetails CreateNotFoundProblemDetails(Exception exception, HttpContext httpContext)
    {
        const int statusCode = StatusCodes.Status404NotFound;

        var exceptionType = exception.GetType();
        var entityIdProperty = exceptionType.GetProperty("EntityId");
        var entityNameProperty = exceptionType.GetProperty("EntityName");

        var entityId = entityIdProperty?.GetValue(exception)?.ToString();
        var entityName = entityNameProperty?.GetValue(exception)?.ToString();

        var extensions = new Dictionary<string, object?>
        {
            ["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier,
            ["exceptionType"] = exception.GetType().Name
        };

        if (!string.IsNullOrEmpty(entityId))
            extensions["entityId"] = entityId;

        if (!string.IsNullOrEmpty(entityName))
            extensions["entityName"] = entityName;

        return new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "Not Found",
            Status = statusCode,
            Detail = exception.Message,
            Instance = httpContext.Request.Path,
            Extensions = extensions
        };
    }

    /// <summary>
    /// Создает ProblemDetails для UseCaseException (400 Bad Request)
    /// </summary>
    private static ProblemDetails CreateUseCaseProblemDetails(
        Exception exception,
        HttpContext httpContext)
    {
        const int statusCode = StatusCodes.Status400BadRequest;

        return new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Use Case Error",
            Status = statusCode,
            Detail = exception.Message,
            Instance = httpContext.Request.Path,
            Extensions =
            {
                ["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier,
                ["exceptionType"] = exception.GetType().Name
            }
        };
    }

    /// <summary>
    /// Создает ProblemDetails для внутренних ошибок сервера (500 Internal Server Error)
    /// </summary>
    private static ProblemDetails CreateInternalServerErrorProblemDetails(
        Exception exception,
        HttpContext httpContext)
    {
        const int statusCode = StatusCodes.Status500InternalServerError;

        return new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "Internal Server Error",
            Status = statusCode,
            Detail = "Произошла внутренняя ошибка сервера. Пожалуйста, попробуйте позже.",
            Instance = httpContext.Request.Path,
            Extensions =
            {
                ["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier,
                ["exceptionType"] = exception.GetType().Name
            }
        };
    }
}