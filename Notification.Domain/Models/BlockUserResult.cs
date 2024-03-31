namespace Notification.Domain.Models;

public enum BlockUserResult
{
    TargetUserHasBlockedRequestingUser,
    TargetUserAlreadyBlocked,
    TargetUserSuccessfullyBlocked,
    EitherRequestingUserOrTargetUserDoesNotExist
}