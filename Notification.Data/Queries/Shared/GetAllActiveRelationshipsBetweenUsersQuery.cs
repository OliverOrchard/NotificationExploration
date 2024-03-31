using Microsoft.Data.Sqlite;
using Notification.Domain.Models;
using Notification.Domain.Queries.Shared;

namespace Notification.Data.Queries.Shared;

public class GetAllActiveRelationshipsBetweenUsersQuery : IGetAllActiveRelationshipsBetweenUsersQuery
{
    private readonly IStorageProvider _storageProvider;

    public GetAllActiveRelationshipsBetweenUsersQuery(IStorageProvider storageProvider)
    {
        _storageProvider = storageProvider;
    }

    public async Task<UserRelationship[]> ExecuteAsync(int User1Id, int User2Id)
    {
        await using var connection = _storageProvider.GetConnection();
        var command = connection.CreateCommand();
        command.Parameters.AddWithValue("@User1Id", User1Id);
        command.Parameters.AddWithValue("@User2Id", User2Id);
        command.CommandText = """
            Select 
                RequestingUserId, 
                TargetUserId, 
                RelationshipTypeId 
            FROM UserRelationship
            WHERE (
                (RequestingUserId = @User1Id AND TargetUserId = @User2Id)
                OR (RequestingUserId = @User2Id AND TargetUserId = @User1Id)
            ) AND EndedAt is null
        """;
        
        await using SqliteDataReader sqliteDataReader = await command.ExecuteReaderAsync();
        var users = new List<UserRelationship>();
        while (sqliteDataReader.Read())
        {
            users.Add(new UserRelationship(
                sqliteDataReader.GetInt32(0), 
                sqliteDataReader.GetInt32(1),
                (RelationshipType)sqliteDataReader.GetInt32(2)
                )
            );
        }

        return users.ToArray();
    }
}