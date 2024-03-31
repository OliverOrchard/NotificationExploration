using FluentAssertions;
using Moq;
using Notification.Domain.Commands.Users;
using Notification.Domain.Models;
using Notification.Domain.Queries.Users;
using Notification.Domain.Services.Users;

namespace Notification.Domain.Test;

public class CreateUserServiceTest
{
    [Test]
    public async Task GivenUsernameAlreadyTaken_ThenReturnUserNameAlreadyInUse()
    {
        // Arrange
        var userName = "testing";
        var checkIfUsernameTakenQuery = new Mock<ICheckIfUsernameTakenQuery>();
        checkIfUsernameTakenQuery.Setup(x => x.ExecuteAsync(userName)).ReturnsAsync(true);
        var service = new CreateUserService(new Mock<ICreateUserCommand>().Object, checkIfUsernameTakenQuery.Object);

        // Act
        var result = await service.ExecuteAsync(userName);

        // Assert
        result.Should().Be(UserCreatedResult.UserNameAlreadyInUse);
    }
    
    [Test]
    public async Task GivenUsernameNotTaken_ThenReturnUserCreatedSuccessfully()
    {
        // Arrange
        var userName = "testing";
        var checkIfUsernameTakenQuery = new Mock<ICheckIfUsernameTakenQuery>();
        checkIfUsernameTakenQuery.Setup(x => x.ExecuteAsync(userName)).ReturnsAsync(false);
        var service = new CreateUserService(new Mock<ICreateUserCommand>().Object, checkIfUsernameTakenQuery.Object);

        // Act
        var result = await service.ExecuteAsync(userName);

        // Assert
        result.Should().Be(UserCreatedResult.UserCreatedSuccessfully);
    }
}