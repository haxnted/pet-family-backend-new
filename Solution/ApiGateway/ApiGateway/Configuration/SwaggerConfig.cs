using System.Text.Json;
using System.Text.Json.Nodes;

namespace ApiGateway.Configuration;

/// <summary>
/// Swagger configuration for Ocelot gateway.
/// </summary>
public static class SwaggerConfig
{
    /// <summary>
    /// Alter upstream swagger JSON to add security definitions.
    /// </summary>
    public static string AlterUpstream(HttpContext context, string swaggerJson)
    {
        var swagger = JsonNode.Parse(swaggerJson);
        if (swagger == null)
        {
            return swaggerJson;
        }

        // Ensure components object exists
        swagger["components"] ??= new JsonObject();
        var components = swagger["components"]!.AsObject();

        // Add security schemes if not present
        if (!components.ContainsKey("securitySchemes"))
        {
            components["securitySchemes"] = new JsonObject
            {
                ["Bearer"] = new JsonObject
                {
                    ["type"] = "http",
                    ["scheme"] = "bearer",
                    ["bearerFormat"] = "JWT",
                    ["description"] = "Enter JWT token obtained from /api/auth/login"
                }
            };
        }

        // Add global security requirement if not present
        if (swagger["security"] == null)
        {
            swagger["security"] = new JsonArray
            {
                new JsonObject
                {
                    ["Bearer"] = new JsonArray()
                }
            };
        }

        return swagger.ToJsonString(new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
}
