using Microsoft.Data.Sqlite;
using Notification.Domain.Queries.Users;

namespace Notification.Data.Queries.Users;

public class CheckIfUsernameTakenQuery : ICheckIfUsernameTakenQuery
{
    private readonly IStorageProvider _storageProvider;

    public CheckIfUsernameTakenQuery(IStorageProvider storageProvider)
    {
        _storageProvider = storageProvider;
    }

    public async Task<bool> ExecuteAsync(string username)
    {
        await using var connection = _storageProvider.GetConnection();
        var command = connection.CreateCommand();
        command.Parameters.AddWithValue("@Username", username);
        command.CommandText = "SELECT Id FROM User WHERE Username = @Username LIMIT 1";
        
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