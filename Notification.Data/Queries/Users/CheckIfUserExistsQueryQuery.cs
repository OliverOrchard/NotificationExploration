using Microsoft.Data.Sqlite;
using Notification.Domain.Queries.Users;

namespace Notification.Data.Queries.Users;

public class CheckIfUserExistsQuery : ICheckIfUserExistsQuery
{
    private readonly IStorageProvider _storageProvider;

    public CheckIfUserExistsQuery(IStorageProvider storageProvider)
    {
        _storageProvider = storageProvider;
    }

    public async Task<bool> ExecuteAsync(int id)
    {
        await using var connection = _storageProvider.GetConnection();
        var command = connection.CreateCommand();
        command.Parameters.AddWithValue("@Id", id);
        command.CommandText = "SELECT Id FROM User WHERE Id = @Id LIMIT 1";
        
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