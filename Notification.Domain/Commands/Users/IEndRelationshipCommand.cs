using Notification.Domain.Models;

namespace Notification.Domain.Commands.Users;

public interface IEndRelationshipCommand
{
    Task ExecuteAsync(int requestingUserId, int targetUserId, RelationshipType relationshipType);
}