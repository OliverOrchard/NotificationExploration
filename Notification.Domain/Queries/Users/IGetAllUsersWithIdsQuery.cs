using Notification.Domain.Models;

namespace Notification.Domain.Queries.Users;

public interface IGetAllUsersWithIdsQuery
{
    Task<User[]> ExecuteAsync(int[] userIds);
}