using Notification.Api.Configuration.Authentication;

namespace Notification.Api.Configuration;

public static class AuthenticationAndAuthorizationSetUpExtensions
{
    public static void AddAuthenticationAndAuthorizationForNotificationApi(this IServiceCollection service, string? apiKey)
    {
        service.AddAuthentication();

        service.AddAuthentication("ApiKey")
            .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>("ApiKey", o => { o.ApiKey = apiKey; });

        service.AddAuthorization(options =>
        {
            options.AddPolicy("ApiKey", policy =>
            {
                policy.AddAuthenticationSchemes("ApiKey");
                policy.RequireAuthenticatedUser();
            });
        });
    }
}