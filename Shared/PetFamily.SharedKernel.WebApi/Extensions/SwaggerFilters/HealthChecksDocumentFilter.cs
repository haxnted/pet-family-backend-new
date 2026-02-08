using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PetFamily.SharedKernel.WebApi.Extensions.SwaggerFilters;

/// <summary>
/// Swagger document filter для отображения health check endpoints.
/// </summary>
public class HealthChecksDocumentFilter : IDocumentFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var healthResponse = new OpenApiResponse
        {
            Description = "Health check result",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = JsonSchemaType.Object,
                        Properties = new Dictionary<string, IOpenApiSchema>
                        {
                            ["status"] = new OpenApiSchema { Type = JsonSchemaType.String },
                            ["timestamp"] = new OpenApiSchema { Type = JsonSchemaType.String, Format = "date-time" }
                        }
                    }
                }
            }
        };

        var detailedResponse = new OpenApiResponse
        {
            Description = "Detailed health check result"
        };

        var tag = new OpenApiTag { Name = "HealthChecks" };

        swaggerDoc.Paths.Add("/health", CreatePath(
            "Liveness probe", "Базовая проверка работоспособности сервиса.", tag, healthResponse));

        swaggerDoc.Paths.Add("/health/ready", CreatePath(
            "Readiness probe", "Детальная проверка готовности сервиса.", tag, detailedResponse));

        swaggerDoc.Paths.Add("/health/db", CreatePath(
            "Database health", "Проверка подключения к базе данных.", tag, detailedResponse));

        swaggerDoc.Paths.Add("/health/messaging", CreatePath(
            "Messaging health", "Проверка подключения к брокеру сообщений.", tag, detailedResponse));
    }

    private static OpenApiPathItem CreatePath(
        string summary, string description, OpenApiTag tag, OpenApiResponse response)
    {
        return new OpenApiPathItem
        {
            Operations = new Dictionary<HttpMethod, OpenApiOperation>
            {
                [HttpMethod.Get] = new OpenApiOperation
                {
                    Tags = new HashSet<OpenApiTagReference> { new OpenApiTagReference(tag.Name ?? "HealthChecks") },
                    Summary = summary,
                    Description = description,
                    Responses = new OpenApiResponses
                    {
                        ["200"] = response,
                        ["503"] = new OpenApiResponse { Description = "Service unavailable" }
                    }
                }
            }
        };
    }
}