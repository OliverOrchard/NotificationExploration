using Microsoft.AspNetCore.Mvc;
using Notification.Api.Requests;
using Notification.Domain.Models;
using Notification.Domain.Services.Messages.Interfaces;

namespace Notification.Api.Endpoints.Messages;

public static class CreateMessageEndpoint
{
    public static async Task<IResult> CreateMessage(
        [FromServices] ICreateMessageService createMessageService, 
        [FromQuery] int userId, 
        [FromBody] MessageRequest messageRequest)
    {
        var result = await createMessageService.ExecuteAsync(userId, messageRequest.RecipientId, messageRequest.Message);
        
        return result switch
        {
            MessageCreatedResult.MessageCreatedSuccessfully => Results.Ok("Message created successfully"),
            MessageCreatedResult.RecipientIsNotAFriend => Results.Conflict("Can only send messages to users who are your friend."),
            MessageCreatedResult.RecipientIsBlocked => Results.Conflict("You cannot send messages to users you have blocked."),
            // We are hiding information about the recipient here deliberately 
            // This is because when a user has been blocked they should not know the other user exists
            // Also in the case where the user doesn't exist we keep the message the same
            // If we had different messages then we would expose information, as you could deduce it was a different state 
            MessageCreatedResult.RecipientHasBlockedRequestingUser or MessageCreatedResult.EitherSenderOrRecipientDoesNotExist => 
                Results.NotFound("Either requesting Recipient or Sender does not exist"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}