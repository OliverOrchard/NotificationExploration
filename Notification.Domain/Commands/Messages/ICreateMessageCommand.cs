using Notification.Domain.Models;

namespace Notification.Domain.Commands.Messages;

public interface ICreateMessageCommand
{
    Task<CreateMessageCommandResult> ExecuteAsync(int senderId, int recipientId, string body);
}