using Notification.Domain.Models;

namespace Notification.Domain.Services.Users.Interfaces;

public interface IGetUsersService
{
    Task<User[]> ExecuteAsync();
}