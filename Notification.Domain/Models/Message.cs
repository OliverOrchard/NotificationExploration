namespace Notification.Domain.Models;

public record Message(int SenderId, int RecipientId, string Body, DateTime SentAt);