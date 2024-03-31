namespace Notification.Domain.Models;

public enum MessageCreatedResult
{
    MessageCreatedSuccessfully,
    RecipientIsNotAFriend,
    RecipientIsBlocked,
    RecipientHasBlockedRequestingUser,
    EitherSenderOrRecipientDoesNotExist
}