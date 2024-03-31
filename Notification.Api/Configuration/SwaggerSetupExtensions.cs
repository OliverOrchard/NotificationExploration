using Microsoft.OpenApi.Models;

namespace Notification.Api.Configuration;

public static class SwaggerSetupExtensions
{
    public static void AddSwaggerGenForNotificationApi(this IServiceCollection service)
    {
        service.AddSwaggerGen(swaggerGenOptions =>
        {
            swaggerGenOptions.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Description = "The api key to access the Api",
                Type = SecuritySchemeType.ApiKey,
                Name = "x-api-key",
                In = ParameterLocation.Header,
                Scheme = "ApiKeyScheme"
            });
            var scheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                },
                In = ParameterLocation.Header
            };
            var requirement = new OpenApiSecurityRequirement()
            {
                { scheme, new List<string>() }
            };
            swaggerGenOptions.AddSecurityRequirement(requirement);
        });
    }
}