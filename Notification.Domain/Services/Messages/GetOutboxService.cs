using Notification.Domain.Models;
using Notification.Domain.Queries.Messages;
using Notification.Domain.Services.Messages.Interfaces;

namespace Notification.Domain.Services.Messages;

public class GetOutboxService : IGetOutboxService
{
    private readonly IGetAllBlockRelationshipUserIds _getAllBlockRelationshipUserIds;
    private readonly IGetOutboxForUserQuery _getOutboxForUserQuery;

    public GetOutboxService(
        IGetAllBlockRelationshipUserIds getAllBlockRelationshipUserIds, 
        IGetOutboxForUserQuery getOutboxForUserQuery)
    {
        _getAllBlockRelationshipUserIds = getAllBlockRelationshipUserIds;
        _getOutboxForUserQuery = getOutboxForUserQuery;
    }

    public async Task<Message[]> ExecuteAsync(int userId)
    {
        var blockedUserIds = await _getAllBlockRelationshipUserIds.ExecuteAsync(userId);
        return await _getOutboxForUserQuery.ExecuteAsync(userId, blockedUserIds);
    }
}