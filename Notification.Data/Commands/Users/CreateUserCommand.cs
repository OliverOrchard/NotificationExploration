using Notification.Domain.Commands.Users;

namespace Notification.Data.Commands.Users;

public class CreateUserCommand : ICreateUserCommand
{
    private readonly IStorageProvider _storageProvider;

    public CreateUserCommand(IStorageProvider storageProvider)
    {
        _storageProvider = storageProvider;
    }

    public async Task ExecuteAsync(string username)
    {
        await using var connection = _storageProvider.GetConnection();
        var command = connection.CreateCommand();
        command.Parameters.AddWithValue("@Username", username);
        command.CommandText = "INSERT INTO User(Username) VALUES(@Username)";
        command.ExecuteNonQuery();
    }
}