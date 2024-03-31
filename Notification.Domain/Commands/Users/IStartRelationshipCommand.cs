using Notification.Domain.Models;

namespace Notification.Domain.Commands.Users;

public interface IStartRelationshipCommand
{
    Task<StartRelationshipCommandResult> ExecuteAsync(int requestingUserId, int targetUserId, RelationshipType relationshipType);
}