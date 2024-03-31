using Notification.Domain.Models;
using Notification.Domain.Queries.Messages;
using Notification.Domain.Services.Messages.Interfaces;

namespace Notification.Domain.Services.Messages;

public class GetInboxService : IGetInboxService
{
    private readonly IGetAllBlockRelationshipUserIds _getAllBlockRelationshipUserIds;
    private readonly IGetInboxForUserQuery _getInboxForUserQuery;

    public GetInboxService(
        IGetAllBlockRelationshipUserIds getAllBlockRelationshipUserIds, 
        IGetInboxForUserQuery getInboxForUserQuery)
    {
        _getAllBlockRelationshipUserIds = getAllBlockRelationshipUserIds;
        _getInboxForUserQuery = getInboxForUserQuery;
    }

    public async Task<Message[]> ExecuteAsync(int userId)
    {
        var blockedUserIds = await _getAllBlockRelationshipUserIds.ExecuteAsync(userId);
        return await _getInboxForUserQuery.ExecuteAsync(userId, blockedUserIds);
    }
}