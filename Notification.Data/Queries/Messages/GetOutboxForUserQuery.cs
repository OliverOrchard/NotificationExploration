using Microsoft.Data.Sqlite;
using Notification.Domain.Models;
using Notification.Domain.Queries.Messages;

namespace Notification.Data.Queries.Messages;

public class GetOutboxForUserQuery : IGetOutboxForUserQuery
{
    private readonly IStorageProvider _storageProvider;

    public GetOutboxForUserQuery(IStorageProvider storageProvider)
    {
        _storageProvider = storageProvider;
    }

    public async Task<Message[]> ExecuteAsync(int userId, int[] blockedUserIds)
    {
        await using var connection = _storageProvider.GetConnection();
        var command = connection.CreateCommand();
        command.Parameters.AddWithValue("@UserId", userId);
        command.CommandText = "Select SenderId, RecipientId, Body, SendAt FROM Message WHERE SenderId = @UserId";
        
        await using SqliteDataReader sqliteDataReader = command.ExecuteReader();
        var messages = new List<Message>();
        while (sqliteDataReader.Read())
        {
            messages.Add(new Message(
                sqliteDataReader.GetInt32(0), 
                sqliteDataReader.GetInt32(1), 
                sqliteDataReader.GetString(2), 
                sqliteDataReader.GetDateTime(3))
            );
        }

        messages.RemoveAll(message => blockedUserIds.Contains(message.RecipientId));

        return messages.ToArray();
    }
}