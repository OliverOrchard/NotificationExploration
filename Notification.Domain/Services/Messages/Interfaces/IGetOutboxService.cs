using Notification.Domain.Models;

namespace Notification.Domain.Services.Messages.Interfaces;

public interface IGetOutboxService
{
    Task<Message[]> ExecuteAsync(int userId);
}