namespace Notification.Domain.Queries.Users;

public interface ICheckIfUsernameTakenQuery
{
    Task<bool> ExecuteAsync(string username);
}