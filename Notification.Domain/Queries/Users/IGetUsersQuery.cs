using Notification.Domain.Models;

namespace Notification.Domain.Queries.Users;

public interface IGetUsersQuery
{
    Task<User[]> ExecuteAsync();
}