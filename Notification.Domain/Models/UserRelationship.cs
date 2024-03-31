namespace Notification.Domain.Models;

public record UserRelationship(int RequestingUserId, int TargetUserId, RelationshipType RelationshipType);