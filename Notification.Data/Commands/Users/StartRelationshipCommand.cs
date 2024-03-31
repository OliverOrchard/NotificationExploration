using Microsoft.Data.Sqlite;
using Notification.Domain.Commands.Users;
using Notification.Domain.Models;

namespace Notification.Data.Commands.Users;

public class StartRelationshipCommand : IStartRelationshipCommand
{
    private readonly IStorageProvider _storageProvider;

    public StartRelationshipCommand(IStorageProvider storageProvider)
    {
        _storageProvider = storageProvider;
    }

    public async Task<StartRelationshipCommandResult> ExecuteAsync(int requestingUserId, int targetUserId, RelationshipType relationshipType)
    {
        await using var connection = _storageProvider.GetConnection();
        var command = connection.CreateCommand();
        command.Parameters.AddWithValue("@RequestingUserId", requestingUserId);
        command.Parameters.AddWithValue("@TargetUserId", targetUserId);
        command.Parameters.AddWithValue("@RelationshipTypeId", relationshipType);
        command.Parameters.AddWithValue("@RequestedAt", DateTime.UtcNow);
        command.CommandText = """
            INSERT INTO UserRelationship(RequestingUserId,TargetUserId,RelationshipTypeId,RequestedAt) 
            VALUES(@RequestingUserId,@TargetUserId,@RelationshipTypeId,@RequestedAt)
        """;
        try
        {
            command.ExecuteNonQuery();
        }
        // This check is not ideal, SQlite does not have specific error types so we have to check the message
        // If SQlite change the error message this will no longer work
        catch (SqliteException exception) when (exception.Message.Equals("SQLite Error 19: 'FOREIGN KEY constraint failed'."))
        {
            return StartRelationshipCommandResult.EitherRequestingUserOrTargetUserDoesNotExist;
        }

        return StartRelationshipCommandResult.RelationshipSuccessfullyStarted;
    }
}