using Microsoft.AspNetCore.Mvc;
using Notification.Api.Responses;
using Notification.Domain.Services.Messages.Interfaces;

namespace Notification.Api.Endpoints.Messages;

public class GetInboxEndpoint
{
    public static async Task<MessageResponse[]> GetInbox(
        [FromServices] IGetInboxService getInboxService,
        [FromQuery] int userId)
    {
        var messages = await getInboxService.ExecuteAsync(userId);
        return messages.Select(message => new MessageResponse(message.SenderId,message.RecipientId,message.Body,message.SentAt)).ToArray();
    }
}