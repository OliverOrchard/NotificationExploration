namespace Notification.Api.Requests;

public record AddFriendRequest(int RequestingUserId, int TargetUserId);