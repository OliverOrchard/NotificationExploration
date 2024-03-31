using Microsoft.Data.Sqlite;
using Notification.Domain.Commands.Messages;
using Notification.Domain.Models;

namespace Notification.Data.Commands.Messages;

public class CreateMessageCommand : ICreateMessageCommand
{
    private readonly IStorageProvider _storageProvider;

    public CreateMessageCommand(IStorageProvider storageProvider)
    {
        _storageProvider = storageProvider;
    }

    public async Task<CreateMessageCommandResult> ExecuteAsync(int senderId, int recipientId, string body)
    {
        await using var connection = _storageProvider.GetConnection();
        var command = connection.CreateCommand();
        command.Parameters.AddWithValue("@SenderId", senderId);
        command.Parameters.AddWithValue("@RecipientId", recipientId);
        command.Parameters.AddWithValue("@Body", body);
        command.Parameters.AddWithValue("@SendAt", DateTime.UtcNow);
        command.CommandText = "INSERT INTO Message(SenderId,RecipientId,Body,SendAt) VALUES(@SenderId,@RecipientId,@Body,@SendAt)";
        
        try
        {
            command.ExecuteNonQuery();
        }
        // This check is not ideal, SQlite does not have specific error types so we have to check the message
        // If SQlite change the error message this will no longer work
        catch (SqliteException exception) when (exception.Message.Equals("SQLite Error 19: 'FOREIGN KEY constraint failed'."))
        {
            return CreateMessageCommandResult.EitherSenderOrRecipientDoesNotExist;
        }

        return CreateMessageCommandResult.MessageSuccessfullyCreated;
    }
}