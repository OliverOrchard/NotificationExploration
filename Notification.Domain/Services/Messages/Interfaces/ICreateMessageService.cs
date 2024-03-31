using Notification.Domain.Models;

namespace Notification.Domain.Services.Messages.Interfaces;

public interface ICreateMessageService
{
    Task<MessageCreatedResult> ExecuteAsync(int senderId, int messageRequestRecipientId, string messageRequestMessage);
}