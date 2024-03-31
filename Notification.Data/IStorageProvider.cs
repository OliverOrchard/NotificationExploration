using Microsoft.Data.Sqlite;

namespace Notification.Data;

public interface IStorageProvider
{
    public SqliteConnection GetConnection();
}