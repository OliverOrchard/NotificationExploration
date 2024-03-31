namespace Notification.Domain.Models;

public enum CreateMessageCommandResult
{
    MessageSuccessfullyCreated,
    EitherSenderOrRecipientDoesNotExist
}