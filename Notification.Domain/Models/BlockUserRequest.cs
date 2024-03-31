namespace Notification.Domain.Models;

public record BlockRequest(int RequestingUserId, int TargetUserId);