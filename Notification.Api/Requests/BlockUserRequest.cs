namespace Notification.Api.Requests;

public record BlockUserRequest(int RequestingUserId, int TargetUserId);