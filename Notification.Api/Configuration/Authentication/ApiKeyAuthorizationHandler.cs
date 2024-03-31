using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Notification.Api.Configuration.Authentication;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) 
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var expectedApiKey = Options.ApiKey;
        
        if (expectedApiKey is null)
        {
            return Task.FromResult(AuthenticateResult.Fail("No Api Key has been configured"));
        }
        
        if (!Request.Headers.TryGetValue("x-api-key",out var apiKeyValues))
        {
            return Task.FromResult(AuthenticateResult.Fail("Missing Api Key"));
        }

        var apiKey = apiKeyValues.FirstOrDefault();

        if (apiKey != expectedApiKey)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Api Key"));
        }

        var claims = new[] { new Claim(ClaimTypes.Name, "APIKeyUser") };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}