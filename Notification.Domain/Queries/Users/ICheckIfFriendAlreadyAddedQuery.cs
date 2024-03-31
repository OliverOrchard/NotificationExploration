using Notification.Domain.Models;

namespace Notification.Domain.Queries.Users;

public interface ICheckIfRelationshipExistsQuery
{
    Task<bool> ExecuteAsync(int requestingUserId, int targetUserId, RelationshipType relationshipType);
}