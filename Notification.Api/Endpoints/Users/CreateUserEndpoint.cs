using Microsoft.AspNetCore.Mvc;
using Notification.Domain.Models;
using Notification.Domain.Services.Users.Interfaces;

namespace Notification.Api.Endpoints.Users;

public static class CreateUserEndpoint
{
    public static async Task<IResult> CreateUser([FromServices] ICreateUserService createUserService,[FromBody] string username)
    {
        var userCreatedResult = await createUserService.ExecuteAsync(username);
        
        return userCreatedResult switch
        {
            UserCreatedResult.UserCreatedSuccessfully => Results.Ok("User created successfully"),
            UserCreatedResult.UserNameAlreadyInUse => Results.Conflict("Username is already taken."),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}