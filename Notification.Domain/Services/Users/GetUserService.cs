using Notification.Domain.Models;
using Notification.Domain.Queries.Users;
using Notification.Domain.Services.Users.Interfaces;

namespace Notification.Domain.Services.Users;

public class GetUserService : IGetUsersService
{
    private readonly IGetUsersQuery _getUsersQuery;

    public GetUserService(IGetUsersQuery getUsersQuery)
    {
        _getUsersQuery = getUsersQuery;
    }

    public async Task<User[]> ExecuteAsync()
    {
        return await _getUsersQuery.ExecuteAsync();
    }
}