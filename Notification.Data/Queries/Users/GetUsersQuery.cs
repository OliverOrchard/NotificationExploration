using Microsoft.Data.Sqlite;
using Notification.Domain.Models;
using Notification.Domain.Queries.Users;

namespace Notification.Data.Queries.Users;

public class GetUsersQuery : IGetUsersQuery
{
    private readonly IStorageProvider _storageProvider;

    public GetUsersQuery(IStorageProvider storageProvider)
    {
        _storageProvider = storageProvider;
    }

    public async Task<User[]> ExecuteAsync()
    {
        await using var connection = _storageProvider.GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = "Select Id, Username FROM User";
        
        await using SqliteDataReader sqliteDataReader = command.ExecuteReader();
        var users = new List<User>();
        while (sqliteDataReader.Read())
        {
            users.Add(new User(
                sqliteDataReader.GetInt32(0), 
                sqliteDataReader.GetString(1))
            );
        }

        return users.ToArray();
    }
}