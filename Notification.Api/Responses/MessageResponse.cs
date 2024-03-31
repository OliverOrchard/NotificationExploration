namespace Notification.Api.Responses;

public record MessageResponse(int SenderId, int RecipientId, string Body, DateTime SentAt);