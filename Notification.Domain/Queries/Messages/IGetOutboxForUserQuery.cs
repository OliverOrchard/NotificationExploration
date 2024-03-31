using Notification.Domain.Models;

namespace Notification.Domain.Queries.Messages;

public interface IGetOutboxForUserQuery
{
    Task<Message[]> ExecuteAsync(int userId, int[] blockedUserIds);
}