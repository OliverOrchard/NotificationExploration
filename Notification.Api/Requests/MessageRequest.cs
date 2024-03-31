namespace Notification.Api.Requests;

public record MessageRequest(int RecipientId, string Message);