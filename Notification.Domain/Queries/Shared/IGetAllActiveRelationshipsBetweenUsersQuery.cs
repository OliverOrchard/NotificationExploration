using Notification.Domain.Models;

namespace Notification.Domain.Queries.Shared;

public interface IGetAllActiveRelationshipsBetweenUsersQuery
{
    Task<UserRelationship[]> ExecuteAsync(int User1Id, int User2Id);
}