using Microsoft.AspNetCore.Mvc;
using Notification.Api.Requests;
using Notification.Domain.Models;
using Notification.Domain.Services.Users.Interfaces;

namespace Notification.Api.Endpoints.Users;

public static class BlockUserEndpoint
{
    public static async Task<IResult> BlockUser([FromServices] IBlockUserService blockUserService,[FromBody] BlockUserRequest blockUserRequest)
    {
        var userCreatedResult = await blockUserService.ExecuteAsync(new BlockRequest(blockUserRequest.RequestingUserId,blockUserRequest.TargetUserId));
        
        return userCreatedResult switch
        {
            BlockUserResult.TargetUserSuccessfullyBlocked => Results.Ok("User successfully blocked"),
            BlockUserResult.TargetUserAlreadyBlocked => Results.Conflict("User is already blocked."),
            // We are hiding information about the target here deliberately 
            // This is because when a user has been blocked they should not know the other user exists
            // Also in the case where the user doesn't exist we keep the message the same
            // If we had different messages then we would expose information, as you could deduce it was a different state 
            BlockUserResult.TargetUserHasBlockedRequestingUser or BlockUserResult.EitherRequestingUserOrTargetUserDoesNotExist => 
                Results.NotFound("Either requesting User or Target User does not exist"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}