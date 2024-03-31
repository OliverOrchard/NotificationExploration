using Notification.Domain.Commands.Users;
using Notification.Domain.Models;
using Notification.Domain.Queries.Users;
using Notification.Domain.Services.Users.Interfaces;

namespace Notification.Domain.Services.Users;

public class CreateUserService : ICreateUserService
{
    private readonly ICheckIfUsernameTakenQuery _checkIfUsernameTakenQuery;
    private readonly ICreateUserCommand _createUserCommand;

    public CreateUserService(
        ICreateUserCommand createUserCommand, 
        ICheckIfUsernameTakenQuery checkIfUsernameTakenQuery)
    {
        _createUserCommand = createUserCommand;
        _checkIfUsernameTakenQuery = checkIfUsernameTakenQuery;
    }

    public async Task<UserCreatedResult> ExecuteAsync(string username)
    {
        var isUsernameTaken = await _checkIfUsernameTakenQuery.ExecuteAsync(username);
        
        if (isUsernameTaken)
        {
            return UserCreatedResult.UserNameAlreadyInUse;
        }
        
        await _createUserCommand.ExecuteAsync(username);
        
        return UserCreatedResult.UserCreatedSuccessfully;
    }
}