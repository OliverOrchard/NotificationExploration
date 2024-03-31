using Microsoft.AspNetCore.Authentication;

namespace Notification.Api.Configuration.Authentication;

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    public string? ApiKey { get; set; }
}