using Notification.Domain.Models;

namespace Notification.Domain.Queries.Messages;

public interface IGetInboxForUserQuery
{
    Task<Message[]> ExecuteAsync(int userId, int[] blockedUserIds);
}