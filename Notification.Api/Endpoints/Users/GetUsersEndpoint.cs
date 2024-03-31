using Microsoft.AspNetCore.Mvc;
using Notification.Api.Responses;
using Notification.Domain.Services.Users.Interfaces;

namespace Notification.Api.Endpoints.Users;

public static class GetUsersEndpoint
{
    public static async Task<UserResponse[]> GetUsers([FromServices] IGetUsersService getUsersService)
    {
        var users = await getUsersService.ExecuteAsync();
        return users.Select(user => new UserResponse(user.Id,user.Username)).ToArray();
    }
}