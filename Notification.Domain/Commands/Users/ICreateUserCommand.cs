namespace Notification.Domain.Commands.Users;

public interface ICreateUserCommand
{
    Task ExecuteAsync(string username);
}