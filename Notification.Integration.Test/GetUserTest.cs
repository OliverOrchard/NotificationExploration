using System.Net;
using AutoFixture;
using FluentAssertions;
using Moq;
using Notification.Api.Responses;
using Notification.Domain.Models;
using Xunit;

namespace Notification.Integration.Test;

[Collection("Local resources collection")]
public class GetUserTest
{
    private readonly NotificationFixture _fixture;

    public GetUserTest(NotificationFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GivenUserInDb_WhenCallingGetUsersAndWithApiKey_ReturnUsers()
    {
        // Arrange
        var fakeDbUsers = _fixture.Fixture.CreateMany<User>().ToArray();
        _fixture.MockGetUsersQuery.Setup(x => x.ExecuteAsync()).ReturnsAsync(fakeDbUsers);
        var client = _fixture.CreateClient();
        
        // Act
        var request = _fixture.CreateRequestWithApiKey(HttpMethod.Get, "user");
        var jsonResponse = await client.SendAsync(request);
        var response = await _fixture.ConvertResponseToObject<IEnumerable<UserResponse>>(jsonResponse);
        
        // Assert
        foreach (var fakeDbUser in fakeDbUsers)
        {
            response.Should().Contain(actualUser =>
                actualUser.Id == fakeDbUser.Id
            );
        }
    }
    [Fact]
    public async Task GivenUserInDb_WhenCallingGetUsersWithoutApiKey_Return401()
    {
        // Arrange
        var fakeDbUsers = _fixture.Fixture.CreateMany<User>().ToArray();
        _fixture.MockGetUsersQuery.Setup(x => x.ExecuteAsync()).ReturnsAsync(fakeDbUsers);
        var client = _fixture.CreateClient();
        
        // Act
        var request = new HttpRequestMessage(HttpMethod.Get, "user");
        var jsonResponse = await client.SendAsync(request);
        
        // Assert
        jsonResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}