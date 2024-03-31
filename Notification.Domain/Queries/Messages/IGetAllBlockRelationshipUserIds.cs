namespace Notification.Domain.Queries.Messages;

public interface IGetAllBlockRelationshipUserIds
{
    Task<int[]> ExecuteAsync(int userId);
}