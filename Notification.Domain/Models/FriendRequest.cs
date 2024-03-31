namespace Notification.Domain.Models;

public record FriendRequest(int RequestingUserId, int TargetUserId);