using Microsoft.Data.Sqlite;

namespace Notification.Data;

public class StorageProvider : IStorageProvider
{
    private const string SqlLiteFile = "db.sqlite";
    
    public SqliteConnection GetConnection()
    {
        var isFirstRun = !File.Exists(SqlLiteFile);
        var connection = new SqliteConnection($"Data Source={SqlLiteFile};Pooling=true");
        connection.Open();
        
        if (isFirstRun)
        {
            CreateTablesForFirstRun(connection);
        }
        
        return connection;
    }
    
    private void CreateTablesForFirstRun(SqliteConnection connection)
    {
        using var command = connection.CreateCommand();
            
        CreateUserTable(command);
        CreateUserRelationshipTable(command);
        CreateMessageTable(command);
    }

    private void CreateUserTable(SqliteCommand command)
    {
        command.CommandText = """
            CREATE TABLE User(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username VARCHAR
            )
        """;
        command.ExecuteNonQuery();
    }

    private void CreateUserRelationshipTable(SqliteCommand command)
    {
        command.CommandText = """
            CREATE TABLE UserRelationship(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                RequestingUserId INTEGER,
                TargetUserId INTEGER,
                RelationshipTypeId INTEGER,
                RequestedAt DATETIME,
                EndedAt DATETIME,
                FOREIGN KEY(RequestingUserId) REFERENCES User(Id),
                FOREIGN KEY(TargetUserId) REFERENCES User(Id)
            )
        """;
        command.ExecuteNonQuery();
    }

    private void CreateMessageTable(SqliteCommand command)
    {
        command.CommandText = """
            CREATE TABLE Message(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                SenderId INTEGER,
                RecipientId INTEGER,
                Body VARCHAR,
                SendAt DATETIME,
                FOREIGN KEY(SenderId) REFERENCES User(Id),
                FOREIGN KEY(RecipientId) REFERENCES User(Id)
            )
        """;
        command.ExecuteNonQuery();
    }
}