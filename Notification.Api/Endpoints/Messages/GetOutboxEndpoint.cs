using Microsoft.AspNetCore.Mvc;
using Notification.Api.Responses;
using Notification.Domain.Services.Messages.Interfaces;

namespace Notification.Api.Endpoints.Messages;

public static class GetOutboxEndpoint
{
    public static async Task<MessageResponse[]> GetOutbox(
        [FromServices] IGetOutboxService getOutboxService,
        [FromQuery] int userId)
    {
        var messages = await getOutboxService.ExecuteAsync(userId);
        return messages.Select(message => new MessageResponse(message.SenderId,message.RecipientId,message.Body,message.SentAt)).ToArray();
    }
}