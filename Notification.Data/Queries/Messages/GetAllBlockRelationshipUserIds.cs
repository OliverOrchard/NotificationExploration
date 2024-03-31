using Microsoft.Data.Sqlite;
using Notification.Domain.Models;
using Notification.Domain.Queries.Messages;

namespace Notification.Data.Queries.Messages;

public class GetAllBlockRelationshipUserIds : IGetAllBlockRelationshipUserIds
{
    private readonly IStorageProvider _storageProvider;

    public GetAllBlockRelationshipUserIds(IStorageProvider storageProvider)
    {
        _storageProvider = storageProvider;
    }

    public async Task<int[]> ExecuteAsync(int userId)
    {
        await using var connection = _storageProvider.GetConnection();
        var usersBlockedByUserId = await GetUsersBlockedByUserId(connection,userId);
        var usersWhoHaveBlockedUserId = await GetUsersWhoHaveBlockedUserId(connection,userId);

        var distinctUserIds = usersBlockedByUserId.Union(usersWhoHaveBlockedUserId).Distinct();
        
        return distinctUserIds.ToArray();
    }

    private async Task<int[]> GetUsersBlockedByUserId(SqliteConnection connection, int userId)
    {
        var command = connection.CreateCommand();
        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@RelationshipTypeId", RelationshipType.Blocked);
        command.CommandText = """
            Select
              RequestingUserId
            FROM UserRelationship
            WHERE TargetUserId = @UserId
            AND RelationshipTypeId = @RelationshipTypeId
            AND EndedAt is null
        """;
        
        await using SqliteDataReader sqliteDataReader = command.ExecuteReader();
        
        var users = new List<int>();
        while (sqliteDataReader.Read())
        {
            users.Add(sqliteDataReader.GetInt32(0));
        }

        return users.ToArray();
    }

    private async Task<int[]> GetUsersWhoHaveBlockedUserId(SqliteConnection connection, int userId)
    {
        var command = connection.CreateCommand();
        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@RelationshipTypeId", RelationshipType.Blocked);
        command.CommandText = """
            Select 
                TargetUserId 
            FROM UserRelationship 
            WHERE RequestingUserId = @UserId 
            AND RelationshipTypeId = @RelationshipTypeId 
            AND EndedAt is null
        """;
        
        await using SqliteDataReader sqliteDataReader = command.ExecuteReader();
        
        var users = new List<int>();
        while (sqliteDataReader.Read())
        {
            users.Add(sqliteDataReader.GetInt32(0));
        }

        return users.ToArray();
    }
}