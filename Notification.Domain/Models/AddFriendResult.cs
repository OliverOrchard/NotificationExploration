namespace Notification.Domain.Models;

public enum AddFriendResult
{
    TargetUserHasBlockedRequestingUser,
    TargetUserAlreadyFriend,
    FriendSuccessfullyAdded,
    EitherRequestingUserOrTargetUserDoesNotExist
}