namespace Notification.Domain.Queries.Users;

public interface ICheckIfUserExistsQuery
{
    Task<bool> ExecuteAsync(int id);
}