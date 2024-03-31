using Microsoft.Data.Sqlite;
using Notification.Domain.Models;
using Notification.Domain.Queries.Users;

namespace Notification.Data.Queries.Users;

public class CheckIfRelationshipExistsQuery : ICheckIfRelationshipExistsQuery
{
    private readonly IStorageProvider _storageProvider;

    public CheckIfRelationshipExistsQuery(IStorageProvider storageProvider)
    {
        _storageProvider = storageProvider;
    }
    public async Task<bool> ExecuteAsync(int requestingUserId, int targetUserId, RelationshipType relationshipType)
    {
        await using var connection = _storageProvider.GetConnection();
        var command = connection.CreateCommand();
        command.Parameters.AddWithValue("@RequestingUserId", requestingUserId);
        command.Parameters.AddWithValue("@TargetUserId", targetUserId);
        command.Parameters.AddWithValue("@RelationshipTypeId", relationshipType);
        command.CommandText = """
            SELECT 
                Id 
            FROM 
                UserRelationship 
            WHERE 
                RequestingUserId = @RequestingUserId 
            AND TargetUserId = @TargetUserId
            AND RelationshipTypeId = @RelationshipTypeId
            AND EndedAt is null
            LIMIT 1
        """;
        
        await using SqliteDataReader sqliteDataReader = command.ExecuteReader();
        sqliteDataReader.Read();
        try
        {
            sqliteDataReader.GetInt32(0);
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }
}