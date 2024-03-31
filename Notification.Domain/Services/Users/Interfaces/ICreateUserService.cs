using Notification.Domain.Models;

namespace Notification.Domain.Services.Users.Interfaces;

public interface ICreateUserService
{
    Task<UserCreatedResult> ExecuteAsync(string username);
}