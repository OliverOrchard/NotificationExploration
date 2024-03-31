using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Newtonsoft.Json;
using Notification.Domain.Queries.Users;

namespace Notification.Integration.Test;

public class NotificationFixture : WebApplicationFactory<Program>
{
    private const string ApiKey = "testing";
    public Mock<IGetUsersQuery> MockGetUsersQuery { get; } = new();
    
    public readonly IFixture Fixture;

    public NotificationFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        Fixture = fixture;
    }

    public HttpRequestMessage CreateRequestWithApiKey(HttpMethod httpMethod, string requestUri)
    {
        var request = new HttpRequestMessage(httpMethod, requestUri);
        request.Headers.Add("x-api-key",ApiKey);
        return request;
    }

    public async Task<T> ConvertResponseToObject<T>(HttpResponseMessage httpResponseMessage)
    {
        return JsonConvert.DeserializeObject<T>(await httpResponseMessage.Content.ReadAsStringAsync());
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton(MockGetUsersQuery.Object);
        });
    }
}