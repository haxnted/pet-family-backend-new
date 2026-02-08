using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PetFamily.SharedKernel.WebApi.Extensions.SwaggerFilters;

/// <summary>
/// Swagger operation filter для корректной обработки загрузки файлов.
/// </summary>
public class FileUploadOperationFilter : IOperationFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fileParameters = context.ApiDescription.ParameterDescriptions
            .Where(p => p.ModelMetadata?.ModelType == typeof(IFormFile) ||
                        p.ModelMetadata?.ModelType == typeof(IFormFile[]) ||
                        p.ModelMetadata?.ModelType == typeof(List<IFormFile>))
            .ToList();

        if (fileParameters.Count == 0)
            return;

        operation.RequestBody = new OpenApiRequestBody
        {
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = JsonSchemaType.Object,
                        Properties = fileParameters.ToDictionary(
                            p => p.Name,
                            p => (IOpenApiSchema)new OpenApiSchema
                            {
                                Type = JsonSchemaType.String,
                                Format = "binary"
                            }),
                        Required = fileParameters
                            .Where(p => p.IsRequired)
                            .Select(p => p.Name)
                            .ToHashSet()
                    }
                }
            }
        };

        foreach (var fileParam in fileParameters)
        {
            if (operation.Parameters == null) continue;

            var paramToRemove = operation.Parameters
                .FirstOrDefault(p => p.Name == fileParam.Name);
            if (paramToRemove != null)
            {
                operation.Parameters.Remove(paramToRemove);
            }
        }
    }
}