using Microsoft.AspNetCore.Mvc;
using Notification.Api.Requests;
using Notification.Domain.Models;
using Notification.Domain.Services.Users.Interfaces;

namespace Notification.Api.Endpoints.Users;

public static class AddFriendEndpoint
{
    public static async Task<IResult> AddFriend([FromServices] IAddFriendService addFriendService,[FromBody] AddFriendRequest friendRequest)
    {
        var userCreatedResult = await addFriendService.ExecuteAsync(new FriendRequest(friendRequest.RequestingUserId,friendRequest.TargetUserId));
        
        return userCreatedResult switch
        {
            AddFriendResult.FriendSuccessfullyAdded => Results.Ok("Friend successfully added"),
            AddFriendResult.TargetUserAlreadyFriend => Results.Conflict("Friend is already added."),
            // We are hiding information about the target friend here deliberately 
            // This is because when a user has been blocked they should not know the other user exists
            // Also in the case where the user doesn't exist we keep the message the same
            // If we had different messages then we would expose information, you could deduce it is a different state 
            AddFriendResult.TargetUserHasBlockedRequestingUser or AddFriendResult.EitherRequestingUserOrTargetUserDoesNotExist => 
                Results.NotFound("Either requesting User or Target User does not exist"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}