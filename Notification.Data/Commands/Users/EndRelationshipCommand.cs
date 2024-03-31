using Notification.Domain.Commands.Users;
using Notification.Domain.Models;

namespace Notification.Data.Commands.Users;

public class EndRelationshipCommand : IEndRelationshipCommand
{
    private readonly IStorageProvider _storageProvider;

    public EndRelationshipCommand(IStorageProvider storageProvider)
    {
        _storageProvider = storageProvider;
    }

    public async Task ExecuteAsync(int requestingUserId, int targetUserId, RelationshipType relationshipType)
    {        
        await using var connection = _storageProvider.GetConnection();
        var command = connection.CreateCommand();
        command.Parameters.AddWithValue("@RequestingUserId", requestingUserId);
        command.Parameters.AddWithValue("@TargetUserId", targetUserId);
        command.Parameters.AddWithValue("@RelationshipTypeId", relationshipType);
        command.Parameters.AddWithValue("@EndedAt", DateTime.UtcNow);
        command.CommandText = """
            UPDATE UserRelationship
            SET EndedAt = @EndedAt
            WHERE RequestingUserId = @RequestingUserId 
            AND TargetUserId = @TargetUserId
            AND RelationshipTypeId = @RelationshipTypeId
            AND EndedAt is null
        """;
        command.ExecuteNonQuery();
    }
}